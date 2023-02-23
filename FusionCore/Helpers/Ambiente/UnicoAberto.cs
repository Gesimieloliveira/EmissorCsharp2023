using System;
using System.Diagnostics;
using FusionCore.Helpers.Wmi;

namespace FusionCore.Helpers.Ambiente
{
    public static class UnicoAberto
    {
        public static bool IsAppAlreadyRunning(Process process)
        {
            var curerntUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToUpper();

            foreach (var p in Process.GetProcessesByName(process.ProcessName))
            {
                try
                {
                    var processOwner = WmiHelper.GetProcessOwner(p.Id).ToUpper();

                    if (process.Id != p.Id && curerntUser == processOwner && !p.HasExited)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            }

            return false;
        }
    }
}