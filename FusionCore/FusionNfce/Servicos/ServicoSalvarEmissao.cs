using FusionCore.FusionNfce.Fiscal;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;

namespace FusionCore.FusionNfce.Servicos
{
    public class ServicoSalvarEmissao
    {
        private readonly IRepositorioNfce _repositorioNfce;
        private readonly NfceEmissao _emissao;

        public ServicoSalvarEmissao(IRepositorioNfce repositorioNfce, NfceEmissao emissao)
        {
            _repositorioNfce = repositorioNfce;
            _emissao = emissao;
        }

        public NfceEmissao Executar()
        {
            _repositorioNfce.SalvarEmissao(_emissao);

            return _emissao;
        }
    }
}