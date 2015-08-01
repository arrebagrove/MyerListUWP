using ChaoFunctionRT;
using GalaSoft.MvvmLight.Messaging;
using NotificationsExtensions.TileContent;
using MyerList.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using System.Threading.Tasks;
using System;
using Windows.UI.Xaml.Input;
using JP.Utils.Data;
using MyerList.Helper;
using HttpReqModule;
using JP.Utils.Debug;
using MyerList.Base;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml.Media;
using MyerListUWP;

namespace MyerList
{

    public sealed partial class MainPage : BindablePage
    {

        private bool _isInAddMode = false;

        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.ToastToken, msg =>
              {
                  ToastControl.ShowMessage(msg.Content);
              });

            Messenger.Default.Register<GenericMessage<string>>(this, "AddScheduleUI", msg =>
            {
                grid.Visibility = Visibility.Visible;
                AddStory.Begin();
                _isInAddMode = true;
            });
            Messenger.Default.Register<GenericMessage<string>>(this, "RemoveScheduleUI", msg =>
            {
                if(grid.Visibility==Visibility.Visible)
                {
                    _isInAddMode = false;
                    grid.Visibility = Visibility.Collapsed;
                    RemoveStory.Begin();
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(""), "ClearNote");
                }
               
            });
            Messenger.Default.Register<GenericMessage<string>>(this, "Modify", msg =>
            {
                grid.Visibility = Visibility.Visible;
                AddStory.Begin();
            });
            Messenger.Default.Register<GenericMessage<ObservableCollection<Schedule>>>(this, "UpdateTile", async schedules =>
            {
                if (LocalSettingHelper.GetValue("EnableTile") == "false")
                {
                    UpdateTileHelper.ClearAllSchedules();
                    return;
                }

                if (LocalSettingHelper.GetValue("EnableBackgroundTask") == "true")
                {
                    UpdateNormalTile(schedules.Content);
                }
                else
                {
                    await UpdateCustomeTile(schedules.Content);
                }
            });

            Messenger.Default.Register<GenericMessage<string>>(this, "InSwipe", act =>
                {
                    Listview.CanReorderItems = false;
                    Listview.CanDragItems = false;
                    Listview.AllowDrop = false;
                });

            Messenger.Default.Register<GenericMessage<string>>(this, "RegisterBackgroundTask", act =>
                {
                    BackgroundTaskHelper.RegisterBackgroundTask(DeviceKind.Windows);
                });

            Messenger.Default.Register<GenericMessage<string>>(this, "UnRegisterBackgroundTask", act =>
            {
                BackgroundTaskHelper.UnRegisterBackgroundTask();
            });

            this.KeyUp += MainPage_KeyUp;
            this.AddStory.Completed += AddStory_Completed;
        }

        private void AddStory_Completed(object sender, object e)
        {
            AddContentBox.Focus(FocusState.Programmatic);
        }

        private void MainPage_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key==Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;

                Messenger.Default.Send(new GenericMessage<string>(this.AddContentBox.Text), "PressEnter");
            }
        }

        #region UPDATE TILE
        private async Task UpdateCustomeTile(ObservableCollection<Schedule> schedules)
        {
            try
            {
                if (LocalSettingHelper.GetValue("EnableTile") == "false")
                {
                    UpdateTileHelper.ClearAllSchedules();
                    return;
                }

                CleanUpTileTemplate();

                if(LocalSettingHelper.GetValue("TransparentTile") =="true")
                {
                    backgrd1.Background = backgrd2.Background = backgrd3.Background = new SolidColorBrush(Colors.Transparent);
                }
                

                List<string> undoList = new List<string>();
                
                foreach (var sche in schedules)
                {
                    if (!sche.IsDone)
                    {
                        undoList.Add(sche.Content);
                    }
                }

                Text0.Text = Text00.Text = undoList.ElementAtOrDefault(0) ?? "";
                Text1.Text = Text01.Text = undoList.ElementAtOrDefault(1) ?? "";
                Text2.Text = Text02.Text = undoList.ElementAtOrDefault(2) ?? "";
                Text3.Text = Text03.Text = undoList.ElementAtOrDefault(3) ?? "";

                Count1.Text = Count2.Text = Count3.Text = undoList.Count.ToString();

                if (undoList.Count == 0)
                {
                    Text0.Text = Text00.Text = "Enjoy your day ;-)";
                }

                UpdateTileHelper.ClearAllSchedules();

                if (undoList.Count<=4)
                {
                    await UpdateTileHelper.UpdatePersonalTile(WideGrid, MediumGrid, SmallGrid,false);
                }
                else
                {
                    await UpdateTileHelper.UpdatePersonalTile(WideGrid, MediumGrid, SmallGrid, true);

                    if (undoList.Count > 4)
                    {
                        Text0.Text = Text00.Text = undoList.ElementAtOrDefault(4) ?? "";
                        Text1.Text = Text01.Text = undoList.ElementAtOrDefault(5) ?? "";
                        Text2.Text = Text02.Text = undoList.ElementAtOrDefault(6) ?? "";
                        Text3.Text = Text03.Text = undoList.ElementAtOrDefault(7) ?? "";
                        await UpdateTileHelper.UpdatePersonalTile(WideGrid, MediumGrid, SmallGrid, true);
                    }
                    if (undoList.Count > 8)
                    {
                        Text0.Text = Text00.Text = undoList.ElementAtOrDefault(8) ?? "";
                        Text1.Text = Text01.Text = undoList.ElementAtOrDefault(9) ?? "";
                        Text2.Text = Text02.Text = undoList.ElementAtOrDefault(10) ?? "";
                        Text3.Text = Text03.Text = undoList.ElementAtOrDefault(11) ?? "";
                        await UpdateTileHelper.UpdatePersonalTile(WideGrid, MediumGrid, SmallGrid, true);
                    }
                }
                
            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);

            }

        }

        private void UpdateNormalTile(ObservableCollection<Schedule> schedules)
        {
            List<string> undoList = new List<string>();
            foreach (var sche in schedules)
            {
                if (!sche.IsDone)
                {
                    undoList.Add(sche.Content);
                }
            }
            UpdateTileHelper.ClearAllSchedules();

            if (undoList.Count<4)
            {
                UpdateTileHelper.UpdateNormalTile(undoList);
            }
            else
            {
                UpdateTileHelper.UpdateNormalTile(undoList,true);
            }
        }

        private void CleanUpTileTemplate()
        {
            Text0.Text = Text1.Text = Text2.Text = Text3.Text = Text00.Text = Text01.Text = Text02.Text = "";
            Count1.Text = Count2.Text = Count3.Text = "0";
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (LocalSettingHelper.GetValue("EnableBackgroundTask")=="true")
            {
                BackgroundTaskHelper.RegisterBackgroundTask(DeviceKind.Windows);
            }
            
            Frame.BackStack.Clear();

            NavigateStory.Begin();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
    }
}
