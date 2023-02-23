using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.CadastroUsuario;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.DI;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.ControleCaixa
{
    public class GridCaixasIndividuaisContexto : ViewModel
    {
        public GridCaixasIndividuaisContexto(ISessaoManager sessaoManager, IControleCaixaProvider caixaProvider)
        {
            SessaoManager = sessaoManager;
            CaixaProvider = caixaProvider;
            EstadoCaixaFiltro = EEstadoCaixa.Aberto;
        }

        public ISessaoManager SessaoManager { get; }
        public IControleCaixaProvider CaixaProvider { get; }

        public IEnumerable<CaixaIndividualDTO> Items
        {
            get => GetValue<IEnumerable<CaixaIndividualDTO>>();
            private set => SetValue(value);
        }

        public CaixaIndividualDTO ItemSelecionado
        {
            get => GetValue<CaixaIndividualDTO>();
            set => SetValue(value);
        }

        public IEnumerable<IUsuario> Operadores
        {
            get => GetValue<IEnumerable<IUsuario>>();
            set => SetValue(value);
        }

        public IUsuario OperadorFiltro
        {
            get => GetValue<IUsuario>();
            set => SetValue(value);
        }

        public EEstadoCaixa? EstadoCaixaFiltro
        {
            get => GetValue<EEstadoCaixa?>();
            set => SetValue(value);
        }

        public DateTime DataSaldo
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public decimal SaldoCaixaLoja
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public IEnumerable<FluxoContaCaixa> EventosCaixaLoja
        {
            get => GetValue<IEnumerable<FluxoContaCaixa>>();
            set => SetValue(value);
        }

        public void CarregarFiltros()
        {
            using (var sessao = SessaoManager.CriaSessao())
            {
                Operadores = new RepositorioUsuario(sessao).BuscaTodos();
            }
        }

        public void CarregarDadosCaixasIndividuais()
        {
            //TODO: Melhorar filtro de operador no gerenciar/listagem de caixas

            using (var sessao = SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioCaixaIndividual(sessao);
                var items = repositorio.ListarCaixas();

                if (OperadorFiltro != null) items = items.Where(i => i.NomeOperador == OperadorFiltro.Login);
                if (EstadoCaixaFiltro != null) items = items.Where(i => i.Estado == EstadoCaixaFiltro);

                Items = items.OrderByDescending(i => i.DataAbertura);
            }
        }

        public void CarregarDadosCaixaLoja()
        {
            var now = DateTime.Now;

            DataSaldo = now;

            using (var sessao = SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioContaCaixa(sessao);

                SaldoCaixaLoja = repositorio.ObtemSaldoAtualCaixaLoja();
                EventosCaixaLoja = repositorio.BuscarOperacoesApartirDe(now.AddDays(-60));
            }
        }
    }
}