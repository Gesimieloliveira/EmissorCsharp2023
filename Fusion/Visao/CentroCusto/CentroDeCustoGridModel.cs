using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using NHibernate.Criterion;
using CentroDeCusto = FusionCore.FusionAdm.Financeiro.CentroCusto;

namespace Fusion.Visao.CentroCusto
{
    public class CentroDeCustoGridModel : ViewModel
    {
        public string DescricaoPesquisa
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public CentroDeCusto ItemSelecionado
        {
            get => GetValue<CentroDeCusto>();
            set => SetValue(value);
        }

        public IList<CentroDeCusto> Items
        {
            get => GetValue<IList<CentroDeCusto>>();
            set => SetValue(value);
        }

        public CentroDeCustoGridModel()
        {
            Items = new List<CentroDeCusto>();
            ObterCentroDeCustos();
        }

        public void ObterCentroDeCustos()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCentroCusto(sessao);
                var todos = repositorio.BuscaTodos();

                Items = todos.OrderBy(i => i.Nivel).ToList();
            }
        }

        public void Novo()
        {
            new CentroCustoForm(new CentroDeCusto {CentroCustoPai = ItemSelecionado}).ShowDialog();

            ObterCentroDeCustos();
        }

        public void Editar()
        {
            new CentroCustoForm(ItemSelecionado).ShowDialog();
            ObterCentroDeCustos();
        }

        public void ObterCentroDeCustosPorDescricao()
        {
            ObterCentroDeCustos();

            var itens = Items.Where(i => i.Descricao.IsLike(DescricaoPesquisa, MatchMode.Anywhere));
            Items = itens.OrderBy(i => i.Nivel).ToList();
        }
    }
}