using GalaSoft.MvvmLight.Messaging;
using HttpReqModule;
using JP.Utils.Data;
using JP.Utils.Debug;
using MyerList.Base;
using MyerList.Helper;
using MyerList.Model;
using MyerList.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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

        private bool _isHamIn = false;
        private bool _isAddIn = false;

        private double _oriX = 0;

        public MainPage()
        {
            this.InitializeComponent();

            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.ToastToken, msg =>
            {
                ToastControl.ShowMessage(msg.Content);
            });
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.CloseHam, msg =>
            {
                HamOutStory.Begin();
                _isHamIn = false;
            });
            //Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.AddScheduleUI, msg =>
            //{
            //    AddGrid.Visibility = Visibility.Visible;
            //    AddStory.Begin();
            //    AddContentBox.Focus(FocusState.Programmatic);
            //});
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.RemoveScheduleUI, msg =>
            {
                if (AddingPane.Visibility == Visibility.Visible)
                {
                    RemoveStory.Begin();
                }
            });
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.ShowModifyUI, msg =>
            {
                AddingPane.Visibility = Visibility.Visible;
                AddStory.Begin();
            });
            Messenger.Default.Register<GenericMessage<ObservableCollection<ToDo>>>(this, MessengerToken.UpdateTile, async schedules =>
            {
                if (LocalSettingHelper.GetValue("EnableTile") == "false")
                {
                    UpdateTileHelper.ClearAllSchedules();
                    return;
                }

                //if (LocalSettingHelper.GetValue("EnableBackgroundTask") == "true")
                //{
                //    UpdateNormalTile(schedules.Content);
                //}
                //else
                //{
                //    await UpdateCustomeTile(schedules.Content);
                //}

                await TileControl.UpdateCustomeTile(schedules.Content);
            });

            RemoveStory.Completed += ((senderc, ec) =>
              {
                  _isAddIn = false;
                  AddingPane.Visibility = Visibility.Collapsed;
                  SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
              });

            AddStory.Completed += ((sendera, ea) =>
              {
                  _isAddIn = true;
                  SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
              });

            this.KeyDown += ((sender,e)=>
            {
                if (_isAddIn && e.Key==Windows.System.VirtualKey.Enter && e.KeyStatus.RepeatCount==1)
                {
                    Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.EnterToAdd);
                    RemoveStory.Begin();
                }
            });
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            _isAddIn = true;
            AddingPane.Visibility = Visibility.Visible;
            AddStory.Begin();
            AddingPane.SetFocus();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }


        #region Hamburger

        private void HamClick(object sender, RoutedEventArgs e)
        {
            _isHamIn = true;
            HamInStory.Begin();
        }
        private void MaskBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isHamIn)
            {
                _isHamIn = false;
                HamOutStory.Begin();
            }
        }
        private void Grid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _oriX = e.Position.X;
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_oriX < 10 && e.Delta.Translation.X > 10 && LocalSettingHelper.GetValue("EnableGesture")=="true")
            {
                HamInStory.Begin();
                _isHamIn = true;
            }
        }

        #endregion

        #region UPDATE TILE
        
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
            if (_isAddIn)
            {
                RemoveStory.Begin();
                _isAddIn = false;
            }
            if (_isHamIn)
            {
                HamOutStory.Begin();
                _isHamIn = false;
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
            if(_isHamIn)
            {
                HamOutStory.Begin();
                _isHamIn = false;
            }
        }

        #endregion

    }
}
