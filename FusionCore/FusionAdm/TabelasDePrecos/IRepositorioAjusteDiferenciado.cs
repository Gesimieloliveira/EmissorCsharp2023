using FusionCore.FusionAdm.TabelasDePrecos.Dtos;

namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public interface IRepositorioAjusteDiferenciado
    {
        AjusteDiferenciadoDto BuscarAjusteDiferenciado(int produtoId, int tabelaPrecoId);
    }
}