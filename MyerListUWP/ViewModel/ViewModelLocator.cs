using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MyerListUWP.ViewModel;

namespace MyerList.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ToDoViewModel>();
            SimpleIoc.Default.Register<DeletedItemViewModel>();

            SimpleIoc.Default.Register<SettingPageViewModel>();
        }

        public MainViewModel MainVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public ToDoViewModel ToDoVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ToDoViewModel>();
            }
        }

        public DeletedItemViewModel DeletedVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DeletedItemViewModel>();
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
