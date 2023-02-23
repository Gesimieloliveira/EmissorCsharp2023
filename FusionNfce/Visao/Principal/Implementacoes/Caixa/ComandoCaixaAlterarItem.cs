using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Servico;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Flags;
using FusionNfce.Visao.Principal.AlteraItem;
using FusionNfce.Visao.Principal.Contratos;
using FusionNfce.Visao.Principal.Model;
using NHibernate.Util;

namespace FusionNfce.Visao.Principal.Implementacoes.Caixa
{
    public class ComandoCaixaAlterarItem : IComandoCaixa
    {
        private VendaModel _model;
        private int _numeroItem;

        public void ExecutaAcao(VendaModel model, string cmd, ProdutoNfce produtoBuscaManual = null)
        {
            _model = model;

            if (_model.Itens.Count == 0)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                _model.InformacaoAcaoBarras = "Venda";
                throw new InvalidOperationException("Não tem itens na lista");
            }

            var itens = _model.Itens.Where(i => i.Cancelado == false).ToList();

            if (itens.Count == 0)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                _model.InformacaoAcaoBarras = "Venda";
                throw new InvalidOperationException("Todos os itens estão cancelados");
            }

            _model.Nfce = _model.AtualizaObjetoNfce();
            _model.RecuperarVenda(this, new NfceEvent(_model.Nfce));

            ValidaSeTemLetras(cmd);

            ObterNumeroDoItem(cmd);

            var item = (NfceItem) _model.Itens.Where(i => i.NumeroItem == _numeroItem && i.Cancelado == false).FirstOrNull();

            if (item == null)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                _model.InformacaoAcaoBarras = "Venda";
                throw new InvalidOperationException("Item não encontrado");
            }

            var alteraItemModel = new AlteraItemFormModel(item, _model.IsTemFormaDePagamento());
            alteraItemModel.RetornaItemAtualizado += AtualizaItem;

            new AlteraItemForm(alteraItemModel).ShowDialog();

            _model.StatusCaixa = StatusCaixa.Normal;
            _model.InformacaoAcaoBarras = "Venda";
            _model.AtualizaTotais();
        }

        private void AtualizaItem(object sender, NfceItemEvent e)
        {
            SalvarNfceItem(e.Item, e.QuantidadeOriginal);

            Application.Current.Dispatcher.Invoke(() => { CollectionViewSource.GetDefaultView(_model.Itens).Refresh(); });
            _model.StatusCaixa = StatusCaixa.Normal;
        }

        private void SalvarNfceItem(NfceItem nfceItem, decimal quantidadeOriginal)
        {
            var quantidadeAntigaMaior = quantidadeOriginal > nfceItem.Quantidade;

            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorioNfceItem = new RepositorioNfce(sessao);

                repositorioNfceItem.SalvarItemESincronizar(nfceItem);

                var repositorioNfce = new RepositorioNfce(sessao);
                repositorioNfce.SalvarESincronizar(nfceItem.Nfce);

                if (quantidadeAntigaMaior)
                {
                    var movimento = quantidadeOriginal - nfceItem.Quantidade;

                    var estoqueServico = EstoqueServicoNfce.Cria(sessao, nfceItem.Produto,
                    OrigemEventoEstoque.MovimentacaoNfceApartirDeAlteracao, TipoEventoEstoque.Entrada, movimento);

                    estoqueServico.Acrescentar();

                }

                if (!quantidadeAntigaMaior)
                {
                    var movimento = nfceItem.Quantidade - quantidadeOriginal;

                    var estoqueServico = EstoqueServicoNfce.Cria(sessao, nfceItem.Produto,
                    OrigemEventoEstoque.MovimentacaoNfceApartirDeAlteracao, TipoEventoEstoque.Saida, movimento);

                    estoqueServico.Descontar();
                }

                transacao.Commit();
            }
        }

        private void ObterNumeroDoItem(string cmd)
        {
            var numeroItem = Regex.Replace(cmd, @"\D", string.Empty);
            try
            {
                _numeroItem = int.Parse(numeroItem);
            }
            catch (OverflowException)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                _model.InformacaoAcaoBarras = "Venda";
                throw new InvalidOperationException("Número muito grande para ser o número de um item\nNúmero de um item e de 1 a 999");
            }
        }

        private void ValidaSeTemLetras(string cmd)
        {
            var regex = new Regex(@"[\d]");
            var validacao = regex.Replace(cmd, "");

            if (string.IsNullOrEmpty(validacao)) return;

            _model.StatusCaixa = StatusCaixa.Normal;
            throw new InvalidOperationException("Porfavor digitar apenas números");
        }
    }
}