using System;
using System.Collections.Generic;
using FusionCore.FusionNfce.Produto;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace FusionCore.Repositorio.Filtros.BuscaProdutoNfce
{
    public class FiltroConsultaNfceProdutoReferencia : EntidadeBase<Guid>, IOpcaoBuscaProdutoNfce
    {
        public string Watermark { get; } = "Busca rápida por Referência (contenha)";

        public IEnumerable<ProdutoBaseDTO> Listar(int limite, string input, ISession sessao)
        {
            var repositorio = new RepositorioProdutoNfce(sessao);

            return repositorio.BuscarPorReferencia(limite, input);
        }

        public Guid Id => new Guid("91DC48DD-761A-482E-A9EA-F33BBC876CA8");

        public override string ToString()
        {
            return "Por Refêrencia";
        }

        protected override Guid ChaveUnica => Id;
    }
}