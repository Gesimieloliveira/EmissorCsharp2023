using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FusionCore.DFe.WsdlCte.Homologacao.Helper;
using FusionCore.DFe.XmlCte;

namespace FusionCore.DFe.RegrasNegocios
{
    public class ResolveQrCode
    {
        public static InfCTeSupl QrCode(FusionCTe fusionCTe, X509Certificate2 certificadoDigital,
            Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            var chave = fusionCTe.InformacoesCTe.Id.Substring(3, 44);

            var qrCode = new StringBuilder(UrlHelper.ObterUrl(ConverteSiglaParaEnum(fusionCTe.InformacoesCTe.Emitente.Endereco.SiglaUf),
                fusionCTe.InformacoesCTe.Identificacao.Ambiente, fusionCTe.InformacoesCTe.Identificacao.FusionTipoEmissaoCTe).QrCode);
            qrCode.Append("?");
            qrCode.Append("chCTe=").Append(chave);
            qrCode.Append("&");
            qrCode.Append("tpAmb=").Append((int)fusionCTe.InformacoesCTe.Identificacao.Ambiente);

            if (fusionCTe.InformacoesCTe.Identificacao.FusionTipoEmissaoCTe != FusionTipoEmissaoCTe.Normal)
            {
                var assinatura = Convert.ToBase64String(CreateSignaturePkcs1(certificadoDigital, encoding.GetBytes(chave)));
                qrCode.Append("&sign=");
                qrCode.Append(assinatura);
            }

            return new InfCTeSupl
            {
                QrCodCTe = qrCode.ToString()
            };
        }

        private static byte[] CreateSignaturePkcs1(X509Certificate2 certificado, byte[] Value)

        {
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certificado.PrivateKey;

            RSAPKCS1SignatureFormatter rsaF = new RSAPKCS1SignatureFormatter(rsa);

            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] hash = null;

            hash = sha1.ComputeHash(Value);

            rsaF.SetHashAlgorithm("SHA1");

            return rsaF.CreateSignature(hash);

        }

        public static FusionEstadoUFCTe ConverteSiglaParaEnum(string siglaUf)
        {
            switch (siglaUf)
            {
                case "AC":
                    return FusionEstadoUFCTe.AC;
                case "AL":
                    return FusionEstadoUFCTe.AL;
                case "AP":
                    return FusionEstadoUFCTe.AP;
                case "AM":
                    return FusionEstadoUFCTe.AM;
                case "BA":
                    return FusionEstadoUFCTe.BA;
                case "CE":
                    return FusionEstadoUFCTe.CE;
                case "DF":
                    return FusionEstadoUFCTe.DF;
                case "ES":
                    return FusionEstadoUFCTe.ES;
                case "GO":
                    return FusionEstadoUFCTe.GO;
                case "MA":
                    return FusionEstadoUFCTe.MA;
                case "MT":
                    return FusionEstadoUFCTe.MT;
                case "MS":
                    return FusionEstadoUFCTe.MS;
                case "MG":
                    return FusionEstadoUFCTe.MG;
                case "PA":
                    return FusionEstadoUFCTe.PA;
                case "PB":
                    return FusionEstadoUFCTe.PB;
                case "PR":
                    return FusionEstadoUFCTe.PR;
                case "PE":
                    return FusionEstadoUFCTe.PE;
                case "PI":
                    return FusionEstadoUFCTe.PI;
                case "RJ":
                    return FusionEstadoUFCTe.RJ;
                case "RN":
                    return FusionEstadoUFCTe.RN;
                case "RS":
                    return FusionEstadoUFCTe.RS;
                case "RO":
                    return FusionEstadoUFCTe.RO;
                case "RR":
                    return FusionEstadoUFCTe.RR;
                case "SC":
                    return FusionEstadoUFCTe.SC;
                case "SP":
                    return FusionEstadoUFCTe.SP;
                case "SE":
                    return FusionEstadoUFCTe.SE;
                case "TO":
                    return FusionEstadoUFCTe.TO;
            }

            throw new ArgumentException("Estado UF inválido");
        }
    }
}