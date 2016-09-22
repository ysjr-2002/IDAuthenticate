using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CloudAPI
{
    //4009101616
    public class MegviiCloud
    {
        const string URL = "http://60.205.107.219";
        const string username = "aobidao";
        const string password = "abd123";

        static string cookie = "";

        public static bool Login()
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
            sw.Stop();
            Console.WriteLine("login:" + sw.ElapsedMilliseconds);

            var headers = response.Headers;

            cookie = headers["Set-Cookie"];
            Console.WriteLine("cookie:" + cookie);

            JavaScriptSerializer serialize = new JavaScriptSerializer();
            var obj = serialize.Deserialize<JsonLogin>(json);

            if (obj.code == "0")
                return true;
            else
                return false;
        }

        public static double Compare(string image1, string image2)
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
            string header = string.Format(headerTemplate, "image1", "image1.jpg");
            sb.Append(header);
            sb.Append("Content-Type: application/octet-stream; charset=utf-8" + end);
            sb.Append(end);

            var str = sb.ToString();
            Console.WriteLine(str);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(str);
            rs.Write(headerbytes, 0, headerbytes.Length);

            fs = System.IO.File.Open(image1, FileMode.Open);
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

            header = string.Format(headerTemplate, "image2", "image2.jpg");//image/jpeg

            sb.Append(header);
            sb.Append("Content-Type: application/octet-stream; charset=utf-8" + end);
            sb.Append(end);

            str = sb.ToString();
            Console.WriteLine(str);
            headerbytes = System.Text.Encoding.UTF8.GetBytes(str);
            rs.Write(headerbytes, 0, headerbytes.Length);

            
            fs = System.IO.File.Open(image2, FileMode.Open);
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
            sw.Stop();
            Console.WriteLine("compare:" + sw.ElapsedMilliseconds);

            JavaScriptSerializer serialze = new JavaScriptSerializer();
            var result = serialze.Deserialize<JsonCompare>(json);

            return result.data.score;
        }
    }
}
