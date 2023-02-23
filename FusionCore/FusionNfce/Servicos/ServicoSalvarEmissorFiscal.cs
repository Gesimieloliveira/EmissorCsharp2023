using FusionCore.FusionNfce.EmissorFiscal;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;

namespace FusionCore.FusionNfce.Servicos
{
    public class ServicoSalvarEmissorFiscal
    {
        private readonly IRepositorioEmissorFiscalNfce _repositorioNfce;
        private readonly NfceEmissorFiscal _emissorFiscal;

        public ServicoSalvarEmissorFiscal(IRepositorioEmissorFiscalNfce repositorioNfce, NfceEmissorFiscal emissorFiscal)
        {
            _repositorioNfce = repositorioNfce;
            _emissorFiscal = emissorFiscal;
        }

        public void Executar()
        {
            _repositorioNfce.SalvarESincronizar(_emissorFiscal);
        }
    }
}