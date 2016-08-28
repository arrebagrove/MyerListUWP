using CompositionHelper.Helper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HttpReqModule;
using JP.Utils.Data;
using System;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace MyerListUWP.Common
{
    public class AppSettings : ViewModelBase
    {
        public static string DefaultCateJsonString = "{ \"modified\":true, \"cates\":[{\"name\":\"Work\",\"color\":\"#FF436998\",\"id\":1},{\"name\":\"Life\",\"color\":\"#FFFFB542\",\"id\":2},{\"name\":\"Family\",\"color\":\"#FFFF395F\",\"id\":3},{\"name\":\"Entertainment\",\"color\":\"#FF55C1C1\",\"id\":4}]}";
        public static string ModifiedCateJsonStringFore = "{ \"modified\":true, \"cates\":";

        public static string LEARNT_ADDING_PANE_GESTURE = "LEARN_ADDING_PANE_GESTURE";

        public bool EnableTile
        {
            get
            {
                return ReadSettings(nameof(EnableTile), true);
            }
            set
            {
                SaveSettings(nameof(EnableTile), value);
                RaisePropertyChanged(() => EnableTile);
                if (value == true)
                {
                    Messenger.Default.Send(new GenericMessage<string>(""), "SyncExecute");
                }
                else
                {
                    UpdateTileHelper.ClearAllSchedules();
                }
            }
        }

        public bool EnableGesture
        {
            get
            {
                return ReadSettings(nameof(EnableGesture), true);
            }
            set
            {
                SaveSettings(nameof(EnableGesture), value);
                RaisePropertyChanged(() => EnableGesture);
            }
        }

        public bool IsAddToBottom
        {
            get
            {
                return ReadSettings(nameof(IsAddToBottom), true);
            }
            set
            {
                SaveSettings(nameof(IsAddToBottom), value);
                RaisePropertyChanged(() => IsAddToBottom);
            }
        }

        public bool EnableBackgroundTask
        {
            get
            {
                return ReadSettings(nameof(EnableBackgroundTask), true);
            }
            set
            {
                SaveSettings(nameof(EnableBackgroundTask), value);
                RaisePropertyChanged(() => EnableBackgroundTask);
            }
        }

        public bool DarkMode
        {
            get
            {
                return ReadSettings(nameof(DarkMode), true);
            }
            set
            {
                SaveSettings(nameof(DarkMode), value);
                RaisePropertyChanged(() => DarkMode);
                RaisePropertyChanged(() => GlobalBackgroundColor);
                RaisePropertyChanged(() => GlobalForegroundColor);
                RaisePropertyChanged(() => GlobalBackgroundColor2);
                RaisePropertyChanged(() => GlobalDrawerMaskBackground);
                RaisePropertyChanged(() => GlobalListPointerOverBackground);
                RaisePropertyChanged(() => GlobalListPressedBackground);
                RaisePropertyChanged(() => GlobalAddPaneMaskBackground);
            }
        }

        public SolidColorBrush GlobalBackgroundColor
        {
            get
            {
                if (DarkMode)
                {
                    return new SolidColorBrush("#FF121212".ToColor());
                }
                else
                {
                    return new SolidColorBrush(Colors.White);
                }
            }
        }

        public SolidColorBrush GlobalBackgroundColor2
        {
            get
            {
                if (DarkMode)
                {
                    return new SolidColorBrush("#FF1D1D1D".ToColor());
                }
                else
                {
                    return new SolidColorBrush(Colors.White);
                }
            }
        }

        public SolidColorBrush GlobalListPointerOverBackground
        {
            get
            {
                if (DarkMode)
                {
                    return new SolidColorBrush("#FF333333".ToColor());
                }
                else
                {
                    return new SolidColorBrush("#FFDEDEDE".ToColor());
                }
            }
        }

        public SolidColorBrush GlobalListPressedBackground
        {
            get
            {
                if (DarkMode)
                {
                    return new SolidColorBrush("#DB333333".ToColor());
                }
                else
                {
                    return new SolidColorBrush("#FFE9E9E9".ToColor());
                }
            }
        }

        public SolidColorBrush GlobalDrawerMaskBackground
        {
            get
            {
                if (DarkMode)
                {
                    return new SolidColorBrush("#FF1D1D1D".ToColor());
                }
                else
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        public SolidColorBrush GlobalAddPaneMaskBackground
        {
            get
            {
                if (DarkMode)
                {
                    return new SolidColorBrush("#FF1D1D1D".ToColor());
                }
                else
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        public SolidColorBrush GlobalForegroundColor
        {
            get
            {
                if (DarkMode)
                {
                    return new SolidColorBrush(Colors.White);
                }
                else
                {
                    return new SolidColorBrush(Colors.Black);
                }
            }
        }

        public ApplicationDataContainer LocalSettings { get; set; }

        public AppSettings()
        {
            LocalSettings = ApplicationData.Current.LocalSettings;
        }

        public bool LearnGesture()
        {
            return LocalSettingHelper.HasValue(LEARNT_ADDING_PANE_GESTURE);
        }

        private void SaveSettings(string key, object value)
        {
            LocalSettings.Values[key] = value;
        }

        private T ReadSettings<T>(string key, T defaultValue)
        {
            if (LocalSettings.Values.ContainsKey(key))
            {
                return (T)LocalSettings.Values[key];
            }
            if (defaultValue != null)
            {
                return defaultValue;
            }
            return default(T);
        }

        private static readonly Lazy<AppSettings> lazy = new Lazy<AppSettings>(() => new AppSettings());

        public static AppSettings Instance { get { return lazy.Value; } }
    }
}
