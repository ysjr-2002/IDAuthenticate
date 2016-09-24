using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAPI
{
    /// <summary>
    /// 图像处理工具
    /// </summary>
    public static class ImageTool
    {
        /// <summary>
        /// 获取图像缩略图
        /// </summary>
        /// <param name="imagepath"></param>
        /// <returns></returns>
        public static string ThumbImage(string imagepath)
        {
            var folder = System.IO.Path.GetDirectoryName(imagepath);
            System.Drawing.Image sourceImage = Bitmap.FromFile(imagepath);

            var width = 160;
            var height = 90;

            KeepRatio(sourceImage.Size, ref width, ref height);

            System.Drawing.Image thumbnailImage = sourceImage.GetThumbnailImage(width, height, null, IntPtr.Zero);
            var savePath = Path.Combine(folder, "thumb.jpg");
            File.Delete(savePath);
            thumbnailImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            thumbnailImage.Dispose();

            return savePath;
        }

        private static void KeepRatio(System.Drawing.Size size, ref int width, ref int height)
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
    }
}
