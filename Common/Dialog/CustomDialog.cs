using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Common.Dialog
{
    /// <summary>
    /// 自定义对话框
    /// </summary>
    public class CustomDialog
    {
        /// <summary>
        /// 消息对话框
        /// </summary>
        /// <param name="msg"></param>
        public static void Show(string msg, string caption = "提示")
        {
            var dialogWindow = new CustomMessageWindow();
            dialogWindow.ShowInfo(msg, caption);
        }
        /// <summary>
        /// 确认对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <returns>yes:确认 no:取消</returns>
        public static MessageBoxResult Confirm(string msg, string caption = "确认")
        {
            var dialogWindow = new CustomMessageWindow();
            dialogWindow.ShowQuestion(msg, caption);
            var dialogResult = dialogWindow.DialogResult.Value;
            if (dialogResult)
                return MessageBoxResult.Yes;
            else
                return MessageBoxResult.No;
        }
    }
}
