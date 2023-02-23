using FusionCore.FusionAdm.Fiscal.Contratos.Componentes;
using FusionCore.FusionAdm.Localidade;

// ReSharper disable MemberCanBePrivate.Global

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class LocalizacaoFiscal : ILocalizacaoFiscal
    {
        public string NomeMunicipio { get; set; }
        public int CodigoMunicipio { get; set; }
        public byte CodigoUF { get; set; }
        public string SiglaUF { get; set; }
        public short CodigoPais { get; private set; } = 1058;
        public string NomePais { get; private set; } = "BRASIL";

        private LocalizacaoFiscal()
        {
            //nhibernate
        }

        public LocalizacaoFiscal(
            string nomeMunicipio,
            int codigoMunicipio,
            byte codigoUF,
            string siglaUF) : this()
        {
            NomeMunicipio = nomeMunicipio;
            CodigoMunicipio = codigoMunicipio;
            CodigoUF = codigoUF;
            SiglaUF = siglaUF;
        }

        public void SetLocalidade(Pais pais)
        {
            CodigoPais = pais?.Bacen ?? 0;
            NomePais = pais?.Nome ?? string.Empty;
        }
    }
}