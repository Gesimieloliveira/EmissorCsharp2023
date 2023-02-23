using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Fusion.Sessao;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Cancelamento;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Desconto;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.EditarItem;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Observacao;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.TabelaPrecos;
using Fusion.Visao.Vendas.FaturamentoCheckout.Models;
using FusionCore.AutorizacaoOperacao.PayloadTypes;
using FusionCore.ControleCaixa.Facades;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Vendas.Repositorio;
using FusionLibrary.VisaoModel;
using NHibernate;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public class FaturamentoCheckoutViewModel : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly UsuarioDTO _usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
        private readonly object _inicilizandoLock = new object();
        private bool _inicilizado;
        private FaturamentoVenda _faturamento;
        private Vendedor _vendedorInicial;
        private EmpresaDTO _empresaInicial;
        private ITabelaPreco _tabelaInicial;

        public FaturamentoCheckoutViewModel()
        {
            Items = new FaturamentoCheckoutModels.Item[] { };
            Preferencias = new PreferenciasFaturamentoFacade(_sessaoSistema.SessaoManager);
        }

        public readonly PreferenciasFaturamentoFacade Preferencias;

        public bool TemPermissaoPreferencias => _usuarioLogado
            .VerificaPermissao
            .IsTemPermissao(Permissao.FATURAMENTO_PREFERENCIAS);

        public FaturamentoCheckoutModels.Faturamento Faturamento
        {
            get => GetValue<FaturamentoCheckoutModels.Faturamento>();
            private set
            {
                SetValue(value);
                PossuiFaturamento = value?.Numero > 0;
            }
        }

        public bool PossuiFaturamento
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public IEnumerable<FaturamentoCheckoutModels.Item> Items
        {
            get => GetValue<IEnumerable<FaturamentoCheckoutModels.Item>>();
            private set => SetValue(value);
        }

        public string TextoInformativo
        {
            get => GetValue();
            set => SetValue(value);
        }

        public FaturamentoCheckoutModels.Empresa Empresa
        {
            get => GetValue<FaturamentoCheckoutModels.Empresa>();
            private set => SetValue(value);
        }

        public FaturamentoCheckoutModels.Usuario Usuario
        {
            get => GetValue<FaturamentoCheckoutModels.Usuario>();
            private set => SetValue(value);
        }

        public FaturamentoCheckoutModels.Vendedor Vendedor
        {
            get => GetValue<FaturamentoCheckoutModels.Vendedor>();
            private set => SetValue(value);
        }

        public FaturamentoCheckoutModels.Item ItemSelecionado
        {
            get => GetValue<FaturamentoCheckoutModels.Item>();
            set => SetValue(value);
        }

        public FaturamentoCheckoutModels.TabelaPreco TabelaPreco
        {
            get => GetValue<FaturamentoCheckoutModels.TabelaPreco>();
            set => SetValue(value);
        }

        public void Inicializar()
        {
            lock (_inicilizandoLock)
            {
                if (_inicilizado) return;

                using (var session = _sessaoSistema.SessaoManager.CriaSessao())
                {
                    if (Empresa is null)
                    {
                        _empresaInicial = new RepositorioEmpresa(session).BuscaPrimeiraEmpresa();
                    }
                }

                _inicilizado = true;
            }

            UpdateViewModel();
        }

        public void LimparViewModel()
        {
            _faturamento = null;
            _tabelaInicial = null;

            UpdateViewModel();
        }

        public void IniciarNovoFaturamento()
        {
            using (var session = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                CriarNovoFaturamento(session);
            }

            UpdateViewModel();
        }

        private void CriarNovoFaturamento(ISession session)
        {
            var empresas = new RepositorioEmpresa(session);
            var tabelas = new RepositorioTabelaPreco(session);

            var empresa = empresas.GetPeloId(Empresa.Id);
            var tabelaPreco = tabelas.GetPeloId(_tabelaInicial?.Id ?? -1);

            _faturamento = new FaturamentoVenda(empresa, _sessaoSistema.UsuarioLogado, tabelaPreco);
            _tabelaInicial = null;

            if (_vendedorInicial != null)
                _faturamento.DefineVendedor(_vendedorInicial);
        }

        private void UpdateViewModel()
        {
            if (_inicilizado == false)
            {
                Inicializar();
                return;
            }

            Usuario = new FaturamentoCheckoutModels.Usuario(_usuarioLogado);

            if (_faturamento is null)
            {
                TextoInformativo = "FATURAMENTO LIVRE";
                Empresa = new FaturamentoCheckoutModels.Empresa(_empresaInicial);
                Vendedor = new FaturamentoCheckoutModels.Vendedor(_vendedorInicial);
                Faturamento = new FaturamentoCheckoutModels.Faturamento();
                TabelaPreco = new FaturamentoCheckoutModels.TabelaPreco(_tabelaInicial);
                Items = new FaturamentoCheckoutModels.Item[] { };
                return;
            }

            Empresa = new FaturamentoCheckoutModels.Empresa(_faturamento.Empresa);
            Vendedor = new FaturamentoCheckoutModels.Vendedor(_faturamento.Vendedor?.Vendedor);
            Faturamento = new FaturamentoCheckoutModels.Faturamento(_faturamento);
            TabelaPreco = new FaturamentoCheckoutModels.TabelaPreco(_faturamento.TabelaPreco);

            var newListItems = _faturamento.Produtos
                .Select(p => new FaturamentoCheckoutModels.Item(p));

            Items = new ObservableCollection<FaturamentoCheckoutModels.Item>(
                newListItems.OrderByDescending(i => i.Numero)
            );
        }

        public void AlterarEmpresaInicial(EmpresaDTO empresa)
        {
            if (_faturamento != null)
                throw new InvalidOperationException("Não é possível alterar a Empresa em um faturamento em andamento");

            _empresaInicial = empresa;

            UpdateViewModel();
        }

        public void FaturarItem(ProdutoDTO produto, decimal quantidade)
        {
            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                if (_faturamento is null) CriarNovoFaturamento(sessao);

                var servico = new FaturamentoVendaService(sessao, _usuarioLogado);
                servico.FaturarProduto(_faturamento, produto, quantidade);
                sessao.Transaction.Commit();
            }

            UpdateViewModel();
            UpdateTextoInformativoUltimoItem();
        }

        private void UpdateTextoInformativoUltimoItem()
        {
            var item = Items.FirstOrDefault();
            if (item is null) return;

            TextoInformativo =
                $"{item.Descricao.SubstringWithTrim(0, 25)}" +
                $" - {item.QuantidadeMedida}" +
                $" - {item.Total:C2}";
        }

        public void AplicarDescontoPercentual(decimal percentual)
        {
            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var servico = new FaturamentoVendaService(sessao, _usuarioLogado);
                servico.AplicarDesconto(_faturamento, percentual);
                sessao.Transaction.Commit();
            }

            UpdateViewModel();
            TextoInformativo = "DESCONTO APLICADO";
        }

        public void VincularCliente(Cliente cliente)
        {
            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                if (_faturamento is null) CriarNovoFaturamento(sessao);
                var servico = new FaturamentoVendaService(sessao, _usuarioLogado);
                servico.VincularDestinatario(_faturamento, cliente);
                sessao.Transaction.Commit();
            }

            UpdateViewModel();
            TextoInformativo = "CLIENTE VINCULADO";
        }

        public void VincularVendedor(Vendedor vendedor)
        {
            if (_faturamento != null)
            {
                using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
                {
                    var servico = new FaturamentoVendaService(sessao, _usuarioLogado);
                    servico.VincularVendedor(_faturamento, vendedor);
                    sessao.Transaction.Commit();
                }
            }

            _vendedorInicial = vendedor;

            UpdateViewModel();
            TextoInformativo = "VENDEDOR VINCULADO";
        }

        public void AlterarObservacao(string observacao)
        {
            if (_faturamento is null)
                throw new InvalidOperationException("Não existe faturamento para ser alterado");

            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var servico = new FaturamentoVendaService(sessao, _usuarioLogado);
                servico.AlterarObservacao(_faturamento, observacao);
                sessao.Transaction.Commit();
            }
        }

        public void RemoverItemSelecionado()
        {
            if (ItemSelecionado is null)
                throw new InvalidOperationException("Nenhum Item está Selecionado");

            var descricaoItem = ItemSelecionado.Descricao;

            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var servico = new FaturamentoVendaService(sessao, _usuarioLogado);
                servico.RemoverItem(_faturamento, ItemSelecionado.Id);
                sessao.Transaction.Commit();
            }

            UpdateViewModel();

            TextoInformativo = $"ITEM REMOVIDO: {descricaoItem}";
        }

        public void CarregarComFaturamento(int id)
        {
            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repository = new RepositorioFaturamento(sessao);
                var faturamento = repository.GetPeloIdCompleto(id);

                if (faturamento is null)
                    throw new InvalidOperationException("Não foi encontrado faturamento para o ID");

                faturamento.ThrowExceptionSeEstadoDiferenteAberto();

                _faturamento = faturamento;
            }

            UpdateViewModel();

            TextoInformativo = $"FATURAMENTO CARREGADO: NÚMERO {id}";
        }

        public void AtualizarFaturamento()
        {
            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repository = new RepositorioFaturamento(sessao);
                var faturamento = repository.GetPeloIdCompleto(_faturamento.Id);
                _faturamento = faturamento;
            }

            UpdateViewModel();
        }

        public void AplicarTabelaPrecos(ITabelaPreco tabela = null)
        {
            if (_faturamento != null)
            {
                using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
                {
                    var servico = new FaturamentoVendaService(sessao, _usuarioLogado);

                    if (tabela is null)
                    {
                        servico.RemoverTabelaPrecos(_faturamento);
                        TextoInformativo = $"TABELA PREÇOS REMOVIDA";
                    }
                    else
                    {
                        servico.AplicarTabelaPrecos(_faturamento, tabela.Id);
                        TextoInformativo = $"TABELA PREÇOS APLICADA: {tabela.Descricao}";
                    }

                    sessao.Transaction.Commit();
                }
            }
            else
            {
                _tabelaInicial = tabela;
            }

            UpdateViewModel();
        }

        public PagamentoViewModel CriarContextoPagamento()
        {
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_usuarioLogado);

            if (_faturamento == null)
                throw new InvalidOperationException("Não existe um faturamento aberto para ser finalizado");

            if (_faturamento.Total <= 0)
                throw new InvalidOperationException("Não é possível finalizar faturamento com valor menor ou igual 0,00");

            if (_faturamento.Produtos.Any(x => x.Produto.Ativo == false))
                throw new InvalidOperationException("Não é possível finalizar faturamento com produtos inativos");

            return new PagamentoViewModel(_faturamento);
        }

        public CancelarFaturamentoViewModel CriarContextoCancelar()
        {
            if (_faturamento is null)
                throw new InvalidOperationException("Nenhum faturamento para ser cancelado!");

            if (_faturamento.EstadoAtual != Estado.Aberto)
                ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_usuarioLogado);

            return new CancelarFaturamentoViewModel(_faturamento);
        }

        public FaturamentoCancelado CriarPayloadCancelamento()
        {
            if (_faturamento is null)
                throw new InvalidOperationException("Nenhum faturamento para ser cancelado!");

            return new FaturamentoCancelado(_faturamento.Id, _faturamento.Total);
        }

        public AplciarDescontoViewModel CriaContextoDesconto()
        {
            if (_faturamento == null || _faturamento.TotalProdutos <= 0)
            {
                throw new InvalidOperationException("Total Produtos precisa ser maior que 0,00 para desconto");
            }

            return new AplciarDescontoViewModel(_faturamento);
        }

        public EditarObservacaoViewModel CriarContextoObservacao()
        {
            if (_faturamento is null)
                throw new InvalidOperationException("Nenhum faturamento para ser editado!");

            var viewModel = new EditarObservacaoViewModel
            {
                Observacao = _faturamento.Observacao
            };

            return viewModel;
        }

        public FaturamentoItemExcluido CriarPayloadItemExcluido()
        {
            if (_faturamento is null)
                throw new InvalidOperationException("Não foi possível obter informações do faturamento");

            if (ItemSelecionado is null)
                throw new InvalidOperationException("Não foi possível obter informações do item a ser cancelado");

            return new FaturamentoItemExcluido(
                itemId: ItemSelecionado.Id,
                faturamentoId: _faturamento.Id,
                produtoId: ItemSelecionado.ProdutoId,
                produtoNome: ItemSelecionado.Descricao,
                quantidade: ItemSelecionado.Quantidade,
                valorUnitario: ItemSelecionado.PrecoUnitario,
                valorTotal: ItemSelecionado.Total
            );
        }

        public EditarItemViewModel CriarContextoEditarItem()
        {
            if (ItemSelecionado is null)
                throw new InvalidOperationException("Nenhum item selecionado para edição");

            var item = _faturamento.Produtos.Single(i => i.Id == ItemSelecionado.Id);

            return new EditarItemViewModel(_sessaoSistema.SessaoManager, item);
        }

        public TabelaPrecosViewModel CriarViewModelTabelaPrecos()
        {
            return new TabelaPrecosViewModel
            {
                TabelaSelecionada = _faturamento?.TabelaPreco
            };
        }
    }
}