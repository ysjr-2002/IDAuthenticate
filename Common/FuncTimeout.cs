using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 定时工作器
    /// </summary>
    public class FuncTimeout
    {
        private System.Timers.Timer timer = null;
        private Action timeoutCallback;
        public long Ticks { get; set; }
        /// <summary>
        /// 初始定时器
        /// </summary>
        public FuncTimeout()
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += (s, e) =>
            {
                if (timeoutCallback != null)
                {
                    timeoutCallback();
                }
            };
        }
        /// <summary>
        /// 一次定时器
        /// </summary>
        /// <param name="timeOut"></param>
        /// <param name="callBack"></param>
        public void StartOnce(int timeOut, Action callBack)
        {
            if (timeOut == 0)
                return;
            timer.Stop();
            timeoutCallback = callBack;
            //一次
            timer.AutoReset = false;
            timer.Interval = timeOut;
            timer.Start();
        }
        /// <summary>
        /// 每N秒执行函数
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="callback"></param>
        public void StartLoop(int timeout, Action callback)
        {
            if (timeout == 0)
                return;

            timer.Stop();
            timeoutCallback = callback;
            //多次
            timer.AutoReset = true;
            timer.Interval = timeout;
            timer.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            timeoutCallback = null;
            timer.Stop();
        }
        /// <summary>
        /// 释放定时器
        /// </summary>
        public void Dispose()
        {
            Stop();
            timer.Dispose();
        }
    }
}
