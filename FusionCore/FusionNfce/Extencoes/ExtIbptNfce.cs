using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionNfce.Fiscal.Tributacoes;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtIbptNfce
    {
        public static NfceIbpt ToNfce(this Ibpt ibpt)
        {
            var nfceIpbt = new NfceIbpt
            {
                Id = new NfceCodigoIbpt(ibpt.Codigo, ibpt.Tipo, ibpt.ExcecaoFiscal),
                Descricao = ibpt.Descricao,
                ChaveIbpt = ibpt.ChaveIbpt,
                Estadual = ibpt.Estadual,
                Importado = ibpt.Importado,
                Nacional = ibpt.Nacional
            };

            return nfceIpbt;
        }
    }
}