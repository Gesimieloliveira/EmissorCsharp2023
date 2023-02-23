using System;
using System.Collections.Generic;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;

namespace FusionWPF.Parcelamento
{
    public class ParcelamentoArgs : EventArgs
    {
        public ParcelamentoArgs(IEnumerable<ParcelaGerada> parcelas, ITipoDocumento tipoDocumento)
        {
            Parcelas = parcelas;
            TipoDocumento = tipoDocumento;
        }

        public IEnumerable<ParcelaGerada> Parcelas { get; }
        public ITipoDocumento TipoDocumento { get; }
    }
}