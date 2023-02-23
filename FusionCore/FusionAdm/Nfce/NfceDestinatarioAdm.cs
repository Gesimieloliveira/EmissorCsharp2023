using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Nfce
{
    public class NfceDestinatarioAdm
    {
        public int NfceId { get; set; }
        public NfceAdm Nfce { get; set; }
        public Cliente Cliente { get; set; }
        public string Nome { get; set; }
        public string DocumentoUnico { get; set; }
        public string Email { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public string InscricaoEstadual { get; set; }
        public CidadeDTO Cidade { get; set; }
    }
}