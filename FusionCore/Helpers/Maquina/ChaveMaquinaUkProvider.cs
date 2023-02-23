using System.Collections.Generic;
using System.Linq;
using System.Text;
using FusionCore.Helpers.Wmi;

namespace FusionCore.Helpers.Maquina
{
    public static class ChaveMaquinaUkProvider
    {
        private const int WinServer = 3;

        public static string ProvideUk()
        {
            var os = WmiHelper.GetOperatingSystem();
            var csp = WmiHelper.GetComputerSystemProduct();
            var disk = WmiHelper.GetDiskDriveZero();

            return os.ProductType == WinServer
                ? $"uk://{csp.UUID}:{disk.Model}"
                : $"uk={disk.Model}:{disk.SerialNumber}";
        }

        public static string ProvieVersion2Uk()
        {
            var disk = WmiHelper.GetDiskDriveZero();
            var memories = WmiHelper.GetPhysicalMemory();
            var processors = WmiHelper.GetWin32Processor();

            var ukString = $"uk://{GetProcessorString(processors)}:{GetMemoryString(memories)}:{disk.Model}&&{disk.SerialNumber}";

            return ukString;
        }

        private static string GetProcessorString(IEnumerable<Win32Processor> processors)
        {
            var values = processors.Select(p => $"{p.Caption}-{p.Name}");
            return string.Join("&&", values);
        }

        private static string GetMemoryString(IEnumerable<PhysicalMemory> memories)
        {
            var values = memories.Select(pm => $"{pm.PartNumber}-{pm.Serial}");
            return string.Join("&&", values);
        }
    }
}