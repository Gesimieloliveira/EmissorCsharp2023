using System;
using System.Collections.ObjectModel;
using Fusion.Sessao;
using Fusion.Visao.MovimentacaoEstoque.Flyouts;
using FusionCore.FusionAdm.Estoque.Movimentacoes;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.MovimentacaoEstoque
{
    public sealed class MovimentoEstoqueFormModel : ModelBase
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private int _movimentoId;
        private MovimentoEstoque _movimento;
        private DateTime? _cadastradoEm;
        private MovimentoItem _itemSelecionado;
        private TipoEventoEstoque? _tipoEvento;
        private string _descricao;
        private decimal _precoCompraTotal;
        private decimal _precoVendaTotal;
        private MovimentoEstoqueFlyoutModel _movimentoFlyoutModel;
        private DateTime? _dataMovimentacao;
        private MovimentoItemFlyoutModel _movimentoItemFlyoutModel;
        private bool _isTipoEntrada;
        private bool _isPermissaoExcluir;

        public bool IsTipoEntrada
        {
            get { return _isTipoEntrada; }
            set
            {
                if (value == _isTipoEntrada) return;
                _isTipoEntrada = value;
                PropriedadeAlterada();
            }
        }

        public TipoEventoEstoque? TipoEvento
        {
            get { return _tipoEvento; }
            set
            {
                if (value == _tipoEvento) return;
                _tipoEvento = value;
                PropriedadeAlterada();
            }
        }

        public string Descricao
        {
            get { return _descricao; }
            set
            {
                if (value == _descricao) return;
                _descricao = value;
                PropriedadeAlterada();
            }
        }

        public decimal PrecoCompraTotal
        {
            get { return _precoCompraTotal; }
            set
            {
                if (value == _precoCompraTotal) return;
                _precoCompraTotal = value;
                PropriedadeAlterada();
            }
        }

        public decimal PrecoVendaTotal
        {
            get { return _precoVendaTotal; }
            set
            {
                if (value == _precoVendaTotal) return;
                _precoVendaTotal = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? CadastradoEm
        {
            get { return _cadastradoEm; }
            set
            {
                if (value.Equals(_cadastradoEm)) return;
                _cadastradoEm = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? DataMovimentacao
        {
            get { return _dataMovimentacao; }
            set
            {
                if (value.Equals(_dataMovimentacao)) return;
                _dataMovimentacao = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<MovimentoItem> Itens { get; } = new ObservableCollection<MovimentoItem>();

        public MovimentoItem ItemSelecionado
        {
            get { return _itemSelecionado; }
            set
            {
                if (Equals(value, _itemSelecionado)) return;
                _itemSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public MovimentoEstoqueFlyoutModel MovimentoFlyoutModel
        {
            get { return _movimentoFlyoutModel; }
            set
            {
                if (Equals(value, _movimentoFlyoutModel)) return;
                _movimentoFlyoutModel = value;
                PropriedadeAlterada();
            }
        }

        public MovimentoItemFlyoutModel MovimentoItemFlyoutModel
        {
            get { return _movimentoItemFlyoutModel; }
            set
            {
                if (Equals(value, _movimentoItemFlyoutModel)) return;
                _movimentoItemFlyoutModel = value;
                PropriedadeAlterada();
            }
        }

        public MovimentoEstoqueFormModel()
        {
            _movimentoId = 0;
        }

        public MovimentoEstoqueFormModel(int movimentoId)
        {
            _movimentoId = movimentoId;
        }

        public event EventHandler OperacaoCancelada;
        public event EventHandler MovimentoExcluido;

        private void OnMomvimentoExcluido()
        {
            MovimentoExcluido?.Invoke(this, EventArgs.Empty);
        }

        public void CarregarDados()
        {
            if (_movimentoId == 0)
            {
                AbreFlyoutParaNovoMovimento();
                return;
            }

            using (var repositorio = new RepositorioMovimentoEstoque(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                _movimento = repositorio.GetPeloId(_movimentoId);
                CarregaDadosComMovimento();
            }
        }

        private void CarregaDadosComMovimento()
        {
            TipoEvento = _movimento?.TipoEvento;
            Descricao = _movimento?.Descricao;
            DataMovimentacao = _movimento?.DataMovimento;
            PrecoCompraTotal = _movimento?.PrecoCompraTotal ?? 0;
            PrecoVendaTotal = _movimento?.PrecoVendaTotal ?? 0;
            CadastradoEm = _movimento?.CadastradoEm;
            IsTipoEntrada = _movimento?.TipoEvento == TipoEventoEstoque.Entrada;

            var usuarioLogado = _sessaoSistema.UsuarioLogado;

            IsPermissaoExcluir = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.MOVIMENTACAO_ESTOQUE_REMOVER);

            Itens.Clear();
            _movimento?.Itens.ForEach(Itens.Add);
        }

        public bool IsPermissaoExcluir
        {
            get => _isPermissaoExcluir;
            set
            {
                if (value == _isPermissaoExcluir) return;
                _isPermissaoExcluir = value;
                PropriedadeAlterada();
            }
        }

        private void AbreFlyoutParaNovoMovimento()
        {
            MovimentoFlyoutModel = new MovimentoEstoqueFlyoutModel {IsOpen = true};
            MovimentoFlyoutModel.MovimentoCadastrado += MovimentoCadastradoHandler;
            MovimentoFlyoutModel.OperacaoFinalizada += OperacaoFinalizadaHandler;
        }

        public void AbrirFlyoutMovimentoItem()
        {
            MovimentoItemFlyoutModel = new MovimentoItemFlyoutModel(_movimento.TipoEvento) {IsOpen = true};
            MovimentoItemFlyoutModel.ItemNovoSalvo += ItemNovoSalvoHandler;
        }

        private void ItemNovoSalvoHandler(object sender, MovimentoItem e)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var transacao = sessao.BeginTransaction();
                var repositorioMovimento = new RepositorioMovimentoEstoque(sessao);

                try
                {
                    if (_movimento.TipoEvento == TipoEventoEstoque.Entrada)
                    {
                        var repositorioProduto = new RepositorioProduto(sessao);
                        var produto = e.Produto;

                        produto.MargemLucro = e.MargemLucro;
                        produto.PrecoVenda = e.PrecoVenda;
                        produto.PrecoCompra = e.PrecoCompra;

                        repositorioProduto.Salvar(produto);
                    }

                    var model = new EstoqueModel(e.Produto, e.Quantidade, _sessaoSistema.UsuarioLogado,
                        OrigemEventoEstoque.InsertMovimentoEstoqueItem);
                    var servico = EstoqueServicoAdmFactory.Cria(sessao);

                    if (_movimento.TipoEvento == TipoEventoEstoque.Entrada) servico.Acrescentar(model);
                    else servico.Descontar(model);

                    _movimento.AdicionarItem(e);
                    repositorioMovimento.Altera(_movimento);

                    transacao.Commit();

                    CarregaDadosComMovimento();
                    ReabrirFlyoutMovimentoItem();
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    DialogBox.MostraErro(ex.Message, ex);
                }
            }
        }

        private void ReabrirFlyoutMovimentoItem()
        {
            MovimentoItemFlyoutModel.IsOpen = false;
            AbrirFlyoutMovimentoItem();
        }

        private void MovimentoCadastradoHandler(object sender, MovimentoEstoque e)
        {
            using (var repositorio = new RepositorioMovimentoEstoque(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                var movimento = repositorio.Persiste(e);
                _movimentoId = movimento.Id;
                CarregarDados();
            }
        }

        private void OperacaoFinalizadaHandler(object sender, EventArgs e)
        {
            if (_movimento == null) OnOperacaoCancelada();
        }

        private void OnOperacaoCancelada()
        {
            OperacaoCancelada?.Invoke(this, EventArgs.Empty);
        }

        public void DeletarItemSelecionado()
        {
            try
            {
                ProcessarExclusaoItemSelecionado();
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void ProcessarExclusaoItemSelecionado()
        {
            if (ItemSelecionado == null)
                throw new InvalidOperationException("Item selecionado inválido para exclusão");

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var transacao = sessao.BeginTransaction();
                var repositorio = new RepositorioMovimentoEstoque(sessao);

                try
                {
                    var model = new EstoqueModel(
                        ItemSelecionado.Produto, 
                        ItemSelecionado.Quantidade,
                        _sessaoSistema.UsuarioLogado,
                        OrigemEventoEstoque.DeleteMovimentoEstoqueItem);

                    var servico = EstoqueServicoAdmFactory.Cria(sessao);

                    if (_movimento.TipoEvento == TipoEventoEstoque.Saida) servico.Acrescentar(model);
                    else servico.Descontar(model);

                    _movimento.RemoverItem(ItemSelecionado);
                    repositorio.Altera(_movimento);
                    transacao.Commit();

                    CarregaDadosComMovimento();
                }
                catch (Exception)
                {
                    transacao.Rollback();
                    throw;
                }
            }
        }

        public void DeletarMovimentacao()
        {
            try
            {
                ProcessarExclusaoMovimentacao();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void ProcessarExclusaoMovimentacao()
        {
            if (_movimento.Itens.Count > 0)
                throw new InvalidOperationException("Movimento possui itens. Necessário excluir os itens antes.");

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var transacao = sessao.BeginTransaction();
                var repositorio = new RepositorioMovimentoEstoque(sessao);

                try
                {
                    repositorio.Deletar(_movimento);
                    transacao.Commit();

                    OnMomvimentoExcluido();
                }
                catch (Exception)
                {
                    transacao.Rollback();
                    throw;
                }
            }
        }
    }
}