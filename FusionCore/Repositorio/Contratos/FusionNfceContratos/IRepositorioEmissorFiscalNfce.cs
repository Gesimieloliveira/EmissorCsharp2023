using FusionCore.FusionNfce.EmissorFiscal;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioEmissorFiscalNfce : IRepositorio<NfceEmissorFiscal, byte>
    {
        void SalvarESincronizar(NfceEmissorFiscal emissorFiscal);
    }
}