using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using Common.WebAPI;
using Main.ViewModel;
using System.IO;
using Common.Dialog;

namespace Main.Camera
{
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
            currentDevice.Start();
            return true;
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

        public void StopCamera()
        {
            if (currentDevice != null)
            {
                currentDevice.Stop();
            }
        }
    }
}
