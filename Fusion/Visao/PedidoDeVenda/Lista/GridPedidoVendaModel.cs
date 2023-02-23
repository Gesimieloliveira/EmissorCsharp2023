using System;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.PedidoVenda.Fabricas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda;
using FusionCore.Repositorio.Filtros;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PedidoDeVenda.Lista
{
    public class GridPedidoVendaModel : ViewModel
    {
        private readonly FiltroGridPedido _filtro = new FiltroGridPedido();

        public GridPedidoVendaModel()
        {
            FiltroCriadoApartir = DateTime.Now.AddDays(-30);
        }

        private ObservableCollection<PedidoVendaDTO> _pedidosDeVenda;
        private PedidoVendaDTO _selecionado;

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

        public EstadoAtual? FiltroEstadoAtual
        {
            get => GetValue<EstadoAtual?>();
            set
            {
                SetValue(value);
                _filtro.EstadoAtual = value;
            }
        }

        public DateTime? FiltroCriadoApartir
        {
            get => GetValue<DateTime?>();
            set
            {
                SetValue(value);
                _filtro.CriadoApos = value;
            }
        }

        public int? FiltroNumero
        {
            get => GetValue<int?>();
            set
            {
                SetValue(value);
                _filtro.NumeroIgual = value;
            }
        }

        public string FiltroNomeCliente
        {
            get => GetValue();
            set
            {
                SetValue(value);
                _filtro.NomeClienteContem = value;
            }
        }

        public string FiltroReferencia
        {
            get => GetValue();
            set
            {
                SetValue(value);
                _filtro.ReferenciaDocumentoContem = value;
            }
        }

        public PedidoVendaDTO Selecionado
        {
            get => _selecionado;
            set
            {
                if (Equals(value, _selecionado)) return;
                _selecionado = value;
                PropriedadeAlterada();
            }
        }

        public void AtualizarLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);
                var lista = repositorio.BuscaPedidosDto(_filtro);
                var listaInversa = lista.OrderByDescending(i => i.CriadoEm);

                PedidosDeVenda = new ObservableCollection<PedidoVendaDTO>(listaInversa);
            }
        }
    }
}