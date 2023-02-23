using System;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.PedidoVenda.Fabricas;
using FusionCore.FusionAdm.PedidoVenda.Servicos.Converter;
using FusionCore.FusionNfce.Extencoes;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate;
using NHibernate.Util;

namespace FusionNfce.Visao.PedidoVendaLista
{
    public class PedidoVendaListaFormModel : ViewModel
    {
        private bool _isPossuiPedidos;
        private ObservableCollection<PedidoVendaDTO> _pedidosDeVenda;
        private PedidoVendaDTO _itemSelecionado;
        private PedidoVenda _pedidoVenda;

        public PedidoVendaListaFormModel()
        {
            EfetuarBusca();
        }

        public event EventHandler<Nfce> NfceFoiConvertida;

        public PedidoVendaDTO ItemSelecionado
        {
            get => _itemSelecionado;
            set
            {
                if (Equals(value, _itemSelecionado)) return;
                _itemSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<PedidoVendaDTO> PedidosDeVenda
        {
            get => _pedidosDeVenda;
            set
            {
                if (Equals(value, _pedidosDeVenda)) return;
                _pedidosDeVenda = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPossuiPedidos
        {
            get => _isPossuiPedidos;
            set
            {
                if (value == _isPossuiPedidos) return;
                _isPossuiPedidos = value;
                PropriedadeAlterada();
            }
        }

        private void EfetuarBusca()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoServerNfce).AbrirSessao())
            {
                var repositorioPedidoVenda = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);

                var filtro = new FiltroListagemPedido
                {
                    Abertos = false,
                    Finalizados = true,
                    ApenasPedidos = true
                };

                var lista = repositorioPedidoVenda.BuscaPedidosDto(filtro);

                var list = lista.OrderByDescending(x => x.Id).ToList();

                PedidosDeVenda = new ObservableCollection<PedidoVendaDTO>(list);

                IsPossuiPedidos = list.Count > 0;
            }
        }

        public void ImprimirPedido()
        {
            if (ItemSelecionado.IsCancelado)
            {
                DialogBox.MostraInformacao("Pedido de venda/orçamento cancelado pelo operador");
                return;
            }

            new ImprimirPedidoVenda().Imprimir(ItemSelecionado.Id);
        }

        public void ConvertePedidoParaNfce()
        {
            CarregarPedidoVenda();
            ConverterParaNfce();
        }

        private void CarregarPedidoVenda()
        {
            using (var s = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoServerNfce).AbrirSessao())
            {
                var repositorioPedidoVenda = FactoryRepositorioPedidoVenda.CriaRepositorio(s);

                _pedidoVenda = repositorioPedidoVenda.GetPeloIdLazy(ItemSelecionado.Id);
                _pedidoVenda.ValidarParaFinalizacao();
            }
        }

        private void ConverterParaNfce()
        {
            // Retirado checagem de estoque negativo por estar aceitando converter apenas Pedidos
            Nfce nfceConvertida = null;

            using (var sServer = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoServerNfce).AbrirSessao())
            using (var sNfce = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                sServer.BeginTransaction();
                sNfce.BeginTransaction();

                var conversor = new ConvertePedidoDeVendaParaNFCe(
                    _pedidoVenda,
                    sNfce,
                    SessaoSistemaNfce.Usuario,
                    SessaoSistemaNfce.Empresa().RegimeTributario
                );

                conversor.Executar();

                var nfce = conversor.ObterNfce();

                PersisteNfce(nfce, sNfce);
                FinalizaPedidoVenda(sServer);

                sServer.Transaction.Commit();
                sNfce.Transaction.Commit();

                nfceConvertida = nfce;
            }

            NfceFoiConvertida?.Invoke(this, nfceConvertida);
        }

        private void FinalizaPedidoVenda(ISession sServer)
        {
            var usuarioAdm = SessaoSistemaNfce.Usuario.ToAdm();
            var repositorio = FactoryRepositorioPedidoVenda.CriaRepositorio(sServer);

            repositorio.Refresh(_pedidoVenda);
            _pedidoVenda.Faturar();

            repositorio.RetirarDaReservaEstoquePedidoVenda(_pedidoVenda, usuarioAdm, OrigemEventoEstoque.PedidoVendaReservaEfetuadaNfce);
            repositorio.SalvarAlteracoes(_pedidoVenda);
        }

        private static void PersisteNfce(Nfce nfce, ISession sNfce)
        {
            var repositorioNfce = new RepositorioNfce(sNfce);

            repositorioNfce.Salvar(nfce);
            repositorioNfce.SalvarEmitente(nfce.Emitente);

            if (nfce.Destinatario != null)
            {
                repositorioNfce.SalvarDestinatario(nfce.Destinatario);
            }

            foreach (var i in nfce.ObterTodosItens())
            {
                repositorioNfce.SalvarItem(i);
            }

            foreach (var pg in nfce.ObterFormaPagamentoNfces())
            {
                repositorioNfce.SalvarPagamento(pg);
            }

            if (nfce.Cobranca != null)
            {
                repositorioNfce.Salvar(nfce.Cobranca);
                nfce.Cobranca.CobrancaDuplicatas.ForEach(repositorioNfce.Salvar);
            }
        }
    }
}