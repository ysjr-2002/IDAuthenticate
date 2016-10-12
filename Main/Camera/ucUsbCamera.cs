using AForge.Video.DirectShow;
using Common.Dialog;
using Common.Log;
using Common.WebAPI;
using Main.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.Camera
{
    /// <summary>
    /// USB摄像头
    /// </summary>
    public partial class ucUsbCamera : UserControl
    {
        private VideoCaptureDevice currentDevice = null;
        public ucUsbCamera()
        {
            InitializeComponent();
            this.Load += UcUsbCamera_Load;
        }

        private void UcUsbCamera_Load(object sender, EventArgs e)
        {
        }

        public bool Connect()
        {
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (ConfigPublic.CameraIndex > videoDevices.Count - 1)
            {
                return false;
            }
            // create video source
            currentDevice = new VideoCaptureDevice(videoDevices[ConfigPublic.CameraIndex].MonikerString);
            videoSourcePlayer1.VideoSource = currentDevice;
            var videoCapabilities = currentDevice.VideoCapabilities;
            foreach (var video in videoCapabilities)
            {
                LogHelper.Info("预览分辨率->" + video.FrameSize.Width + "*" + video.FrameSize.Height);
            }
            if (videoCapabilities.Count() > 0)
                currentDevice.VideoResolution = currentDevice.VideoCapabilities.Last();

            var snapVabalities = currentDevice.SnapshotCapabilities;
            foreach (var snap in snapVabalities)
            {
                LogHelper.Info("抓拍分辨率->" + snap.FrameSize.Width + "*" + snap.FrameSize.Height);
            }
            if (snapVabalities.Count() > 0)
                currentDevice.SnapshotResolution = currentDevice.SnapshotCapabilities.Last();
            currentDevice.Start();

            //currentDevice.ProvideSnapshots = true;
            //currentDevice.SnapshotFrame += CurrentDevice_SnapshotFrame;
            return true;
        }
        /// <summary>
        /// 触发抓拍
        /// </summary>
        public void Snap()
        {
            currentDevice.SimulateTrigger();
        }
        /// <summary>
        /// 抓拍人脸图片
        /// </summary>
        /// <returns></returns>
        public string Snap(string filename)
        {
            var filepath = Path.Combine(FileManager.GetFolder(), filename + ".jpg");
            var bitmap = videoSourcePlayer1.GetCurrentVideoFrame();
            bitmap.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
            return filepath;
        }

        private void CurrentDevice_SnapshotFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            var filename = DateTime.Now.ToString("HHmmss");
            var filepath = Path.Combine(FileManager.GetFolder(), filename + ".jpg");
            eventArgs.Frame.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public void StopCamera()
        {
            if (currentDevice != null)
            {
                currentDevice.SignalToStop();
            }
        }
    }
}
