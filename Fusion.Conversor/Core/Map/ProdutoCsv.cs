namespace Fusion.Conversor.Core.Map
{
    public sealed class ProdutoCsv
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Estoque { get; set; }
        public string Grupo { get; set; }
        public string SiglaUnidade { get; set; }
        public string PrecoCusto { get; set; }
        public string PrecoCompra { get; set; }
        public string MargemLucro { get; set; }
        public string PrecoVenda { get; set; }
        public string CodigoBarra { get; set; }
        public string Ncm { get; set; }
        public string Cest { get; set; }
        public string CodigoCst { get; set; }
        public string AliquotaCst { get; set; }
        public string CodigoIpi { get; set; }
        public string AliquotaIpi { get; set; }
        public string CodigoPis { get; set; }
        public string AliquotaPis { get; set; }
        public string CodigoCofins { get; set; }
        public string AliquotaCofins { get; set; }
        public string Referencia { get; set; }
        public string CodigoBalanca { get; set; }
        public string CodigoAnp { get; set; }
        public string CodigoEnquadramentoIpi { get; set; }
        public string PercentualMva { get; set; }
        public string ReducaoIcms { get; set; }
        public string Observacao { get; set; }

        public override string ToString()
        {
            return $"cod: {Codigo}, nome: {Nome}";
        }
    }
}