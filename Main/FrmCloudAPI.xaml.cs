using CloudAPI;
using Common;
using Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for FrmCloudAPI.xaml
    /// http://topmanopensource.iteye.com/blog/1605238
    /// http://www.cnblogs.com/liuyinjun/p/3980091.html
    /// </summary>
    public partial class FrmCloudAPI : Window
    {
        const string URL = "http://60.205.107.219";
        const string username = "aobidao";
        const string password = "abd123";

        const string sourceFile = "d:\\zp\\zp.bmp";
        const string snapFile = "d:\\zp\\thumb.jpg";

        public FrmCloudAPI()
        {
            InitializeComponent();

            this.Loaded += FrmCloudAPI_Loaded;
        }

        private void FrmCloudAPI_Loaded(object sender, RoutedEventArgs e)
        {
            zp.Source = Funs.ToBitmapSource(sourceFile);

            snap.Source = Funs.ToBitmapSource(snapFile);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var data = "username=" + username + "&password=" + password;
            var buffer = System.Text.Encoding.UTF8.GetBytes(data);

            var url = string.Concat(URL, "/auth/login");

            Stopwatch sw = Stopwatch.StartNew();
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "application/x-www-form-urlencoded";
            wr.Method = "POST";
            wr.ContentLength = buffer.Length;

            var requeststream = wr.GetRequestStream();
            requeststream.Write(buffer, 0, buffer.Length);
            requeststream.Close();

            HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
            var stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            var json = sr.ReadToEnd();
            Console.WriteLine(json);
            sw.Stop();
            Console.WriteLine("耗时:" + sw.ElapsedMilliseconds);

            var headers = response.Headers;

            cookie = headers["Set-Cookie"];
            Console.WriteLine("cookie:" + cookie);

            JavaScriptSerializer serialize = new JavaScriptSerializer();
            var obj = serialize.Deserialize<JsonLogin>(json);
        }

        string cookie = "";

        private void btnGetStatus_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var url = string.Concat(URL, "/auth/status");
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.Method = "Get";
            wr.Headers["cookie"] = cookie;
            var response = wr.GetResponse();
            var stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            var json = sr.ReadToEnd();

            sr.Close();
            response.Close();

            sw.Stop();
            Console.WriteLine("耗时:" + sw.ElapsedMilliseconds);

            Console.WriteLine(json);

            JavaScriptSerializer serialize = new JavaScriptSerializer();
            var obj = serialize.Deserialize<JsonStatus>(json);
        }

        private void btnComapre_Click(object sender, RoutedEventArgs e)
        {
            var url = string.Concat(URL, "/api/compare");

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            wr.Headers["cookie"] = cookie;

            FileStream fs = null;

            String prefix = "--";
            string end = "\r\n";

            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);
            sb.Append(boundary);
            sb.Append(end);

            Stopwatch sw = Stopwatch.StartNew();

            Stream rs = wr.GetRequestStream();
            //图像一
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" + end;
            string header = string.Format(headerTemplate, "image1", "face.jpg");
            sb.Append(header);
            sb.Append("Content-Type: application/octet-stream; charset=utf-8" + end);
            sb.Append(end);

            var str = sb.ToString();
            Console.WriteLine(str);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(str);
            rs.Write(headerbytes, 0, headerbytes.Length);

            fs = System.IO.File.Open(@"f:\zp\zp.bmp", FileMode.Open);
            byte[] data = new byte[1024];
            var len = 0;
            while ((len = fs.Read(data, 0, data.Length)) > 0)
            {
                rs.Write(data, 0, len);
            }
            fs.Close();
            Array.Clear(data, 0, data.Length);


            /** 每个文件结束后有换行 **/
            byte[] byteFileEnd = Encoding.UTF8.GetBytes(end);
            rs.Write(byteFileEnd, 0, byteFileEnd.Length);

            //图片二
            sb.Clear();

            sb.Append(prefix);
            sb.Append(boundary);
            sb.Append(end);

            header = string.Format(headerTemplate, "image2", "abc.jpg");//image/jpeg

            sb.Append(header);
            sb.Append("Content-Type: application/octet-stream; charset=utf-8" + end);
            sb.Append(end);

            str = sb.ToString();
            Console.WriteLine(str);
            headerbytes = System.Text.Encoding.UTF8.GetBytes(str);
            rs.Write(headerbytes, 0, headerbytes.Length);

            Stopwatch temp = Stopwatch.StartNew();
            var filePath = @"f:\zp\ysj.jpg";
            System.Drawing.Image sourceImage = Bitmap.FromFile(filePath);

            var width = 160;
            var height = 90;
            KeepRatio(sourceImage.Size, ref width, ref height);

            System.Drawing.Image thumbnailImage = sourceImage.GetThumbnailImage(width, height, null, IntPtr.Zero);
            var savePath = "f:\\zp\\thumb.jpg";
            File.Delete(savePath);
            thumbnailImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            thumbnailImage.Dispose();

            temp.Stop();
            Console.WriteLine("缩略图:" + temp.ElapsedMilliseconds);

            fs = System.IO.File.Open(savePath, FileMode.Open);
            while ((len = fs.Read(data, 0, data.Length)) > 0)
            {
                rs.Write(data, 0, len);
            }
            fs.Close();

            /** 每个文件结束后有换行 **/
            rs.Write(byteFileEnd, 0, byteFileEnd.Length);

            //文件结束标志
            byte[] byte1 = Encoding.UTF8.GetBytes(prefix + boundary + prefix + end);
            rs.Write(byte1, 0, byte1.Length);
            rs.Close();


            var response = wr.GetResponse();
            var stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            var json = sr.ReadToEnd();

            Console.WriteLine(json);

            sw.Stop();
            Console.WriteLine("耗时->" + sw.ElapsedMilliseconds);

            JavaScriptSerializer serialze = new JavaScriptSerializer();
            var result = serialze.Deserialize<JsonCompare>(json);

            Console.WriteLine(result.data.score);
            if (result.data.score > 78)
            {
                lblCompare.Content = "比对通过";
            }
            else
            {
                lblCompare.Content = "比对失败";
            }
        }

        private void KeepRatio(System.Drawing.Size size, ref int width, ref int height)
        {
            double heightRatio = (double)size.Height / size.Width;
            double widthRatio = (double)size.Width / size.Height;

            var tempheigth = (int)(width / widthRatio);
            if (tempheigth < size.Height)
            {
                height = tempheigth;
            }
            else
            {
                var tempwidth = (int)(height / heightRatio);
                width = tempwidth;
            }
        }

        private void btnUnixDT_Click(object sender, RoutedEventArgs e)
        {
            var dt = ConvertIntDateTime(1477843200);
            Console.WriteLine(dt.ToString());
        }

        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }

        public static double ConvertDateTimeInt(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }
    }
}
