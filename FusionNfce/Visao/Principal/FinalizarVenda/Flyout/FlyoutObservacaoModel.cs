using System;
using FusionLibrary.VisaoModel;

namespace FusionNfce.Visao.Principal.FinalizarVenda.Flyout
{
    public class RetornoAdicionaObservacao : EventArgs
    {
        public string Observacao { get; set; }

        public RetornoAdicionaObservacao(string observacao)
        {
            Observacao = observacao;
        }
    }

    public class FlyoutObservacaoModel : ViewModel
    {
        private bool _isOpen;
        private string _observacao;

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        public string Observacao
        {
            get { return _observacao; }
            set
            {
                if (value == _observacao) return;
                _observacao = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<RetornoAdicionaObservacao> AdicionaObservacao;

        public virtual void OnAdicionaObservacao()
        {
            AdicionaObservacao?.Invoke(this, new RetornoAdicionaObservacao(Observacao?.Trim() ?? string.Empty));
            IsOpen = false;
        }
    }
}