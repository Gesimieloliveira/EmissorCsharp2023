using System.Collections.Generic;
using System.IO;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using NHibernate.Util;

namespace Fusion.Visao.ProdutoLocalizacoes
{
    public class ProdutoLocalizacaoGridPickerModel : GridPickerModel
    {
        protected override void OnInicializar()
        {
            BuscaTodos();
        }

        private void BuscaTodos()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProdutoLocalizacao(sessao);
                PreecherListaCom(repositorio.BuscaTodos());
            }
        }

        public override void AplicaPesquisa(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                BuscaTodos();
                return;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProdutoLocalizacao(sessao);
                PreecherListaCom(repositorio.BuscaRapida(input));
            }
        }

        private void PreecherListaCom(IEnumerable<ProdutoLocalizacao> produtoLocalizacoes)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (produtoLocalizacoes == null)
                throw new InvalidDataException("Nenhum produto para listar");

            produtoLocalizacoes.ForEach(AddProdutoLocalizacaoLista);
        }

        private void AddProdutoLocalizacaoLista(ProdutoLocalizacao produtoLocalizacao)
        {
            ItensLista.Add(new GridPickerItem
            {
                Titulo = produtoLocalizacao.Nome,
                Coluna1 = $"#ID: {produtoLocalizacao.Id}",
                ItemReal = produtoLocalizacao
            });
        }
    }
}