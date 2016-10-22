using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RebootMachineAPI
    {
        const int TOKEN_QUERY = 0x00000008;
        const int SE_PRIVILEGE_ENABLED = 0x00000002;
        const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ExitWindowsEx(ExitWindows uFlags, ShutdownReason dwReason);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall, ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        /// <summary>
        /// Shutdown
        /// </summary>
        public static void ShuwDown()
        {
            Process.Start("shutdown", "-r -f -t 5");
        }
        /// <summary>
        /// ExitWindow
        /// </summary>
        public static void ExitWindow()
        {
            TokPriv1Luid tp;
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = SE_PRIVILEGE_ENABLED;
            LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);
            AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            ExitWindowsEx(ExitWindows.Reboot, ShutdownReason.MajorOther);
        }
    }

    [Flags]
    enum ExitWindows : uint
    {
        // ONE of the following five:
        LogOff = 0x00,
        ShutDown = 0x01,
        Reboot = 0x02,
        PowerOff = 0x08,
        RestartApps = 0x40,
        // plus AT MOST ONE of the following two:
        Force = 0x04,
        ForceIfHung = 0x10,
    }


    [Flags]
    enum ShutdownReason : uint
    {
        MajorApplication = 0x00040000,
        MajorHardware = 0x00010000,
        MajorLegacyApi = 0x00070000,
        MajorOperatingSystem = 0x00020000,
        MajorOther = 0x00000000,
        MajorPower = 0x00060000,
        MajorSoftware = 0x00030000,
        MajorSystem = 0x00050000,

        MinorBlueScreen = 0x0000000F,
        MinorCordUnplugged = 0x0000000b,
        MinorDisk = 0x00000007,
        MinorEnvironment = 0x0000000c,
        MinorHardwareDriver = 0x0000000d,
        MinorHotfix = 0x00000011,
        MinorHung = 0x00000005,
        MinorInstallation = 0x00000002,
        MinorMaintenance = 0x00000001,
        MinorMMC = 0x00000019,
        MinorNetworkConnectivity = 0x00000014,
        MinorNetworkCard = 0x00000009,
        MinorOther = 0x00000000,
        MinorOtherDriver = 0x0000000e,
        MinorPowerSupply = 0x0000000a,
        MinorProcessor = 0x00000008,
        MinorReconfig = 0x00000004,
        MinorSecurity = 0x00000013,
        MinorSecurityFix = 0x00000012,
        MinorSecurityFixUninstall = 0x00000018,
        MinorServicePack = 0x00000010,
        MinorServicePackUninstall = 0x00000016,
        MinorTermSrv = 0x00000020,
        MinorUnstable = 0x00000006,
        MinorUpgrade = 0x00000003,
        MinorWMI = 0x00000015,

        FlagUserDefined = 0x40000000,
        FlagPlanned = 0x80000000
    }
}
