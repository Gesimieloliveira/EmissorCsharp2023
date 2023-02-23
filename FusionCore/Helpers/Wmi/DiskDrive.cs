namespace FusionCore.Helpers.Wmi
{
    public class DiskDrive
    {
        public DiskDrive(string serialNumber, string model)
        {
            SerialNumber = serialNumber?.Trim();
            Model = model?.Trim();
        }

        public string SerialNumber { get; }
        public string Model { get; }
    }
}