using JP.Utils.Data;
using MyerListUWP;
using MyerListUWP.Common;
using MyerListUWP.Helper;
using MyerListUWP.View;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyerList.View
{
    public sealed partial class ThemeSelectionPage : Page
    {
        public ThemeSelectionPage()
        {
            this.InitializeComponent();
            this.SizeChanged += ThemeSelectionPage_SizeChanged;
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            TitleBarHelper.SetUpForeWhiteTitleBar();
        }

        private void ThemeSelectionPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width >= 650)
            {
                SP.Orientation = Orientation.Horizontal;
                IMG1.Width = IMG2.Width = 300;
            }
            else
            {
                SP.Orientation = Orientation.Vertical;
                IMG1.Width = IMG2.Width = 250;
            }
        }

        private void LightRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            AppSettings.Instance.DarkMode = false;
            ChangeColorAnim.To = (App.Current.Resources["DefaultColorLight"] as SolidColorBrush).Color;
            ChangeColorAnim.From = (RootGrid.Background as SolidColorBrush).Color;
            ChangeColorStory.Begin();
        }

        private void DarkRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            AppSettings.Instance.DarkMode = true;
            ChangeColorAnim.To = AppSettings.Instance.GlobalBackgroundColor2.Color;
            ChangeColorAnim.From = (RootGrid.Background as SolidColorBrush).Color;
            ChangeColorStory.Begin();
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            LocalSettingHelper.AddValue(AppSettings.UPGRADE_41, "true");
            Frame.Navigate(typeof(MainPage), LoginMode.Login);
        }
    }
}
