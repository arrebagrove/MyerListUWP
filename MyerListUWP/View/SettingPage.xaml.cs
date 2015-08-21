using MyerList.Base;
using MyerList.ViewModel;
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

    public sealed partial class SettingPage : BindablePage
    {
        private SettingPageViewModel SettingVM
        {
            get
            {
                return this.DataContext as SettingPageViewModel;
            }
        }
        public SettingPage()
        {
            this.InitializeComponent();
        }
    }
}
