using FusionCore.Core.Net;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public interface IConfiguracaoEmail
    {
        string Email { get; set; }
        int Id { get; set; }
        int Porta { get; set; }
        string Senha { get; set; }
        bool Ssl { get; set; }
        string UrlServidorEmail { get; set; }
        bool UsarFusionZohoo { get; set; }
        ProtocoloSeguranca ProtocoloSeguranca { get; set; }
        string EmailResposta { get; set; }
    }
}