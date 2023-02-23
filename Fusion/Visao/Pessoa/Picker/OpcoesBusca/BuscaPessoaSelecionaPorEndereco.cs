using System.Collections.Generic;
using FusionCore.CadastroPessoa;
using FusionWPF.Base.GridPicker.OpcoesBuscas;
using NHibernate;

namespace Fusion.Visao.Pessoa.Picker.OpcoesBusca
{
    public class BuscaPessoaSelecionaPorEndereco : IOpcaoBusca
    {
        public string Watermark { get; } = "pesquisa por endereço (logradouro)";

        public IList<T> Listar<T>(string input, ISession sessao)
        {
            var repositorio = new RepositorioPessoaPicker(sessao);
            var pessoas = repositorio.BuscaPorEndereco(input);

            var lista = (IList<T>)pessoas;

            return lista;
        }

        public override string ToString()
        {
            return "endereço (logradouro)";
        }
    }
}