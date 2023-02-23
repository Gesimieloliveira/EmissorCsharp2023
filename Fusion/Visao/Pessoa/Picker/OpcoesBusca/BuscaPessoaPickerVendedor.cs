using System.Collections.Generic;
using FusionCore.CadastroPessoa;
using FusionWPF.Base.GridPicker.OpcoesBuscas;
using NHibernate;

namespace Fusion.Visao.Pessoa.Picker.OpcoesBusca
{
    public class BuscaPessoaPickerVendedor : IOpcaoBusca
    {
        public string Watermark { get; } = "pesquisa por vendedor";

        public IList<TPessoaGrid> Listar<TPessoaGrid>(string nome, ISession sessao)
        {
            var repositorio = new RepositorioPessoaPicker(sessao);
            var vendedores = repositorio.BuscaPorVendedor(nome);

            return vendedores as IList<TPessoaGrid>;
        }

        public override string ToString()
        {
            return "vendedor";
        }
    }
}
