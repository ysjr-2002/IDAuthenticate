﻿using Common.Dialog;
using IDReader;
using Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;

namespace Main
{
    /// <summary>
    /// 主窗口
    /// 账号:aobidao abd123
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = MainViewModel.Instance;
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigPublic.Init();
            MainViewModel.Instance.UsbCamera = camera;
            MainViewModel.Instance.Init();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainViewModel.Instance.Dispose();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var dialog = CustomDialog.Confirm("确认退出系统吗？");
            if (dialog == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
            base.OnClosing(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var filename = DateTime.Now.ToString("HHmmss");
            camera.Snap(filename);
        }
    }
}
