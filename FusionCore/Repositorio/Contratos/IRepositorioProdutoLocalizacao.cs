using System.Collections.Generic;
using FusionCore.FusionAdm.Produtos;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioProdutoLocalizacao : IRepositorio<ProdutoLocalizacao, short>
    {
        void Salvar(ProdutoLocalizacao produtoLocalizacao);
        void Deletar(ProdutoLocalizacao produtoLocalizacao);
        IList<ProdutoLocalizacao> BuscaRapida(string texto);
    }
}