using System;
using System.Collections.ObjectModel;
using FusionCore.FusionAdm.Compras;
using FusionCore.FusionAdm.Sessao;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace Fusion.Visao.Compras.Precos
{
    public class GeradorPrecoVendaViewModel : ViewModel
    {
        private readonly NotaFiscalCompra _nota;

        public ObservableCollection<ItemPrecoModel> Itens
        {
            get => GetValue<ObservableCollection<ItemPrecoModel>>();
            set => SetValue(value);
        }

        public decimal LucroGeral
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public GeradorPrecoVendaViewModel(NotaFiscalCompra nota)
        {
            _nota = nota;
        }

        public void Inicializar()
        {
            Itens = new ObservableCollection<ItemPrecoModel>();

            foreach (var itemCompra in _nota.Itens)
            {
                Itens.Add(new ItemPrecoModel(itemCompra));
            }
        }

        public void AplicaLucroGeral()
        {
            if (LucroGeral <= 0)
            {
                throw new InvalidOperationException("Preciso que o Lucro seja maior que 0.00%");
            }

            Itens.ForEach(i => { i.NovoLucro = LucroGeral; });
        }

        public void MantemLucroAtual()
        {
            Itens.ForEach(i => { i.NovoLucro = i.LucroAtual; });
        }

        public void MantemVendaAtual()
        {
            Itens.ForEach(i => { i.NovaVenda = i.VendaAtual; });
        }

        public void SalvarAlteracoes()
        {
            ValidaIntegridadeItens();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transaction = sessao.BeginTransaction())
            {
                foreach (var itemPreco in Itens)
                {
                    var itemCompra = itemPreco.Item;
                    var produto = itemCompra.Produto;

                    sessao.Load(produto, produto.Id);

                    produto.PrecoCompra = itemCompra.CalculoPrecoCompraUnitario;
                    produto.PrecoCusto = itemCompra.CalculoPrecoCustoUnitario;
                    produto.MargemLucro = itemPreco.NovoLucro < 0 ? 0.00M : itemPreco.NovoLucro;
                    produto.PrecoVenda = itemPreco.NovaVenda;

                    sessao.Update(produto);
                    sessao.Flush();

                    sessao.Evict(produto);
                }

                transaction.Commit();
            }
        }

        private void ValidaIntegridadeItens()
        {
            foreach (var item in Itens)
            {
                if (item.NovaVenda <= 0)
                {
                    throw new InvalidOperationException($"{item.NomeProduto} está com nova venda inválido");
                }
            }
        }
    }
}
