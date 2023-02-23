using System.Collections.Generic;
using FusionCore.FusionNfce.Servico.Estoque;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioEstoqueModelNfce : IRepositorio<EstoqueModelNfce, int>
    {
        void SalvarESincronizar(EstoqueModelNfce estoqueModel);

        void SalvarENaoSincronizar(EstoqueModelNfce estoqueModel);

        IEnumerable<EstoqueModelNfce> BuscarTodosEstoquesParaSincronizacao();
    }
}