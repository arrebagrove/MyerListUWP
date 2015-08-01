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
    public static class TitleBarHelper
    {
        public static void SetUpTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = (App.Current.Resources["MyerListBlue"] as SolidColorBrush).Color;
            titleBar.ForegroundColor = Colors.White;
            titleBar.InactiveBackgroundColor = titleBar.BackgroundColor;
            titleBar.InactiveForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = (App.Current.Resources["MyerListBlue"] as SolidColorBrush).Color;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonInactiveBackgroundColor = titleBar.BackgroundColor;
            titleBar.ButtonInactiveForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = (App.Current.Resources["MyerListBlueLight"] as SolidColorBrush).Color;
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonPressedBackgroundColor = (App.Current.Resources["MyerListBlueLight"] as SolidColorBrush).Color;
        }
    }
}
