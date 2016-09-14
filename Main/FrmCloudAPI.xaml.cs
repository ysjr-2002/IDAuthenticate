using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

            var url = string.Concat("https://v2.koalacam.net/auth/login", "");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "application/x-www-form-urlencoded";
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.UserAgent= "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0;)";
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wr.UserAgent = "Koala Admin";
            wr.ContentLength = buffer.Length;

            var requeststream = wr.GetRequestStream();
            requeststream.Write(buffer, 0, buffer.Length);
            requeststream.Close();

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

        private void btnGetStatus_Click(object sender, RoutedEventArgs e)
        {
            var url = string.Concat("https://60.205.107.219/auth/status", "");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            wr.Accept = "*/*";
            wr.Timeout = 150000;
            wr.AllowAutoRedirect = false;

            var response = wr.GetResponse();
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
    }
}
