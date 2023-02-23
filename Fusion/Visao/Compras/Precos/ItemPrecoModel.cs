using FusionCore.FusionAdm.Compras;
using FusionLibrary.VisaoModel;

// ReSharper disable ExplicitCallerInfoArgument

namespace Fusion.Visao.Compras.Precos
{
    public class ItemPrecoModel : ViewModel
    {
        public ItemCompra Item { get; }

        public string NomeProduto
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal CustoAtual
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal LucroAtual
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal VendaAtual
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal NovoCusto
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaNovaVenda();
            }
        }

        public decimal NovoLucro
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaNovaVenda();
            }
        }

        public decimal NovaVenda
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaNovoLucro();
            }
        }

        public ItemPrecoModel(ItemCompra item)
        {
            NomeProduto = item.Produto.Nome;
            CustoAtual = item.Produto.PrecoCusto;
            LucroAtual = item.Produto.MargemLucro;
            VendaAtual = item.Produto.PrecoVenda;
            NovoCusto = item.CalculoPrecoCustoUnitario;
            NovoLucro = LucroAtual;

            Item = item;
        }

        private void CalculaNovaVenda()
        {
            var fatorLucro = 1 + NovoLucro / 100;
            var novaVenda = decimal.Round(NovoCusto * fatorLucro, 2);

            SetValue(novaVenda, nameof(NovaVenda));
        }

        private void CalculaNovoLucro()
        {
            var lucro = decimal.Round(NovaVenda * 100 / NovoCusto - 100, 6);
            SetValue(lucro, nameof(NovoLucro));
        }
    }
}