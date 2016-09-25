using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.IO;

namespace Main.ViewModel
{
    /// <summary>
    /// 文件管理器
    /// </summary>
    public static class FileManager
    {
        private const string root = "D:\\face";

        static FileManager()
        {
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
        }

        public static string GetFolder()
        {
            var day = DateTime.Now.ToString("yyyyMMdd");
            var folder = Path.Combine(root, day);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }
    }
}
