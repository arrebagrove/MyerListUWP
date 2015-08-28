using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media;
using Windows.UI;
using MyerList.Model;
using MyerList.Helper;
using JP.Utils.Data;
using JP.Utils.Debug;
using MyerList.Interface;
using System.Collections.Generic;
using JP.Utils.Network;
using MyerListUWP;
using MyerListUWP.Model;
using DialogExt;
using MyerListUWP.Helper;
#if WINDOWS_PHONE_APP
#endif


namespace MyerList.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {

        private AddMode _addMode = AddMode.None;

        private bool LOAD_ONCE = false;

        #region NAVIGATION UI

        private int _selectedCate;
        public int SelectedCate
        {
            get
            {
                return _selectedCate;
            }
            set
            {
                if(_selectedCate!=value)
                {
                    _selectedCate = value;
                    RaisePropertyChanged(() => SelectedCate);
                    SelectCateCommand.Execute(value);
                }
            }
        }

        public Visibility ShowCategory
        {
            get
            {
                if (SelectedCate == 0) return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        private SolidColorBrush _cateColor;
        public SolidColorBrush CateColor
        {
            get
            {
                return _cateColor;
            }
            set
            {
                if (_cateColor != value)
                {
                    _cateColor = value;
                    RaisePropertyChanged(() => CateColor);
                }
            }
        }

        private SolidColorBrush _cateColorLight;
        public SolidColorBrush CateColorLight
        {
            get
            {
                return _cateColorLight;
            }
            set
            {
                if (_cateColorLight != value)
                {
                    _cateColorLight = value;
                    RaisePropertyChanged(() => CateColorLight);
                }
            }
        }

        private SolidColorBrush _cateColorDark;
        public SolidColorBrush CateColorDark
        {
            get
            {
                return _cateColorDark;
            }
            set
            {
                if (_cateColorDark != value)
                {
                    _cateColorDark = value;
                    RaisePropertyChanged(() => CateColorDark);
                }
            }
        }

        private RelayCommand<object> _selectCateCommand;
        public RelayCommand<object> SelectCateCommand
        {
            get
            {
                if (_selectCateCommand != null) return _selectCateCommand;
                return _selectCateCommand = new RelayCommand<object>((param) =>
                 {
                     //SelectedCate = (int)param;
                     ChangeDisplayCateList((int)param);
                 });
            }
        }


        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    RaisePropertyChanged(() => SelectedIndex);

                    switch (value)
                    {
                        case 0:
                            {
                                Title = ResourcesHelper.GetString("ToDoPivotItem");
                                DeleteIconAlpha =0.3;
                                TodoIconAlpha = 1;
                            }; break;
                        case 1:
                            {
                                Title = ResourcesHelper.GetString("DeletedPivotItem");
                                DeleteIconAlpha = 1;
                                TodoIconAlpha = 0.3;
                            }; break;
                    }
                }
            }
        }

        private RelayCommand _selectToDoCommand;
        public RelayCommand SelectToDoCommand
        {
            get
            {
                if (_selectToDoCommand != null) return _selectToDoCommand;
                return _selectToDoCommand = new RelayCommand(() =>
                 {
                     SelectedIndex = 0;
                 });
            }
        }

        private RelayCommand _selectDeleteCommand;
        public RelayCommand SelectDeleteCommand
        {
            get
            {
                if (_selectDeleteCommand != null) return _selectDeleteCommand;
                return _selectDeleteCommand = new RelayCommand(() =>
                 {
                     SelectedIndex = 1;
                 });
            }
        }

        private double _todoIconAlpha;
        public double TodoIconAlpha
        {
            get
            {
                return _todoIconAlpha;
            }
            set
            {
                if (_todoIconAlpha != value)
                {
                    _todoIconAlpha = value;
                    RaisePropertyChanged(() => TodoIconAlpha);
                }
            }
        }

        private double _deleteIconAlpha;
        public double DeleteIconAlpha
        {
            get
            {
                return _deleteIconAlpha;
            }
            set
            {
                if (_deleteIconAlpha != value)
                {
                    _deleteIconAlpha = value;
                    RaisePropertyChanged(() => DeleteIconAlpha);
                }
            }
        }
        #endregion


        /// <summary>
        ///是否显示要求登录按钮
        /// </summary>
        private Visibility _showLoginBtnVisibility;
        public Visibility ShowLoginBtnVisibility
        {
            get
            {
                return _showLoginBtnVisibility;
            }
            set
            {
                _showLoginBtnVisibility = value;
                RaisePropertyChanged(() => ShowLoginBtnVisibility);
            }
        }

        /// <summary>
        /// 是否显示账户信息
        /// </summary>
        private Visibility _showAccountInfoVisibility;
        public Visibility ShowAccountInfoVisibility
        {
            get
            {
                return _showAccountInfoVisibility;
            }
            set
            {
                if (_showAccountInfoVisibility != value)
                {
                    _showAccountInfoVisibility = value;
                    RaisePropertyChanged(() => ShowAccountInfoVisibility);
                }
            }
        }

        /// <summary>
        /// 是否加载中
        /// </summary>
        private Visibility _isLoading;
        public Visibility IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }


        /// <summary>
        /// 表示当前的用户
        /// </summary>
        private MyerListUser _currentUser;
        public MyerListUser CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    RaisePropertyChanged(() => CurrentUser);
                }
            }
        }

        /// <summary>
        /// 对话框显示的标题
        /// </summary>
        private string _modetitle;
        public string ModeTitle
        {
            get
            {
                return _modetitle;
            }
            set
            {
                if (_modetitle != value)
                    _modetitle = value;
                RaisePropertyChanged(() => ModeTitle);
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

        #region CommandBar

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
                    ModeTitle = ResourcesHelper.GetString("AddTitle");
                        
                    NewToDo = new ToDo();

                    _addMode = AddMode.Add;
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

        private RelayCommand _toggleReorderCommand;
        public RelayCommand ToggleReorderCommand
        {
            get
            {
                if (_toggleReorderCommand != null) return _toggleReorderCommand;
                return _toggleReorderCommand = new RelayCommand(() =>
                  {

                  });
            }
        }

        #endregion

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
        ///添加/修改待办事项时候的“取消”
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
                    Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.RemoveScheduleUI);
                    NewToDo = new ToDo();
                });
            }
        }


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

        private ObservableCollection<ToDo> _currentDisplayToDos;
        public ObservableCollection<ToDo> CurrrentDisplayToDos
        {
            get
            {
                return _currentDisplayToDos;
            }
            set
            {
                if (_currentDisplayToDos != value)
                {
                    _currentDisplayToDos = value;
                    RaisePropertyChanged(() => CurrrentDisplayToDos);
                }
            }
        }


        /// <summary>
        ///删除待办事项
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
                    await CompleteTodo(id);
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


                        Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.ShowModifyUI);

                        ModeTitle = ResourcesHelper.GetString("ModifyTitle");

                        _addMode = AddMode.Modify;
                    }
                    catch (Exception e)
                    {
                        var task = ExceptionHelper.WriteRecord(e);
                    }
                });
            }
        }

        private RelayCommand<object> _changeCateCommand;
        public RelayCommand<object> ChangeCateCommand
        {
            get
            {
                if (_changeCateCommand != null) return _changeCateCommand;
                return _changeCateCommand = new RelayCommand<object>(async(param) =>
                 {
                     var id = (string)param;
                     await ChangeCategory(id);
                 });
            }
        }


        #region Deleted Page
        /// <summary>
        /// 已经删除了的待办事项
        /// </summary>
        private ObservableCollection<ToDo> _deletedToDos;
        public ObservableCollection<ToDo> DeletedToDos
        {
            get
            {
                if (_deletedToDos != null)
                {
                    if (_deletedToDos.Count == 0) NoDeletedItemsVisibility = Visibility.Visible;
                    else NoDeletedItemsVisibility = Visibility.Collapsed;
                    return _deletedToDos;
                }
                return _deletedToDos = new ObservableCollection<ToDo>();
            }
            set
            {
                if (_deletedToDos != value)
                {
                    _deletedToDos = value;
                    RaisePropertyChanged(() => DeletedToDos);
                }
            }
        }

        /// <summary>
        /// 没有已经删除的内容
        /// </summary>
        private Visibility _noDeletedItemsVisibility;
        public Visibility NoDeletedItemsVisibility
        {
            get
            {
                return _noDeletedItemsVisibility;
            }
            set
            {
                if (_noDeletedItemsVisibility != value)
                {
                    _noDeletedItemsVisibility = value;
                    RaisePropertyChanged(() => NoDeletedItemsVisibility);
                }
            }
        }

        /// <summary>
        /// 重新添加回列表
        /// </summary>
        private RelayCommand<string> _redoCommand;
        public RelayCommand<string> RedoCommand
        {
            get
            {
                if (_redoCommand != null) return _redoCommand;
                return _redoCommand = new RelayCommand<string>(async (id) =>
                {
                    var scheToAdd = DeletedToDos.ToList().Find(s =>
                    {
                        if (s.ID == id) return true;
                        else return false;
                    });

                    _addMode = AddMode.None;
                    NewToDo = scheToAdd;
                    OkCommand.Execute(null);

                    DeletedToDos.Remove(scheToAdd);
                    await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(DeletedToDos, "deleteditems.sch", true);

                });
            }
            set
            {
                if (_redoCommand != value)
                {
                    _redoCommand = value;
                    RaisePropertyChanged(() => RedoCommand);
                }
            }
        }

        /// <summary>
        /// 永久删除
        /// </summary>
        private RelayCommand<string> _permanentDeleteCommand;
        public RelayCommand<string> PermanentDeleteCommand
        {
            get
            {
                if (_permanentDeleteCommand != null) return _permanentDeleteCommand;
                return _permanentDeleteCommand = new RelayCommand<string>(async (id) =>
                {
                    DeletedToDos.Remove(DeletedToDos.ToList().Find(s =>
                    {
                        if (s.ID == id) return true;
                        else return false;
                    }));
                    await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(DeletedToDos, "deleteditems.sch", true);

                });
            }
            set
            {
                if (_permanentDeleteCommand != value)
                {
                    _permanentDeleteCommand = value;
                    RaisePropertyChanged(() => PermanentDeleteCommand);
                }
            }
        }

        #endregion


        #region Hamburger menu

        private RelayCommand _toStartPageCommand;
        public RelayCommand ToStartPageCommand
        {
            get
            {
                if (_toStartPageCommand != null) return _toStartPageCommand;
                return _toStartPageCommand = new RelayCommand(() =>
                {
                    var rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(StartPage));
                });
            }
        }


        /// <summary>
        /// 跳到 About 页面
        /// </summary>
        private RelayCommand _goToAboutCommand;
        public RelayCommand GoToAboutCommand
        {
            get
            {
                if (_goToAboutCommand != null)
                {
                    return _goToAboutCommand;
                }
                return _goToAboutCommand = new RelayCommand(() =>
                {
                    Frame frame = Window.Current.Content as Frame;
                    if (frame != null) frame.Navigate(typeof(AboutPage));
                });
            }
        }

        /// <summary>
        /// 跳到 Settings 页面
        /// </summary>
        private RelayCommand _gotoSettingCommand;
        public RelayCommand GoToSettingCommand
        {
            get
            {
                if (_gotoSettingCommand != null)
                {
                    return _gotoSettingCommand;
                }
                return _gotoSettingCommand = new RelayCommand(() =>
                {
                    Frame frame = Window.Current.Content as Frame;
                    if (frame != null) frame.Navigate(typeof(SettingPage));
                });
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        private RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand
        {
            get
            {
                if (_logoutCommand != null)
                {
                    return _logoutCommand;
                }

                return _logoutCommand = new RelayCommand(async() =>
                {
                    Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.CloseHam);

                    ContentDialogEx cdex = new ContentDialogEx(ResourcesHelper.GetString("Notice"), ResourcesHelper.GetString("LogoutContent"));
                    cdex.OnLeftBtnClick += ((senderl, el) =>
                      {
                          LocalSettingHelper.CleanUpAll();

                          Frame rootFrame = Window.Current.Content as Frame;
                          if (rootFrame != null) rootFrame.Navigate(typeof(StartPage));
                      });
                    cdex.OnRightBtnClick += ((senderr, er) =>
                      {
                          cdex.Hide();
                      });
                    await cdex.ShowAsync();
                    //MessageDialog md = new MessageDialog(ResourcesHelper.GetString("LogoutContent"), ResourcesHelper.GetString("Notice"));
                    //md.Commands.Add(new UICommand(ResourcesHelper.GetString("Ok"), act =>
                    //{
                        
                    //}));
                    //md.Commands.Add(new UICommand(ResourcesHelper.GetString("Cancel"), act =>
                    //{

                    //}));
                    //await md.ShowAsync();

                });
            }
        }

        #endregion

        public MainViewModel()
        {
            IsLoading = Visibility.Collapsed;
            NoDeletedItemsVisibility = Visibility.Collapsed;

            //初始化
            NewToDo = new ToDo();
            CurrentUser = new MyerListUser();
            MyToDos = new ObservableCollection<ToDo>();
            DeletedToDos = new ObservableCollection<ToDo>();
            CurrrentDisplayToDos = MyToDos;

            SelectedCate = 0;

            CateColor = App.Current.Resources["DefaultColor"] as SolidColorBrush;
            CateColorLight= App.Current.Resources["DefaultColorLight"] as SolidColorBrush;
            CateColorDark = App.Current.Resources["DefaultColorDark"] as SolidColorBrush;

            //设置当前页面为 To-Do
            SelectedIndex = 0;
            TodoIconAlpha = 1;
            DeleteIconAlpha = 0.3;
            Title = ResourcesHelper.GetString("ToDoPivotItem");

            //按下Enter后
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.EnterToAdd, act =>
            {
                if (!string.IsNullOrEmpty(NewToDo.Content))
                {
                    OkCommand.Execute(null);
                }
            });

            //完成ToDo
            Messenger.Default.Register<GenericMessage<string>>(this, "Check", act =>
            {
                var id = act.Content;
                CheckCommand.Execute(id);
            });

            //删除To-Do
            Messenger.Default.Register<GenericMessage<string>>(this, "Delete", act =>
            {
                var id = act.Content;
                DeleteCommand.Execute(id);
            });

          
            
            Messenger.Default.Register<GenericMessage<ToDo>>(this, "Redo", act =>
            {
                this.NewToDo = act.Content;
                OkCommand.Execute(false);
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

                    if (_addMode != AddMode.None)
                    {
                        Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.RemoveScheduleUI);
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
            if (App.isInOfflineMode || App.IsNoNetwork)
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

                Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), MessengerToken.UpdateTile);

                NewToDo = new ToDo();

                IsLoading = Visibility.Collapsed;

            }
            else if (App.IsNoNetwork)
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

                    Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), MessengerToken.UpdateTile);

                    IsLoading = Visibility.Collapsed;


                    await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(MyToDos, SerializerFileNames.ToDoFileName);

                }
            }

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
                var itemToDeleted = MyToDos.ToList().Find(s =>
                  {
                      if (s.ID == id) return true;
                      else return false;
                  });

                DeletedToDos.Add(itemToDeleted);
                await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(DeletedToDos, "deleteditems.sch", true);

                if (itemToDeleted !=null)
                {
                    MyToDos.Remove(itemToDeleted);
                    await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(MyToDos, "myschedules.sch", true);
                }

                if (!App.isInOfflineMode)
                {
                    var result = await PostHelper.DeleteSchedule(id);
                    await PostHelper.SetMyOrder(LocalSettingHelper.GetValue("sid"), ToDo.GetCurrentOrderString(MyToDos));
                }

                Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), MessengerToken.UpdateTile);

            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
            }
        }

        /// <summary>
        /// 完成待办事项
        /// </summary>
        /// <param name="id">待办事项的ID</param>
        /// <returns></returns>
        private async Task CompleteTodo(string id)
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

                        Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), MessengerToken.UpdateTile);
                    }
                }
                else Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), MessengerToken.UpdateTile);
            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
            }

        }

        /// <summary>
        /// 从云端同步所有待办事项
        /// </summary>
        /// <returns></returns>
        private async Task SyncAllToDos()
        {
            try
            {
                //没网络
                if (App.IsNoNetwork)
                {
                    //通知没有网络
                    Messenger.Default.Send(new GenericMessage<string>(ResourcesHelper.GetString("NoNetworkHint")), MessengerToken.ToastToken);
                    return;
                }

                //加载滚动条
                IsLoading = Visibility.Visible;

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
                    //获得无序的待办事项
                    var scheduleWithoutOrder = ToDo.ParseJsonToObs(result);

                    //获得顺序列表
                    var orders = await PostHelper.GetMyOrder(LocalSettingHelper.GetValue("sid"));

                    //排序
                    MyToDos = ToDo.SetOrderByString(scheduleWithoutOrder, orders);

                    CurrrentDisplayToDos = MyToDos;

                    Messenger.Default.Send(new GenericMessage<string>(ResourcesHelper.GetString("SyncSuccessfully")), MessengerToken.ToastToken);

                    await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(MyToDos, "myschedules.sch", true);
                }


                //最后更新动态磁贴
                Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), MessengerToken.UpdateTile);
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
        /// 修改待办事项
        /// </summary>
        /// <returns></returns>
        private async Task ModifyToDo()
        {
            IsLoading = Visibility.Visible;

            //离线模式
            if (App.isInOfflineMode)
            {
                //修改当前列表
                MyToDos.ToList().Find(sche =>
                {
                    if (sche.ID == NewToDo.ID) return true;
                    else return false;
                }).Content = NewToDo.Content;

                await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(MyToDos, "myschedules.sch", true);

                Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.RemoveScheduleUI);

                NewToDo = new ToDo();

                Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), MessengerToken.UpdateTile);

                return;

            }
            //非离线模式
            else
            {
                var resultUpdate = await PostHelper.UpdateContent(NewToDo.ID, NewToDo.Content,NewToDo.Category);
                if (resultUpdate)
                {
                    MyToDos.ToList().Find(sche =>
                    {
                        if (sche.ID == NewToDo.ID) return true;
                        else return false;
                    }).Content = NewToDo.Content;

                    Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.RemoveScheduleUI);
                    NewToDo = new ToDo();

                    Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), MessengerToken.UpdateTile);
                }

            }
            IsLoading = Visibility.Collapsed;

        }

        private async Task ChangeCategory(string id)
        {
            var scheduleToChange = MyToDos.ToList().Find(s =>
              {
                  if (s.ID == id) return true;
                  else return false;
              });
            if(scheduleToChange!= null)
            {
                scheduleToChange.Category++;
                if(!App.IsNoNetwork && !App.isInOfflineMode) await PostHelper.UpdateContent(id, scheduleToChange.Content, scheduleToChange.Category);
            }
        }

        private void ChangeDisplayCateList(int id)
        {
            Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.CloseHam);

            var cateid = id;
            CateColor = App.Current.Resources[Enum.GetName(typeof(CateColors), cateid)] as SolidColorBrush;
            CateColorLight = App.Current.Resources[Enum.GetName(typeof(CateColors), cateid) + "Light"] as SolidColorBrush;
            CateColorDark = App.Current.Resources[Enum.GetName(typeof(CateColors) , cateid) + "Dark"] as SolidColorBrush;

            TitleBarHelper.SetUpCateTitleBar(Enum.GetName(typeof(CateColors), cateid));

            if (id != 0)
            {
                var newList = from e in MyToDos where e.Category == id select e;
                CurrrentDisplayToDos = new ObservableCollection<ToDo>();
                newList.ToList().ForEach(s => CurrrentDisplayToDos.Add(s));
            }
            else CurrrentDisplayToDos = MyToDos;

            switch (cateid)
            {
                case 0:
                    {
                        Title = ResourcesHelper.GetString("CateDefault");
                    }; break;
                case 1:
                    {
                        Title = ResourcesHelper.GetString("CateWork");

                    }; break;
                case 2:
                    {
                        Title = ResourcesHelper.GetString("CateLife");

                    }; break;
                case 3:
                    {
                        Title = ResourcesHelper.GetString("CateFamily");

                    }; break;
                case 4:
                    {
                        Title = ResourcesHelper.GetString("CateEnter");
                    }; break;
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
                Messenger.Default.Send(new GenericMessage<ObservableCollection<ToDo>>(MyToDos), MessengerToken.UpdateTile);
            }
            DeletedToDos =await SerializerHelper.DeserializerFromJsonByFileName<ObservableCollection<ToDo>>(SerializerFileNames.DeletedFileName);

            LOAD_ONCE = true;
        }


        /// <summary>
        /// 进入 MainPage 会调用
        /// </summary>
        /// <param name="param"></param>
        public async void Activate(object param)
        {
            TitleBarHelper.SetUpCateTitleBar(Enum.GetName(typeof(CateColors), SelectedCate));

            if (param is LoginMode)
            {
                if (LOAD_ONCE) return;
                LOAD_ONCE = true;

                var mode = (LoginMode)param;
                switch (mode)
                {
                    //已经登陆过的了
                    case LoginMode.Login:
                        {
                            CurrentUser.Email = LocalSettingHelper.GetValue("email");
                            ShowLoginBtnVisibility = Visibility.Collapsed;
                            ShowAccountInfoVisibility = Visibility.Visible;

                            //没有网络
                            if (App.IsNoNetwork)
                            {
                                await RestoreData(true);
                                await Task.Delay(500);
                                Messenger.Default.Send(new GenericMessage<string>(ResourcesHelper.GetString("NoNetworkHint")), MessengerToken.ToastToken);
                            }
                            //有网络
                            else
                            {
                                Messenger.Default.Send(new GenericMessage<string>(ResourcesHelper.GetString("WelcomeBackHint")), MessengerToken.ToastToken);
                                SyncCommand.Execute(null);
                                var resotreTask = RestoreData(false);
                            }
                        }; break;
                    //处于离线模式
                    case LoginMode.OfflineMode:
                        {
                            ShowLoginBtnVisibility = Visibility.Visible;
                            ShowAccountInfoVisibility = Visibility.Collapsed;
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
