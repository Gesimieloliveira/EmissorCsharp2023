namespace FusionCore.Helpers.Wmi
{
    public class Win32Processor
    {
        public Win32Processor(string name, string caption)
        {
            Name = name;
            Caption = caption;
        }

        public string Name { get; }
        public string Caption { get; }
    }
}