using System;
using System.Net;

namespace FusionCore.Core.Net
{
    public static class ProtocoloSegurancaExt
    {
        public static SecurityProtocolType ToSecurityProtocol(this ProtocoloSeguranca protocolo)
        {
            switch (protocolo)
            {
                case ProtocoloSeguranca.Ssl3: return SecurityProtocolType.Ssl3;
                case ProtocoloSeguranca.Tls1: return SecurityProtocolType.Tls;
                case ProtocoloSeguranca.Tls11: return SecurityProtocolType.Tls11;
                case ProtocoloSeguranca.Tls12: return SecurityProtocolType.Tls12;
            }

            throw new InvalidOperationException("Protocolo de segurança para conexão inválido. Ex: SSL3");
        }
    }
}