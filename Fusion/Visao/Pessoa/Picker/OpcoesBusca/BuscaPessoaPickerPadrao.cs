using System.Collections.Generic;
using FusionCore.CadastroPessoa;
using FusionWPF.Base.GridPicker.OpcoesBuscas;
using NHibernate;

namespace Fusion.Visao.Pessoa.Picker.OpcoesBusca
{
    public class BuscaPessoaPickerPadrao : IOpcaoBusca
    {
        public string Watermark { get; } = "pesquisa por nome, nome fantasia, cnpj, cpf, código";

        public IList<TPessoaGrid> Listar<TPessoaGrid>(string input, ISession sessao)
        {
            var repositorio = new RepositorioPessoaPicker(sessao);
            var pessoas = repositorio.BuscarPorVariasCorrespondencias(input);

            return pessoas as IList<TPessoaGrid>;
        }

        public override string ToString()
        {
            return "padrão";
        }
    }
}