using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.WorkFlow
{
    /// <summary>
    /// 欢迎页(欢迎信息)
    /// </summary>
    public partial class WelcomePage : UserControl
    {
        public WelcomePage()
        {
            InitializeComponent();
            this.Loaded += WelcomePage_Loaded;
        }

        private void WelcomePage_Loaded(object sender, RoutedEventArgs e)
        {
            lblRow1.Content = "欢迎光临";
            lblRow2.Content = "请刷身份证";
        }
    }
}
