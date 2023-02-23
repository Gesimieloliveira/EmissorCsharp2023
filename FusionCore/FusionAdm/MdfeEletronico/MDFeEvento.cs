using System;
using MDFe.Classes.Informacoes.Evento.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeEvento
    {
        public int Id { get; set; }
        public MDFeEletronico Mdfe { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
        public MDFeTipoEvento Evento { get; set; }
        public DateTime FeitoEm { get; set; } 
    }
}