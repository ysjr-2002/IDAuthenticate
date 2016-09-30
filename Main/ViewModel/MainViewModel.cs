using CloudAPI;
using Common;
using Common.Dialog;
using Common.NotifyBase;
using IDReader;
using Main.Camera;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private DispatcherTimer _timer = null;
        private static MainViewModel _instance = new MainViewModel();

        private MainViewModel()
        {
            IDCardInfo = new IDCardInfo();
            timeOut = new FuncTimeout();

            WorkVisibility = Visibility.Collapsed;
            InitVisibility = Visibility.Visible;
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

        public Visibility WorkVisibility
        {
            get { return this.GetValue(s => s.WorkVisibility); }
            set { this.SetValue(s => s.WorkVisibility, value); }
        }

        public Visibility InitVisibility
        {
            get { return this.GetValue(s => s.InitVisibility); }
            set { this.SetValue(s => s.InitVisibility, value); }
        }

        public string CompareResult
        {
            get { return this.GetValue(s => s.CompareResult); }
            set { this.SetValue(s => s.CompareResult, value); }
        }

        public string CurrentDateTime
        {
            get { return this.GetValue(s => s.CurrentDateTime); }
            set { this.SetValue(s => s.CurrentDateTime, value); }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public async void Init()
        {
            var taskOpenDeive = SFZReader.Instance.Open();
            CompareResult = "连接证件阅读器...";
            await taskOpenDeive;

            SFZReader.Instance.SetReadIDCardCallback(OnReadCardCallback);

            var taskLogin = MegviiCloud.Login();
            await taskLogin;

            if (!taskLogin.Result)
            {
                CompareResult = "登录比对服务器失败";
                return;
            }

            var connect = UsbCamera.Connect();
            if(!connect)
            {
                CompareResult = "配置的摄像头需要不存在";
                return;
            }

            var taskStatus = MegviiCloud.GetAccountStatus();
            await taskStatus;

            JsonStatus status = taskStatus.Result;
            if (status.code == 0)
            {
                CompareResult = "剩余调用次数:" + status.data.limitation.quota;
                EntraceWorkMode(1000);
                UpdateDateTime();
            }
        }

        private void UpdateDateTime()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (a, b) =>
            {
                CurrentDateTime = DateTime.Now.ToStandard();
            };
            _timer.Start();
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
            var filename = DateTime.Now.ToString("HHmmss");

            CompareResult = "抓拍人脸，请稍等...";
            var imagepath2 = UsbCamera.Snap(filename);
            CompareResult = "正在比对，请稍等...";

            var zp = Environment.CurrentDirectory + "\\dll\\zp.bmp";
            var imagepath1 = Path.Combine(FileManager.GetFolder(), filename + ".bmp");
            //拷贝文件
            File.Copy(zp, imagepath1);

            var score = (int)MegviiCloud.Compare(imagepath1, imagepath2);
            if (score == -1)
            {
                CompareResult = "请求比对服务失败";
            }
            else if (score > 78)
            {
                CompareResult = "比对通过->" + score;
            }
            else
            {
                CompareResult = "比对失败->" + score;
            }
            ClearComapreResult(2000);
        }

        private void EntraceWorkMode(int millsecond)
        {
            timeOut.StartOnce(millsecond, () =>
            {
                CompareResult = string.Empty;
                WorkVisibility = Visibility.Visible;
                InitVisibility = Visibility.Collapsed;
            });
        }
        /// <summary>
        /// 清空比对结果
        /// </summary>
        /// <param name="millsecond"></param>
        private void ClearComapreResult(int millsecond)
        {
            timeOut.StartOnce(millsecond, () =>
            {
                CompareResult = "";
            });
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
            UsbCamera.StopCamera();
            SFZReader.Instance.Stop();
        }
    }
}
