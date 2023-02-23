using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fusion.Visao.PedidoDeVenda.Servicos;
using FusionCore.FusionAdm.PedidoVenda.Fabricas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda;
using FusionCore.Repositorio.Filtros;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PedidoDeVenda
{
    public class ListaPedidoVendaControlModel : ViewModel
    {
        private readonly FiltroListagemPedido _filtroRapido;
        private ObservableCollection<PedidoVendaDTO> _pedidosDeVenda;
        private PedidoVendaDTO _itemSelecionado;

        public ListaPedidoVendaControlModel()
        {
            _filtroRapido = new FiltroListagemPedido { Abertos = true };
        }

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
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool FinalizadosChecked
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                _filtroRapido.Finalizados = value;

                AtualizarListagem();
            }
        }

        public bool SevenDaysChecked
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                _filtroRapido.Ultimos7Dias = value;

                AtualizarListagem();
            }
        }

        public event EventHandler<PedidoVendaDTO> FoiSelecionado;

        public void AtualizarListagem()
        {
            _filtroRapido.Abertos = true;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);
                var lista = repositorio.BuscaPedidosDto(_filtroRapido);
                var listaInversa = lista.OrderByDescending(i => i.Id);

                PedidosDeVenda = new ObservableCollection<PedidoVendaDTO>(listaInversa);
                IsPossuiPedidos = PedidosDeVenda.Any();
            }
        }

        public void VisualizaPedido()
        {
            var impressor = new ImpressorPedidoVenda(new SessaoManagerAdm());

            impressor.Visualizar(ItemSelecionado.Id);
        }

        public void SelecionarPedido()
        {
            if (ItemSelecionado == null)
            {
                throw new InvalidOperationException("Preciso que selecione um Pedido da Lista!");
            }

            if (ItemSelecionado.IsCancelado)
            {
                throw new InvalidOperationException("Está cancelado!");
            }

            if (ItemSelecionado.IsFaturado)
            {
                throw new InvalidOperationException("Está Faturado");
            }

            FoiSelecionado?.Invoke(this, ItemSelecionado);
        }
    }
}