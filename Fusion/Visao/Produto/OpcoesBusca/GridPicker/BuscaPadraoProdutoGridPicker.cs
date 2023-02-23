using System.Collections.Generic;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace Fusion.Visao.Produto.OpcoesBusca.GridPicker
{
    public class BuscaPadraoProdutoGridPicker : FusionWPF.Base.GridPicker.OpcoesBuscas.IOpcaoBusca
    {
        public string Watermark { get; } = "Digite enter para buscar";

        public IList<T> Listar<T>(string input, ISession sessao)
        {
            var repositorio = new RepositorioProduto(sessao);
            return (IList<T>) repositorio.BuscaRapida(input);
        }

        public override string ToString()
        {
            return "padrão";
        }
    }
}