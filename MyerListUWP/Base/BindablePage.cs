using MyerList.Helper;
using MyerList.Interface;
using MyerListUWP.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace MyerList.Base
{
    public class BindablePage:Page
    {
        public BindablePage()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();

            var info = new ContinuumNavigationTransitionInfo();

            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            this.Transitions = collection;

            this.IsTextScaleFactorEnabled = false;

            SetUpTitleBar();
            SetUpStatusBar();

            NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        protected virtual void SetUpTitleBar()
        {
            TitleBarHelper.SetUpBlueTitleBar();
        }

        protected virtual void SetUpStatusBar()
        {
            if (ApiInformationHelper.HasStatusBar())
            {
                StatusBarHelper.SetUpBlueStatusBar();
            }
        }

        protected virtual void RegisterHandleBackLogic()
        {
            try
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += BindablePage_BackRequested;
                if (ApiInformationHelper.HasHardwareButton())
                {
                    HardwareButtons.BackPressed += HardwareButtons_BackPressed;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        protected virtual void UnRegisterHandleBackLogic()
        {
            try
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= BindablePage_BackRequested;
                if (ApiInformationHelper.HasHardwareButton())
                {
                    HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame != null)
            {
                if (Frame.CanGoBack)
                {
                    e.Handled = true;
                    Frame.GoBack();
                }
            }
        }

        private void BindablePage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame != null)
            {
                if (Frame.CanGoBack)
                {
                    e.Handled = true;
                    Frame.GoBack();
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var NavigationViewModel = (INavigable)this.DataContext;
            if (NavigationViewModel != null)
            {
                NavigationViewModel.Activate(e.Parameter);
            }
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            RegisterHandleBackLogic();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var NavigationViewModel = (INavigable)this.DataContext;
            if (NavigationViewModel != null)
            {
                NavigationViewModel.Deactivate(null);
            }
            UnRegisterHandleBackLogic();
        }
    }
}
