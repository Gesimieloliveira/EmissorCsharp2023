using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.CentroDeLucro
{
    public class CentroLucroPickerModel : GridPickerModel
    {
        private IList<CentroLucro> _cache;

        private void PreencherListaCom()
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCentroLucro(sessao);

                _cache = repositorio.BuscaTodos().OrderBy(cl => cl.Ordenacao).ToList();
            }

            _cache.ForEach(AddCentroLucroLista);
        }

        private void AddCentroLucroLista(CentroLucro centroLucro)
        {
            ItensLista.Add(new GridPickerItem
            {
                Titulo = centroLucro.Nivel + " - " + centroLucro.Descricao,
                ItemReal = centroLucro
            });
        }

        protected override void OnInicializar()
        {
            PreencherListaCom();
        }

        protected override void OnPickItem(IGridPickerItem item)
        {
            var centroLucro = ItemSelecionado.ItemReal as CentroLucro;

            if (centroLucro?.Itens != null && centroLucro.Itens.Count != 0)
            {
                DialogBox.MostraInformacao("Centro de Lucro não é o último nível");
                return;
            }

            base.OnPickItem(item);
        }

        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();
            if (string.IsNullOrEmpty(input))
            {
                _cache.ForEach(AddCentroLucroLista);
                return;
            }

            var lista = _cache.Where(cl => cl.Descricao.Contains(input.ToUpper())).OrderBy(cl => cl.Ordenacao).ToList();

            lista.ForEach(AddCentroLucroLista);
        }
    }
}