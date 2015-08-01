using ChaoFunctionRT;
using MyerListUWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Popups;
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
    
    public sealed partial class AboutPage : Page
    {
        private bool _isinStory;

        public AboutPage()
        {
            this.InitializeComponent();
            this.StoryInStory.Completed += ((senders, es) =>
                {
                    this._isinStory = true;
                });
            this.StoryOutStory.Completed += ((senderc, ec) =>
                {
                    this._isinStory = false;
                });
            StatusBar.GetForCurrentView().BackgroundColor = (App.Current.Resources["MyerListBlueLight"] as SolidColorBrush).Color;
            StatusBar.GetForCurrentView().BackgroundOpacity = 100;
            StatusBar.GetForCurrentView().ForegroundColor = Colors.White;

            this.VersionValueTB.Text = App.Current.Resources["AppVersion"] as string;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if(_isinStory)
            {
                e.Handled = true;
                StoryOutStory.Begin();
                return;
            }
            Frame rootframe = Window.Current.Content as Frame;
            Page rootpage = rootframe.Content as Page;
            if (rootpage.GetType() == typeof(AboutPage))
            {
                if (rootframe != null && rootframe.CanGoBack)
                {
                    e.Handled = true;
                    rootframe.GoBack();
                }
            }
        }

        private async void FeedbackClick(object sender,RoutedEventArgs e)
        {
            EmailRecipient rec = new EmailRecipient("dengweichao@hotmail.com");
            EmailMessage mes = new EmailMessage();
            mes.To.Add(rec);
            mes.Subject = "MyerList feedback";
            await EmailManager.ShowComposeNewEmailAsync(mes);
        }

        private async void TapToDonate(object sender,TappedRoutedEventArgs e)
        {
            var licenseInformation = CurrentApp.LicenseInformation;
            if (!licenseInformation.ProductLicenses["MyerListDonate"].IsActive)
            {
                try
                {
                    // The customer doesn't own this feature, so 
                    // show the purchase dialog.

                    await CurrentApp.RequestProductPurchaseAsync("MyerListDonate");

                    if(licenseInformation.ProductLicenses["MyerListDonate"].IsActive)
                    {
                        await new MessageDialog("Thanks :D", "FROM DEVELOPER").ShowAsync();
                    }
                    //Check the license state to determine if the in-app purchase was successful.
                }
                catch (Exception)
                {
                    await new MessageDialog("Some errors happened. Still THANK YOU !", "ERROR").ShowAsync();
                }
            }
            else
            {
                await new MessageDialog("Thanks :D", "FROM DEVELOPER").ShowAsync();
            }

        }

        private async void helpClick(object sender,TappedRoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            string content=loader.GetString("HelpHint");
            MessageDialog md = new MessageDialog(content);
            await md.ShowAsync();
        }
       
    }
}
