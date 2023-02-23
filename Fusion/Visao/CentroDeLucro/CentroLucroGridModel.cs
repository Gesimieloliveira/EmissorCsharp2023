using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using NHibernate.Criterion;

namespace Fusion.Visao.CentroDeLucro
{
    public class CentroLucroGridModel : ViewModel
    {
        public CentroLucroGridModel()
        {
            Items = new ObservableCollection<CentroLucro>();
            ObterCentroDeLucro();
        }

        public string DescricaoPesquisa
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public CentroLucro ItemSelecionado
        {
            get => GetValue<CentroLucro>();
            set => SetValue(value);
        }

        public IList<CentroLucro> Items
        {
            get => GetValue<IList<CentroLucro>>();
            set => SetValue(value);
        }

        public void ObterCentroDeLucro()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCentroLucro(sessao);
                var todos = repositorio.BuscaTodos();

                Items = todos.OrderBy(i => i.Nivel).ToList();
            }
        }

        public void Novo()
        {
            new CentroLucroForm(new CentroLucro {CentroLucroPai = ItemSelecionado}).ShowDialog();
            ObterCentroDeLucro();
        }

        public void Editar()
        {
            new CentroLucroForm(ItemSelecionado).ShowDialog();
            ObterCentroDeLucro();
        }

        public void ObterCentroDeLucroPorDescricao()
        {
            ObterCentroDeLucro();
            var itens = Items.Where(i => i.Descricao.IsLike(DescricaoPesquisa, MatchMode.Anywhere));

            Items = itens.OrderBy(i => i.Nivel).ToList();
        }
    }
}