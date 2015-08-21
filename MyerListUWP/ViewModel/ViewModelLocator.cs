using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace MyerList.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<SettingPageViewModel>(false);
        }

        public MainViewModel MainVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public SettingPageViewModel SettingVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingPageViewModel>();
            }
        }

        public static void Cleanup()
        {

        }
    }
}
