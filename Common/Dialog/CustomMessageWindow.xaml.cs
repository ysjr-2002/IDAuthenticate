using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Common.Dialog
{
    /// <summary>
    /// 自定义消息对话框
    /// </summary>
    partial class CustomMessageWindow : Window
    {
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(CustomMessageWindow));

        public static readonly DependencyProperty DialogImageProperty =
            DependencyProperty.Register("DialogImage", typeof(string), typeof(CustomMessageWindow));

        public static readonly DependencyProperty DialogInformationProperty =
            DependencyProperty.Register("DialogInformation", typeof(string), typeof(CustomMessageWindow));
        public CustomMessageWindow()
        {
            InitializeComponent();
            this.KeyDown += CustomMessageWindow_KeyDown;
            this.DataContext = this;
        }
        /// <summary>
        /// 响应Escape
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomMessageWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }
        /// <summary>
        /// 信息内容
        /// </summary>
        public string DialogInformation
        {
            get { return (string)GetValue(DialogInformationProperty); }
            set { SetValue(DialogInformationProperty, value); }
        }
        /// <summary>
        /// 对话框图标
        /// </summary>
        public string DialogImage
        {
            get { return (string)GetValue(DialogImageProperty); }
            set { SetValue(DialogImageProperty, value); }
        }
        /// <summary>
        /// 消息对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        public void ShowInfo(string msg, string caption)
        {
            Caption = caption;
            DialogImage = "/Common;component/Images/dialog_info.png";
            DialogInformation = msg;
            spInfo.Visibility = Visibility.Visible;
            spQuestion.Visibility = Visibility.Collapsed;
            FocusManager.SetFocusedElement(this, btnOK);
            this.ShowDialog();
        }
        /// <summary>
        /// 确认对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        public void ShowQuestion(string msg, string caption)
        {
            Caption = caption;
            DialogImage = "/Common;component/Images/dialog_confirm.png";
            DialogInformation = msg;
            spInfo.Visibility = Visibility.Collapsed;
            spQuestion.Visibility = Visibility.Visible;
            FocusManager.SetFocusedElement(this, btnYes);
            this.ShowDialog();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void WindowTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
