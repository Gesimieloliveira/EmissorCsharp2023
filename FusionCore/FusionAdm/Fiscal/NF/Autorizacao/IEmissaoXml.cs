using System;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.NF.Autorizacao
{
    public interface IEmissaoXml
    {
        string TagId { get; }
        string VersaoDocumento { get; }
        ChaveSefaz Chave { get; }
        TipoEmissao TipoEmissao { get; }
        TipoAmbiente Ambiente { get; }
        string MotivoContingencia { get; }
        DateTime? InicioContingencia { get; }
        bool ContingenciaAtivada();
    }
}