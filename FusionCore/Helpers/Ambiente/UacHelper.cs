using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace FusionCore.Helpers.Ambiente
{
    public class UacHelper : IDisposable
    {
        private readonly string _seSystemPrivilege;
        private bool _enableDisable;
        TokenPrivileges _tokenPrivilegesAnterior;
        private readonly IntPtr _process;

        public UacHelper(string seSystemPrivilege)
        {
            _seSystemPrivilege = seSystemPrivilege;
            _enableDisable = true;
            _process = Process.GetCurrentProcess().Handle;
            EnableDisablePrivilege();
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct Luid
        {
            public uint LowPart { get; set; }

            public int HighPart { get; set; }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LuidAndAttributes
        {
            public Luid Luid { get; set; }

            public uint Attributes { get; set; }
        }

        private struct TokenPrivileges
        {
            public uint PrivilegeCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public LuidAndAttributes[] Privileges;
        }

        [DllImport("advapi32", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr processHandle, TokenAccessLevels desiredAccess, out IntPtr tokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AdjustTokenPrivileges(IntPtr tokenHandle, bool disableAllPrivileges, ref TokenPrivileges newState, uint bufferLength, ref TokenPrivileges previousState, out uint returnLength);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out Luid lpLuid);


        private void EnableDisablePrivilege()
        {
            IntPtr htok;

            if (!OpenProcessToken(_process, TokenAccessLevels.AdjustPrivileges | TokenAccessLevels.Query, out htok))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                return;
            }
            var tkp = new TokenPrivileges { PrivilegeCount = 1, Privileges = new LuidAndAttributes[1] };

            Luid luid;
            if (!LookupPrivilegeValue(null, _seSystemPrivilege, out luid))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                return;
            }
            tkp.Privileges[0].Luid = luid;
            tkp.Privileges[0].Attributes = (uint)(_enableDisable ? 2 : 0);

            uint rb;

            if (!AdjustTokenPrivileges(htok, false, ref tkp, 256, ref _tokenPrivilegesAnterior, out rb))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

        public void Dispose()
        {
            _enableDisable = false;
            EnableDisablePrivilege();
        }
    }

}
