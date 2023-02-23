using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.CteEletronico.Inutilizacao
{
    public interface IInutilizar
    {
        EstadoDTO Estado { get; }
        string Cnpj { get; }
        ModeloDocumento ModeloDocumento { get; }
        short SerieInutilizar { get; }
        TipoAmbiente TipoAmbienteSefaz { get; }
    }
}