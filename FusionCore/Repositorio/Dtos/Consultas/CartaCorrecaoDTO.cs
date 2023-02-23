using System;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class CartaCorrecaoDTO : ICartaCorrecaoCteDTO
    {
        public int Id { get; set; }
        public DateTime OcorreuEm { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
        public string XmlCte { get; set; }
        public string ChaveId { get; set; }
    }
}