using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;

namespace MyerList.Helper
{
    public class ResourcesHelper
    {
        public static ResourceLoader loader = new ResourceLoader();
        public static string GetString(string key)
        {
            return loader.GetString(key);
        }
    }
}
