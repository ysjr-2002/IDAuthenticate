using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModel
{
    /// <summary>
    /// 人脸比对
    /// </summary>
    public class JsonCompare
    {
        public int code { get; set; }

        public data data { get; set; }
    }

    public class data
    {
        public face face1 { get; set; }

        public face face2 { get; set; }
        /// <summary>
        /// 两个人脸的相似度 0-100
        /// </summary>
        public double score { get; set; }
    }

    public class face
    {
        public attrs attrs { get; set; }

        public double quality { get; set; }

        public rect rect { get; set; }
    }

    public class attrs
    {
        /// <summary>
        /// 高斯模糊 0-1
        /// </summary>
        public float gaussian { get; set; }
        /// <summary>
        /// 动态模糊 0-1
        /// </summary>
        public float motion { get; set; }
        /// <summary>
        /// 上下倾斜幅度 0-1
        /// </summary>
        public float pitch { get; set; }
        /// <summary>
        /// 左右倾斜幅度 0-1
        /// </summary>
        public float yaw { get; set; }
    }

    public class rect
    {
        /// <summary>
        /// 人脸框的高度
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// 人脸框左侧在图片中的位置
        /// </summary>
        public int left { get; set; }
        /// <summary>
        /// 人脸框上方在图片中的位置
        /// </summary>
        public int top { get; set; }
        /// <summary>
        /// 人脸框的宽度
        /// </summary>
        public int width { get; set; }
    }
}
