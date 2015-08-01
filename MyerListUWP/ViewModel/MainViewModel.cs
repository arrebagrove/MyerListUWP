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
#if WINDOWS_PHONE_APP
#endif


namespace MyerList.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {
        private bool isInModify = false;
        private bool fromOfflineToOnline = false;
        private bool fromNewUser = false;
        private bool isInReorder = false;

        private bool LOAD_ONCE = false;

        #region Propterty

        /// <summary>
        /// Add or Modify
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

        private ScheduleUser _currentUser;
        public ScheduleUser CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                if(_currentUser!=value)
                {
                    _currentUser = value;
                    RaisePropertyChanged(() => CurrentUser);
                }
            }
        }

        /// <summary>
        /// No network visibility
        /// </summary>
        private Visibility _nonetwork;
        public Visibility NoNetwork
        {
            get
            {
                return _nonetwork;
            }
            set
            {
                _nonetwork = value;
                RaisePropertyChanged(() => NoNetwork);
                if (value == Visibility.Visible)
                {
                    ShowNoItems = Visibility.Collapsed;
                }

            }
        }

        /// <summary>
        /// Decide whether App Bar shows or not
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

        private Visibility _showAccountInfoVisibility;
        public Visibility ShowAccountInfoVisibility
        {
            get
            {
                return _showAccountInfoVisibility;
            }
            set
            {
                if(_showAccountInfoVisibility!=value)
                {
                    _showAccountInfoVisibility = value;
                    RaisePropertyChanged(() => ShowAccountInfoVisibility);
                }
            }
        }

        /// <summary>
        /// For Progress Bar
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
        /// Show no items
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

        private Visibility _noDeletedItemsVisibility;
        public Visibility NoDeletedItemsVisibility
        {
            get
            {
               return _noDeletedItemsVisibility;
            }
            set
            {
                if(_noDeletedItemsVisibility!=value)
                {
                    _noDeletedItemsVisibility = value;
                    RaisePropertyChanged(() => NoDeletedItemsVisibility);
                }
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
        #endregion

        #region Data

        private Schedule _newMemo;
        public Schedule NewMemo
        {
            get
            {
                return _newMemo;
            }
            set
            {
                if (_newMemo != value)
                {
                    _newMemo = value;
                    RaisePropertyChanged(() => NewMemo);
                }

            }
        }

        private ObservableCollection<Schedule> _mySchedules;
        public ObservableCollection<Schedule> MySchedules
        {
            get
            {
                if (_mySchedules != null)
                {
                    if (_mySchedules.Count == 0) ShowNoItems = Visibility.Visible;
                    else ShowNoItems = Visibility.Collapsed;
                    return _mySchedules;
                }
                return _mySchedules = new ObservableCollection<Schedule>();
            }
            set
            {
                if (_mySchedules != value)
                {
                    _mySchedules = value;
                }
                RaisePropertyChanged(() => MySchedules);
            }
        }

        private ObservableCollection<Schedule> _deletedSchedules;
        public ObservableCollection<Schedule> DeletedSchedules
        {
            get
            {
                if(_deletedSchedules!=null)
                {
                    if (_deletedSchedules.Count == 0) NoDeletedItemsVisibility = Visibility.Visible;
                    else NoDeletedItemsVisibility = Visibility.Collapsed;
                    return _deletedSchedules;
                }
                return _deletedSchedules = new ObservableCollection<Schedule>();
            }
            set
            {
                if(_deletedSchedules!=value)
                {
                    _deletedSchedules = value;
                    RaisePropertyChanged(() => DeletedSchedules);
                }
            }
        }

        private ObservableCollection<SendingItem> _stageSchedules;
        public ObservableCollection<SendingItem> StageSchedules
        {
            get
            {
                if (_stageSchedules != null)
                {
                    return _stageSchedules;
                }
                return _stageSchedules = new ObservableCollection<SendingItem>();
            }
            set
            {
                if (_stageSchedules != value)
                {
                    _stageSchedules = value;
                    RaisePropertyChanged(() => StageSchedules);
                }
            }
        }

        #endregion

        #region Command



        private RelayCommand _reorderCommand;
        public RelayCommand ReorderCommand
        {
            get
            {
                if (_reorderCommand != null)
                {
                    return _reorderCommand;
                }
                return _reorderCommand = new RelayCommand(() =>
                {
#if WINDOWS_PHONE_APP
                    if (ReorderMode == ListViewReorderMode.Enabled)
                    {
                        ReorderMode = ListViewReorderMode.Disabled;
                        Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(""), "OutReorder");
                    }
                    else
                    {
                        ReorderMode = ListViewReorderMode.Enabled;

                    }
#endif
                });
            }
        }

        /// <summary>
        /// Pop out the add memo content dialog
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
                    Messenger.Default.Send(new GenericMessage<string>(""), "AddScheduleUI");

                    var loader = new ResourceLoader();
                    ModeTitle = loader.GetString("AddTitle");

                    isInModify = false;
                });
            }
        }

        /// <summary>
        /// Delete items
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

                return _deleteCommand = new RelayCommand<object>(async(param) =>
                {
                    string id = (string)param;
                    await DeleteItem(id);
                });
            }
        }

        /// <summary>
        /// Make the memo done
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
                    await SetDone(id);
                });
            }
        }

        /// <summary>
        /// Sync the list
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
                    await Sync();
                });
            }
        }

        /// <summary>
        /// Add a memo and post to server
        /// </summary>
        private RelayCommand _okCommand;
        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand=new RelayCommand(async () =>
                {
                    await AddOrModifyItem();
                });
            }
        }

        /// <summary>
        /// When press the item in the list, trigger the modify mode
        /// </summary>
        private RelayCommand<object> _modifyCommand;
        public RelayCommand<object> ModifyCommand
        {
            get
            {
                if (_modifyCommand != null)
                {
                    return _modifyCommand;
                }

                return _modifyCommand = new RelayCommand<object>((ID) =>
                {
                    try
                    {
                        if (isInReorder) return;

                        var id = (string)ID;
                        var scheduleToModify = MySchedules.ToList().Find(sche =>
                        {
                            if (sche.ID == id) return true;
                            else return false;
                        });

                        this.NewMemo.ID = scheduleToModify.ID;
                        this.NewMemo.Content = scheduleToModify.Content;

                        if (scheduleToModify == null)
                        {
                            return;
                        }
                        Messenger.Default.Send(new GenericMessage<string>(""), "Modify");

                        var loader = new ResourceLoader();
                        ModeTitle = loader.GetString("ModifyTitle");

                        isInModify = true;
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
                    NewMemo = new Schedule();
                });
            }
        }

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
                      LOAD_ONCE = false;
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

                        MySchedules.Clear();
                        DeletedSchedules.Clear();

                        LOAD_ONCE = false;

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


        /// <summary>
        /// Go to deleted items page
        /// </summary>
        private RelayCommand _recycleCommand;
        public RelayCommand RecycleCommand
        {
            get
            {
                if (_recycleCommand != null) return _recycleCommand;
                return _recycleCommand = new RelayCommand(async () =>
                {
#if WINDOWS_APP
                      RecycleFlyout flyout = new RecycleFlyout();
                      flyout.ShowIndependent();
#else
                    await Task.Delay(30);
                    var rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(RecyclePage));
#endif
                });
            }
            set
            {
                if (_recycleCommand != value)
                {
                    _recycleCommand = value;
                    RaisePropertyChanged(() => RecycleCommand);
                }
            }
        }

        #endregion

       
        #region Deleted involved
        /// <summary>
        /// Readd the item into the list
        /// </summary>
        private RelayCommand<string> _redoCommand;
        public RelayCommand<string> RedoCommand
        {
            get
            {
                if (_redoCommand != null) return _redoCommand;
                return _redoCommand = new RelayCommand<string>(async(id) =>
                  {
                      var scheToAdd = DeletedSchedules.ToList().Find(s =>
                        {
                            if (s.ID == id) return true;
                            else return false;
                        });
                      this.NewMemo = scheToAdd;
                      OkCommand.Execute(false);
                      DeletedSchedules.Remove(scheToAdd);
                      await SerializerHelper.SerializerToJson<ObservableCollection<Schedule>>(DeletedSchedules, "deleteditems.sch", true);

                  });
            }
            set
            {
                if(_redoCommand!=value)
                {
                    _redoCommand = value;
                    RaisePropertyChanged(() => RedoCommand);
                }
            }
        }

        /// <summary>
        /// Delete the items permanently
        /// </summary>
        private RelayCommand<string> _permanentDeleteCommand;
        public RelayCommand<string> PermanentDeleteCommand
        {
            get
            {
                if (_permanentDeleteCommand != null) return _permanentDeleteCommand;
                return _permanentDeleteCommand = new RelayCommand<string>(async(id) =>
                  {
                      DeletedSchedules.Remove(DeletedSchedules.ToList().Find(s =>
                      {
                          if (s.ID == id) return true;
                          else return false;
                      }));
                      await SerializerHelper.SerializerToJson<ObservableCollection<Schedule>>(DeletedSchedules, "deleteditems.sch", true);

                  });
            }
            set
            {
                if(_permanentDeleteCommand!=value)
                {
                    _permanentDeleteCommand = value;
                    RaisePropertyChanged(() => PermanentDeleteCommand);
                }
            }
        }
        #endregion

        #endregion

        public MainViewModel()
        {
            IsLoading = Visibility.Collapsed;
            NoDeletedItemsVisibility = Visibility.Collapsed;
            NoNetwork = Visibility.Collapsed;

            //var loader = ResourceLoader.GetForCurrentView();

            CurrentUser = new ScheduleUser();
            //CurrentUser.Email = loader.GetString("EmptyUserHint");

#if WINDOWS_PHONE_APP
            ReorderMode = ListViewReorderMode.Disabled;
#endif

            MySchedules = new ObservableCollection<Schedule>();
            DeletedSchedules=new ObservableCollection<Schedule>();
            StageSchedules = new ObservableCollection<SendingItem>();
            NewMemo = new Schedule();

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

            Messenger.Default.Register<GenericMessage<string>>(this, "FromVoice", async act =>
                {

                    await Task.Delay(50);

                    this.NewMemo = new Schedule();
                    this.NewMemo.Content = act.Content;

                    OkCommand.Execute(null);
                });

            Messenger.Default.Register<GenericMessage<string>>(this, "ClearNote", act =>
                {
                    this.NewMemo = new Schedule();
                });
            Messenger.Default.Register<GenericMessage<string>>(this, "PressEnter", act =>
             {
                 if (!string.IsNullOrEmpty(act.Content))
                 {
                     NewMemo.Content = act.Content;
                     OkCommand.Execute(null);
                 }
             });
        }

        private async Task AddOrModifyItem()
        {
            try
            {
                if (NewMemo != null)
                {
                    IsLoading = Visibility.Visible;

                    Messenger.Default.Send(new GenericMessage<string>(""), "SyncStoryBegin");

                    //添加
                    if (!isInModify)
                    {
                        await AddItem();
                    }
                    //修改
                    else
                    {
                        await ModifyItem();
                    }
                }
            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
            }
        }

        private async Task AddItem()
        {
            //离线模式
            if (App.isInOfflineMode || App.isNoNetwork)
            {
                //Schedule scheduletoadd = new Schedule();
                //scheduletoadd.Content = NewMemo.Content;
                //scheduletoadd.ID = Guid.NewGuid().ToString();
                //scheduletoadd.IsDone = false;
                NewMemo.ID = Guid.NewGuid().ToString();

                if (LocalSettingHelper.GetValue("AddMode") == "0")
                {
                    MySchedules.Insert(0, NewMemo);
                }
                else
                {
                    MySchedules.Add(NewMemo);
                }
                await SerializerHelper.SerializerToJson<ObservableCollection<Schedule>>(MySchedules, SerializerFileNames.ToDoFileName);

                Messenger.Default.Send(new GenericMessage<string>(""), "RemoveScheduleUI");
                Messenger.Default.Send(new GenericMessage<ObservableCollection<Schedule>>(MySchedules), "UpdateTile");

                NewMemo = new Schedule();

                IsLoading = Visibility.Collapsed;

                return;
            }
            if (App.isNoNetwork)
            {
                //TO DO: Store the schedule in SendingQueue
                return;
            }
            //在线模式
            var result = await PostHelper.AddSchedule(LocalSettingHelper.GetValue("sid"), NewMemo.Content, "0");
            if (!String.IsNullOrEmpty(result))
            {
                Schedule newSchedule = Schedule.ParseJsonTo(result);

                if (LocalSettingHelper.GetValue("AddMode") == "0")
                {
                    MySchedules.Insert(0, newSchedule);
                }
                else
                {
                    MySchedules.Add(newSchedule);
                }

                await PostHelper.SetMyOrder(LocalSettingHelper.GetValue("sid"), Schedule.GetCurrentOrderString(MySchedules));

                Messenger.Default.Send(new GenericMessage<string>(""), "RemoveScheduleUI");
                NewMemo = new Schedule();

                Messenger.Default.Send(new GenericMessage<ObservableCollection<Schedule>>(MySchedules), "UpdateTile");
                IsLoading = Visibility.Collapsed;

                await SerializerHelper.SerializerToJson<ObservableCollection<Schedule>>(MySchedules, SerializerFileNames.ToDoFileName);

            }
        }

        private async Task ModifyItem()
        {
            IsLoading = Visibility.Visible;

            if (App.isInOfflineMode)
            {
                MySchedules.ToList().Find(sche =>
                {
                    if (sche.ID == NewMemo.ID) return true;
                    else return false;
                }).Content = NewMemo.Content;

                await SerializerHelper.SerializerToJson<ObservableCollection<Schedule>>(MySchedules, "myschedules.sch", true);

                Messenger.Default.Send(new GenericMessage<string>(""), "RemoveScheduleUI");
                NewMemo = new Schedule();

                Messenger.Default.Send(new GenericMessage<ObservableCollection<Schedule>>(MySchedules), "UpdateTile");
                IsLoading = Visibility.Collapsed;

                return;

            }

            var resultUpdate = await PostHelper.UpdateContent(this.NewMemo.ID, this.NewMemo.Content);
            if (resultUpdate)
            {
                MySchedules.ToList().Find(sche =>
                {
                    if (sche.ID == NewMemo.ID) return true;
                    else return false;
                }).Content = NewMemo.Content;

                Messenger.Default.Send(new GenericMessage<string>(""), "RemoveScheduleUI");
                NewMemo = new Schedule();

                Messenger.Default.Send(new GenericMessage<ObservableCollection<Schedule>>(MySchedules), "UpdateTile");
                IsLoading = Visibility.Collapsed;
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
        private async Task DeleteItem(string id)
        {
            try
            {
                var idTodelete = MySchedules.ToList().FindIndex(sch =>
                {
                    if (sch.ID == id) return true;
                    else return false;
                });


                DeletedSchedules.Add(MySchedules.ElementAt(idTodelete));
                await SerializerHelper.SerializerToJson<ObservableCollection<Schedule>>(DeletedSchedules, "deleteditems.sch", true);


                if (idTodelete != -1)
                {
                    MySchedules.RemoveAt(idTodelete);
                    await SerializerHelper.SerializerToJson<ObservableCollection<Schedule>>(MySchedules, "myschedules.sch", true);
                }


                if (!App.isInOfflineMode)
                {
                    //Post update to server
                    var result = await PostHelper.DeleteSchedule(id);

                    //Update order
                    await PostHelper.SetMyOrder(LocalSettingHelper.GetValue("sid"), Schedule.GetCurrentOrderString(MySchedules));
                }

                Messenger.Default.Send(new GenericMessage<ObservableCollection<Schedule>>(MySchedules), "UpdateTile");

            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
            }

        }

        private async Task SetDone(string id)
        {
            try
            {
                var currentSche = MySchedules.ElementAt(MySchedules.ToList().FindIndex(sch =>
                {
                    if (sch.ID == id) return true;
                    else return false;
                }));
                currentSche.IsDone = !currentSche.IsDone;
                await SerializerHelper.SerializerToJson<ObservableCollection<Schedule>>(MySchedules, "myschedules.sch", true);


                if (!App.isInOfflineMode)
                {
                    var result = await PostHelper.FinishSchedule(id, currentSche.IsDone ? "1" : "0");
                    if (result)
                    {
                        await PostHelper.SetMyOrder(LocalSettingHelper.GetValue("sid"), Schedule.GetCurrentOrderString(MySchedules));

                        Messenger.Default.Send(new GenericMessage<ObservableCollection<Schedule>>(MySchedules), "UpdateTile");
                    }
                    return;
                }
                Messenger.Default.Send(new GenericMessage<ObservableCollection<Schedule>>(MySchedules), "UpdateTile");
            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
            }

        }

        private async Task Sync()
        {
            try
            {
                //Offline mode
                if (App.isInOfflineMode)
                {
                    var loader = new ResourceLoader();

                    MessageDialog md = new MessageDialog(loader.GetString("SignUpContent"), loader.GetString("Notice"));
                    md.Commands.Add(new UICommand(loader.GetString("Ok"), act =>
                    {
                        Frame rootFrame = Window.Current.Content as Frame;
                        rootFrame.Navigate(typeof(StartPage));
                        fromOfflineToOnline = true;
                    }));
                    md.Commands.Add(new UICommand(loader.GetString("Cancel"), act =>
                    {
                        return;
                    }));
                    await md.ShowAsync();
                    return;
                }
                else if(App.isNoNetwork)
                {
                    Messenger.Default.Send(new GenericMessage<string>("NoNetwork"), MessengerToken.ToastToken);
                    return;
                }
                
                IsLoading = Visibility.Visible;
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(""), "SyncStoryBegin");
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
                    var scheduleWithoutOrder = Schedule.ParseJsonToObs(result);

                    //get the order of the list items
                    var orders = await PostHelper.GetMyOrder(LocalSettingHelper.GetValue("sid"));

                    MySchedules = Schedule.SetOrderByString(scheduleWithoutOrder, orders);

                    await SerializerHelper.SerializerToJson<ObservableCollection<Schedule>>(MySchedules, "myschedules.sch", true);
                }
                Messenger.Default.Send(new GenericMessage<ObservableCollection<Schedule>>(MySchedules), "UpdateTile");
            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
            }

        }

        private async Task<bool> AddAllSchedules()
        {
            foreach (var sche in MySchedules)
            {
                var result = await PostHelper.AddSchedule(LocalSettingHelper.GetValue("sid"), sche.Content, sche.IsDone ? "1" : "0");
            }

            return true;
        }

        /// <summary>
        /// 从储存反序列化所有数据
        /// </summary>
        /// <returns></returns>
        private async Task RestoreData()
        {
            //whatever deserialize the delted items from file
            DeletedSchedules = await SerializerHelper.DeserializerFromJsonByFileName<ObservableCollection<Schedule>>(SerializerFileNames.DeletedFileName);
            MySchedules = await SerializerHelper.DeserializerFromJsonByFileName<ObservableCollection<Schedule>>(SerializerFileNames.ToDoFileName);
            StageSchedules = await SerializerHelper.DeserializerFromJsonByFileName<ObservableCollection<SendingItem>>(SerializerFileNames.StageFileName);
            LOAD_ONCE = true;
        }

        /// <summary>
        /// 进入 MainPage 会调用
        /// </summary>
        /// <param name="param"></param>
        public void Activate(object param)
        {
            if(param is LoginMode)
            {
                if (LOAD_ONCE)
                {
                    return;
                }

                var mode = (LoginMode)param;
                switch(mode)
                {
                    //已经登陆过的了
                    case LoginMode.Login:
                        {
                            //没有网络
                            if(!NetworkHelper.HasNetWork())
                            {
                                Messenger.Default.Send(new GenericMessage<string>(ResourcesHelper.GetString("NoNetworkHint")), MessengerToken.ToastToken);
                                App.isNoNetwork = true;

                                var restoreTask = RestoreData();
                               
                            }
                            //有网络
                            else
                            {
                                Messenger.Default.Send(new GenericMessage<string>(ResourcesHelper.GetString("WelcomeBackHint")), MessengerToken.ToastToken);
                                App.isNoNetwork = false;

                                CurrentUser.Email = LocalSettingHelper.GetValue("email");
                                ShowLoginBtnVisibility = Visibility.Collapsed;
                                ShowAccountInfoVisibility = Visibility.Visible;
                                SyncCommand.Execute(null);

                                LOAD_ONCE = true;
                            }
                        };break;
                    //处于离线模式
                    case LoginMode.OfflineMode:
                        {
                            ShowLoginBtnVisibility = Visibility.Visible;
                            ShowAccountInfoVisibility = Visibility.Collapsed;
                            App.isNoNetwork = false;

                            var restoreTask = RestoreData();
                        };break;
                }

                
            }
            
        }

        public void Deactivate(object param)
        {
           
        }
    }
}
