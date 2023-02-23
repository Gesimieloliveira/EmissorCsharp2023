using FusionCore.Core.Net;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class ConfiguracaoEmailDTO : IEntidade, ISincronizavelAdm, IConfiguracaoEmail
    {
        public virtual int Id { get; set; }
        public virtual string Email { get; set; }
        public virtual string Senha { get; set; }
        public virtual int Porta { get; set; }
        public virtual bool Ssl { get; set; }
        public virtual string UrlServidorEmail { get; set; }
        public virtual bool UsarFusionZohoo { get; set; }
        public virtual string EmailResposta { get; set; }

        public virtual string Referencia => Id.ToString();

        public virtual EntidadeSincronizavel EntidadeSincronizavel { get; } =
            EntidadeSincronizavel.ConfiguracaoEmail;

        public virtual ProtocoloSeguranca ProtocoloSeguranca { get; set; } = 
            ProtocoloSeguranca.Ssl3;

        public virtual bool IsValido()
        {
            if (UsarFusionZohoo && !string.IsNullOrWhiteSpace(EmailResposta)) return true;

            return
                !string.IsNullOrWhiteSpace(Email) &&
                !string.IsNullOrWhiteSpace(Senha) &&
                !string.IsNullOrWhiteSpace(UrlServidorEmail);
        }
    }
}