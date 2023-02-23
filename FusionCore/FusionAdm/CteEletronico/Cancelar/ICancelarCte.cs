using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using TipoEmissao = FusionCore.FusionAdm.CteEletronico.Flags.TipoEmissao;

namespace FusionCore.FusionAdm.CteEletronico.Cancelar
{
    public interface ICancelarCte
    {
        object GetCte();
        string NumeroFiscal { get; }
        string Chave { get; }
        EstadoDTO Estado { get; }
        TipoAmbiente TipoAmbiente { get; }
        string Protocolo { get; }
        string CnpjCpfEmitente { get; }
        EmissorFiscal EmissorFiscal { get; }
        Documento Documento { get; }
        TipoEmissao TipoEmissao { get; }
        void SetHistorico(CteEmissaoHistorico historico);
    }
}