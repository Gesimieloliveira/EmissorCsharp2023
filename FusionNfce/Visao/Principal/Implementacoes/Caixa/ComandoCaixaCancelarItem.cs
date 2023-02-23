using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using FusionCore.AutorizacaoOperacao.Autorizacao;
using FusionCore.AutorizacaoOperacao.PayloadTypes;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Servico;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Flags;
using FusionNfce.Visao.Principal.Contratos;
using FusionWPF.SharedViews.AutorizarOperacao;
using NHibernate;
using NHibernate.Util;

namespace FusionNfce.Visao.Principal.Implementacoes.Caixa
{
    public class ComandoCaixaCancelarItem : IComandoCaixa
    {
        private VendaModel _model;
        private short _numeroItem;

        public void ExecutaAcao(VendaModel model, string cmd, ProdutoNfce produtoBuscaManual = null)
        {
            _model = model;

            if (_model.Itens.Count == 0)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                _model.InformacaoAcaoBarras = "Venda";
                throw new InvalidOperationException("Não existe itens na venda");
            }

            if (_model.Itens.Count(item => item.Cancelado == false) == 1)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                _model.InformacaoAcaoBarras = "Venda";
                throw new InvalidOperationException("Não e possível cancelar um item em uma venda com apenas um item\nSugestão: cancele a venda");
            }

            _model.Nfce = _model.AtualizaObjetoNfce();

            if (_model.IsTemFormaDePagamento())
            {
                throw new InvalidOperationException("Verifiquei que houve tentativa de transmissão ou existem pagamentos lançados, cancele a mesma ou transmita\n ou tente limpar as formas de pagamento");
            }

            ConvertCmd(cmd);
            CancelaItem();
            _model.StatusCaixa = StatusCaixa.Normal;
        }

        private void ConvertCmd(string cmd)
        {
            try
            {
                _numeroItem = short.Parse(cmd);
            }
            catch (OverflowException)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                _model.InformacaoAcaoBarras = "Venda";
                throw new InvalidOperationException("Número muito grande para ser o número de um item\nNúmero de um item e de 1 a 999");
            }
            catch (FormatException ex)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                _model.InformacaoAcaoBarras = "Venda";
                throw new InvalidOperationException("Número digitado está inválido.", ex);
            }
        }

        private void CancelaItem()
        {
            var item = _model.Itens.SingleOrDefault(i => i.NumeroItem == _numeroItem && i.Cancelado == false);
            var temItem = _model.Itens.Where(i => i.Cancelado == false).ToList();

            if (temItem.Count == 0)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                _model.InformacaoAcaoBarras = "Venda";
                throw new InvalidOperationException("Não a itens para cancelar");
            }

            if (item == null)
            {
                _model.StatusCaixa = StatusCaixa.Normal;
                _model.InformacaoAcaoBarras = "Venda";
                throw new InvalidOperationException("Item não encontrado");
            }



            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            ObservableCollection<NfceItem> itensLista;

            using (sessao)
            using (transacao)
            {
                var numero = 0;

                item.Cancelado = true;
                item.NumeroItem = 0;
                DeletaNfceItem(item, sessao);
                sessao.Flush();
                sessao.Clear();

                itensLista = new ObservableCollection<NfceItem>(_model.Nfce.ObterOsItens());

                itensLista.OrderBy(x => x.NumeroItem).ForEach(i =>
                {
                    i.NumeroItem = (short)++numero;
                    SalvarNovaNumeracao(i, sessao);
                    sessao.Flush();
                    sessao.Clear();
                });

                transacao.Commit();
            }

            AtualizaModel(itensLista);
        }

        private void SalvarNovaNumeracao(NfceItem nfceItem, ISession sessao)
        {
            var repositorioNfceItem = new RepositorioNfce(sessao);

            repositorioNfceItem.SalvarItemESincronizar(nfceItem);
        }

        private void AtualizaModel(ObservableCollection<NfceItem> itens)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _model.Itens = new ObservableCollection<NfceItem>(itens);

                CollectionViewSource.GetDefaultView(_model.Itens).Refresh();
                _model.AtualizaTotais();
            });
        }

        private void DeletaNfceItem(NfceItem nfceItem, ISession sessao)
        {
            var payload = new NfceItemExcluido(
            nfceItem.Id,
            _model.Nfce.Id,
            nfceItem.Id,
            nfceItem.Nome,
            nfceItem.Quantidade,
            nfceItem.ValorUnitario,
            nfceItem.ValorTotal);

            var autorizarUsuario = new AutorizarUsuarioNfce(SessaoSistemaNfce.SessaoManager);
            var autorizarCancelamento = new AutorizarOperacaoView(SessaoSistemaNfce.SessaoManager, autorizarUsuario, SessaoSistemaNfce.Usuario, _model.Nfce.Id.ToString(), Permissao.EXCLUIR_ITEM_NFCE, payload, () =>
            {
                ExcluirItemAction( nfceItem, sessao);
            });

            autorizarCancelamento.ExecutarAcao();

        }

        private void ExcluirItemAction(NfceItem nfceItem, ISession sessao)
        {
            var repositorioNfceItem = new RepositorioNfce(sessao);

            MovimentaEstoque(nfceItem, sessao);

            _model.Nfce.RemoverItem(nfceItem);
            _model.RemoverItemSilencioso(nfceItem);
            repositorioNfceItem.SalvarESincronizar(_model.Nfce);
        }

        private static void MovimentaEstoque(NfceItem nfceItem, ISession sessao)
        {
            var estoqueServico = EstoqueServicoNfce.Cria(sessao, nfceItem.Produto,
                OrigemEventoEstoque.ItemRemovidoNfce, TipoEventoEstoque.Entrada, nfceItem.Quantidade);

            estoqueServico.Acrescentar();
        }
    }
}