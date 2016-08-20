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
    }
}
