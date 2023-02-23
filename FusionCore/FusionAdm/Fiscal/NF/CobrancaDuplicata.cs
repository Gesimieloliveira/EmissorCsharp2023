using System;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public sealed class CobrancaDuplicata : Entidade
    {
        public int Id { get; set; }
        public Cobranca Cobranca { get; set; }
        public string Descricao { get; set; }
        public DateTime? VenceEm { get; set; }
        public decimal Valor { get; set; }
        public byte NumeroDuplicata { get; set; }
        public DateTime EmitidoEm { get; set; }
        protected override int ReferenciaUnica => Id;
    }
}