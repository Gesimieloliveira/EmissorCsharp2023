namespace FusionCore.Helpers.Wmi
{
    public class PhysicalMemory
    {
        public PhysicalMemory(string partNumber, string serial)
        {
            PartNumber = partNumber;
            Serial = serial;
        }

        public string PartNumber { get; }
        public string Serial { get; }
    }
}