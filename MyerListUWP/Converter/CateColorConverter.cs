using MyerListUWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MyerList.Converter
{
    public class CateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((int)value)
            {
                case 0:
                    {
                        return new SolidColorBrush(new Color() { A = 255, R =186,G=186,B=186});
                    };
                case 1:
                    {
                        return new SolidColorBrush(new Color() {A=255, R = 255, G = 97, B = 97 });
                    };
                case 2:
                    {
                        return (App.Current.Resources["MyerListBlueLight"] as SolidColorBrush);
                    };
                case 3:
                    {
                        return new SolidColorBrush(new Color() { A = 255, R = 255, G = 194, B = 97 });
                    }; 
                case 4:
                    {
                        return new SolidColorBrush(new Color() { A = 255, R = 61, G = 202, B = 169 });
                    }; 
                
            }
            return new SolidColorBrush(new Color() { A = 255, R = 80, G = 94, B = 166 });
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
