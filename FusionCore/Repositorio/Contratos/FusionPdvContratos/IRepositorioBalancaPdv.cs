using FusionCore.FusionPdv.Configuracoes;

namespace FusionCore.Repositorio.Contratos.FusionPdvContratos
{
    public interface IRepositorioBalancaPdv : IRepositorio<BalancaPdv, byte>
    {
        void Salvar(BalancaPdv balancaPdv);
        BalancaPdv BuscaUnicaConfiguracao();
    }
}