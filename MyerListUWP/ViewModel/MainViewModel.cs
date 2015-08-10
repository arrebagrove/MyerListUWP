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
#if WINDOWS_PHONE_APP
#endif


namespace MyerList.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {

        private bool isInReorder = false;

        private AddMode _addMode = AddMode.None;

        #region UI PROPERTY
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
                                Title = "待办事项";
                            }; break;
                        case 1:
                            {
                                Title = "已删除";
                            }; break;
                        case 2:
                            {
                                Title = "标签";
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

        private RelayCommand _selectCateCommand;
        public RelayCommand SelectCateCommand
        {
            get
            {
                if (_selectCateCommand != null) return _selectCateCommand;
                return _selectCateCommand = new RelayCommand(() =>
                 {
                     SelectedIndex = 2;
                 });
            }
        }

        #endregion

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

#if WINDOWS_PHONE_APP
        /// <summary>
        /// For Windows Phone- reorder mode
        /// </summary>
        private ListViewReorderMode _reorderMode;
        public ListViewReorderMode ReorderMode
        {
            get
            {
                return _reorderMode;
            }
            set
            {
                if (_reorderMode != value)
                {
                    _reorderMode = value;

                    if (value == ListViewReorderMode.Enabled)
                    {
                        isInReorder = true;
                        Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(""), "InReorder");
                    }

                    else
                    {
                        isInReorder = false;
                        Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(""), "OutReorder");
                    }

                    var task=PostHelper.SetMyOrder(LocalSettingHelper.GetValue("sid"), Schedule.GetCurrentOrderString(MySchedules));
                    Messenger.Default.Send(new GenericMessage<ObservableCollection<Schedule>>(MySchedules), "UpdateTile");

                    RaisePropertyChanged(() => ReorderMode);

                }
            }
        }

#endif

        #region Data

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

                    Messenger.Default.Send(new GenericMessage<string>(loader.GetString("AddTitle")), "SetTitle");

                    NewToDo = new ToDo();

                    _addMode = AddMode.Add;
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

        #endregion

        #region Command

        //        private RelayCommand _reorderCommand;
        //        public RelayCommand ReorderCommand
        //        {
        //            get
        //            {
        //                if (_reorderCommand != null)
        //                {
        //                    return _reorderCommand;
        //                }
        //                return _reorderCommand = new RelayCommand(() =>
        //                {
        //#if WINDOWS_PHONE_APP
        //                    if (ReorderMode == ListViewReorderMode.Enabled)
        //                    {
        //                        ReorderMode = ListViewReorderMode.Disabled;
        //                        Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(""), "OutReorder");
        //                    }
        //                    else
        //                    {
        //                        ReorderMode = ListViewReorderMode.Enabled;

        //                    }
        //#endif
        //                });
        //            }
        //        }



        private RelayCommand _retryCommand;
        public RelayCommand RetryCommand
        {
            get
            {
                if (_retryCommand != null)
                {
                    return _retryCommand;
                }
                return _retryCommand = new RelayCommand(() =>
                {
                    //await Login();
                });
            }
        }


        #region Menu Command

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
        /// Go to about
        /// </summary>
        private RelayCommand _aboutCommand;
        public RelayCommand AboutCommand
        {
            get
            {
                if (_aboutCommand != null)
                {
                    return _aboutCommand;
                }
                return _aboutCommand = new RelayCommand(async () =>
                {
                    await Task.Delay(50);
                    Frame frame = Window.Current.Content as Frame;
                    if (frame != null) frame.Navigate(typeof(AboutPage));
                });
            }
        }

        /// <summary>
        /// Go to setting
        /// </summary>
        private RelayCommand _settingCommand;
        public RelayCommand SettingCommand
        {
            get
            {
                if (_settingCommand != null)
                {
                    return _settingCommand;
                }
                return _settingCommand = new RelayCommand(async () =>
                {
                    await Task.Delay(50);
                    Frame frame = Window.Current.Content as Frame;
                    if (frame != null) frame.Navigate(typeof(SettingPage));
                });
            }
        }

        private RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand
        {
            get
            {
                if (_logoutCommand != null)
                {
                    return _logoutCommand;
                }

                return _logoutCommand = new RelayCommand(async () =>
                {
                    var loader = new ResourceLoader();

                    MessageDialog md = new MessageDialog(loader.GetString("LogoutContent"), loader.GetString("Notice"));
                    md.Commands.Add(new UICommand(loader.GetString("Ok"), act =>
                    {
                        LocalSettingHelper.CleanUpAll();

                        Frame rootFrame = Window.Current.Content as Frame;
                        if (rootFrame != null) rootFrame.Navigate(typeof(StartPage));
                    }));
                    md.Commands.Add(new UICommand(loader.GetString("Cancel"), act =>
                    {

                    }));
                    await md.ShowAsync();

                });
            }
        }

        #endregion



        #endregion

        public MainViewModel()
        {
            IsLoading = Visibility.Collapsed;
            NewToDo = new ToDo();

            CurrentUser = new MyerListUser();

            SelectedIndex = 0;
            Title = "待办事项";

            Messenger.Default.Register<GenericMessage<string>>(this,"SetLoading", msg =>
             {
                 IsLoading = Visibility.Visible;
             });
            Messenger.Default.Register<GenericMessage<string>>(this, "UnSetLoading", msg =>
              {
                  IsLoading = Visibility.Collapsed;
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
                    Messenger.Default.Send(new GenericMessage<string>(""), "SetLoading");

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

                Messenger.Default.Send(new GenericMessage<string>(""), "UnSetLoading");


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
                    Messenger.Default.Send(new GenericMessage<string>(""), "UnSetLoading");


                    await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(MyToDos, SerializerFileNames.ToDoFileName);

                }
            }

        }

        private async Task ModifyToDo()
        {
            Messenger.Default.Send(new GenericMessage<string>(""), "SetLoading");

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
            Messenger.Default.Send(new GenericMessage<string>(""), "UnSetLoading");

        }



        /// <summary>
        /// 进入 MainPage 会调用
        /// </summary>
        /// <param name="param"></param>
        public void Activate(object param)
        {
            if (param is LoginMode)
            {
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
                            }
                            //有网络
                            else
                            {
                                Messenger.Default.Send(new GenericMessage<string>(ResourcesHelper.GetString("WelcomeBackHint")), MessengerToken.ToastToken);
                                App.isNoNetwork = false;

                                CurrentUser.Email = LocalSettingHelper.GetValue("email");
                                ShowLoginBtnVisibility = Visibility.Collapsed;
                                ShowAccountInfoVisibility = Visibility.Visible;

                            }
                        }; break;
                    //处于离线模式
                    case LoginMode.OfflineMode:
                        {
                            ShowLoginBtnVisibility = Visibility.Visible;
                            ShowAccountInfoVisibility = Visibility.Collapsed;
                            App.isNoNetwork = false;

                        }; break;
                }
            }
        }

        public void Deactivate(object param)
        {

        }
    }
}
