using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.ChaveEletronica
{
    public interface IChaveComponentes
    {
        string GetDocumentoUnico();
        byte GetCodigoUf();
        DateTime GetDhEmissao();
        ModeloDocumento GetModelo();
        long GetNumeroDocumento();
        short GetSerie();
        string GetCodigoNumerico();
        TipoEmissao GetTipoEmissao();
        string GetVersao();
    }
}