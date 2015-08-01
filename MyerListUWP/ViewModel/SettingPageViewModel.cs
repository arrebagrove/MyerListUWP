﻿using System;
using Windows.UI.Xaml.Media.Imaging;
using ChaoFunctionRT;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml;
using Windows.Globalization;
using Windows.ApplicationModel.Resources.Core;
using GalaSoft.MvvmLight.Messaging;
using JP.Utils.Data;
using HttpReqModule;

namespace MyerList.ViewModel
{
    public class SettingPageViewModel:ViewModelBase
    {
        private BitmapImage tileimage;
        public BitmapImage TileImage
        {
            get
            {
                return tileimage;
            }
            set
            {
                tileimage = value;
                RaisePropertyChanged();
            }
        }

        private bool isopen;
        public bool IsOpen
        {
            get
            {
                return isopen;
            }
            set
            {
                if(isopen!=value)
                {
                    isopen = value;
                    RaisePropertyChanged(() => IsOpen);
                }
            }
        }

        private bool _enableBackgroundTask;
        public bool EnableBackgroundTask
        {
            get { return _enableBackgroundTask; }
            set
            {
                if(_enableBackgroundTask!=value)
                {
                    _enableBackgroundTask = value;
                }
                RaisePropertyChanged(() => EnableBackgroundTask);
                if (value == true)
                {
                    LocalSettingHelper.AddValue("EnableBackgroundTask", "true");
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(""), "RegisterBackgroundTask");
                }
                else
                {
                    LocalSettingHelper.AddValue("EnableBackgroundTask", "false");
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(""), "UnRegisterBackgroundTask");
                }
            }
        }

        private bool _enableTile;
        public bool EnableTile
        {
            get
            { 
                return _enableTile;
            }
            set
            {
                if(_enableTile!=value)
                     _enableTile = value;
                RaisePropertyChanged(()=>EnableTile);
                if(value==true)
                {
                    LocalSettingHelper.AddValue("EnableTile", "true");
                    IsOpen = true;
                    TileImage.UriSource = new Uri("ms-appx:///Assets/LiveTile.png");
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(""), "SyncExecute");
                }
                else
                {
                    LocalSettingHelper.AddValue("EnableTile", "false");
                    IsOpen = false;
                    TileImage.UriSource = new Uri("ms-appx:///Assets/ori.png");
                    UpdateTileHelper.ClearAllSchedules();
                }
            }
        }

        private bool _enablegesture;
        public bool EnableGesture
        {
            get
            {
                return _enablegesture;
            }
            set
            {
                if(_enablegesture!=value)
                    _enablegesture = value;
                RaisePropertyChanged(()=>EnableGesture);
                if (value == true)
                {
                    LocalSettingHelper.AddValue("EnableGesture", "true");
                }
                else
                {
                    LocalSettingHelper.AddValue("EnableGesture", "false");
                }
            }
        }

        private bool _showkeyboard;
        public bool ShowKeyboard
        {
            get
            {
                return _showkeyboard;
            }
            set
            {
                if(_showkeyboard!=value)
                    _showkeyboard = value;
                RaisePropertyChanged(()=>ShowKeyboard);
                if (value == true)
                {
                    LocalSettingHelper.AddValue("ShowKeyboard", "true");
                }
                else
                {
                    LocalSettingHelper.AddValue("ShowKeyboard", "false");
                }
            }
        }

        private bool _transparentTile;
        public bool TransparentTile
        {
            get
            {
                return _transparentTile;
            }
            set
            {
                if (_transparentTile != value)
                    _transparentTile = value;
                RaisePropertyChanged(() => TransparentTile);
                if (value == true)
                {
                    LocalSettingHelper.AddValue("TransparentTile", "true");
                }
                else
                {
                    LocalSettingHelper.AddValue("TransparentTile", "false");
                }
            }
        }

        private bool _isAddToBottom;
        public bool IsAddToBottom
        {
            get
            {
                return _isAddToBottom;
            }
            set
            {
                if (_isAddToBottom != value)
                    _isAddToBottom = value;
                RaisePropertyChanged(() => IsAddToBottom);
                if (value == true)
                {
                    LocalSettingHelper.AddValue("AddMode", "1");
                }
                else
                {
                    LocalSettingHelper.AddValue("AddMode", "0");
                }
            }
        }

        private int _currentLanguage;
        public int CurrentLanguage
        {
            get
            {
                return _currentLanguage;
            }
            set
            {
               if(_currentLanguage!=value)
               {
                   _currentLanguage=value;
                   ShowHint=Visibility.Visible;
                   ChangeLanguage();
               }
               RaisePropertyChanged(() => CurrentLanguage);
            }
        }

        private Visibility _showHint;
        public Visibility ShowHint
        {
            get
            {
                return _showHint;
            }
            set
            {
                if(_showHint!=value)
                {
                    _showHint=value;
                }
                RaisePropertyChanged(()=>ShowHint);
            }
        }

        public SettingPageViewModel()
        {
            TileImage = new BitmapImage();
            ShowHint=Visibility.Collapsed;

            var lang = LocalSettingHelper.GetValue("AppLang");
            if (lang.Contains("zh"))
            {
                CurrentLanguage = 1;
            }
            else CurrentLanguage = 0;

            if(LocalSettingHelper.IsExist("EnableTile"))
            {
                this.EnableTile = LocalSettingHelper.GetValue("EnableTile") == "true" ? true : false;
            }
            else
            {
                LocalSettingHelper.AddValue("EnableTile","true");
                this.EnableTile = true;
            }

            if (LocalSettingHelper.IsExist("EnableBackgroundTask"))
            {
                EnableBackgroundTask = LocalSettingHelper.GetValue("EnableBackgroundTask") == "true" ? true : false;
            }
            else
            {
                LocalSettingHelper.AddValue("EnableBackgroundTask", "true");
                EnableBackgroundTask = true;
            }

            if (LocalSettingHelper.IsExist("EnableGesture"))
            {
                EnableGesture = LocalSettingHelper.GetValue("EnableGesture") == "true" ? true : false;
            }
            else
            {
                LocalSettingHelper.AddValue("EnableGesture", "true");
                EnableGesture = true;
            }

            if (LocalSettingHelper.IsExist("ShowKeyboard"))
            {
                ShowKeyboard = LocalSettingHelper.GetValue("ShowKeyboard") == "true" ? true : false;
            }
            else
            {
                LocalSettingHelper.AddValue("ShowKeyboard", "true");
                ShowKeyboard = true;
            }

            if (LocalSettingHelper.IsExist("AddMode"))
            {
                IsAddToBottom = LocalSettingHelper.GetValue("AddMode") == "1" ? true : false;
            }
            else
            {
                LocalSettingHelper.AddValue("AddMode", "1");
                IsAddToBottom = true;
            }
            if (LocalSettingHelper.IsExist("TransparentTile"))
            {
                TransparentTile = LocalSettingHelper.GetValue("TransparentTile") == "true" ? true : false;
            }
            else
            {
                LocalSettingHelper.AddValue("TransparentTile", "true");
                TransparentTile = true;
            }
        }

        private void ChangeLanguage()
        {
            if (CurrentLanguage == 1)
            {
                ApplicationLanguages.PrimaryLanguageOverride = "zh-CN";
                LocalSettingHelper.AddValue("AppLang", "zh-CN");
                var resourceContext = ResourceContext.GetForCurrentView();
                resourceContext.Reset();
            }
            else
            {
                ApplicationLanguages.PrimaryLanguageOverride = "en-US";
                LocalSettingHelper.AddValue("AppLang", "en-US");
                var resourceContext = ResourceContext.GetForCurrentView();
                resourceContext.Reset();
            }
        }
    }
}
