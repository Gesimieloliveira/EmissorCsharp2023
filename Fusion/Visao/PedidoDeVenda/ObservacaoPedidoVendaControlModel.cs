using System;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Contratos;

namespace Fusion.Visao.PedidoDeVenda
{
    public class ObservacaoPedidoVendaControlModel : ViewModel, IChildContext
    {
        private string _observacao;

        public string Observacao
        {
            get => _observacao;
            set
            {
                if (value == _observacao) return;
                _observacao = value;
                PropriedadeAlterada();
            }
        }

        public string TituloChild { get; } = "Adicionais Documento";

        public event EventHandler SolicitaFechamento;
        public event EventHandler ConfirmouAlteracoes;

        public void ConfirmarAlteracoes()
        {
            ConfirmouAlteracoes?.Invoke(this, EventArgs.Empty);
            SolicitaFechamento?.Invoke(this, EventArgs.Empty);
        }
    }
}