using FusionCore.NfceSincronizador.Flags;

namespace FusionCore.NfceSincronizador.Contratos
{
    public interface ISincronizavelAdm
    {
        string Referencia { get; }
        EntidadeSincronizavel EntidadeSincronizavel { get; }
    }
}
