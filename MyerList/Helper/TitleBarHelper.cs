﻿using JP.Utils.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;

namespace MyerListUWP.Helper
{
    public enum CateColors
    {
        DefaultColor,
        WorkColor,
        LifeColor,
        FamilyColor,
        EnterColor,
        DeletedColor
    }
    public static class TitleBarHelper
    {
        public static void SetUpForeBlackTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.Black;
            titleBar.ButtonHoverBackgroundColor = ColorConverter.HexToColor("#DEDEDE");
            titleBar.ButtonPressedBackgroundColor = ColorConverter.HexToColor("#BBBBBB");
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = ColorConverter.HexToColor("#676767");
        }

        public static void SetUpForeWhiteTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ForegroundColor = Colors.White;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
            titleBar.InactiveForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = ColorConverter.HexToColor("#31DEDEDE");
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonPressedBackgroundColor = ColorConverter.HexToColor("#22DEDEDE");
        }
    }
}
