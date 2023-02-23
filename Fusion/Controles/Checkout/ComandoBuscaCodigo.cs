using System.Windows;
using System.Windows.Media;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;

namespace Fusion.Controles.Checkout
{
    public class ComandoBuscaCodigo : IComando
    {
        private readonly ISessaoManager _sessaoManager;
        private readonly CheckoutBalanca _checkoutBalanca;

        public ComandoBuscaCodigo(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
            _checkoutBalanca = new CheckoutBalanca(sessaoManager);
        }

        public string TextoInformativo { get; } = "CÓDIGO OU BARRAS";
        public Brush Background { get; } = (Brush) Application.Current.FindResource("InfoBrush");

        public void Executar(CheckoutBox checkout)
        {
            var textoComando = checkout.GetTextoComando();

            if (string.IsNullOrWhiteSpace(textoComando))
            {
                return;
            }

            if (_checkoutBalanca.IsBalancaAtiva && _checkoutBalanca.CompativelComBalanca(textoComando))
            {
                CeckoutProdutoBalanca(textoComando, checkout);
                return;
            }

            CheckoutProdutoCodigo(textoComando, checkout);
        }

        private void CeckoutProdutoBalanca(string textoComando, CheckoutBox checkout)
        {
            var produto = _checkoutBalanca.BuscaProduto(textoComando);

            if (produto != null)
            {
                var quantidade = _checkoutBalanca.CalculaQuantidade(produto, textoComando);
                checkout.CheckoutItem = new CheckoutItem(produto, quantidade, codigoBalanca: true);
                return;
            }

            CheckoutProdutoCodigo(textoComando, checkout);
        }

        private void CheckoutProdutoCodigo(string textoComando, CheckoutBox checkout)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioProduto(sessao);
                var produto = repositorio.BuscaPeloCodigo(textoComando);

                if (produto == null)
                {
                    throw new CheckoutException("Não encontrado produto para esse código");
                }

                checkout.CheckoutItem = new CheckoutItem(produto, checkout.GetQuantidade());
            }
        }
    }
}