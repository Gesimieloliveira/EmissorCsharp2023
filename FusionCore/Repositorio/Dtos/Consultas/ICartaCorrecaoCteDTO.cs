using System;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public interface ICartaCorrecaoCteDTO
    {
        int Id { get; set; }
        DateTime OcorreuEm { get; set; }
        string XmlEnvio { get; set; }
        string XmlRetorno { get; set; }
        string XmlCte { get; set; }
        string ChaveId { get; set; }
    }
}