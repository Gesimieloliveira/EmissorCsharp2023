using System;
using System.Collections.Generic;
using FusionCore.Sessao;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Vendas.Repositorio;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Vendas.Gerenciamento.Listagem
{
    public class GerenciarFaturamentosContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;

        public GerenciarFaturamentosContexto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
            FiltroEstado = Estado.Aberto;
        }

        public DateTime? FiltroCriadoApartir
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public Estado? FiltroEstado
        {
            get => GetValue<Estado?>();
            set => SetValue(value);
        }

        public int? FiltroNumero
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public string FiltroNomeCliente
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public IList<FaturamentoSlim> Faturamentos
        {
            get => GetValue<IList<FaturamentoSlim>>();
            set => SetValue(value);
        }

        public FaturamentoSlim Selecionado
        {
            get => GetValue<FaturamentoSlim>();
            set => SetValue(value);
        }

        public void Inicializar()
        {
            AplicaBusca();
        }

        private void AplicaBusca()
        {
            var filtro = FaturamentoFiltroBuilder.Novo
                .ComEstadoAtual(FiltroEstado)
                .ComCriacaoApartir(FiltroCriadoApartir)
                .ComNumero(FiltroNumero)
                .ComNomeCliente(FiltroNomeCliente);

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioFaturamento(sessao);
                Faturamentos = repositorio.Lista(filtro, true);
            }
        }

        public void AplicaFiltro()
        {
            AplicaBusca();
        }
    }
}