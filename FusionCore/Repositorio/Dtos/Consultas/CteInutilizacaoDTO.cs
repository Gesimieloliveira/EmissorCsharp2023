using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class CteInutilizacaoDTO
    {
        public int Id { get; set; }
        public long NumeroInicial { get; set; }
        public long NumeroFinal { get; set; }
        public string Justificativa { get; set; }
        public ModeloDocumento Documento { get; set; }
    }
}