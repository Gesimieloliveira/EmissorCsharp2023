using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;
using CentroDeCusto = FusionCore.FusionAdm.Financeiro.CentroCusto;

namespace Fusion.Visao.CentroCusto
{
    public class CentroCustoPickerModel : GridPickerModel
    {
        private IList<CentroDeCusto> _cache;

        private void PreencherListaCom()
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCentroCusto(sessao);

                _cache = repositorio.BuscaTodos().OrderBy(cl => cl.Ordenacao).ToList();
            }

            _cache.ForEach(AddCentroCustoLista);
        }

        private void AddCentroCustoLista(CentroDeCusto centroCusto)
        {
            ItensLista.Add(new GridPickerItem
            {
                Titulo = centroCusto.Nivel + " - " + centroCusto.Descricao,
                ItemReal = centroCusto
            });
        }

        protected override void OnInicializar()
        {
            PreencherListaCom();
        }

        protected override void OnPickItem(IGridPickerItem item)
        {
            var centroCusto = ItemSelecionado.ItemReal as CentroDeCusto;

            if (centroCusto?.Itens != null && centroCusto.Itens.Count != 0)
            {
                DialogBox.MostraInformacao("Centro de Custo não é o último nível");
                return;
            }

            base.OnPickItem(item);
        }

        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();
            if (string.IsNullOrEmpty(input))
            {
                _cache.ForEach(AddCentroCustoLista);
                return;
            }

            var lista = _cache.Where(cl => cl.Descricao.Contains(input.ToUpper())).OrderBy(cl => cl.Ordenacao).ToList();

            lista.ForEach(AddCentroCustoLista);
        }
    }
}