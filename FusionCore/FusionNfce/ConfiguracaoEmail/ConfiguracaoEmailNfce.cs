using FusionCore.Core.Net;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.ConfiguracaoEmail
{
    public class ConfiguracaoEmailNfce : IConfiguracaoEmail
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int Porta { get; set; }
        public bool Ssl { get; set; }
        public string UrlServidorEmail { get; set; }
        public virtual bool UsarFusionZohoo { get; set; }
        public ProtocoloSeguranca ProtocoloSeguranca { get; set; }
        public string EmailResposta { get; set; }
    }
}