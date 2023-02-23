using System;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class EventoSincronizacaoDt : IEntidade
    {
        public Int64 Id { get; set; }
        public DateTime IniciadoEm { get; set; }
        public DateTime TerminadoEm { get; set; }
        public String Tag { get; set; }
        public DateTime? AlteradoEm { get; set; }
    }
}