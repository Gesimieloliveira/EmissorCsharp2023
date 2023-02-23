using System.Collections.Generic;
using System.IO;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.GridPicker;
using NHibernate.Util;

namespace Fusion.Visao.Terminal
{
    public class CfopPickerNfceModel : GridPickerModel
    {
        private IList<CfopDTO> _cacheLista;

        protected override void OnInicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCfop(sessao);
                _cacheLista = repositorio.BuscarCfopParaNfce();
            }

            PreecherListaCom(_cacheLista);
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