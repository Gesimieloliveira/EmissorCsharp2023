using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading;
using FusionCore.Excecoes;
using FusionCore.FusionAdm.Acbr;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Basico;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Criptografia;
using NHibernate;
using NHibernate.Util;
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.Sat;

namespace FusionNfce.AutorizacaoSatFiscal.Helper
{
    public static class DanfeSatHelper
    {
        private static string GeraArquivoXml(XmlAutorizadoDto xmlAutorizado, string nomeFantasiaCustomisado)
        {
            if (xmlAutorizado == null || xmlAutorizado.IsNull())
                throw new ArgumentException("CF-e não possui um XML Autorizado para o DANFE");


            if (nomeFantasiaCustomisado.IsNotNullOrEmpty())
            {
                var xmlSerializado = CFe.Load(xmlAutorizado.Xml);
                xmlSerializado.InfCFe.Emit.XFant = nomeFantasiaCustomisado;
                xmlSerializado.InfCFe.Emit.XNome = nomeFantasiaCustomisado;

                xmlAutorizado.Xml = xmlSerializado.GetXml();
            }

            var unique = Md5Helper.ComputaUnique();
            var uniqueXml = Path.Combine(DiretorioAssembly.GetPastaTemp(), $"XML-{unique}-cfe.xml");

            using (var sw = new StreamWriter(uniqueXml))
            {
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + xmlAutorizado.Xml);
            }

            return uniqueXml;
        }

        public static string GeraArquivoXmlCancelado(XmlCancelamentoDto xmlCancelamento, string nomeFantasiaCustomizado)
        {
            if (xmlCancelamento == null || xmlCancelamento.IsNull())
                throw new ArgumentException("CF-e não possui um XML Autorizado para o DANFE");


            if (nomeFantasiaCustomizado.IsNotNullOrEmpty())
            {
                var xmlSerializado = CFeCanc.Load(xmlCancelamento.Xml);
                xmlSerializado.InfCFe.Emit.XFant = nomeFantasiaCustomizado;
                xmlSerializado.InfCFe.Emit.XNome = nomeFantasiaCustomizado;

                xmlCancelamento.Xml = xmlSerializado.GetXml();
            }

            var unique = Md5Helper.ComputaUnique();
            var uniqueXml = Path.Combine(DiretorioAssembly.GetPastaTemp(), $"XML-{unique}-cfe.xml");

            using (var sw = new StreamWriter(uniqueXml))
            {
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + xmlCancelamento.Xml);
            }

            return uniqueXml;
        }

        public static void Imprimir(XmlAutorizadoDto xmlAutorizado, string impressora, string nomeFantasiaCustomizado)
        {
            try
            {
                var xmlFile = GeraArquivoXml(xmlAutorizado, nomeFantasiaCustomizado);
                ImprimirComImpressora(xmlFile, impressora);
            }
            catch (Exception e)
            {
                throw new ImpressaoException(e.Message, e);
            }
        }

        public static void ImprimirCancelamento(XmlAutorizadoDto xmlAutorizado,
            XmlCancelamentoDto xmlCancelamento,
            string impressora, string nomeFantasiaCustomizado)
        {
            var arquivoXmlAutorizado = GeraArquivoXml(xmlAutorizado, nomeFantasiaCustomizado);

            var arquivoXmlCancelado = GeraArquivoXmlCancelado(xmlCancelamento, nomeFantasiaCustomizado);

            ImprimirCancelamentoComImpressora(arquivoXmlAutorizado, arquivoXmlCancelado, impressora);
        }

        private static void ImprimirCancelamentoComImpressora(string arquivoXmlAutorizado, string arquivoXmlCancelado, string impressora)
        {
            try
            {
                using (var acbr = new AcbrMonitorPlus())
                {
                    acbr.EnviarComando(AcbrMonitorPlusComando.SAT_ImprimirExtratoCancelamento, arquivoXmlAutorizado, arquivoXmlCancelado, impressora);
                }
            }
            catch (SocketException)
            {
                throw new ArgumentException("Verifique se o ACBrMonitor está ativo e configurado");
            }
        }

        private static void ImprimirComImpressora(string arquivoXml, string impressora)
        {
            try
            {
                using (var acbr = new AcbrMonitorPlus())
                {
                    acbr.EnviarComando(AcbrMonitorPlusComando.SAT_ImprimirExtratoVenda, arquivoXml, impressora);
                }
            }
            catch (SocketException)
            {
                throw new ArgumentException("Verifique se o ACBrMonitor está ativo e configurado");
            }
        }

        private static string GeraArquivoDanfe(string chaveNfe, string arquivoXml)
        {
            var uniqueHash = Md5Helper.ComputaUnique().ToUpper();

            try
            {
                var arquivoDanfe = Path.Combine(DiretorioAssembly.GetPastaTemp(), $"DANFE-{uniqueHash}-cfe.pdf");

                using (var acbr = new AcbrMonitorPlus())
                {
                    var danfe = Path.Combine(@"C:\SistemaFusion\FusionAcbr\PDF", $"{chaveNfe}-cfe.pdf");

                    acbr.EnviarComando(AcbrMonitorPlusComando.SAT_gerarpdfextratovenda, arquivoXml, danfe);
                    var tentativa = 10;

                    while (tentativa-- >= 0)
                    {
                        try
                        {
                            if (File.Exists(danfe))
                            {
                                File.Move(danfe, arquivoDanfe);
                                return arquivoDanfe;
                            }
                        }
                        catch (IOException)
                        {
                            //ignora
                        }

                        Thread.Sleep(500);
                    }
                }

                throw new ArgumentException(@"DANFE não localizado em C:\SistemaFusion\FusionAcbr\PDF");
            }
            catch (SocketException)
            {
                throw new ArgumentException("Verifique se o ACBrMonitor está ativo e configurado");
            }
        }

        public static void EnviaEmail(Nfce nfe, IEnumerable<Email> emails, string assunto, string coproMensagem, string nomeFantasiaCustomizado)
        {
            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var xmlDto = new XmlAutorizadoDto
                    {
                        Xml = nfe.FinalizaEmissaoSat.XmlRetorno
                    };

                    var xml = GeraArquivoXml(xmlDto, nomeFantasiaCustomizado);
                    var danfe = GeraArquivoDanfe(nfe.FinalizaEmissaoSat.Chave, xml);

                    var emailBuilder = new EmailBuilder(GetConfigEmail(sessao));

                    emailBuilder
                        .Assunto(assunto)
                        .AddAnexo(xml)
                        .AddAnexo(danfe)
                        .Mensagem(coproMensagem);

                    emails.ForEach(e => emailBuilder.AddDestinatario(e.Valor));

                    emailBuilder.Enviar();
                }
            }
            catch (SmtpException)
            {
                throw new InvalidOperationException("Não foi possível conectar no SMTP de email");
            }
        }

        private static IConfiguracaoEmail GetConfigEmail(ISession sessao)
        {
            var repositorio = new RepositorioConfiguracaoEmailNfce(sessao);

            return repositorio.BuscarUnicaConfiguracao();
        }
    }
}