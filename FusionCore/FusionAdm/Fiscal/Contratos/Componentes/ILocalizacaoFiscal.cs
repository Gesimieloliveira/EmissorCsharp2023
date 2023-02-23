using FusionCore.FusionAdm.Localidade;

namespace FusionCore.FusionAdm.Fiscal.Contratos.Componentes
{
    public interface ILocalizacaoFiscal
    {
        string NomeMunicipio { get; set; }
        int CodigoMunicipio { get; set; }
        byte CodigoUF { get; set; }
        string SiglaUF { get; set; }
        short CodigoPais { get; }
        string NomePais { get; }
        void SetLocalidade(Pais pais);
    }
}