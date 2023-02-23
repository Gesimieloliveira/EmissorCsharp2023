using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioCentroCusto : IRepositorio<CentroCusto, short>
    {
        void Salvar(CentroCusto centroCusto);

        IList<CentroCusto> ObterCategoriasPai();

        IList<CentroCusto> ObterCategoriasPorDescricao(string descricao);

        void Deletar(CentroCusto centroCusto);
    }
}