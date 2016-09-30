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
            try
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
            catch (Exception)
            {
            }
        }

        private void FillData()
        {
            try
            {
                //姓名
                byte[] name = new byte[30];
                int length = 30;
                CVRSDK.GetPeopleName(ref name[0], ref length);

                //号码
                byte[] number = new byte[30];
                length = 36;
                CVRSDK.GetPeopleIDCode(ref number[0], ref length);

                //民族
                byte[] people = new byte[30];
                length = 3;
                CVRSDK.GetPeopleNation(ref people[0], ref length);

                //有效起始日期
                byte[] validtermOfStart = new byte[30];
                length = 16;
                CVRSDK.GetStartDate(ref validtermOfStart[0], ref length);

                //生日
                byte[] birthday = new byte[30];
                length = 16;
                CVRSDK.GetPeopleBirthday(ref birthday[0], ref length);

                //住址
                byte[] address = new byte[128];
                length = 128;
                CVRSDK.GetPeopleAddress(ref address[0], ref length);

                //有效终止期
                byte[] validtermOfEnd = new byte[30];
                length = 16;
                CVRSDK.GetEndDate(ref validtermOfEnd[0], ref length);

                //签发机关
                byte[] signdate = new byte[30];
                length = 30;
                CVRSDK.GetDepartment(ref signdate[0], ref length);

                //性别
                byte[] sex = new byte[30];
                length = 3;
                CVRSDK.GetPeopleSex(ref sex[0], ref length);

                //安全模块
                byte[] samid = new byte[32];
                CVRSDK.CVR_GetSAMID(ref samid[0]);

                IDCardInfo.Name = name.ToGBString();
                IDCardInfo.Sex = sex.ToGBString();
                IDCardInfo.Nation = people.ToGBString();
                IDCardInfo.Number = number.ToGBString();
                IDCardInfo.Address = address.ToGBString();
                IDCardInfo.Birthday = birthday.ToGBString();
                IDCardInfo.StartDate = validtermOfStart.ToGBString();
                IDCardInfo.EndDate = validtermOfEnd.ToGBString();
                IDCardInfo.Department = signdate.ToGBString();

                var zpPath = Environment.CurrentDirectory + "\\dll\\zp.bmp";
                imgPhoto.Source = Funs.ToBitmapSource(zpPath);
                //System.IO.File.Delete(zpPath);
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
                    btnOpen.IsEnabled = false;
                }
                else
                {
                    CustomDialog.Show("打开设备失败");
                }
            }
            catch (Exception ex)
            {
                CustomDialog.Show(ex.ToString());
            }
        }

        private void btnOpenID_Click(object sender, RoutedEventArgs e)
        {
            OpenDevice();

            DoubleAnimation opacity = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(5)));
            opacity.AutoReverse = true;
            lblTip.BeginAnimation(Label.OpacityProperty, opacity);

            btnOpen.IsEnabled = false;
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
