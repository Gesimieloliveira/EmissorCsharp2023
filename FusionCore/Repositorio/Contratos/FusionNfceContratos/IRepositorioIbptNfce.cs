using FusionCore.FusionNfce.Fiscal.Tributacoes;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioIbptNfce : IRepositorio<NfceIbpt, NfceCodigoIbpt>
    {
        void Salvar(NfceIbpt ibpt);
        NfceIbpt GetPeloNcm(string ncm);
    }
}