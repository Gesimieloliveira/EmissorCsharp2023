using System;
using System.Collections.Generic;
using FusionCore.FusionNfce.Produto;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace FusionCore.Repositorio.Filtros.BuscaProdutoNfce
{
    public class FiltroConsultaNfceProdutoBase : EntidadeBase<Guid>, IOpcaoBuscaProdutoNfce
    {
        public string Watermark { get; } = "Busca rápida por Nome (contenha) ou ID (igual)";

        public IEnumerable<ProdutoBaseDTO> Listar(int limite, string input, ISession sessao)
        {
            return new RepositorioProdutoNfce(sessao).BuscaProdutosAtivos(limite, input);
        }

        public Guid Id => new Guid("BB0C7ABB-39E2-45DD-A694-4BCF72AE365B");

        public override string ToString()
        {
            return "Por Nome ou ID";
        }


        protected override Guid ChaveUnica => Id;
    }
}