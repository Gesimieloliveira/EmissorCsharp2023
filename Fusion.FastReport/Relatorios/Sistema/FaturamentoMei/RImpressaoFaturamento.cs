using System.Collections.Generic;
using Fusion.FastReport.DataSources;
using Fusion.FastReport.Repositorios;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Sistema.FaturamentoMei
{
    public abstract class RImpressaoFaturamento : RelatorioBase
    {
        private int _faturamentoId;
        private bool _duplicarImpressao;

        protected RImpressaoFaturamento(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        public void ComId(int id)
        {
            _faturamentoId = id;
        }

        public void DuplicarImpressao(bool opcao)
        {
            _duplicarImpressao = opcao;
        }

        protected override void PrepararDados()
        {
            using (var sessao = SessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioFaturamento(sessao);
                var faturamento = repositorio.GetFaturamento(_faturamentoId);
                var itens = repositorio.GetItens(_faturamentoId);

                var faturamentos = new List<DsFaturamento> {faturamento};

                if (_duplicarImpressao)
                    NumeroCopias(2);

                RegistraDados("dsFaturamento", faturamentos);
                RegistraDados("dsItem", itens);
            }
        }
    }
}