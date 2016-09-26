using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Common;
using System.Diagnostics;
using System.Threading;
using Common.Dialog;
using Common.Log;

namespace IDReader
{
    /// <summary>
    /// 身份证阅读器
    /// </summary>
    /// <remarks>
    /// 单实例类
    /// </remarks>
    public class SFZReader
    {
        private Thread _thread = null;
        private bool _isStopRead = false;
        private Action<IDCardInfo> _readIDCardCallback = null;

        private const int Interval = 500;
        private SFZReader()
        {
            IDCardInfo = new IDCardInfo();
        }

        private static SFZReader _instance = null;
        public static SFZReader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SFZReader();

                return _instance;
            }
        }

        public Task<bool> Open()
        {
            var task = Task.Factory.StartNew(() =>
            {
                int usbInterface = 0;
                int comInterface = 0;
                for (var port = 1001; port <= 1016; port++)
                {
                    usbInterface = CVRSDK.CVR_InitComm(port);
                    if (usbInterface == 1)
                    {
                        break;
                    }
                }
                if (usbInterface != 1)
                {
                    for (var port = 1; port <= 4; port++)
                    {
                        comInterface = CVRSDK.CVR_InitComm(port);
                        if (comInterface == 1)
                        {
                            break;
                        }
                    }
                }
                if (usbInterface == 1 || comInterface == 1)
                {
                    StartReadID();
                    return true;
                }
                else
                {
                    return false;
                }
            });
            return task;
        }

        public void Stop()
        {
            _isStopRead = true;
            if (_thread != null)
            {
                _thread.Join();
                _thread = null;
            }
            CVRSDK.CVR_CloseComm();
        }

        public IDCardInfo IDCardInfo
        {
            get; set;
        }

        public void SetReadIDCardCallback(Action<IDCardInfo> callback)
        {
            _readIDCardCallback = callback;
        }

        private void StartReadID()
        {
            _thread = new Thread(doRead);
            _thread.Start();
        }

        private void doRead()
        {
            while (!_isStopRead)
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
                    Thread.Sleep(Interval);
                }
                catch (Exception ex)
                {
                    LogHelper.Info("读取证件信息出错:" + ex.Message);
                }
            }
        }

        private void FillData()
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

            _readIDCardCallback?.Invoke(IDCardInfo);
        }
    }
}
