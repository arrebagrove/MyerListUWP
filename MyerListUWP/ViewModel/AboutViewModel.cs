using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.System;

namespace MyerList.ViewModel
{
    public class AboutViewModel:ViewModelBase
    {
        /// <summary>
        /// Rate
        /// </summary>
        private RelayCommand _rateCommand;
        public RelayCommand RateCommand
        {
            get
            {
                if (_rateCommand != null)
                {
                    return _rateCommand;
                }
                return _rateCommand = new RelayCommand(async () =>
                {
#if WINDOWS_PHONE_APP
                    await Windows.System.Launcher.LaunchUriAsync(
                            new Uri("ms-windows-store:reviewapp?appid=52172f00-cfe1-4695-b4f7-a2be0c1bfacc"));
#else
                    await Launcher.LaunchUriAsync(
                            new Uri("ms-windows-store:reviewapp?appid=31eb52eb-aaee-43d9-b573-22ee91490502"));
#endif
                });
            }
        }

        public AboutViewModel()
        {

        }
    }
}
