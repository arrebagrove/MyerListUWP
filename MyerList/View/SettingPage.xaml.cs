using JP.Utils.Helper;
using MyerList.Base;
using MyerList.Helper;
using MyerList.ViewModel;
using MyerListUWP.Common;
using MyerListUWP.Helper;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace MyerList
{
    public sealed partial class SettingPage : CustomTitleBarPage
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(DeviceHelper.IsMobile)
            {
               StatusBarHelper.SetUpBlackStatusBar();
            }
            else
            {
                if(AppSettings.Instance.DarkMode)
                {
                    TitleBarUC?.SetForegroundColor(Colors.White);
                    TitleBarHelper.SetUpForeWhiteTitleBar();
                }
                else
                {
                    TitleBarUC?.SetForegroundColor(Colors.Black);
                    TitleBarHelper.SetUpForeBlackTitleBar();
                }
            }
        }

        private void DarkModeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (AppSettings.Instance.DarkMode)
            {
                TitleBarHelper.SetUpForeWhiteTitleBar();
                TitleBarUC?.SetForegroundColor(Colors.White);
            }
            else
            {
                TitleBarHelper.SetUpForeBlackTitleBar();
                TitleBarUC?.SetForegroundColor(Colors.Black);
            }
        }
    }
}
