using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioInutilizacao : ISuporteSalvar<NfeInutilizacaoNumeracaoDTO>
    {
        bool FaixaInutilizadaJa(short serie, ModeloDocumento modeloDocumento, int numeroInicial, int numeroFinal);
    }
}