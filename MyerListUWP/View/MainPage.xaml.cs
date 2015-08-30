using GalaSoft.MvvmLight.Messaging;
using JP.Utils.Data;
using MyerList.Base;
using MyerList.Helper;
using MyerList.ViewModel;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


namespace MyerListUWP.View
{

    public sealed partial class MainPage : BindablePage
    {
        public MainViewModel MainVM
        {
            get
            {
                return DataContext as MainViewModel;
            }
        }

        private bool _isDrawerSlided = false;
        private bool _isAddingPaneShowed = false;

        private double _pointOriX = 0;

        public MainPage()
        {
            this.InitializeComponent();

            Messenger.Default.Register<GenericMessage<string>>(this, MessengerTokens.ToastToken, msg =>
            {
                ToastControl.ShowMessage(msg.Content);
            });
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerTokens.CloseHam, msg =>
            {
                if(_isDrawerSlided)
                {
                    SlideOutStory.Begin();
                    HamburgerBtn.PlayHamOutStory();
                    _isDrawerSlided = false;
                }
            });
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerTokens.RemoveScheduleUI, msg =>
            {
                RemoveStory.Begin();
            });
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerTokens.ShowModifyUI, msg =>
            {
                AddingPane.Visibility = Visibility.Visible;
                AddStory.Begin();
            });
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerTokens.ChangeCommandBarToDelete, msg =>
              {
                  SwitchCommandBarToDelete.Begin();
              });
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerTokens.ChangeCommandBarToDefault, msg =>
            {
                SwitchCommandBarToDefault.Begin();
            });
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerTokens.GoToSort, act =>
            {
                DisplayedListView.CanDragItems = true;
                DisplayedListView.CanReorderItems = true;
                DisplayedListView.AllowDrop = true;
            });
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerTokens.LeaveSort, act =>
            {
                DisplayedListView.CanDragItems = false;
                DisplayedListView.CanReorderItems = false;
                DisplayedListView.AllowDrop = true;

            });
            RemoveStory.Completed += ((senderc, ec) =>
              {
                  _isAddingPaneShowed = false;
              });

            this.KeyDown += ((sender, e) =>
            {
                if (_isAddingPaneShowed && e.Key == Windows.System.VirtualKey.Enter && e.KeyStatus.RepeatCount == 1)
                {
                    Messenger.Default.Send(new GenericMessage<string>(""), MessengerTokens.EnterToAdd);
                    RemoveStory.Begin();
                }
            });
        }

        #region CommandBar
        private void AddClick(object sender, RoutedEventArgs e)
        {
            _isAddingPaneShowed = true;
            AddingPane.Visibility = Visibility.Visible;
            AddStory.Begin();
            AddingPane.SetFocus();
        }
        #endregion

        #region 汉堡按钮

        private void HamClick(object sender, RoutedEventArgs e)
        {
            _isDrawerSlided = true;
            HamburgerBtn.PlayHamInStory();
            SlideInStory.Begin();
        }
        private void MaskBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isDrawerSlided)
            {
                _isDrawerSlided = false;
                HamburgerBtn.PlayHamOutStory();
                SlideOutStory.Begin();
            }
        }
        #endregion

        #region 手势打开
        private void Grid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _pointOriX = e.Position.X;
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_pointOriX < 10 && e.Delta.Translation.X > 10 && LocalSettingHelper.GetValue("EnableGesture") == "true")
            {
                SlideInStory.Begin();
                HamburgerBtn.PlayHamInStory();
                _isDrawerSlided = true;
            }
        }

        #endregion

        #region 更新磁贴

        //private void UpdateNormalTile(ObservableCollection<ToDo> schedules)
        //{
        //    List<string> undoList = new List<string>();
        //    foreach (var sche in schedules)
        //    {
        //        if (!sche.IsDone)
        //        {
        //            undoList.Add(sche.Content);
        //        }
        //    }
        //    UpdateTileHelper.ClearAllSchedules();

        //    if (undoList.Count < 4)
        //    {
        //        UpdateTileHelper.UpdateNormalTile(undoList);
        //    }
        //    else
        //    {
        //        UpdateTileHelper.UpdateNormalTile(undoList, true);
        //    }
        //}


        #endregion

        #region Override
        protected override void SetNavigationBackBtn()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        protected override void RegisterHandleBackLogic()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += NewMainPage_BackRequested;
            if (ApiInformationHelper.HasHardwareButton())
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
        }

        protected override void UnRegisterHandleBackLogic()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= NewMainPage_BackRequested;
            if (ApiInformationHelper.HasHardwareButton())
            {
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            }
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            HandleBackLogic();
        }

        private void NewMainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;

            HandleBackLogic();
        }

        private void HandleBackLogic()
        {
            if (_isAddingPaneShowed)
            {
                RemoveStory.Begin();
                _isAddingPaneShowed = false;
            }
            if (_isDrawerSlided)
            {
                SlideOutStory.Begin();
                HamburgerBtn.PlayHamOutStory();
                _isDrawerSlided = false;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (LocalSettingHelper.GetValue("EnableBackgroundTask") == "true")
            {
                BackgroundTaskHelper.RegisterBackgroundTask(DeviceKind.Windows);
            }

            Frame.BackStack.Clear();

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (_isDrawerSlided)
            {
                SlideOutStory.Begin();
                HamburgerBtn.PlayHamOutStory();
                _isDrawerSlided = false;
            }
        }

        #endregion

    }
}
