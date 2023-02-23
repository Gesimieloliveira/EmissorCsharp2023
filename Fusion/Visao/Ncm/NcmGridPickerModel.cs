using System.Collections.Generic;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Ncm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.GridPicker;
using NHibernate.Util;

namespace Fusion.Visao.Ncm
{
    public class NcmGridPickerModel : GridPickerModel
    {
        private readonly RepositorioComun<NcmDTO> _repositorio;

        public NcmGridPickerModel()
        {
            _repositorio = new RepositorioComun<NcmDTO>();
        }

        protected override void OnInicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _repositorio.Sessao = sessao;
                var ncms = _repositorio.Busca(new TodosNcm());
                PopularListaCom(ncms);
            }
        }

        public override void AplicaPesquisa(string input)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _repositorio.Sessao = sessao;
                var ncms = _repositorio.Busca(new NcmBuscaRapida(input));
                PopularListaCom(ncms);
            }
        }

        private void PopularListaCom(IEnumerable<NcmDTO> ncms)
        {
            ItemSelecionado = null;
            ItensLista.Clear();

            ncms.ForEach(ncm =>
            {
                var item = new GridPickerItem
                {
                    Titulo = ncm.Descricao,
                    Coluna1 = "Ncm: " + ncm.Id,
                    Coluna2 = "CEST: " + ncm.Cest,
                    ItemReal = ncm
                };

                ItensLista.Add(item);
            });
        }
    }
}