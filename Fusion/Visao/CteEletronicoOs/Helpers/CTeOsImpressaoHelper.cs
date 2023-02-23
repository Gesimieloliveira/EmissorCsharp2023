using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using DFe.DocumentosEletronicos.ManipuladorDeXml;
using FusionCore.FusionAdm.Acbr;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ConfiguracaoEmail;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Criptografia;
using NHibernate;
using NHibernate.Util;

namespace Fusion.Visao.CteEletronicoOs.Helpers
{
    public class CTeOsImpressaoHelper
    {
        public CTeOsImpressaoHelper()
        {
            new ManipulaPasta(@"C:\SistemaFusion\FusionAcbr\PDF").CriaPastaSeNaoExistir();
        }

        public string GeraArquivoXml(CteOsEmissaoFinalizada emissao)
        {
            if (emissao?.Autorizado != true)
                throw new ArgumentException("CT-e ainda não foi emitida na SEFAZ");

            if (emissao.XmlAutorizado == null)
                throw new ArgumentException("CT-e não possui um XML Autorizado para o DANFE");

            var unique = Md5Helper.ComputaUnique();
            var uniqueXml = Path.Combine(DiretorioAssembly.GetPastaTemp(), $"XML-{unique}-cte.xml");

            using (var sw = new StreamWriter(uniqueXml))
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + emissao.XmlAutorizado);

            return uniqueXml;
        }

        public string GeraArquivoXml(string xml)
        {
            var unique = Md5Helper.ComputaUnique();
            var uniqueXml = Path.Combine(DiretorioAssembly.GetPastaTemp(), $"XML-{unique}-cte.xml");

            using (var sw = new StreamWriter(uniqueXml))
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + xml);

            return uniqueXml;
        }

        public string GeraDanfe(CteOs cte)
        {
            var uniqueHash = Md5Helper.ComputaUnique();

            var xmlFile = GeraArquivoXml(cte.Emissao); // gerar arquivo XML DO CT-E em disco e retornar o caminho


            var xmlProntoParaImpressao = XMLProntoParaImpressao(xmlFile);
            var danfeFile = GeraArquivoDanfe(cte.Emissao, xmlProntoParaImpressao, uniqueHash); // Aqui ele retorna o caminho do pdf 

            return danfeFile;
        }

        private string XMLProntoParaImpressao(string xmlFile)
        {
            var xmlcteOSProcString = FuncoesXml.ObterNodeDeArquivoXml("cteOSProc", xmlFile);
            var xmlProtCTeString = FuncoesXml.ObterNodeDeArquivoXml("protCTe", xmlFile);

            var xmlCteOSProc = new XmlDocument();
            xmlCteOSProc.LoadXml(xmlcteOSProcString);

            var fragmentoProtCte = xmlCteOSProc.CreateDocumentFragment();
            fragmentoProtCte.InnerXml = xmlProtCTeString;

            xmlCteOSProc.GetElementsByTagName("CTeOS")[0].AppendChild(fragmentoProtCte);

            var xml = xmlCteOSProc.OuterXml;

            var xmlProntoParaImpressao = GeraArquivoXml(xml);
            return xmlProntoParaImpressao;
        }

        private string GeraArquivoDanfe(CteOsEmissaoFinalizada emissao, string arquivoXml, string nomeTemp)
        {
            try
            {
                var arquivoDanfe = Path.Combine(DiretorioAssembly.GetPastaTemp(), $"DANFE-{nomeTemp}-cte.pdf");

                using (var acbr = new AcbrMonitorPlus())
                {
                    acbr.EnviarComando(AcbrMonitorPlusComando.CTE_ImprimirDACTePDF, arquivoXml);
                    var danfe = Path.Combine(@"C:\SistemaFusion\FusionAcbr\PDF", $"{emissao.Chave}-cte.pdf"); // aqui peço para salvar em C:\sistemafusion fica a seu criterio
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

        public void EnviaEmail(CteOs cte, IEnumerable<Email> emails, string assunto, string corpoMsg)
        {
            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var nomeTemp = Md5Helper.ComputaUnique();

                    var xml = GeraArquivoXml(cte.Emissao);

                    var xmlProntoParaImpressao = XMLProntoParaImpressao(xml);

                    var danfe = GeraArquivoDanfe(cte.Emissao, xmlProntoParaImpressao, nomeTemp);

                    var emailBuilder = new EmailBuilder(GetConfigEmail(sessao));

                    emailBuilder
                        .Assunto(assunto)
                        .AddAnexo(xml)
                        .AddAnexo(danfe)
                        .Mensagem(corpoMsg);

                    emails.ForEach(e => emailBuilder.AddEmail(e));

                    emailBuilder.Enviar();
                }
            }
            catch (SmtpException)
            {
                throw new InvalidOperationException("Não foi possível conectar no SMTP de email");
            }
        }

        public string GeraCCe(CteOs cte, string xmlCCe, string chaveId)
        {
            var uniqueHash = Md5Helper.ComputaUnique();

            var xmlFileNFe = GeraArquivoXml(cte.Emissao);
            var xmlFileCCe = GeraArquivoCCeXml(xmlCCe);

            var danfeFile = GeraArquivoCartaCorrecao(xmlFileNFe, xmlFileCCe, uniqueHash, chaveId);

            return danfeFile;
        }

        private string GeraArquivoCartaCorrecao(string xmlFileNFe, string xmlFileCCe, string nomeTemp, string chave)
        {
            try
            {
                var arquivoDanfe = Path.Combine(DiretorioAssembly.GetPastaTemp(), $"DANFE-{nomeTemp}-cce.pdf");

                using (var acbr = new AcbrMonitorPlus())
                {
                    acbr.EnviarComando(AcbrMonitorPlusComando.CTE_ImprimirEventoPDF, xmlFileCCe, xmlFileNFe);
                    var danfe = Path.Combine(@"C:\SistemaFusion\FusionAcbr\PDF", $"{chave}-procEventoCTe.pdf");
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

                throw new ArgumentException(@"Carta Correção não localizado em C:\SistemaFusion\FusionAcbr\PDF");
            }
            catch (SocketException)
            {
                throw new ArgumentException("Verifique se o ACBrMonitor está ativo e configurado");
            }
        }

        public string GeraArquivoCCeXml(string xmlCCe)
        {
            var unique = Md5Helper.ComputaUnique();
            var uniqueXml = Path.Combine(DiretorioAssembly.GetPastaTemp(), $"XML-{unique}-cce.xml");

            using (var sw = new StreamWriter(uniqueXml))
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + xmlCCe);

            return uniqueXml;
        }

        private static ConfiguracaoEmailDTO GetConfigEmail(ISession sessao)
        {
            var repositorio = new RepositorioComun<ConfiguracaoEmailDTO>(sessao);
            return repositorio.Busca(new UnicaConfiguracaoEmail());
        }
    }
}