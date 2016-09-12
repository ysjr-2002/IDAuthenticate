using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public static class FunExtends
    {
        #region Byte方法扩展
        /// <summary>
        /// 字节转换为16进制
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToHex(this byte b)
        {
            return b.ToString("X2");
        }
        /// <summary>
        /// 字节数字转为为16进制字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ToHex(this byte[] buffer)
        {
            var sb = new StringBuilder();
            foreach (var b in buffer)
            {
                sb.Append(b.ToHex() + " ");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 16进制转换为字节
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte FromHexToByte(this string str)
        {
            byte b = 0;
            try
            {
                b = Convert.ToByte(str, 16);
            }
            catch { }
            return b;
        }
        /// <summary>
        /// 字符串转换为字节
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte ToByte(this string str)
        {
            byte b = 0;
            byte.TryParse(str, out b);
            return b;
        }
        /// <summary>
        /// 字节转换为位组
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BitArray ToBitArray(this byte b)
        {
            return new BitArray(new byte[] { b });
        }
        #endregion

        #region Int16方法扩展
        public static byte[] ToBytes(this Int16 val)
        {
            var bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            //var bytes = new byte[2];
            //bytes[0] = ((val & 0xFF00) >> 8).ToByte();
            //bytes[1] = (val & 0xFF).ToByte();
            return bytes;
        }
        #endregion

        #region Int32方法扩展
        /// <summary>
        /// 16进制转换为Int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int HexToInt32(this string str)
        {
            var b = 0;
            try
            {
                b = Convert.ToInt32(str, 16);
            }
            catch { }
            return b;
        }
        /// <summary>
        /// 字符转换Int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this string str)
        {
            UInt32 ret = 0;
            UInt32.TryParse(str, out ret);
            return ret;
        }
        /// <summary>
        /// 字符转换Int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt32(this string str)
        {
            var ret = 0;
            Int32.TryParse(str, out ret);
            return ret;
        }
        /// <summary>
        /// 字符串转为Int64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this string str)
        {
            Int64 ret = 0;
            Int64.TryParse(str, out ret);
            return ret;
        }
        /// <summary>
        /// 数值对象转换为Int32
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt32(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static byte ToByte(this int val)
        {
            return (byte)val;
        }
        /// <summary>
        /// 返回4字节数组
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] ToByte4(this int i)
        {
            byte[] buffer = new byte[4];
            buffer[3] = (byte)(i & 0xFF);
            buffer[2] = (byte)(i >> 8 & 0xFF);
            buffer[1] = (byte)(i >> 16 & 0xFF);
            buffer[0] = (byte)(i >> 24 & 0xFF);
            return buffer;
        }
        public static byte[] ToByte4(this UInt32 i)
        {
            byte[] buffer = new byte[4];
            buffer[3] = (byte)(i & 0xFF);
            buffer[2] = (byte)(i >> 8 & 0xFF);
            buffer[1] = (byte)(i >> 16 & 0xFF);
            buffer[0] = (byte)(i >> 24 & 0xFF);
            return buffer;
        }
        #endregion

        #region Char方法扩展
        public static byte HexCharToByte(this char c)
        {
            byte b = 0;
            try
            {
                b = Convert.ToByte(c.ToString(), 16);
            }
            catch { }
            return b;
        }
        #endregion

        #region Float扩展方法
        public static byte[] ToBytes(this float f)
        {
            var buffer = BitConverter.GetBytes(f);
            //Array.Reverse(buffer);
            return buffer;
        }
        #endregion

        #region String方法扩展
        /// <summary>
        /// 通讯时处理，转换字符后有时以'\0'结尾
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CommProcess(this string str)
        {
            return str.Replace("\0", "").Trim();
        }
        /// <summary>
        /// 车牌特殊字符替换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToPlate(this string str)
        {
            //o和O替换为0
            //I替换为1
            str = str.Replace("o", "0").Replace("O", "0");
            str = str.Replace("I", "1");
            return str;
        }
        /// <summary>
        /// 数据库操作时调用
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Process(this string str)
        {
            var s = str.Trim();
            return s;
        }
        /// <summary>
        /// 日期字符串转换为DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            DateTime date = DateTime.MinValue;
            if (!DateTime.TryParse(str, out date))
                date = DateTime.Now;
            return date;
        }
        /// <summary>
        /// 日期格式为：20140110
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToCustomDateTime(this string str)
        {
            DateTime date = new DateTime(1970, 1, 1);
            if (str.Length != 8)
                return date;

            var newDate = string.Format("{0}-{1}-{2}", str.Substring(0, 4), str.Substring(4, 2), str.Substring(6, 2));
            DateTime.TryParse(newDate, out date);
            return date;
        }
        /// <summary>
        /// 合法Email地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static bool IsEmail(this string str)
        //{
        //    Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        //    return regex.IsMatch(str);
        //}
        /// <summary>
        /// 数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsOnlyNumber(this string value)
        {
            Regex r = new Regex(@"^[0-9]+$");
            return r.Match(value).Success;
        }
        /// <summary>
        /// 浮点数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsOnlyNumberDot(this string value)
        {
            Regex r = new Regex(@"^\d+[.]?\d*$");
            return r.Match(value).Success;
        }
        /// <summary>
        /// 转换为Float
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
        {
            var f = 0.0f;
            float.TryParse(str, out f);
            return f;
        }
        /// <summary>
        /// 转换为Decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str)
        {
            decimal d = 0;
            decimal.TryParse(str, out d);
            return d;
        }
        /// <summary>
        /// 转换为Double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            double d = 0;
            double.TryParse(str, out d);
            return d;
        }
        /// <summary>
        /// 字符串MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(this string str)
        {
            byte[] buffer = Encoding.Default.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(buffer);
            return Convert.ToBase64String(output);
        }
        /// <summary>
        /// 图片文件转换为字节数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ToImageByte(this string path)
        {
            if (File.Exists(path) == false)
                return null;

            var ext = Path.GetExtension(path).ToUpper();
            if (ext != ".JPG" && ext != ".JPEG")
                return null;

            byte[] buffer = null;
            try
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                }
            }
            catch { }
            return buffer;
        }
        /// <summary>
        /// 字符串ASCII
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToASCII(this string value)
        {
            if (value.IsEmpty())
                return null;

            return Encoding.ASCII.GetBytes(value);
        }
        /// <summary>
        /// 字符串GB2312
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToGB2312(this string value)
        {
            //if (value.IsEmpty())
            //    return null;

            return Encoding.GetEncoding("GB2312").GetBytes(value);
        }
        /// <summary>
        /// 字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 转全角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(this string input)
        {
            // 半角转全角：
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 32)
                {
                    array[i] = (char)12288;
                    continue;
                }
                if (array[i] < 127)
                {
                    array[i] = (char)(array[i] + 65248);
                }
            }
            return new string(array);
        }
        /// <summary>
        /// 转半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(this string input)
        {
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 12288)
                {
                    array[i] = (char)32;
                    continue;
                }
                if (array[i] > 65280 && array[i] < 65375)
                {
                    array[i] = (char)(array[i] - 65248);
                }
            }
            return new string(array);
        }
        #endregion

        #region byte[] 扩展方法
        /// <summary>
        /// 按指定编码格式，将数组转换为字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string BufferToString(this byte[] buffer, Encoding encode)
        {
            return encode.GetString(buffer).CommProcess();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ToUTF8String(this byte[] buffer)
        {
            return buffer.BufferToString(Encoding.UTF8);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ToGBString(this byte[] buffer)
        {
            return buffer.BufferToString(Encoding.Default);
        }

        public static string ToAscII(this byte[] buffer)
        {
            var code = Encoding.ASCII.GetString(buffer);
            code = code.Remove(code.IndexOf((char)0));
            return code;
        }
        #endregion

        #region Datetime 扩展方法
        /// <summary>
        /// 缩减版年月日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToYmd(this DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }
        /// <summary>
        /// 短日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToShortDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToShortTime(this DateTime date)
        {
            return date.ToString("HH:mm:ss");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToHM(this DateTime date)
        {
            return date.ToString("HH:mm");
        }
        /// <summary>
        /// 长日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToStandard(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 年-月-日 时:分
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToYmdhm(this DateTime date)
        {
            return date.ToString("yy-MM-dd HH:mm");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToShotShow(this DateTime date)
        {
            return date.ToString("yy/MM/dd HH:mm:ss");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToFileName(this DateTime date)
        {
            ///应对多次抓拍的情况，需要毫秒
            return date.ToString("yyyyMMddHHmmssfff");
        }
        /// <summary>
        /// 日期起始
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetDayStart(this DateTime date)
        {
            return date.Date;
        }
        /// <summary>
        /// 日期最大
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetDayEnd(this DateTime date)
        {
            return date.Date.AddDays(1).AddSeconds(-1);
        }
        /// <summary>
        /// 周日开始
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime WeekOfStartDay(this DateTime date)
        {
            int today = (int)date.DayOfWeek;
            return date.AddDays(-today);
        }
        /// <summary>
        /// 周日结束
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime WeekOfEndDay(this DateTime date)
        {
            int today = (int)date.DayOfWeek;
            return date.AddDays(6 - today);
        }
        /// <summary>
        /// 月开始
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime MonthOfFirstDay(this DateTime date)
        {
            return date.AddDays(1 - date.Day).GetDayStart();
        }
        /// <summary>
        /// 月截止
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime MonthOfEndDay(this DateTime date)
        {
            return date.AddDays(1 - date.Day).AddMonths(1).AddDays(-1).GetDayEnd();
        }
        /// <summary>
        /// 季度开始
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime SeasonOfFirstDay(this DateTime date)
        {
            return DateTime.Parse(date.AddMonths(0 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01"));
        }
        /// <summary>
        /// 季度结束
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime SeasonOfEndDay(this DateTime date)
        {
            return DateTime.Parse(date.AddMonths(3 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01")).AddDays(-1);
        }
        #endregion

        #region DataTable[]扩展方法
        public static void Init<T>(this T[] arr) where T : new()
        {
            if (arr == null)
                return;

            var len = arr.Length;
            for (var i = 0; i < len; i++)
            {
                arr[i] = new T();
            }
        }
        #endregion

        #region TextBox 扩展方法
        
        #endregion

        #region BitArray 扩展方法
        public static byte ConvertToByte(BitArray bits)
        {
            if (bits.Count > 8)
                throw new ArgumentException("ConvertToByte can only work with a BitArray containing a maximum of 8 values");

            byte result = 0;

            for (byte i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    result |= (byte)(1 << i);
            }
            return result;
        }
        #endregion

        #region TimeSpan 扩展方法
        public static string Formate(this TimeSpan ts)
        {
            var dTotalHours = ts.TotalMinutes / 60;
            dTotalHours = Math.Floor(dTotalHours);
            var iTotalHours = Math.Floor(dTotalHours).ToInt32();
            return string.Format("{0:d2}时{1:d2}分", iTotalHours, ts.Minutes);
        }
        #endregion

        #region float扩展方法
        public static string ToMoney(this float f)
        {
            return f.ToString("0.00");
        }
        #endregion

        #region Decimal扩展方法
        public static string ToMoney(this decimal money)
        {
            return money.ToString("0.00");
        }
        #endregion

        #region double扩展方法
        public static string ToMoney(this double d)
        {
            return d.ToString("0.00");
        }
        #endregion
    }
}
