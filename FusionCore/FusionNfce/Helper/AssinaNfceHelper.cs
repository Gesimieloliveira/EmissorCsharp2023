using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using DFe.Classes.Flags;
using DFe.Utils;
using FusionCore.FusionNfce.Extencoes;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionNfce;
using NFe.Classes;
using NFe.Utils;
using NFe.Utils.InformacoesSuplementares;
using NFe.Utils.NFe;
using Shared.NFe.Utils.InfRespTec;
using ModeloDocumento = FusionCore.FusionAdm.Fiscal.Flags.ModeloDocumento;
using Signature = DFe.Classes.Assinatura.Signature;
using TipoAmbiente = FusionCore.FusionAdm.Fiscal.Flags.TipoAmbiente;
using ZeusNfe = NFe.Classes.NFe;

namespace FusionCore.FusionNfce.Helper
{
    public class AssinaNfceHelper
    {
        private readonly X509Certificate2 _certificado;
        private readonly NfceEmissaoHistorico _emissaoHistorico;
        private readonly Nfce _nfce;

        public AssinaNfceHelper(X509Certificate2 certificado, NfceEmissaoHistorico emissaoHistorico, Nfce nfce)
        {
            _certificado = certificado;
            _emissaoHistorico = emissaoHistorico;
            _nfce = nfce;
        }

        public string GeraXmlAssinado()
        {
            var chave = _emissaoHistorico.ChaveTexto.Valor;

            if (chave?.Length != 44)
                throw new ArgumentException("Emissão deve possuir uma chave válida");

            var chaveTag = "NFe" + chave; 

            if (chaveTag.Length != 47)
                throw new ArgumentException("Emissão deve possuir uma TagId válida");

            var reference = new Reference { Uri = $"#{chaveTag}" };
            var nfezeus = _nfce.ToZeus(_emissaoHistorico, SessaoSistemaNfce.Configuracao.EmissorFiscal.AutorizadoBaixarXml);

            PreencherHashCsrt(nfezeus, _nfce.Emitente.Empresa.Estado.Id);

            AlteraNomeDestinatarioDoXmlSeHomologacao(nfezeus, _emissaoHistorico.AmbienteSefaz, _emissaoHistorico.Chave.ModeloDocumento);
            AlteraNomeDoPrimeiroItemDoXmlSeHomologacao(nfezeus, _emissaoHistorico.AmbienteSefaz);

            var xmlString = FuncoesXml.ClasseParaXmlString(nfezeus).RemoverAcentos();
            var documento = new XmlDocument { PreserveWhitespace = true };

            documento.LoadXml(xmlString);

            var docXml = new SignedXml(documento) { SigningKey = _certificado.PrivateKey };

            // adicionando EnvelopedSignatureTransform a referencia
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigC14NTransform());

            docXml.AddReference(reference);

            // carrega o certificado em KeyInfoX509Data para adicionar a KeyInfo
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(_certificado));

            docXml.KeyInfo = keyInfo;
            docXml.ComputeSignature();

            //// recuperando a representacao do XML assinado
            var xmlDigitalSignature = docXml.GetXml();
            var assinatura = FuncoesXml.XmlStringParaClasse<Signature>(xmlDigitalSignature.OuterXml);
            nfezeus.Signature = assinatura;


            var emissorNfce = SessaoSistemaNfce.Configuracao.EmissorFiscal.EmissorFiscalNfce;

            var urlQrCode = nfezeus.infNFeSupl.ObterUrlQrCode(
                nfezeus,
                VersaoQrCode.QrCodeVersao2,
                emissorNfce.IdToken.ToString("D6"),
                emissorNfce.Csc);

            var urChave = nfezeus.infNFeSupl.ObterUrl(
                nfezeus.infNFe.ide.tpAmb,
                nfezeus.infNFe.ide.cUF,
                TipoUrlConsultaPublica.UrlConsulta,
                VersaoServico.Versao400,
                VersaoQrCode.QrCodeVersao2);

            nfezeus.infNFeSupl = new infNFeSupl
            {
                qrCode = urlQrCode,
                urlChave = urChave
            };

            return nfezeus.ObterXmlString();
        }

        private void PreencherHashCsrt(ZeusNfe nfezeus, byte ufId)
        {
            if (ExisteCsrt(ufId) == false) return;
            if (nfezeus.infNFe.infRespTec == null) return;

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var responsavelTecnico =
                    new RepositorioResponsavelTecnicoNfce(sessao).BuscarPorUf(_nfce.Emitente.Empresa.Estado.Id);
                nfezeus.infNFe.infRespTec.hashCSRT =
                    GerarHashCSRT.HashCSRT(responsavelTecnico.Csrt, _emissaoHistorico.ChaveTexto.Valor);
                nfezeus.infNFe.infRespTec.idCSRT = responsavelTecnico.CsrtId;
            }
        }

        private static bool ExisteCsrt(byte ufId)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var responsavelTecnicoRepositorio = new RepositorioResponsavelTecnicoNfce(sessao);

                return responsavelTecnicoRepositorio.ExisteCsrt(ufId);
            }
        }

        private void AlteraNomeDoPrimeiroItemDoXmlSeHomologacao(ZeusNfe nfezeus, TipoAmbiente tipoAmbiente)
        {
            if (tipoAmbiente == TipoAmbiente.Producao)
                return;

            nfezeus.infNFe.det[0].prod.xProd = "NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
        }

        private static void AlteraNomeDestinatarioDoXmlSeHomologacao(ZeusNfe nfezeus, TipoAmbiente tipoAmbiente, ModeloDocumento modelo)
        {
            if (tipoAmbiente == TipoAmbiente.Producao)
                return;

            if (modelo == ModeloDocumento.NFCe)
            {
                if (nfezeus.infNFe.dest == null)
                    return;
            }
        

            nfezeus.infNFe.dest.xNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
        }
    }
}