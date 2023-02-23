using FusionCore.FusionNfce.Cidade;
using FusionCore.FusionNfce.Cliente;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.Fiscal
{
    public class NfceDestinatario : Entidade
    {
        public int NfceId { get; set; }
        public Nfce Nfce { get; set; }
        public ClienteNfce Cliente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string DocumentoUnico { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string InscricaoEstadual { get; set; } = string.Empty;
        public CidadeNfce Cidade { get; set; }

        public bool IsAddEndereco => GetIsAddEndereco();

        private bool GetIsAddEndereco()
        {
            return Logradouro.IsNotNullOrEmpty() || Numero.IsNotNullOrEmpty() || Bairro.IsNotNullOrEmpty() ||
                   Cep.IsNotNullOrEmpty() || Complemento.IsNotNullOrEmpty() || Cidade != null;
        }

        protected override int ReferenciaUnica => NfceId;
        
    }
}