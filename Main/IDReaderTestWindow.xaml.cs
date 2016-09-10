using Common;
using Common.Dialog;
using IDReader;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Main
{
    /// <summary>
    /// 身份证阅读器测试
    /// </summary>
    public partial class IDReaderTestWindow
    {
        private int _iRetUsb = 0;
        private int _iRetCom = 0;
        private DispatcherTimer _readTimer;

        public IDReaderTestWindow()
        {
            InitializeComponent();
            IDCardInfo = new IDReader.IDCardInfo();
            this.Loaded += IDReaderTestWindow_Loaded;
            this.Closing += IDReaderTestWindow_Closing;

            gdIDInfo.DataContext = IDCardInfo;
        }
        /// <summary>
        /// 身份证信息
        /// </summary>
        public IDCardInfo IDCardInfo
        {
            get; set;
        }

        private void IDReaderTestWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _readTimer.Stop();
            CVRSDK.CVR_CloseComm();
        }

        private void IDReaderTestWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _readTimer = new DispatcherTimer();
            _readTimer.Tick += _readTimer_Tick;
        }

        private void _readTimer_Tick(object sender, EventArgs e)
        {
            int authenticate = CVRSDK.CVR_Authenticate();
            if (authenticate == 1)
            {
                int readContent = CVRSDK.CVR_Read_Content(4);
                if (readContent == 1)
                {
                    FillData();
                }
            }
        }

        private void FillData()
        {
            try
            {
                byte[] name = new byte[30];
                int length = 30;
                CVRSDK.GetPeopleName(ref name[0], ref length);
                byte[] number = new byte[30];
                length = 36;
                CVRSDK.GetPeopleIDCode(ref number[0], ref length);
                byte[] people = new byte[30];
                length = 3;
                CVRSDK.GetPeopleNation(ref people[0], ref length);
                byte[] validtermOfStart = new byte[30];
                length = 16;
                CVRSDK.GetStartDate(ref validtermOfStart[0], ref length);
                byte[] birthday = new byte[30];
                length = 16;
                CVRSDK.GetPeopleBirthday(ref birthday[0], ref length);
                byte[] address = new byte[128];
                length = 128;
                CVRSDK.GetPeopleAddress(ref address[0], ref length);
                byte[] validtermOfEnd = new byte[30];
                length = 16;
                CVRSDK.GetEndDate(ref validtermOfEnd[0], ref length);
                byte[] signdate = new byte[30];
                length = 30;
                CVRSDK.GetDepartment(ref signdate[0], ref length);
                byte[] sex = new byte[30];
                length = 3;
                CVRSDK.GetPeopleSex(ref sex[0], ref length);

                byte[] samid = new byte[32];
                CVRSDK.CVR_GetSAMID(ref samid[0]);

                IDCardInfo.Name = name.ToGB2312String();
                IDCardInfo.Sex = sex.ToGB2312String();
                IDCardInfo.Nation = people.ToGB2312String();
                IDCardInfo.Number = number.ToGB2312String();
                IDCardInfo.Address = address.ToGB2312String();
                IDCardInfo.Birthday = birthday.ToGB2312String();
                IDCardInfo.StartDate = validtermOfStart.ToGB2312String();
                IDCardInfo.EndDate = validtermOfEnd.ToGB2312String();
                IDCardInfo.Department = signdate.ToGB2312String();
                imgPhoto.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\dll\\zp.bmp", UriKind.Absolute));
            }
            catch (Exception ex)
            {
                CustomDialog.Show(ex.Message);
            }
        }

        private void OpenDevice()
        {
            try
            {
                int iPort;
                for (iPort = 1001; iPort <= 1016; iPort++)
                {
                    _iRetUsb = CVRSDK.CVR_InitComm(iPort);
                    if (_iRetUsb == 1)
                    {
                        Console.WriteLine("打开设备:" + iPort);
                        break;
                    }
                }
                if (_iRetUsb != 1)
                {
                    for (iPort = 1; iPort <= 4; iPort++)
                    {
                        _iRetCom = CVRSDK.CVR_InitComm(iPort);
                        if (_iRetCom == 1)
                        {
                            break;
                        }
                    }
                }
                if (_iRetUsb == 1 || _iRetCom == 1)
                {
                    _readTimer.Interval = TimeSpan.FromMilliseconds(500);
                    _readTimer.Start();
                }
                else
                {
                    CustomDialog.Show("打开设备失败");
                }
            }
            catch (Exception ex)
            {
                //CMessageBox.Show(ex.ToString());
            }
        }

        private void btnOpenID_Click(object sender, RoutedEventArgs e)
        {
            OpenDevice();

            DoubleAnimation opacity = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(5)));
            opacity.AutoReverse = true;
            lblTip.BeginAnimation(Label.OpacityProperty, opacity);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IDCardInfo.Name = "ysj";
            IDCardInfo.Sex = "男";
            IDCardInfo.Number = "2323232";
            IDCardInfo.Nation = "汉";
            IDCardInfo.Birthday = "1981.1.10";
            IDCardInfo.StartDate = "1981.2.10";
            IDCardInfo.EndDate = "1981.10.10";
            IDCardInfo.Department = "江苏昆山";
            IDCardInfo.Address = "江苏省昆山市玉山镇青阳南路135号";
            IDCardInfo.Photo = "F:\\2.png";
        }
    }
}
