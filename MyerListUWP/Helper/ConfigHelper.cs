using System;
using System.Collections.Generic;
using System.Text;
using ChaoFunctionRT;
using JP.Utils.Data;

namespace MyerList.Helper
{
    public class ConfigHelper
    {
        public static void CheckConfig()
        {
            if (!LocalSettingHelper.IsExist("EnableTile"))
            {
                LocalSettingHelper.AddValue("EnableTile", "true");
            }

            if (!LocalSettingHelper.IsExist("EnableBackgroundTask"))
            {
                LocalSettingHelper.AddValue("EnableBackgroundTask", "true");
            }

            if (!LocalSettingHelper.IsExist("EnableGesture"))
            {
                LocalSettingHelper.AddValue("EnableGesture", "true");
            }

            if (!LocalSettingHelper.IsExist("ShowKeyboard"))
            {
                LocalSettingHelper.AddValue("ShowKeyboard", "true");
            }

            if (!LocalSettingHelper.IsExist("TransparentTile"))
            {
                LocalSettingHelper.AddValue("TransparentTile", "true");
            }

            if (!LocalSettingHelper.IsExist("AddMode"))
            {
                LocalSettingHelper.AddValue("AddMode", "1");
            }
        }
    }
}
