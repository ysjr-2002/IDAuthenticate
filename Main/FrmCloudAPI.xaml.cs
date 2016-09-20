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
            //foreach (var key in headers.AllKeys)
            //{
            //    var val = headers[key];
            //    Console.WriteLine(string.Format("key={0};value={1}", key, val));

            //    if (key.Contains("Set-Cookie"))
            //    {
            //        cookie = val;
            //        Console.WriteLine("break");
            //        break;
            //    }
            //}


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

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            //wr.ContentType = "application/x-www-form-urlencoded";
            //wr.Method = "Get";
            //wr.KeepAlive = true;
            //wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            //request.UserAgent = "Koala Admin";
            //request.ContentLength = buffer.Length;

            var requeststream = wr.GetRequestStream();
            //requeststream.Write(buffer, 0, buffer.Length);
            //requeststream.Close();

            HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
            var stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            var json = sr.ReadToEnd();
            Console.WriteLine(json);

            var cookies = response.Cookies;
            foreach (Cookie cookie in cookies)
            {
                Console.WriteLine(cookie);
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
