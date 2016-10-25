using Iaspec.Common.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Common.Log
{
    public static class LogHelper
    {
        static ILogOutput log = DefaultLogFacade.CreateLog("");
        static LogHelper()
        {
            DefaultLogFacade.RegisterLogWriter(DefaultLogFacade.CreateFilePathWriter("Logs"));
        }
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="str"></param>
        public static void Info(object str, params string[] args)
        {
            var msg = string.Format("gate:" + DateTime.Now.ToString("HH:mm:ss") + " " + str, args);
            Trace.WriteLine(msg);
            log.WriteInfomation(msg);
        }

        public static void LogJson(string str)
        {
            var msg = "gate:" + DateTime.Now.ToString("HH:mm:ss") + " " + str;
            Trace.WriteLine(msg);
            log.WriteInfomation(msg);
        }

        public static void ToFile(string str)
        {

        }
    }
}
