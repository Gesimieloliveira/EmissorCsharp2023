using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioCentroLucro : IRepositorio<CentroLucro, short>
    {
        void Salvar(CentroLucro centroLucro);

        IList<CentroLucro> ObterCategoriasPai();

        IList<CentroLucro> ObterCategoriasPorDescricao(string descricao);

        void Deletar(CentroLucro centroLucro);
    }
}