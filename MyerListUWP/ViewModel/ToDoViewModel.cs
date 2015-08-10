using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JP.Utils.Data;
using JP.Utils.Debug;
using JP.Utils.Framework;
using JP.Utils.Network;
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
        private bool LOAD_ONCE = false;

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
                        Messenger.Default.Send(new GenericMessage<string>(loader.GetString("ModifyTitle")), "SetTitle");

                        _addMode = AddMode.Modify;
                    }
                    catch (Exception e)
                    {
                        var task = ExceptionHelper.WriteRecord(e);
                    }
                });
            }
        }

        
        public ToDoViewModel()
        {
            MyToDos = new ObservableCollection<ToDo>();

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

                Messenger.Default.Send(new GenericMessage<ToDo>(MyToDos.ElementAt(idTodelete)),"Delete");
                

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
                Messenger.Default.Send(new GenericMessage<string>(""), "SetLoading");

                Messenger.Default.Send(new GenericMessage<string>(""), "SyncStoryBegin");

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(3.2);
                timer.Tick += ((sendert, et) =>
                {
                    Messenger.Default.Send(new GenericMessage<string>(""), "UnSetLoading");
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

        /// <summary>
        /// 从储存反序列化所有数据
        /// </summary>
        /// <returns></returns>
        private async Task RestoreData(bool restoreMainList)
        {
            if (restoreMainList)
            {
                MyToDos = await SerializerHelper.DeserializerFromJsonByFileName<ObservableCollection<ToDo>>(SerializerFileNames.ToDoFileName);
                Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), "UpdateTile");
            }
            //var task1 = SerializerHelper.DeserializerFromJsonByFileName<ObservableCollection<ToDo>>(SerializerFileNames.DeletedFileName);
            //var tasj2 = await SerializerHelper.DeserializerFromJsonByFileName<ObservableCollection<SendingItem>>(SerializerFileNames.StageFileName);

            //DeletedToDos = await task1;

            LOAD_ONCE = true;
        }


        public void Activate(object param)
        {
            if (param is LoginMode)
            {
                if (LOAD_ONCE)
                {
                    return;
                }
                LOAD_ONCE = true;
                var mode = (LoginMode)param;
                switch (mode)
                {
                    //已经登陆过的了
                    case LoginMode.Login:
                        {
                            //没有网络
                            if (!NetworkHelper.HasNetWork)
                            {
                                Messenger.Default.Send(new GenericMessage<string>(ResourcesHelper.GetString("NoNetworkHint")), MessengerToken.ToastToken);
                                App.isNoNetwork = true;

                                var restoreTask = RestoreData(true);
                            }
                            //有网络
                            else
                            {
                                Messenger.Default.Send(new GenericMessage<string>(ResourcesHelper.GetString("WelcomeBackHint")), MessengerToken.ToastToken);
                                App.isNoNetwork = false;

                                SyncCommand.Execute(null);

                                var resotreTask = RestoreData(false);
                            }
                        }; break;
                    //处于离线模式
                    case LoginMode.OfflineMode:
                        {
                            App.isNoNetwork = false;

                            var restoreTask = RestoreData(true);
                        }; break;
                }
            }
        }

        public void Deactivate(object param)
        {
           
        }
    }
}
