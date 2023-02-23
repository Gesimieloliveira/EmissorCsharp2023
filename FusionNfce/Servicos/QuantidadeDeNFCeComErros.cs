using FusionCore.Repositorio.Contratos.FusionNfceContratos;

namespace FusionNfce.Servicos
{
    public class QuantidadeDeNFCeComErros
    {
        private readonly IRepositorioNfce _repositorioNfce;

        public QuantidadeDeNFCeComErros(IRepositorioNfce repositorioNfce)
        {
            _repositorioNfce = repositorioNfce;
        }

        public int ObterQuantidade()
        {
            return _repositorioNfce.QuantidadeDeNFCeOffiline();
        }
    }
}