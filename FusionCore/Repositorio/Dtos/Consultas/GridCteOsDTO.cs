using FusionCore.FusionAdm.CteEletronicoOs.Flags;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class GridCteOsDTO
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public int NumeroDocumento { get; set; }
        public short SerieDocumento { get; set; }
        public string TomadorNome { get; set; }
        public string EmitenteNome { get; set; }
        public decimal ValorServico { get; set; }
        public decimal ValorReceber { get; set; }
        public string DescricaoServico { get; set; }
        public string Chave { get; set; }
    }
}