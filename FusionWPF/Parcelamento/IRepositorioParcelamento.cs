using System;
using System.Collections.Generic;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;

namespace FusionWPF.Parcelamento
{
    public interface IRepositorioParcelamento : IDisposable
    {
        IEnumerable<ITipoDocumento> BuscaTiposDocumentos();
    }
}