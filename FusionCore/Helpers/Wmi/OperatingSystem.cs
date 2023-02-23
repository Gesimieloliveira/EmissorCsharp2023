namespace FusionCore.Helpers.Wmi
{
    public struct OperatingSystem
    {
        public OperatingSystem(string caption, int productType)
        {
            Caption = caption;
            ProductType = productType;
        }

        public string Caption { get; }
        public int ProductType { get; }
    }
}