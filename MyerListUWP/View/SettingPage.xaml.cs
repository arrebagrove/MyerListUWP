using MyerListUWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace MyerList
{

    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();

            StatusBar.GetForCurrentView().BackgroundColor = (App.Current.Resources["MyerListBlueLight"] as SolidColorBrush).Color;
            StatusBar.GetForCurrentView().BackgroundOpacity = 100;
            StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            StartStory.Begin();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame rootframe = Window.Current.Content as Frame;

            if (rootframe != null && rootframe.CanGoBack)
            {
                e.Handled = true;
                rootframe.GoBack();
            }
        }

    }
}
