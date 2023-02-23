using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fusion.Controles.Objetos;
using Fusion.Sessao;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.PedidoVenda.Fabricas;
using FusionCore.FusionAdm.PedidoVenda.Servicos;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.FusionAdm.TabelasDePrecos.Dtos;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PedidoDeVenda
{
    public sealed class PedidoVendaFormModel : ViewModel
    {
        private EmpresaDTO _empresa;
        private readonly ISessaoManager _sessaoManager;
        private bool _isPermissaoPreferencias;
        private ObservableCollection<TabelaPrecoDto> _tabelasPrecosLista = new ObservableCollection<TabelaPrecoDto>();
        private TabelaPrecoDto _tabelaPrecoSelecionada;

        public event EventHandler<ITabelaPreco> AtualizaTabelaPrecoListagem; 

        public PedidoVendaFormModel(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
            Produtos = new List<PedidoVendaProduto>();
            IsPermissaoPreferencias = SessaoSistema.ObterUsuarioLogado().VerificaPermissao
                .IsTemPermissao(Permissao.PEDIDO_VENDA_PREFERENCIAS);

            CarregarTabelasPrecos();
        }

        private void CarregarTabelasPrecos()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTabelaPreco(sessao);
                var tabelas = repositorio.BuscarTodasTabelasDto();

                TabelasPrecosLista = new ObservableCollection<TabelaPrecoDto>(tabelas);
            }
        }

        public bool IsPermissaoPreferencias
        {
            get => _isPermissaoPreferencias;
            set
            {
                if (value == _isPermissaoPreferencias) return;
                _isPermissaoPreferencias = value;
                PropriedadeAlterada();
            }
        }

        public PedidoVenda PedidoVenda
        {
            get => GetValue<PedidoVenda>();
            private set
            {
                SetValue(value);
                NotificarAlteracaoEstado();
            }
        }

        public IEnumerable<PedidoVendaProduto> Produtos
        {
            get => GetValue<IEnumerable<PedidoVendaProduto>>();
            set => SetValue(value);
        }

        public decimal Quantidade
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                DefinirPrecoTotal();
            }
        }

        public PedidoVendaProduto ItemSelecionado
        {
            get => GetValue<PedidoVendaProduto>();
            set => SetValue(value);
        }

        public ProdutoCombo ProdutoCombo
        {
            get => GetValue<ProdutoCombo>();
            set
            {
                PrecoUnitario = 0.0m;
                SetValue(value);
                if (value != null)
                {
                    PrecoUnitario = ProdutoCombo.Produto.PrecoVenda;
                }
            }
        }

        public int Numero
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public DateTime CriadoEm
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public decimal TotalProdutos
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal PercentualDesconto
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal Total
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public string NomeEmpresa
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public string DocumentoEmpresa
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public string EnderecoEmpresa
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public string NomeCliente
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public string NomeVendedor
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public string DocumentoCliente
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public string EnderecoCliente
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public string Referencia
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal PrecoUnitario
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                DefinirPrecoTotal();
            }
        }

        public bool IsPedidoVenda => PedidoVenda?.IsPedidoVenda ?? false;
        public bool IsOrcamento => PedidoVenda?.IsOrcamento ?? false;
        public bool PedidoEmAndamento => PedidoVenda?.IsNovo == false;
        public bool PedidoEstaAberto => PedidoVenda?.EstaAberto ?? false;
        public bool PermiteEdicao => PedidoVenda?.EstadoAtual.PermiteEdicao() == true;
        public bool IsTemDestinatario => PedidoVenda?.Destinatario != null;

        public decimal PrecoTotal
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public ObservableCollection<TabelaPrecoDto> TabelasPrecosLista
        {
            get => _tabelasPrecosLista;
            set
            {
                _tabelasPrecosLista = value;
                PropriedadeAlterada();
            }
        }

        public TabelaPrecoDto TabelaPrecoSelecionada
        {
            get => _tabelaPrecoSelecionada;
            set
            {
                _tabelaPrecoSelecionada = value;

                AtualizarPrecosComTabelaPreco();
                PropriedadeAlterada();
            }
        }

        private void DefinirPrecoTotal()
        {
            SetValue(decimal.Round(Quantidade*PrecoUnitario, 2), nameof(PrecoTotal));
        }

        public void IniciarNovo()
        {
            LimparMapaValores();

            if (_empresa == null)
            {
                _empresa = BuscarEmpresa();
            }

            PedidoVenda = new PedidoVenda
            {
                TabelaPreco = CarregaTabelaPrecoPorId(TabelaPrecoSelecionada)
            };

            Produtos = new List<PedidoVendaProduto>();
            Quantidade = 1;

            AtualizarDadosEmpresa();
        }

        private void CarregarListaItens()
        {
            Produtos = PedidoVenda.ItensPedidoVenda.OrderByDescending(i => i.Numero);
        }

        public void NotificarAlteracaoEstado()
        {
            PropriedadeAlterada(nameof(PedidoEmAndamento));
            PropriedadeAlterada(nameof(PedidoEstaAberto));
            PropriedadeAlterada(nameof(IsPedidoVenda));
            PropriedadeAlterada(nameof(IsOrcamento));
            PropriedadeAlterada(nameof(PermiteEdicao));
            PropriedadeAlterada(nameof(PedidoVenda));
        }

        public void MarcarComo(TipoPedido tipo)
        {
            if (tipo == PedidoVenda.TipoPedido)
            {
                return;
            }

            if (tipo == TipoPedido.Orcamento)
            {
                MarcarComoOrcamento();
            }

            if (tipo == TipoPedido.PedidoVenda)
            {
                MarcarComoPedidoVenda();
            }
        }

        public void MarcarComoOrcamento()
        {
            PedidoVenda.MarcarComoOrcamento();
            NotificarAlteracaoEstado();
        }

        public void MarcarComoPedidoVenda()
        {
            PedidoVenda.MarcarComoPedido();

            if (PedidoVenda.IsNovo == false)
            {
                PersistirComReservaDeEstoque();
            }

            NotificarAlteracaoEstado();
        }

        private void PersistirComReservaDeEstoque()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var usuario = SessaoSistema.Instancia.UsuarioLogado;
                var repositorio = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);

                repositorio.DescontarEstoqueEReservaEstoque(PedidoVenda, usuario);
                repositorio.SalvarAlteracoes(PedidoVenda);

                transacao.Commit();
            }
        }

        public void AdicionarProduto()
        {
            if (ProdutoCombo == null)
            {
                throw new InvalidOperationException("Adicionar um produto antes de lançar o produto");
            }

            var produto = ProdutoCombo.CarregaProduto();

            ProdutoAjuda.ValidarUnidadeMedidaPodeFracionar(produto, Quantidade);

            if (IsPedidoVenda)
            {
                BoqueioEstoqueHelper.ChecaEstoqueNegativoAdm(produto, Quantidade);
            }

            if (PedidoVenda.Id == 0)
            {
                SalvarPedidoVenda();
            }

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            PedidoVenda.AddItem(produto, Quantidade, PrecoUnitario, usuarioLogado);

            SalvarPedidoVenda();

            Quantidade = 1.0M;
            PrecoUnitario = 0.0M;
        }

        private EmpresaDTO BuscarEmpresa()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioEmpresa = new RepositorioEmpresa(sessao);
                var empresa = repositorioEmpresa.GetPeloId(1);

                return empresa;
            }
        }

        private void AtualizarDadosEmpresa()
        {
            NomeEmpresa = _empresa.RazaoSocial;
            DocumentoEmpresa = _empresa.Cnpj;
            EnderecoEmpresa = _empresa.Endereco;
        }

        public void TrocaEmpresaPara(EmpresaDTO empresa)
        {
            _empresa = empresa;

            PedidoVenda.Empresa = empresa;
            AtualizarDadosEmpresa();
        }

        public void ComVisitante(Visitante visitante)
        {
            PedidoVenda.Destinatario = new PedidoDestinatario(visitante, PedidoVenda);
            SalvarDestinatario();
        }

        private void CarregarDadosDestinatario()
        {
            if (PedidoVenda.Destinatario == null)
            {
                NomeCliente = string.Empty;
                DocumentoCliente = string.Empty;
                EnderecoCliente = string.Empty;

                return;
            }

            NomeCliente = PedidoVenda.Destinatario.GetNome;
            DocumentoCliente = PedidoVenda.Destinatario.GetDocumento;
            EnderecoCliente = PedidoVenda.Destinatario.GetEndereco;
        }

        private void CarregarDadosVendedor()
        {
            if (PedidoVenda.Vendedor == null)
            {
                NomeVendedor = string.Empty;

                return;
            }

            NomeVendedor = PedidoVenda.Vendedor.ObterNome;
        }

        public void ComDestinatario(Cliente cliente)
        {
            PedidoVenda.Destinatario = new PedidoDestinatario(cliente, PedidoVenda);

            SalvarDestinatario();
        }

        private void SalvarDestinatario()
        {
            SalvarPedidoVenda();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPedidoVenda(sessao);

                repositorio.Salvar(PedidoVenda.Destinatario);
                transacao.Commit();
            }

            AtualizarNomeCliente();
        }

        private void AtualizarNomeCliente()
        {
            NomeCliente = PedidoVenda?.Destinatario.GetNome;
        }

        public void ComVendedor(Vendedor vendedor)
        {
            PedidoVenda.DefineVendedor(vendedor);
            
            SalvarPedidoVenda();
        }

        public void AplicarDescontoNoTotal()
        {
            PedidoVenda.AplicaDesconto(PercentualDesconto);
            SalvarPedidoVenda();
        }

        public bool PossuiItens()
        {
            return Produtos.Any();
        }

        private void SalvarPedidoVenda()
        {
            if (PedidoVenda.Id == 0)
            {
                PedidoVenda.Abrir(_empresa, SessaoSistema.Instancia.UsuarioLogado);
            }

            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);
                repositorio.SalvarAlteracoes(PedidoVenda);

                transacao.Commit();
            }

            AtualizarDadosTela();
        }

        private void AtualizarDadosTela()
        {
            Numero = PedidoVenda.Id;
            CriadoEm = PedidoVenda.AbertoEm;
            Total = PedidoVenda.Total;
            TotalProdutos = PedidoVenda.TotalProdutos;
            PercentualDesconto = PedidoVenda.PercentualDesconto;

            CarregarDadosDestinatario();
            CarregarDadosVendedor();
            NotificarAlteracaoEstado();
            CarregarListaItens();
        }

        public void CarregarPedidoSelecionado(PedidoVendaDTO dto)
        {
            var pedido = BuscaPedidoPeloId(dto.Id);

            IniciarNovo();

            PedidoVenda = pedido;

            Numero = PedidoVenda.Id;
            CriadoEm = PedidoVenda.AbertoEm;
            Referencia = PedidoVenda.Referencia;
            TotalProdutos = PedidoVenda.TotalProdutos;
            Total = PedidoVenda.Total;
            PercentualDesconto = PedidoVenda.PercentualDesconto;
            _tabelaPrecoSelecionada = PedidoVenda.TabelaPreco != null ? new TabelaPrecoDto
            {
                Descricao = PedidoVenda.TabelaPreco.Descricao,
                Id = PedidoVenda.TabelaPreco.Id
            } : null;
            PropriedadeAlterada(nameof(TabelaPrecoSelecionada));

            TrocaEmpresaPara(PedidoVenda.Empresa);
            CarregarDadosDestinatario();
            CarregarDadosVendedor();
            CarregarListaItens();
        }

        private PedidoVenda BuscaPedidoPeloId(int id)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);
                var pedido = repositorio.GetPeloIdLazy(id);

                return pedido;
            }
        }

        public void SalvarReferencia()
        {
            PedidoVenda.Referencia = Referencia ?? string.Empty;
            SalvarPedidoVenda();
        }

        public void CancelarDocumento(string motivoCancelamento)
        {
            var servico = new ServicoCancelarPedidoVenda(PedidoVenda, SessaoSistema.Instancia.UsuarioLogado);
            servico.Cancelar(motivoCancelamento);

            IniciarNovo();
        }

        public Visitante GetVisitante()
        {
            return PedidoVenda.UsandoVisitante
                ? PedidoVenda.Destinatario.Visitante
                : new Visitante();
        }

        public void UpdateItemSelecionado(ItemValue args)
        {
            PedidoVenda.UpdateItem(ItemSelecionado.Id, args);

            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);

                var item = PedidoVenda.ItensPedidoVenda.SingleOrDefault(x => x.Id == ItemSelecionado.Id);

                if (args.Quantidade < args.QuantidadeAntiga && IsPedidoVenda)
                {
                    EstoqueEntrada(item, args.QuantidadeAntiga-args.Quantidade, repositorio);
                }

                if (args.Quantidade > args.QuantidadeAntiga && IsPedidoVenda)
                {
                    var quantidadeSaida = args.Quantidade - args.QuantidadeAntiga;

                    BoqueioEstoqueHelper.ChecaEstoqueNegativoAdm(item.Produto, quantidadeSaida);
                    EstoqueSaida(item, quantidadeSaida, repositorio);
                }

                repositorio.SalvarAlteracoes(PedidoVenda);

                transacao.Commit();
            }

            AtualizarDadosTela();
        }

        private void EstoqueEntrada(PedidoVendaProduto item, decimal quantidade, RepositorioPedidoVenda repositorio)
        {
            repositorio.RetirarDaReservaEAdicionarNoEstoque(item, quantidade, SessaoSistema.ObterUsuarioLogado(), OrigemEventoEstoque.MovimentacaoPedidoVendaApartirDeAlteracao);
        }

        private void EstoqueSaida(PedidoVendaProduto item, decimal quantidade, RepositorioPedidoVenda repositorio)
        {
            repositorio.RetirarDoEstoqueEAdicionarNaReserva(item, quantidade, SessaoSistema.ObterUsuarioLogado(), OrigemEventoEstoque.MovimentacaoPedidoVendaApartirDeAlteracao);
        }

        public void DeletarItemSelecionado()
        {
            if (ItemSelecionado == null)
            {
                throw new InvalidOperationException("Nenhum item selecionado para exclusão");
            }

            PedidoVenda.RemoverItem(ItemSelecionado);
            SalvarPedidoVenda();
        }

        public string ObterObservacao()
        {
            return PedidoVenda.Observacao;
        }

        public void AtualizarObservacao(string observacao)
        {
            PedidoVenda.Observacao = observacao.TrimOrEmpty();
            SalvarPedidoVenda();
        }

        public bool PossuiProdutoInativo()
        {
            return PedidoVenda.ItensPedidoVenda.Any(x => x.Produto.Ativo == false);
        }

        private void AtualizarPrecosComTabelaPreco()
        {
            if (PedidoVenda == null) return;

            var tabelaPreco = CarregaTabelaPrecoPorId(TabelaPrecoSelecionada);

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                PedidoVenda.TabelaPreco = tabelaPreco;
                PedidoVenda.RecalcularComtabelaPreco(new RepositorioTabelaPreco(sessao));

                SalvarPedidoVenda();

                transacao.Commit();
            }

            OnAtualizaTabelaPrecoListagemProdutos(tabelaPreco);
        }

        private TabelaPreco CarregaTabelaPrecoPorId(TabelaPrecoDto tabela)
        {
            if (tabela == null) return null;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioTabelaPreco = new RepositorioTabelaPreco(sessao);

                return repositorioTabelaPreco.GetPeloId(tabela.Id);
            }
        }

        private void OnAtualizaTabelaPrecoListagemProdutos(ITabelaPreco e)
        {
            AtualizaTabelaPrecoListagem?.Invoke(this, e);
        }
    }
}