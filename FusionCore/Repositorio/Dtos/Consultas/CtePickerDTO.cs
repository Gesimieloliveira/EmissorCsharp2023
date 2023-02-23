using System;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class CtePickerDTO
    {
        public int Id { get; set; }
        public string Chave { get; set; }
        public string NomeDestinatario { get; set; }
        public string NomeEmitente { get; set; }
        public string NomeTomador { get; set; }
        public string NomeRemetente { get; set; }
        public decimal ValorServico { get; set; }
        public int StatusCancelamento { get; set; }
        public int CodigoAutorizacao { get; set; }
        public DateTime? EmitidaEm { get; set; }
        public CidadeDTO CidadeFinalOperacao { get; set; }
    }
}