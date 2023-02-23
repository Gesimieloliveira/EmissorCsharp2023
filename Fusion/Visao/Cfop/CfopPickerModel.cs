using System.Collections.Generic;
using System.IO;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Cfop;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.GridPicker;
using NHibernate.Util;

namespace Fusion.Visao.Cfop
{
    public class CfopPickerModel : GridPickerModel
    {
        private readonly RepositorioComun<CfopDTO> _repositorio;
        private IList<CfopDTO> _cacheLista;

        public CfopPickerModel()
        {
            _repositorio = new RepositorioComun<CfopDTO>();
        }

        protected override void OnInicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _repositorio.Sessao = sessao;
                var cfops = _repositorio.Busca(new TodosCfop());
                _cacheLista = cfops;
                PreecherListaCom(cfops);
            }
        }

        private void PreecherListaCom(IList<CfopDTO> cfops)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (cfops == null)
                throw new InvalidDataException("Nenhum cfop para listar");

            cfops.ForEach(AddCfopLista);
        }

        private void AddCfopLista(CfopDTO cfop)
        {
            ItensLista.Add(new GridPickerItem
            {
                Titulo = cfop.Descricao,
                Coluna1 = $"#Código CFOP: {cfop.Id}",
                ItemReal = cfop
            });
        }

        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (string.IsNullOrEmpty(input))
            {

                var lista = _cacheLista.ToList();

                lista.ForEach(AddCfopLista);
                return;
            }

            var listaFiltrada =
                _cacheLista.Where(cfop => cfop.Id == input || cfop.Descricao.ToUpper().Contains(input.ToUpper()))
                    .ToList();

            listaFiltrada.ForEach(AddCfopLista);
        }
    }
}