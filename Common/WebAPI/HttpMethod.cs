using Common.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Common.WebAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpMethod : IHttpMethod
    {
        public string APIUrl
        {
            get;
            set;
        }

        public SortedDictionary<string, string> GetRequestParam(string qrcode)
        {
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("content", qrcode);
            sParaTemp.Add("guard", AlipayConfig.TermID);
            sParaTemp.Add("app", AlipayConfig.App);
            return sParaTemp;
        }

        private string GetSign(string qrcode)
        {
            var inputPara = GetRequestParam(qrcode);

            Dictionary<string, string> sPara = new Dictionary<string, string>();
            //过滤空值、sign与sign_type参数
            sPara = Core.FilterPara(inputPara);
            //获取待签名字符串
            string preSignStr = Core.CreateLinkString(sPara);

            var mysign = AlipayMD5.Sign(preSignStr, AlipayConfig.Key, AlipayConfig.Input_charset);

            return mysign;
        }

        public T HttpGet<T>(string qrcode, out string requesterror) where T : class, new()
        {
            var mysign = GetSign(qrcode);

            //真正的url
            var inputPara = GetRequestParam(qrcode);
            inputPara.Add("sign", mysign);
            inputPara.Add("sign_type", AlipayConfig.Sign_type);

            var urlcode = Encoding.GetEncoding(AlipayConfig.Input_charset);
            var preSignStr = Core.CreateLinkStringUrlencode(inputPara, urlcode);

            try
            {
                var httpGetUrl = string.Concat(APIUrl, "?" + preSignStr);
                LogHelper.Info(httpGetUrl);
                var request = (HttpWebRequest)WebRequest.Create(httpGetUrl);
                request.Method = "Get";
                request.ContentType = "application/x-www-form-urlencoded";

                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                var jsonStr = "";
                using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    jsonStr = streamReader.ReadToEnd();
                }
                if (string.IsNullOrEmpty(jsonStr))
                {
                    requesterror = "返回数据为空";
                    return new T();
                }

                var jsonObject = Deserialize<T>(jsonStr);
                requesterror = string.Empty;
                return jsonObject;
            }
            catch (Exception ex)
            {
                requesterror = ex.Message;
                return new T();
            }
        }

        public void Dispose()
        {
        }

        private T Deserialize<T>(string input)
        {
            JavaScriptSerializer serial = new JavaScriptSerializer();
            return serial.Deserialize<T>(input);
        }
    }
}
