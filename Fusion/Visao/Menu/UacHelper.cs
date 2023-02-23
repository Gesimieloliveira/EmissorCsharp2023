using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Fusion.Visao.Login;
using Microsoft.Win32;

namespace Fusion.Visao.Menu
{
    public static class UacHelper
    {
        private static uint STANDARD_RIGHTS_READ = 0x00020000;
        private static uint TOKEN_QUERY = 0x00000008;
        private const uint TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        


        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID_AND_ATTRIBUTES
        {
            public LUID Luid;
            public UInt32 Attributes;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public UInt32 PrivilegeCount;
            public LUID Luid;
            public UInt32 Attributes;
        }




        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, UInt32 BufferLengthInBytes, ref TOKEN_PRIVILEGES PreviousState, out UInt32 ReturnLengthInBytes);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool LookupPrivilegeValue(string lpSystemName, string lpName,
    out LUID lpLuid);


        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            [MarshalAs(UnmanagedType.U2)]
            public short Year;
            [MarshalAs(UnmanagedType.U2)]
            public short Month;
            [MarshalAs(UnmanagedType.U2)]
            public short DayOfWeek;
            [MarshalAs(UnmanagedType.U2)]
            public short Day;
            [MarshalAs(UnmanagedType.U2)]
            public short Hour;
            [MarshalAs(UnmanagedType.U2)]
            public short Minute;
            [MarshalAs(UnmanagedType.U2)]
            public short Second;
            [MarshalAs(UnmanagedType.U2)]
            public short Milliseconds;

            public SYSTEMTIME(DateTime dt)
            {
                dt = dt.ToUniversalTime();  // SetSystemTime expects the SYSTEMTIME in UTC
                Year = (short)dt.Year;
                Month = (short)dt.Month;
                DayOfWeek = (short)dt.DayOfWeek;
                Day = (short)dt.Day;
                Hour = (short)dt.Hour;
                Minute = (short)dt.Minute;
                Second = (short)dt.Second;
                Milliseconds = (short)dt.Millisecond;
            }
        }

        public static void TrocarAcesso()
        {
            IntPtr tokenHandle;
            var luid = new LUID();
            var tp = new TOKEN_PRIVILEGES();
            var oldTp = new TOKEN_PRIVILEGES();
            uint dwSize;


            var openprocess = OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY,
                out tokenHandle);

            if (!openprocess)
            {
                Console.WriteLine("LookupPrivilegeValue() failed, error = {0} .SeDebugPrivilege is not available", Marshal.GetLastWin32Error());
            }

            var lookup = LookupPrivilegeValue(null, "SeSystemtimePrivilege", out luid);

            if (!lookup)
            {
                Console.WriteLine("LookupPrivilegeValue() failed, error = {0} .SeDebugPrivilege is not available", Marshal.GetLastWin32Error());
            }

            tp.PrivilegeCount = 1;
            tp.Luid = luid;
            tp.Attributes = SE_PRIVILEGE_ENABLED;

            var adjustToke = AdjustTokenPrivileges(tokenHandle, false, ref tp, (uint)Marshal.SizeOf(typeof(TOKEN_PRIVILEGES)), ref oldTp, out dwSize);

            if (!adjustToke)
            {
                Console.WriteLine("LookupPrivilegeValue() failed, error = {0} .SeDebugPrivilege is not available", Marshal.GetLastWin32Error());
            }


            // Set system date and time
            var updatedTime = new SYSTEMTIME();
            updatedTime.Year = 2016;
            updatedTime.Month = 3;
            updatedTime.Day = 16;
            updatedTime.Hour = 10;
            updatedTime.Minute = 0;
            updatedTime.Second = 0;
            // Call the unmanaged function that sets the new date and time instantly

            bool sucesso = SetSystemTime(ref updatedTime);

            var erro = GetLastError();
        }










        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetSystemTime(ref SYSTEMTIME time);

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();










    }

}
