using System;
using FusionCore.FusionNfce.Produto;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;

namespace FusionNfce.Visao.Principal.SolicitaInformacoes
{
    public class ProdutoComQuantidadePreco : EventArgs
    {
        private readonly decimal _quantidade;

        public ProdutoComQuantidadePreco(decimal quantidade)
        {
            _quantidade = quantidade;
        }

        public ProdutoNfce ProdutoNfce { get; set; }

        public decimal QuantidadeCalculada()
        {
            return _quantidade;
        }
    }

    public class SolicitaInforamcoesItemContexto : ViewModel
    {
        private readonly ProdutoNfce _produto;
        private string _nomeProduto;
        private decimal _valorUnitario;
        private decimal _quantidade;
        private decimal _total;

        public SolicitaInforamcoesItemContexto(ProdutoNfce produto, decimal quantidade)
        {
            _produto = produto;
            NomeProduto = produto.Nome;
            ValorUnitario = produto.PrecoVenda;
            Quantidade = quantidade;
        }

        public event EventHandler<ProdutoComQuantidadePreco> Finalizar; 

        public string NomeProduto
        {
            get => _nomeProduto;
            set
            {
                if (value == _nomeProduto) return;
                _nomeProduto = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorUnitario
        {
            get => _valorUnitario;
            set
            {
                if (value == _valorUnitario) return;
                _valorUnitario = value;

                AtualizarTotal();
                PropriedadeAlterada();
            }
        }

        private void AtualizarTotal()
        {
            _total = (_valorUnitario.Arredonda(6) * _quantidade.Arredonda(4)).Arredonda(2);
            PropriedadeAlterada(nameof(Total));
        }

        public decimal Quantidade
        {
            get => _quantidade;
            set
            {
                if (value == _quantidade) return;
                _quantidade = value;
                AtualizarTotal();
                PropriedadeAlterada();
            }
        }

        public decimal Total
        {
            get => _total;
            set
            {
                if (value == _total) return;
                _total = value;
                PropriedadeAlterada();
            }
        }

        public void EnviarProduto()
        {
            if (ValorUnitario <= 0) throw new InvalidOperationException("Valor Unitário deve ser maior que 0");
            if (Quantidade <= 0) throw new InvalidOperationException("Quantidade deve ser maior que 0");

            _produto.Quantidade = Quantidade.Arredonda(4);
            _produto.PrecoVenda = ValorUnitario.Arredonda(6);

            OnFinalizar(new ProdutoComQuantidadePreco(_produto.Quantidade) {ProdutoNfce = _produto});
        }

        protected virtual void OnFinalizar(ProdutoComQuantidadePreco e)
        {
            Finalizar?.Invoke(this, e);
        }
    }
}