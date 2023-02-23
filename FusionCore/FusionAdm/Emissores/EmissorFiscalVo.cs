namespace FusionCore.FusionAdm.Emissores
{
    public struct EmissorFiscalVo
    {
        public byte Id { get; set; }
        public string Descricao { get; set; }

        private bool Equals(EmissorFiscalVo other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is EmissorFiscalVo vo && Equals(vo);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}