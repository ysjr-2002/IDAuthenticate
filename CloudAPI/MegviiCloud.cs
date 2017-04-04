using Common.Log;
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

        /// <summary>
        /// 登录比对服务器
        /// </summary>
        /// <returns></returns>
        public static Task<bool> Login()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var data = "username=" + username + "&password=" + password;
                var buffer = System.Text.Encoding.UTF8.GetBytes(data);
                var url = string.Concat(URL, "/auth/login");
                try
                {
                    var wr = WebRequest.Create(url);
                    wr.Timeout = 5000;
                    wr.ContentType = "application/x-www-form-urlencoded";
                    wr.Method = "POST";
                    wr.ContentLength = buffer.Length;
                    wr.Headers["user-agent"] = "Koala Admin";

                    var requeststream = wr.GetRequestStream();
                    requeststream.Write(buffer, 0, buffer.Length);
                    requeststream.Close();

                    var response = wr.GetResponse();
                    var stream = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
                    var json = sr.ReadToEnd();

                    var headers = response.Headers;
                    cookie = headers["Set-Cookie"];

                    JavaScriptSerializer serialize = new JavaScriptSerializer();
                    var obj = serialize.Deserialize<JsonLogin>(json);
                    if (obj.code == 0)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    LogHelper.Info("登录服务器失败->" + ex.Message);
                    return false;
                }
            });

            return task;
        }
        /// <summary>
        /// 获取账号状态
        /// </summary>
        /// <returns></returns>
        public static Task<JsonStatus> GetAccountStatus()
        {
            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    var url = string.Concat(URL, "/auth/status");
                    var wr = (HttpWebRequest)WebRequest.Create(url);
                    wr.Method = "Get";
                    wr.Headers["cookie"] = cookie;
                    var response = wr.GetResponse();
                    var stream = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
                    var json = sr.ReadToEnd();
                    sr.Close();
                    response.Close();

                    JavaScriptSerializer serialize = new JavaScriptSerializer();
                    var status = serialize.Deserialize<JsonStatus>(json);
                    return status;
                }
                catch (Exception ex)
                {
                    LogHelper.Info("获取账号信息失败->" + ex.Message);
                    return new JsonStatus();
                }
            });
            return task;
        }
        /// <summary>
        /// 比对图片
        /// </summary>
        /// <param name="imagepath1"></param>
        /// <param name="imagepath2"></param>
        /// <returns></returns>
        public static double Compare(string imagepath1, string imagepath2)
        {
            var image1 = File.ReadAllBytes(imagepath1);
            //缩略图
            imagepath2 = ImageTool.ThumbImage(imagepath2);
            var image2 = File.ReadAllBytes(imagepath2);
            return Compare(image1, image2);
        }

        public static double Compare(byte[] image1, byte[] image2)
        {
            var url = string.Concat(URL, "/api/compare");

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Headers["cookie"] = cookie;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            String prefix = "--";
            string end = "\r\n";

            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);
            sb.Append(boundary);
            sb.Append(end);

            //图像一
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" + end;
            string header = string.Format(headerTemplate, "image1", "image1.jpg");
            sb.Append(header);
            sb.Append("Content-Type: application/octet-stream; charset=utf-8" + end);
            sb.Append(end);

            var str = sb.ToString();
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(str);

            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                Stream rs = wr.GetRequestStream();
                rs.Write(headerbytes, 0, headerbytes.Length);

                Write(image1, rs);

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
                headerbytes = System.Text.Encoding.UTF8.GetBytes(str);
                rs.Write(headerbytes, 0, headerbytes.Length);

                Write(image2, rs);

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

                LogHelper.Info("Compare->" + sw.ElapsedMilliseconds);

                JavaScriptSerializer serialze = new JavaScriptSerializer();
                var result = serialze.Deserialize<JsonCompare>(json);
                return result.data.score;
            }
            catch (Exception ex)
            {
                LogHelper.Info("比对调用失败->" + ex.Message);
                return -1;
            }
        }

        private static void Write(byte[] buffer, Stream output)
        {
            using (var input = new MemoryStream(buffer))
            {
                var len = 0;
                var data = new byte[1024];
                while ((len = input.Read(data, 0, data.Length)) > 0)
                {
                    output.Write(data, 0, len);
                }
            }
        }
    }
}
