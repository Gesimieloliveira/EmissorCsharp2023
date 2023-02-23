using System;
using FusionCore.FusionAdm.PedidoVenda;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PedidoDeVenda
{
    public class TipoPedidoContexto : ViewModel
    {
        public event EventHandler<TipoPedido> EscolhaCompleta;

        public TipoPedido? TipoEscolhido
        {
            get => GetValue<TipoPedido?>();
            set => SetValue(value);
        }

        public void EscolherPedido()
        {
            TipoEscolhido = TipoPedido.PedidoVenda;
            OnTipoEscolhido();
        }

        private void OnTipoEscolhido()
        {
            if (TipoEscolhido == null)
            {
                throw new Exception("Tipo escolhido não pode ser nulo");
            }

            EscolhaCompleta?.Invoke(this, TipoEscolhido.Value);
        }

        public void EscolherOrcamento()
        {
            TipoEscolhido = TipoPedido.Orcamento;
            OnTipoEscolhido();
        }
    }
}