using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            UsbCamera form = new Main.UsbCamera();
            System.Windows.Forms.Application.Run(form);
        }
    }
}
