using System;
using System.Net;
using DFe.Utils;
using FusionCore.Core.Net;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionNfce.Sessao.Sistema
{
    public class DadosSefazNfce : IDadosServicoSefaz
    {
        public DadosSefazNfce(ModeloDocumento modelo, TipoAmbiente ambiente, byte ibgeEstadoEmissao, ConfiguracaoCertificado certificado, ProtocoloSeguranca protocoloSeguranca)
        {
            Modelo = modelo;
            Ambiente = ambiente;
            IbgeEstadoEmissao = ibgeEstadoEmissao;
            Certificado = certificado;
            ProtocoloSeguranca = protocoloSeguranca;
        }

        public ModeloDocumento Modelo { get; }
        public TipoAmbiente Ambiente { get; }
        public byte IbgeEstadoEmissao { get; }
        public ConfiguracaoCertificado Certificado { get; }
        public ProtocoloSeguranca ProtocoloSeguranca { get; }

        public SecurityProtocolType GetProtocoloSeguranca()
        {
            switch (ProtocoloSeguranca)
            {
                case ProtocoloSeguranca.Ssl3:
                    return SecurityProtocolType.Ssl3;
                case ProtocoloSeguranca.Tls1:
                    return SecurityProtocolType.Tls;
                case ProtocoloSeguranca.Tls11:
                    return SecurityProtocolType.Tls11;
                case ProtocoloSeguranca.Tls12:
                    return SecurityProtocolType.Tls12;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}