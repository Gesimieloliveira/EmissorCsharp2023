namespace FusionCore.Helpers.Wmi
{
    public class ComputerSystemProduct
    {
        public ComputerSystemProduct(string name, string uuid)
        {
            Name = name;
            UUID = uuid;
        }

        public string Name { get; }
        public string UUID { get; }
    }
}