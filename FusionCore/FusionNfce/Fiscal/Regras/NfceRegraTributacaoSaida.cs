using FusionCore.FusionNfce.Cfop;
using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Regras;

namespace FusionCore.FusionNfce.Fiscal.Regras
{
    public class NfceRegraTributacaoSaida : Entidade
    {
        private NfceRegraTributacaoSaida()
        {
            //nhibernate
        }

        protected override int ReferenciaUnica => Id;
        public short Id { get; private set; }
        public string Descricao { get; private set; }
        public string SituacaoTributariaIcms { get; set; }
        public string SituacaoTributariaCsosn { get; set; }
        public CfopNfce Cfop { get; private set; }

        public static NfceRegraTributacaoSaida From(RegraTributacaoSaida regra)
        {
            return new NfceRegraTributacaoSaida
            {
                Id = regra.Id,
                Descricao = regra.Descricao,
                SituacaoTributariaIcms = regra.Cst.Codigo,
                SituacaoTributariaCsosn = regra.Csosn.Codigo,
                Cfop = CfopNfce.From(regra.CfopNfce)
            };
        }
    }
}