using Common.NotifyBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IDReader
{
    /// <summary>
    /// 证件信息
    /// </summary>
    public class IDCardInfo : PropertyNotifyObject
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get { return this.GetValue(s => s.Name); }
            set { this.SetValue(s => s.Name, value); }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex
        {
            get { return this.GetValue(s => s.Sex); }
            set { this.SetValue(s => s.Sex, value); }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address
        {
            get { return this.GetValue(s => s.Address); }
            set { this.SetValue(s => s.Address, value); }
        }
        /// <summary>
        /// 号码
        /// </summary>
        public string Number
        {
            get { return this.GetValue(s => s.Number); }
            set { this.SetValue(s => s.Number, value); }
        }
        /// <summary>
        /// 民族
        /// </summary>
        public string Nation
        {
            get { return this.GetValue(s => s.Nation); }
            set { this.SetValue(s => s.Nation, value); }
        }
        /// <summary>
        /// 有效起始日期
        /// </summary>
        public string StartDate
        {
            get { return this.GetValue(s => s.StartDate); }
            set { this.SetValue(s => s.StartDate, value); }
        }
        /// <summary>
        /// 有效截至日期
        /// </summary>
        public string EndDate
        {
            get { return this.GetValue(s => s.EndDate); }
            set { this.SetValue(s => s.EndDate, value); }
        }
        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday
        {
            get { return this.GetValue(s => s.Birthday); }
            set { this.SetValue(s => s.Birthday, value); }
        }
        /// <summary>
        /// 签发机关
        /// </summary>
        public string Department
        {
            get { return this.GetValue(s => s.Department); }
            set { this.SetValue(s => s.Department, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Photo
        {
            get { return this.GetValue(s => s.Photo); }
            set { this.SetValue(s => s.Photo, value); }
        }


        public ImageSource Face
        {
            get { return this.GetValue(s => s.Face); }
            set { this.SetValue(s => s.Face, value); }
        }
    }
}
