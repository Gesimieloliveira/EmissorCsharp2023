using System;
using System.Collections.Generic;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.DI;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace FusionWPF.SharedViews.ControleCaixa
{
    public class ResultadoDoCaixaContexto : ViewModel
    {
        public ResultadoDoCaixaContexto(ISessaoManager sessaoManager, IControleCaixaProvider caixaProvider)
        {
            SessaoManager = sessaoManager;
            CaixaProvider = caixaProvider;
            DataInicio = DateTime.Today;
            DataFinal = DataInicio.AddSeconds(86400 - 1);
        }

        public ISessaoManager SessaoManager { get; }
        public IControleCaixaProvider CaixaProvider { get; }

        public IEnumerable<CaixaIndividualDTO> Caixas
        {
            get => GetValue<IEnumerable<CaixaIndividualDTO>>();
            private set => SetValue(value);
        }

        public DateTime DataInicio
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime DataFinal
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public void CarregarResultado()
        {
            using (var sessao = SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioResultadoCaixa(sessao);
                var caixas = repositorio.PorPeriodo(DataInicio, DataFinal);

                Caixas = caixas;
            }
        }
    }
}