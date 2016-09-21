using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class UsbCamera : Form
    {
        public UsbCamera()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            // create video source
            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSourcePlayer1.VideoSource = videoSource;

            videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            var bitmap = (Bitmap)eventArgs.Frame.Clone();

            bitmap.Save("d:\\ok.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            bitmap.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
