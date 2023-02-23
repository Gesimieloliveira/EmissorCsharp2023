using System;
using System.Collections.Generic;
using FusionCore.FusionNfce.Produto;
using NHibernate;

namespace FusionCore.Repositorio.Filtros.BuscaProdutoNfce
{
    public interface IOpcaoBuscaProdutoNfce
    {
        string Watermark { get; }
        IEnumerable<ProdutoBaseDTO> Listar(int limite, string input, ISession sessao);

        Guid Id { get; }
    }
}