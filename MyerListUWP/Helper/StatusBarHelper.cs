using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace MyerList.Helper
{
    public class StatusBarHelper
    {
        public static StatusBar sb = StatusBar.GetForCurrentView();

        public async static void Hide()
        {
            sb.ForegroundColor = new Color { A = 100, R = 255, G = 103, B = 74 };
            sb.ProgressIndicator.Text = "";
            sb.ProgressIndicator.ProgressValue = 0;
            await sb.ProgressIndicator.ShowAsync();
        }

        public async static Task ShowWithText(string text)
        {
            sb.ForegroundColor = Colors.White;
            sb.BackgroundColor = new Color { A = 255, R = 62, G = 146, B = 178 };
            sb.BackgroundOpacity = 100;
            sb.ProgressIndicator.Text = text;
            await sb.ProgressIndicator.ShowAsync();
        }

        public async static Task HideWithSpacText()
        {
            sb.ProgressIndicator.Text = "";
            await sb.ProgressIndicator.HideAsync();
        }

    }
}
