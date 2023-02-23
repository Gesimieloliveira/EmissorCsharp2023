﻿using System.Collections.Generic;
using FusionCore.CadastroPessoa;
using FusionWPF.Base.GridPicker.OpcoesBuscas;
using NHibernate;

namespace Fusion.Visao.Pessoa.Picker.OpcoesBusca
{
    public class BuscaPessoaPickerPorDocumento : IOpcaoBusca
    {
        public string Watermark { get; } = "pesquisa por cpf, cnpj";

        public IList<T> Listar<T>(string input, ISession sessao)
        {
            var repositorio = new RepositorioPessoaPicker(sessao);
            var pessoas = repositorio.BuscaPorDocumento(input);

            var lista = (IList<T>)pessoas;

            return lista;
        }

        public override string ToString()
        {
            return "cpf/cnpj";
        }
    }
}