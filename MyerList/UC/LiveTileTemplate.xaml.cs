﻿using GalaSoft.MvvmLight.Messaging;
using HttpReqModule;
using JP.Utils.Debug;
using MyerList.Model;
using MyerListUWP.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyerList.UC
{
    public sealed partial class LiveTileTemplate : UserControl
    {
        public LiveTileTemplate()
        {
            this.InitializeComponent();

            Messenger.Default.Register<GenericMessage<ObservableCollection<ToDo>>>(this, MessengerTokens.UpdateTile, async msg =>
            {
                try
                {
                    if (!AppSettings.Instance.EnableTile)
                    {
                        UpdateTileHelper.ClearAllSchedules();
                        return;
                    }

                    var list = msg.Content;

                    await UpdateCustomeTile(list);
                }
                catch(Exception)
                {

                }
                
            });
        }

        private void CleanUpTileTemplate()
        {
            LargeText0.Text = LargeText1.Text = LargeText2.Text = LargeText3.Text = "";
            WideText0.Text = WideText1.Text = WideText2.Text = WideText3.Text = "";
            MiddleText0.Text = MiddleText1.Text = MiddleText2.Text = MiddleText3.Text = "";

            LargeCount.Text = WideCount.Text = MiddleCount.Text = "";
        }

        public async Task UpdateCustomeTile(ObservableCollection<ToDo> schedules)
        {
            try
            {
                //关闭了磁贴更新
                if (!AppSettings.Instance.EnableTile)
                {
                    UpdateTileHelper.ClearAllSchedules();
                    return;
                }

                CleanUpTileTemplate();

                LargeBackGrd.Background = WideBackGrd.Background = MiddleBackGrd.Background = SmallBackGrd.Background = new SolidColorBrush(Colors.Transparent);

                List<string> undoList = new List<string>();

                foreach (var sche in schedules)
                {
                    if (!sche.IsDone)
                    {
                        undoList.Add(sche.Content);
                    }
                }

                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();

                LargeText0.Text = WideText0.Text = MiddleText0.Text = undoList.ElementAtOrDefault(0) ?? "";
                LargeText1.Text = WideText1.Text = MiddleText1.Text = undoList.ElementAtOrDefault(1) ?? "";
                LargeText2.Text = WideText2.Text = MiddleText2.Text = undoList.ElementAtOrDefault(2) ?? "";
                LargeText3.Text = WideText3.Text = MiddleText3.Text = undoList.ElementAtOrDefault(3) ?? "";

                LargeText4.Text = undoList.ElementAtOrDefault(4) ?? "";
                LargeText5.Text = undoList.ElementAtOrDefault(5) ?? "";
                LargeText6.Text = undoList.ElementAtOrDefault(6) ?? "";
                LargeText7.Text = undoList.ElementAtOrDefault(7) ?? "";
                LargeText8.Text = undoList.ElementAtOrDefault(8) ?? "";

                LargeCount.Text = WideCount.Text = MiddleCount.Text = SmallCount.Text = undoList.Count.ToString();

                if (undoList.Count == 0)
                {
                    LargeText0.Text = WideText0.Text = MiddleText0.Text = "Enjoy your day ;-)";
                }

                UpdateTileHelper.ClearAllSchedules();

                //少于4个待办事项，不轮播
                if (undoList.Count <= 4)
                {
                    await UpdateTileHelper.UpdatePersonalTile(LargeGrid, WideGrid, MiddleGrid, SmallGrid,true, false);
                }
                else
                {
                    //把前4条插入轮播
                    await UpdateTileHelper.UpdatePersonalTile(LargeGrid, WideGrid, MiddleGrid, SmallGrid,true, true);

                    if (undoList.Count > 4)
                    {
                        WideText0.Text = MiddleText0.Text = undoList.ElementAtOrDefault(4) ?? "";
                        WideText1.Text = MiddleText1.Text = undoList.ElementAtOrDefault(5) ?? "";
                        WideText2.Text = MiddleText2.Text = undoList.ElementAtOrDefault(6) ?? "";
                        WideText3.Text = MiddleText3.Text = undoList.ElementAtOrDefault(7) ?? "";

                        //把5~8条加入轮播
                        await UpdateTileHelper.UpdatePersonalTile(LargeGrid, WideGrid, MiddleGrid, SmallGrid,false, true);
                    }
                    if (undoList.Count > 8)
                    {
                        WideText0.Text = MiddleText0.Text = undoList.ElementAtOrDefault(8) ?? "";
                        WideText1.Text = MiddleText1.Text = undoList.ElementAtOrDefault(9) ?? "";
                        WideText2.Text = MiddleText2.Text = undoList.ElementAtOrDefault(10) ?? "";
                        WideText3.Text = MiddleText3.Text = undoList.ElementAtOrDefault(11) ?? "";

                        //大于8的加入轮播
                        await UpdateTileHelper.UpdatePersonalTile(LargeGrid, WideGrid, MiddleGrid, SmallGrid, false,true);
                    }
                }
            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecordAsync(e);
            }
        }
    }
}
