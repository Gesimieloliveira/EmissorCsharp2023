namespace Fusion.FastReport.DataSources
{
    public struct DsProdutoEstoque
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Grupo { get; set; }
        public decimal Estoque { get; set; }
        public decimal PrecoVenda { get; set; }
        public string CodigoBarras { get; set; }
        public string Referencia { get; set; }
        public string CodigoNcm { get; set; }

        private bool Equals(DsProdutoEstoque other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DsProdutoEstoque estoque && Equals(estoque);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}