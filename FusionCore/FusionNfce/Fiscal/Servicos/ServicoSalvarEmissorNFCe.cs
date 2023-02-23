using FusionCore.FusionNfce.EmissorFiscal;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;

namespace FusionCore.FusionNfce.Fiscal.Servicos
{
    public class ServicoSalvarEmissorNFCe
    {
        private readonly NfceEmissorFiscal _emissorFiscal;
        private readonly IRepositorioEmissorFiscalNfce _repositorioNfce;

        public ServicoSalvarEmissorNFCe(NfceEmissorFiscal emissorFiscal, IRepositorioEmissorFiscalNfce repositorioNfce)
        {
            _emissorFiscal = emissorFiscal;
            _repositorioNfce = repositorioNfce;
        }

        public void Executar()
        {
            _repositorioNfce.SalvarESincronizar(_emissorFiscal);
        }

    }
}