namespace FusionCore.Sintegra.Dto
{
    public class Registro75Dto : IRegistro75Dto
    {
        public int CodigoProdutoServico { get; set; }
        public string CodigoNcm { get; set; }
        public string Descricao { get; set; }
        public string UnidadeMedida { get; set; }
        public decimal AliquotaIpi { get; set; }
        public decimal AliquotaIcms { get; set; }
        public decimal ReducaoBaseCalculoIcms { get; set; }
        public decimal BaseCalculoIcmsSt { get; set; }

        public string GetCodigoProdutoServico()
        {
            return CodigoProdutoServico.ToString();
        }

        public string GetCodigoNcm()
        {
            return CodigoNcm;
        }

        public string GetDescricao()
        {
            return Descricao;
        }

        public string GetUnidadeMedida()
        {
            return UnidadeMedida;
        }

        public decimal GetAliquotaIpi()
        {
            return AliquotaIpi;
        }

        public decimal GetAliquotaIcms()
        {
            return AliquotaIcms;
        }

        public decimal GetReducaoIcms()
        {
            return ReducaoBaseCalculoIcms;
        }

        public decimal GetBaseCalculoIcmsSt()
        {
            return BaseCalculoIcmsSt;
        }
    }
}