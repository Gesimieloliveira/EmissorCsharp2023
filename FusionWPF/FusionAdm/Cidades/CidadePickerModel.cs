using System.Collections.Generic;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.GridPicker;
using NHibernate.Util;

namespace FusionWPF.FusionAdm.Cidades
{
    public class CidadePickerModel : GridPickerModel
    {
        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCidade(sessao);
                var cidades = repositorio.Busca(input);

                AddLista(cidades);
            }
        }

        private void AddLista(ICollection<CidadeDTO> cidades)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (cidades == null || cidades.Count == 0)
            {
                return;
            }

            cidades.ForEach(AddLista);
        }

        private void AddLista(CidadeDTO cidade)
        {
            ItensLista.Add(new GridPickerItem
            {
                Titulo = cidade.Nome,
                // ReSharper disable once PossibleNullReferenceException
                Coluna1 = $"#Id: {cidade.Id}",
                Coluna2 = $"#SiglaUF: {cidade.SiglaUf}",
                Coluna3 = $"#Codigo IBGE: {cidade.CodigoIbge}",
                ItemReal = cidade
            });
        }
    }
}