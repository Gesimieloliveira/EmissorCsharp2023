using System.Collections.Generic;
using FusionCore.FusionAdm.Estoque.Movimentacoes;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioMovimentoEstoque : IRepositorio<MovimentoEstoque, int>
    {
        MovimentoEstoque GetPeloId(int id, bool loadLazy = true);
        IList<MovimentoEstoque> BuscaRapida(string input);
        MovimentoEstoque Persiste(MovimentoEstoque movimento);
        MovimentoEstoque Altera(MovimentoEstoque movimento);
        void Deletar(MovimentoEstoque movimento);
    }
}