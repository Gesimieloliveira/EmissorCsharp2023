using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Filtros;
using FusionCore.Sessao;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Vendas.Repositorio;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.ListarAbertos
{
    public class ListarAbertosViewModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;

        public ListarAbertosViewModel(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
            SetValue(true, nameof(MostraAbertosIsChecked));
        }

        public bool PossuiFaturamentos
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public IList<FaturamentoSlim> Faturamentos
        {
            get => GetValue<IList<FaturamentoSlim>>();
            set => SetValue(value);
        }

        public FaturamentoSlim ItemSelecionado
        {
            get => GetValue<FaturamentoSlim>();
            set => SetValue(value);
        }

        public bool MostraAbertosIsChecked
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                RefreshData();
            }
        }

        public bool MostraFinalizadosChecked
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                RefreshData();
            }
        }

        public event EventHandler<FaturamentoSlim> Selecionado;
        public event EventHandler<FaturamentoSlim> Impressao;

        public void RefreshData()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioFaturamento(sessao);
                Faturamentos = BuscaOsFaturamentos(repositorio);
                PossuiFaturamentos = Faturamentos.Any();
            }
        }

        private IList<FaturamentoSlim> BuscaOsFaturamentos(RepositorioFaturamento repositorio)
        {
            var faturamentos = new List<FaturamentoSlim>();

            if (MostraAbertosIsChecked)
            {
                var filtro = FaturamentoFiltroBuilder.Novo.ComEstadoAtual(Estado.Aberto);
                faturamentos.AddRange(repositorio.Lista(filtro));
            }

            if (MostraFinalizadosChecked)
            {
                var today = DateTime.Today;
                var periodo = new FiltroPeriodo(today.AddDays(-7), today);
                var filtro = FaturamentoFiltroBuilder.Novo.ComPeriodoFinalizacao(periodo);

                faturamentos.AddRange(repositorio.Lista(filtro));
            }

            return faturamentos.OrderByDescending(i => i.Id).ToList();
        }

        public void OnSelecionarItem()
        {
            Selecionado?.Invoke(this, ItemSelecionado);
        }

        public void OnImprimirSelecionado()
        {
            Impressao?.Invoke(this, ItemSelecionado);
        }
    }
}