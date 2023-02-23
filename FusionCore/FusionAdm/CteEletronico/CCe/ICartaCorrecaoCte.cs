using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using TipoEmissao = FusionCore.FusionAdm.CteEletronico.Flags.TipoEmissao;

namespace FusionCore.FusionAdm.CteEletronico.CCe
{
    public interface ICartaCorrecaoCte
    {
        int Id { get; set; }
        TipoEmissao TipoEmissao { get; set; }
        string Chave { get; }
        TipoAmbiente TipoAmbiente { get; }
        string CnpjOuCpf { get; }
        EmissorFiscal EmissorFiscal { get; }
    }
}