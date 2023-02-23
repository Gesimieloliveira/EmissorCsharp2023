using System.Collections.Generic;
using System.IO;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;

namespace FusionWPF.FusionAdm.TabelaIbpt
{
    public class IbptPicker : GridPickerModel
    {
        private IList<Ibpt> _cache;

        protected override void OnInicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _cache = new RepositorioIbpt(sessao).BuscarTodosPeloNbs();
            }

            PreecherListaCom(_cache.ToList());
        }

        private void PreecherListaCom(List<Ibpt> listaIbpt)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (listaIbpt == null)
                throw new InvalidDataException("Nenhum IBPT para listar");

            listaIbpt.ForEach(AddCfopLista);
        }

        private void AddCfopLista(Ibpt obj)
        {
            ItensLista.Add(new GridPickerItem
            {
                Titulo = obj.Descricao,
                Coluna1 = $"#Código IBPT: {obj.Codigo}",
                ItemReal = obj
            });
        }

        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (string.IsNullOrEmpty(input))
            {

                var lista = _cache.ToList();

                lista.ForEach(AddCfopLista);
                return;
            }

            var listaFiltrada =
                _cache.Where(cfop => cfop.Codigo == input || cfop.Descricao.ToUpper().Contains(input.ToUpper()))
                    .ToList();

            listaFiltrada.ForEach(AddCfopLista);
        }
    }
}