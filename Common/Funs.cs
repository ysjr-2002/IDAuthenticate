using Common.Log;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Permissions;
using System.Text;
namespace Common
{
    public static class Funs
    {
        //64 SoftWare\Wow6432Node\Microsoft\Windows\CurrentVersion\\Run
        const string keyName = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        //[RegistryPermission(SecurityAction.PermitOnly, Read=keyName, Write = keyName)]
        public static bool runWhenStart(bool started, string exeName, string path)
        {
            RegistryKey key = null;
            try
            {
                key = Registry.LocalMachine.OpenSubKey(keyName, true);//打开注册表子项
                if (key == null)
                {
                    //如果该项不存在，则创建该子项
                    key = Registry.LocalMachine.CreateSubKey(keyName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("设置开机启动失败：" + ex.Message);
                return false;
            }
            if (started == true)
            {
                try
                {
                    key.SetValue(exeName, path);//设置为开机启动
                    key.Close();
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    key.DeleteValue(exeName);//取消开机启动
                    key.Close();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public static List<string> SerialPorts()
        {
            var ports = SerialPort.GetPortNames().ToList();
            if (ports.Count > 0)
            {
                ports = ports.OrderBy(s => s.Length).ToList();
            }
            ports.Insert(0, "None");
            return ports;
        }


        public static System.Windows.Media.Imaging.BitmapSource ToBitmapSource(string sFilePath)
        {
            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(sFilePath,
                System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    fs.Dispose();

                    System.Windows.Media.Imaging.BitmapImage bitmapImage =
                        new System.Windows.Media.Imaging.BitmapImage();
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = ms;
                        bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze();
                    }
                    return bitmapImage;
                }
            }
            catch
            {
                LogHelper.Info("读取图片失败：" + sFilePath);
                return null;
            }
        }
    }
}
