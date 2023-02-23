using System;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class EventoEletronicoResumido
    {
        public int Id { get; set; }
        public string DocumentoUnicoEmitente { get; set; }
        public string Chave { get; set; }
        public DateTime EventoEm { get; set; }
        public TipoEvento TipoEvento { get; set; }
        public short NumeroSequencialEvento { get; set; }
        public string Descricao { get; set; }
        public DateTime AutorizadoEm { get; set; }
        public long NumeroProtocolo { get; set; }
    }
}