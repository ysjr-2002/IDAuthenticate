using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Configuration;
using Common.Log;

namespace Main.ViewModel
{
    static class ConfigPublic
    {
        public static int CameraIndex { get; set; }

        /// <summary>
        /// 读取配置参数
        /// </summary>
        public static void Init()
        {
            CameraIndex = GetKey("camera").ToInt32();
            if (CameraIndex == 0)
                CameraIndex = 0;
        }

        private static string GetKey(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                var val = ConfigurationManager.AppSettings[key];
                LogHelper.Info(string.Format("参数[{0}]={1}", key, val));
                return val;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
