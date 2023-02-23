using System.Windows;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Servico;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Venda;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Flags;
using FusionNfce.Visao.Principal.Contratos;

namespace FusionNfce.Visao.Principal.Implementacoes
{
    public class ComandoVenderItem : IComandoVenda
    {
        private NfceItem _nfceItem;

        public void ExecutaAcao(VendaModel model, ItemEspera item)
        {
            if (model.StatusVenda == StatusVenda.EmUso) return;

            model.StatusVenda = StatusVenda.EmUso;

            model.Nfce = model.AtualizaObjetoNfce();

            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorioIbptNfce = new RepositorioIbptNfce(sessao);
                var repositorioTabelaPrecoNfce = new RepositorioTabelaPrecoNfce(sessao);

                _nfceItem = NfceItem.ConstroiNfceItem(
                    item, 
                    model.Itens.Count + 1, 
                    model.Nfce, 
                    repositorioIbptNfce,
                    repositorioTabelaPrecoNfce
                );
            }

            SalvarNfceItem(_nfceItem);
            AtualizaModel(model, item);
            model.StatusVenda = StatusVenda.Venda;
        }

        private void SalvarNfceItem(NfceItem nfceItem)
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorioNfceItem = new RepositorioNfce(sessao);

                repositorioNfceItem.SalvarItemESincronizar(nfceItem);

                var estoqueServico = EstoqueServicoNfce.Cria(sessao, nfceItem.Produto,
                    OrigemEventoEstoque.ItemAdicionadoNfce, TipoEventoEstoque.Saida, nfceItem.Quantidade);

                estoqueServico.Descontar();

                sessao.Flush();
                sessao.Clear();

                transacao.Commit();
            }
        }

        private void AtualizaModel(VendaModel model, ItemEspera item)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                model.AtualizaListas(_nfceItem, item);
                model.AtualizaTotais();
            });
        }
    }
}