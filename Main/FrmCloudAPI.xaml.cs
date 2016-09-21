using Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// </summary>
    public partial class FrmCloudAPI : Window
    {
        const string URL = "http://60.205.107.219";
        const string username = "aobidao";
        const string password = "abd123";
        public FrmCloudAPI()
        {
            InitializeComponent();
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
            var data = "username=" + username + "&password=" + password;
            var buffer = System.Text.Encoding.UTF8.GetBytes(data);

            var url = string.Concat("https://60.205.107.219/api/compare", "");

            //HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            //wr.ContentType = "application/x-www-form-urlencoded";
            //wr.Method = "Get";
            //wr.KeepAlive = true;
            //wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            //request.UserAgent = "Koala Admin";
            //request.ContentLength = buffer.Length;
            //var requeststream = wr.GetRequestStream();
            //requeststream.Write(buffer, 0, buffer.Length);
            //requeststream.Close();
            //HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
            //var stream = response.GetResponseStream();
            //StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            //var json = sr.ReadToEnd();
            //Console.WriteLine(json);


            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            wr.Headers["cookie"] = cookie;

            var fs = System.IO.File.Open(@"C:\Users\ysj\Desktop\Face\girl.jpg", FileMode.Open);
            byte[] fileByte = new byte[fs.Length];

            fs.Read(fileByte, 0, fileByte.Length);
            fs.Close();

          
            Stream rs = wr.GetRequestStream();
            //图像一
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "image1", fileByte, "text/plain");//image/jpeg
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);
            rs.Write(fileByte, 0, fileByte.Length);

            rs.Write(boundarybytes, 0, boundarybytes.Length);

            //图片二
            headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            header = string.Format(headerTemplate, "image2", fileByte, "text/plain");//image/jpeg
            headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);
            rs.Write(fileByte, 0, fileByte.Length);

            //byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            //rs.Write(trailer, 0, trailer.Length);
            //rs.Close();

            var response = wr.GetResponse();
            var stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            var json = sr.ReadToEnd();
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
