using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace FusionCore.Helpers.Wmi
{
    public static class WmiHelper
    {
        public static DiskDrive GetDiskDriveZero()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive");
                var management = searcher.Get().Cast<ManagementBaseObject>();

                foreach (var drive in management)
                {
                    var deviceId = drive["DeviceID"]?.ToString().Trim().ToUpper() ?? string.Empty;

                    if (deviceId.Contains("DRIVE0"))
                    {
                        var serial = (string)drive["SerialNumber"];
                        var model = (string)drive["Model"];

                        return new DiskDrive(serial, model);
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Falha ao acessar Win32_DiskDrive {e.Message}", e);
                throw;
            }

            throw new InvalidOperationException("Não foi encontrado DRIVE0");
        }

        public static IEnumerable<PhysicalMemory> GetPhysicalMemory()
        {
            IEnumerable<ManagementBaseObject> objects;

            try
            {
                var searcher = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
                objects = searcher.Get().Cast<ManagementBaseObject>();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Falha ao acessar Win32_PhysicalMemory {e.Message}", e);
            }

            foreach (var i in objects)
            {
                var partNumber = i["PartNumber"]?.ToString() ?? "";
                var serialNumber = i["SerialNumber"]?.ToString() ?? "";

                yield return new PhysicalMemory(partNumber, serialNumber);
            }
        }

        public static IEnumerable<Win32Processor> GetWin32Processor()
        {
            IEnumerable<ManagementBaseObject> objects;

            try
            {
                var searcher = new ManagementObjectSearcher("select * from Win32_Processor");
                objects = searcher.Get().Cast<ManagementBaseObject>();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Falha ao acessar Win32_Processor {e.Message}", e);
            }

            foreach (var i in objects)
            {
                var name = i["Name"]?.ToString() ?? "";
                var caption = i["Caption"]?.ToString() ?? "";

                yield return new Win32Processor(name, caption);
            }
        }

        public static ComputerSystemProduct GetComputerSystemProduct()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystemProduct");

                foreach (var managementBaseObject in searcher.Get())
                {
                    var mo = (ManagementObject)managementBaseObject;

                    var nome = (string)mo["Name"];
                    var uuid = (string)mo["UUID"];

                    return new ComputerSystemProduct(nome, uuid);
                }
            }
            catch (ManagementException e)
            {
                throw new InvalidOperationException($"Falha ao acessar Win32_ComputerSystemProduct {e.Message}", e);
            }

            throw new InvalidOperationException("Não foi encontrado Win32_ComputerSystemProduct");
        }

        public static OperatingSystem GetOperatingSystem()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");

                foreach (var managementBaseObject in searcher.Get())
                {
                    var mo = (ManagementObject)managementBaseObject;

                    var nome = (string)mo["Caption"];
                    var type = int.Parse(mo["ProductType"].ToString());

                    return new OperatingSystem(nome, type);
                }
            }
            catch (ManagementException e)
            {
                throw new InvalidOperationException($"Falha ao acessar Win32_OperatingSystem {e.Message}", e);
            }

            throw new InvalidOperationException("Não foi encontrado Win32_OperatingSystem");
        }

        public static string GetProcessOwner(int processId)
        {
            var query = "Select * From Win32_Process Where ProcessID = " + processId;
            var searcher = new ManagementObjectSearcher(query);
            var processList = searcher.Get();

            foreach (var o in processList)
            {
                var obj = (ManagementObject)o;
                object[] argList = { string.Empty, string.Empty };

                var returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));

                if (returnVal == 0)
                {
                    return argList[1] + "\\" + argList[0];
                }
            }

            return "NO OWNER";
        }
    }
}