using GalaSoft.MvvmLight.Messaging;
using HttpReqModule;
using JP.Utils.Data;
using JP.Utils.Debug;
using MyerList.Helper;
using MyerList.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace MyerList.UC
{
    public sealed partial class LiveTileTemplate : UserControl
    {
        public LiveTileTemplate()
        {
            this.InitializeComponent();

            Messenger.Default.Register<GenericMessage<ObservableCollection<ToDo>>>(this, MessengerTokens.UpdateTile, async schedules =>
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

                await UpdateCustomeTile(schedules.Content);
            });
        }

        private void CleanUpTileTemplate()
        {
            Text0.Text = Text1.Text = Text2.Text = Text3.Text = Text00.Text = Text01.Text = Text02.Text = "";
            Count1.Text = Count2.Text = Count3.Text = "0";
        }

        public async Task UpdateCustomeTile(ObservableCollection<ToDo> schedules)
        {
            try
            {
                if (LocalSettingHelper.GetValue("EnableTile") == "false")
                {
                    UpdateTileHelper.ClearAllSchedules();
                    return;
                }

                CleanUpTileTemplate();

                if (LocalSettingHelper.GetValue("TransparentTile") == "true")
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

                if (undoList.Count <= 4)
                {
                    await UpdateTileHelper.UpdatePersonalTile(WideGrid, MediumGrid, SmallGrid, false);
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

    }
}
