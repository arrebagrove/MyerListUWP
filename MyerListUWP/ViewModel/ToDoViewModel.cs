using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JP.Utils.Data;
using JP.Utils.Debug;
using JP.Utils.Framework;
using MyerList.Helper;
using MyerList.Model;
using MyerListUWP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace MyerListUWP.ViewModel
{
    public class ToDoViewModel : ViewModelBase, INavigable
    {

        /// <summary>
        ///显示没有待办事项
        /// </summary>
        private Visibility _shownoitems;
        public Visibility ShowNoItems
        {
            get
            {
                return _shownoitems;
            }
            set
            {
                _shownoitems = value;
                RaisePropertyChanged(() => ShowNoItems);
            }
        }

        /// <summary>
        /// 表示当前添加的待办事项
        /// </summary>
        private ToDo _newToDo;
        public ToDo NewToDo
        {
            get
            {
                return _newToDo;
            }
            set
            {
                if (_newToDo != value)
                {
                    _newToDo = value;
                    RaisePropertyChanged(() => NewToDo);
                }
            }
        }

        /// <summary>
        /// 当前的待办事项
        /// </summary>
        private ObservableCollection<ToDo> _myToDos;
        public ObservableCollection<ToDo> MyToDos
        {
            get
            {
                if (_myToDos != null)
                {
                    if (_myToDos.Count == 0) ShowNoItems = Visibility.Visible;
                    else ShowNoItems = Visibility.Collapsed;
                    return _myToDos;
                }
                return _myToDos = new ObservableCollection<ToDo>();
            }
            set
            {
                if (_myToDos != value)
                {
                    _myToDos = value;
                }
                RaisePropertyChanged(() => MyToDos);
            }
        }

        /// <summary>
        /// 点击+号添加新的待办事项
        /// </summary>
        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                if (_addCommand != null)
                {
                    return _addCommand;
                }
                return _addCommand = new RelayCommand(() =>
                {
                    //Messenger.Default.Send(new GenericMessage<string>(""), "AddScheduleUI");

                    var loader = new ResourceLoader();
                    ModeTitle = loader.GetString("AddTitle");

                    NewToDo = new ToDo();

                    _addMode = AddMode.Add;
                });
            }
        }

        /// <summary>
        ///点击垃圾桶图标
        /// </summary>
        private RelayCommand<object> _deleteCommand;
        public RelayCommand<object> DeleteCommand
        {
            get
            {
                if (_deleteCommand != null)
                {
                    return _deleteCommand;
                }

                return _deleteCommand = new RelayCommand<object>(async (param) =>
                {
                    string id = (string)param;
                    await DeleteToDo(id);
                });
            }
        }

        /// <summary>
        /// 完成待办事项
        /// </summary>
        private RelayCommand<object> _checkCommand;
        public RelayCommand<object> CheckCommand
        {
            get
            {
                if (_checkCommand != null) return _checkCommand;
                return _checkCommand = new RelayCommand<object>(async (param) =>
                {
                    string id = (string)param;
                    await CheckTodoDone(id);
                });
            }
        }

        /// <summary>
        /// 同步列表
        /// </summary>
        private RelayCommand _syncCommand;
        public RelayCommand SyncCommand
        {
            get
            {
                if (_syncCommand != null)
                {
                    return _syncCommand;
                }
                return _syncCommand = new RelayCommand(async () =>
                {
                    await SyncAllToDos();
                });
            }
        }



        /// <summary>
        /// 添加/修改待办事项时候的“完成”
        /// </summary>
        private RelayCommand _okCommand;
        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand = new RelayCommand(async () =>
                {
                    await AddOrModifyToDo();
                });
            }
        }

        /// <summary>
        /// 点击列表的项目，修改待办事项
        /// </summary>
        private RelayCommand<ToDo> _modifyCommand;
        public RelayCommand<ToDo> ModifyCommand
        {
            get
            {
                if (_modifyCommand != null)
                {
                    return _modifyCommand;
                }

                return _modifyCommand = new RelayCommand<ToDo>((todo) =>
                {
                    try
                    {
                        if (isInReorder) return;

                        var id = todo.ID;
                        var scheduleToModify = MyToDos.ToList().Find(sche =>
                        {
                            if (sche.ID == id) return true;
                            else return false;
                        });

                        if (scheduleToModify == null)
                        {
                            return;
                        }

                        this.NewToDo.ID = scheduleToModify.ID;
                        this.NewToDo.Content = scheduleToModify.Content;


                        Messenger.Default.Send(new GenericMessage<string>(""), "Modify");

                        var loader = new ResourceLoader();
                        ModeTitle = loader.GetString("ModifyTitle");

                        _addMode = AddMode.Modify;
                    }
                    catch (Exception e)
                    {
                        var task = ExceptionHelper.WriteRecord(e);
                    }
                });
            }
        }

        /// <summary>
        /// Pop out the content dialog
        /// </summary>
        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand != null)
                {
                    return _cancelCommand;
                }
                return _cancelCommand = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new GenericMessage<string>(""), "RemoveScheduleUI");
                    NewToDo = new ToDo();
                });
            }
        }

        public ToDoViewModel()
        {
            MyToDos = new ObservableCollection<ToDo>();
            NewToDo = new ToDo();

            Messenger.Default.Register<GenericMessage<string>>(this, "Check", act =>
            {
                var id = act.Content;
                CheckCommand.Execute(id);
            });

            Messenger.Default.Register<GenericMessage<string>>(this, "Delete", act =>
            {
                var id = act.Content;
                DeleteCommand.Execute(id);
            });
            Messenger.Default.Register<GenericMessage<string>>(this, "ClearNote", act =>
            {
                this.NewToDo = new ToDo();
            });
            Messenger.Default.Register<GenericMessage<string>>(this, "PressEnter", act =>
            {
                if (!string.IsNullOrEmpty(act.Content))
                {
                    NewToDo.Content = act.Content;
                    OkCommand.Execute(null);
                }
            });
        }

        /// <summary>
        /// 添加or修改内容
        /// </summary>
        /// <returns></returns>
        private async Task AddOrModifyToDo()
        {
            try
            {
                if (NewToDo != null)
                {
                    IsLoading = Visibility.Visible;

                    Messenger.Default.Send(new GenericMessage<string>(""), "SyncStoryBegin");

                    if (_addMode != AddMode.None)
                    {
                        Messenger.Default.Send(new GenericMessage<string>(""), "RemoveScheduleUI");
                    }

                    //添加
                    if (_addMode == AddMode.Add)
                    {
                        await AddToDo();
                    }
                    //修改
                    else if (_addMode == AddMode.Modify)
                    {
                        await ModifyToDo();
                    }

                }
            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
            }
        }

        private async Task AddToDo()
        {
            //离线模式
            if (App.isInOfflineMode || App.isNoNetwork)
            {
                NewToDo.ID = Guid.NewGuid().ToString();

                //0 for insert,1 for add
                if (LocalSettingHelper.GetValue("AddMode") == "0")
                {
                    MyToDos.Insert(0, NewToDo);
                }
                else
                {
                    MyToDos.Add(NewToDo);
                }
                await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(MyToDos, SerializerFileNames.ToDoFileName);

                Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), "UpdateTile");

                NewToDo = new ToDo();

                IsLoading = Visibility.Collapsed;

            }
            else if (App.isNoNetwork)
            {
                //TO DO: Store the schedule in SendingQueue
            }
            else
            {
                //在线模式
                var result = await PostHelper.AddSchedule(LocalSettingHelper.GetValue("sid"), NewToDo.Content, "0");
                if (!string.IsNullOrEmpty(result))
                {
                    ToDo newSchedule = ToDo.ParseJsonTo(result);

                    if (LocalSettingHelper.GetValue("AddMode") == "0")
                    {
                        MyToDos.Insert(0, newSchedule);
                    }
                    else
                    {
                        MyToDos.Add(newSchedule);
                    }

                    await PostHelper.SetMyOrder(LocalSettingHelper.GetValue("sid"), ToDo.GetCurrentOrderString(MyToDos));

                    NewToDo = new ToDo();

                    Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), "UpdateTile");
                    IsLoading = Visibility.Collapsed;

                    await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(MyToDos, SerializerFileNames.ToDoFileName);

                }
            }

        }

        private async Task ModifyToDo()
        {
            IsLoading = Visibility.Visible;

            if (App.isInOfflineMode)
            {
                MyToDos.ToList().Find(sche =>
                {
                    if (sche.ID == NewToDo.ID) return true;
                    else return false;
                }).Content = NewToDo.Content;

                await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(MyToDos, "myschedules.sch", true);

                Messenger.Default.Send(new GenericMessage<string>(""), "RemoveScheduleUI");

                NewToDo = new ToDo();

                Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), "UpdateTile");

                return;

            }
            else
            {
                var resultUpdate = await PostHelper.UpdateContent(this.NewToDo.ID, this.NewToDo.Content);
                if (resultUpdate)
                {
                    MyToDos.ToList().Find(sche =>
                    {
                        if (sche.ID == NewToDo.ID) return true;
                        else return false;
                    }).Content = NewToDo.Content;

                    Messenger.Default.Send(new GenericMessage<string>(""), "RemoveScheduleUI");
                    NewToDo = new ToDo();

                    Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), "UpdateTile");
                }

            }

            IsLoading = Visibility.Collapsed;
        }


        /// <summary>
        /// 删除todo
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        /// 先从列表删除，然后把列表内容都序列化保存，接着：
        /// 1.如果已经登陆的，尝试发送请求；
        /// 2.离线模式，不用管
        private async Task DeleteToDo(string id)
        {
            try
            {
                var idTodelete = MyToDos.ToList().FindIndex(sch =>
                {
                    if (sch.ID == id) return true;
                    else return false;
                });


                DeletedToDos.Add(MyToDos.ElementAt(idTodelete));
                await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(DeletedToDos, "deleteditems.sch", true);


                if (idTodelete != -1)
                {
                    MyToDos.RemoveAt(idTodelete);
                    await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(MyToDos, "myschedules.sch", true);
                }


                if (!App.isInOfflineMode)
                {
                    var result = await PostHelper.DeleteSchedule(id);
                    await PostHelper.SetMyOrder(LocalSettingHelper.GetValue("sid"), ToDo.GetCurrentOrderString(MyToDos));
                }

                Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), "UpdateTile");

            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
            }
        }

        private async Task CheckTodoDone(string id)
        {
            try
            {
                var currentMemo = MyToDos.ElementAt(MyToDos.ToList().FindIndex(sch =>
                {
                    if (sch.ID == id) return true;
                    else return false;
                }));
                currentMemo.IsDone = !currentMemo.IsDone;

                await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(MyToDos, "myschedules.sch", true);

                if (!App.isInOfflineMode)
                {
                    var isDone = await PostHelper.FinishSchedule(id, currentMemo.IsDone ? "1" : "0");
                    if (isDone)
                    {
                        await PostHelper.SetMyOrder(LocalSettingHelper.GetValue("sid"), ToDo.GetCurrentOrderString(MyToDos));

                        Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), "UpdateTile");
                    }
                }
                else Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), "UpdateTile");
            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
            }

        }

        private async Task SyncAllToDos()
        {
            try
            {
                if (App.isNoNetwork)
                {
                    Messenger.Default.Send(new GenericMessage<string>("NoNetwork"), MessengerToken.ToastToken);
                    return;
                }

                IsLoading = Visibility.Visible;
                Messenger.Default.Send(new GenericMessage<string>(""), "SyncStoryBegin");

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(3.2);
                timer.Tick += ((sendert, et) =>
                {
                    IsLoading = Visibility.Collapsed;
                });
                timer.Start();

                var result = await PostHelper.GetMySchedules(LocalSettingHelper.GetValue("sid"));
                if (!string.IsNullOrEmpty(result))
                {
                    //get all the memos
                    var scheduleWithoutOrder = ToDo.ParseJsonToObs(result);

                    //get the order of the list items
                    var orders = await PostHelper.GetMyOrder(LocalSettingHelper.GetValue("sid"));

                    MyToDos = ToDo.SetOrderByString(scheduleWithoutOrder, orders);

                    int cate = 0;
                    foreach (var item in MyToDos)
                    {
                        if (cate == 5) cate++;
                        item.Category = cate;
                        cate++;
                    }

                    await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(MyToDos, "myschedules.sch", true);
                }
                Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), "UpdateTile");
            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
            }
        }

        private async Task AddAllToDos()
        {
            foreach (var sche in MyToDos)
            {
                var result = await PostHelper.AddSchedule(LocalSettingHelper.GetValue("sid"), sche.Content, sche.IsDone ? "1" : "0");
            }
        }
        public void Activate(object param)
        {
            
        }

        public void Deactivate(object param)
        {
           
        }
    }
}
