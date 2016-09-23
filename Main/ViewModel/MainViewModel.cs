using CloudAPI;
using Common;
using Common.Dialog;
using Common.NotifyBase;
using IDReader;
using Main.Camera;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Main.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class MainViewModel : PropertyNotifyObject, IDisposable
    {
        private FuncTimeout timeOut = null;
        private static MainViewModel _instance = new MainViewModel();
        private MainViewModel()
        {
            IDCardInfo = new IDCardInfo();
            timeOut = new FuncTimeout();
        }

        public static MainViewModel Instance
        {
            get
            {
                return _instance;
            }
        }

        public ucUsbCamera UsbCamera
        {
            get; set;
        }

        public IDCardInfo IDCardInfo
        {
            get; set;
        }

        public Visibility WelcomeVisibility
        {
            get { return this.GetValue(s => s.WelcomeVisibility); }
            set { this.SetValue(s => s.WelcomeVisibility, value); }
        }

        public Visibility SZFVisibility
        {
            get { return this.GetValue(s => s.SZFVisibility); }
            set { this.SetValue(s => s.SZFVisibility, value); }
        }

        public Visibility PassVisibility
        {
            get { return this.GetValue(s => s.PassVisibility); }
            set { this.SetValue(s => s.PassVisibility, value); }
        }

        public string CompareResult
        {
            get { return this.GetValue(s => s.CompareResult); }
            set { this.SetValue(s => s.CompareResult, value); }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            var open = SFZReader.Instance.Open();
            if (open)
            {
                SFZReader.Instance.SetReadIDCardCallback(OnReadCardCallback);
            }
            else
            {
            }

            var login = MegviiCloud.Login();
            if (login)
            {
            }
            else
            {
            }
        }
        /// <summary>
        /// 读取到证件信息回调
        /// </summary>
        /// <param name="card"></param>
        private void OnReadCardCallback(IDCardInfo card)
        {
            Task.Factory.StartNew(Compare);
        }

        private void Compare()
        {
            CompareResult = "抓拍人脸，请稍等...";

            var imagepath2 = UsbCamera.Snap();
            CompareResult = "正在比对，请稍等...";

            var imagepath1 = Environment.CurrentDirectory + "\\dll\\zp.bmp";
            var score = (int)MegviiCloud.Compare(imagepath1, imagepath2);
            if (score > 78)
            {
                CompareResult = "比对通过->" + score;
            }
            else
            {
                CompareResult = "比对失败->" + score;
            }
            timeOut.StartOnce(2000, () =>
            {
                CompareResult = "";
            });
        }

        public void Dispose()
        {
            SFZReader.Instance.Stop();
        }
    }
}
