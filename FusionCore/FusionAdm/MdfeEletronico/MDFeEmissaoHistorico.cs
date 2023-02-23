using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.MdfeEletronico.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeEmissaoHistorico
    {
        public int Id { get; set; }
        public MDFeEletronico MDFeEletronico { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public MDFeTipoEmissao TipoEmissao { get; set; }
        public string Chave { get; set; } = string.Empty;
        public bool Finalizada { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? EnviadoEm { get; set; }
        public string NumeroRecibo { get; set; } = string.Empty;
        public string XmlLote { get; set; }

        public void Finalizado(string xmlRetorno)
        {
            Finalizada = true;
            XmlRetorno = xmlRetorno;
        }
    }
}