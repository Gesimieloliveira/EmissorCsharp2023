using System;
using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.Fiscal.NF.Pagamentos
{
    public abstract class FormaPagamentoNfe
    {
        public int Id { get; private set; }
        public Nfeletronica NFe { get; protected set; }
        public decimal Valor { get; protected set; }
        public DateTime CriadoEm { get; protected set; } = DateTime.Now;
        public UsuarioDTO Usuario { get; protected set; }
        public bool PossuiParcelamento { get; protected set; }
        public ITipoDocumento TipoDocumento { get; protected set; }

        public abstract IEnumerable<ParcelaNfe> Parcelas { get; }
        public abstract ETipoPagamento Especie { get; }

        public string TextoTextView => Especie.GetDescription();

        public void AnexarNfe(Nfeletronica nfe)
        {
            if (NFe != null)
            {
                throw new InvalidOperationException("Pagamento já anexado a uma NF-e");
            }

            NFe = nfe;
        }
    }
}