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

namespace Main.Camera
{
    public partial class ucUsbCamera : UserControl
    {
        public ucUsbCamera()
        {
            InitializeComponent();

            this.Load += UcUsbCamera_Load;
        }

        private void UcUsbCamera_Load(object sender, EventArgs e)
        {
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                // create video source
                VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSourcePlayer1.VideoSource = videoSource;
                videoSource.Start();
            }
        }
        /// <summary>
        /// 抓拍人脸图片
        /// </summary>
        /// <returns></returns>
        public string Snap()
        {
            var filepath = "d:\\snap\\" + DateTime.Now.Ticks + ".jpg";
            var bitmap = videoSourcePlayer1.GetCurrentVideoFrame();
            bitmap.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
            return filepath;
        }
    }
}
