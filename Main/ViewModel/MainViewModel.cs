using CloudAPI;
using Common;
using Common.Dialog;
using Common.NotifyBase;
using IDReader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Main.ViewModel
{
    class MainViewModel : PropertyNotifyObject
    {
        private DispatcherTimer _readTimer = null;

        private MainViewModel()
        {
            IDCardInfo = new IDCardInfo();
            timeout = new FuncTimeout();
        }

        private static MainViewModel _instance = new MainViewModel();
        public static MainViewModel Instance
        {
            get
            {
                return _instance;
            }
        }

        public ucUsbCamera UsbCamera
        {
            get; set;
        }

        public IDCardInfo IDCardInfo
        {
            get; set;
        }

        public Visibility WelcomeVisibility
        {
            get { return this.GetValue(s => s.WelcomeVisibility); }
            set { this.SetValue(s => s.WelcomeVisibility, value); }
        }

        public Visibility SZFVisibility
        {
            get { return this.GetValue(s => s.SZFVisibility); }
            set { this.SetValue(s => s.SZFVisibility, value); }
        }


        public Visibility PassVisibility
        {
            get { return this.GetValue(s => s.PassVisibility); }
            set { this.SetValue(s => s.PassVisibility, value); }
        }

        public string CompareResult
        {
            get { return this.GetValue(s => s.CompareResult); }
            set { this.SetValue(s => s.CompareResult, value); }
        }


        public void Init()
        {
            var open = SFZReader.Instance.Open();
            if (open)
            {
                _readTimer = new DispatcherTimer();
                _readTimer.Tick += ReadCard_Tick;
                _readTimer.Interval = TimeSpan.FromMilliseconds(500);
                _readTimer.Start();
            }
            else
            {
            }

            var login = MegviiCloud.Login();
            if (login)
            {
                Console.WriteLine("登录成功");
            }
            else
            {
                Console.WriteLine("登录失败");
            }
        }

        private void ReadCard_Tick(object sender, EventArgs e)
        {
            try
            {
                int authenticate = CVRSDK.CVR_Authenticate();
                if (authenticate == 1)
                {
                    int readContent = CVRSDK.CVR_Read_Content(4);
                    if (readContent == 1)
                    {
                        CompareResult = "读取证件...";
                        Stopwatch sw = Stopwatch.StartNew();
                        FillData();
                        sw.Stop();
                        Console.WriteLine("read card:" + sw.ElapsedMilliseconds);
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

                Console.WriteLine(IDCardInfo.Name);

                System.Threading.Tasks.Task.Factory.StartNew(Compare);

            }
            catch (Exception ex)
            {
                CustomDialog.Show(ex.Message);
            }
        }

        private FuncTimeout timeout = null;
        private void Compare()
        {
            CompareResult = "抓拍人脸，请稍等...";

            var image2 = UsbCamera.Snap();
            Thread.Sleep(500);
            CompareResult = "正在比对，请稍等...";

            var image1 = Environment.CurrentDirectory + "\\dll\\zp.bmp";

            var score = (int)MegviiCloud.Compare(image1, image2);

            if (score > 78)
            {
                CompareResult = "比对通过->" + score;
            }
            else
            {
                CompareResult = "比对失败->" + score;
            }

            timeout.StartOnce(2000, () =>
            {
                CompareResult = "";
            });
        }




        public void Work()
        {

        }

        public void Stop()
        {

        }

        private void OnReadIDCardInfo(IDCardInfo card)
        {

        }
    }
}
