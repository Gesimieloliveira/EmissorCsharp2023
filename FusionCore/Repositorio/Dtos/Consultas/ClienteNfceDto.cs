using FusionCore.FusionNfce.Cidade;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class ClienteNfceDto
    {
        public string Nome { get; set; } = string.Empty;
        public string DocumentoUnico { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int IdRemoto { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string InscricaoEstadual { get; set; } = string.Empty;
        public CidadeNfce Cidade { get; set; }
    }
}