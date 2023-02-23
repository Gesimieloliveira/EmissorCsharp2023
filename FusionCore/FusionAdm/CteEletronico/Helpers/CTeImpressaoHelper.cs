using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading;
using CTe.Classes;
using CTe.Classes.Servicos.Consulta;
using CTe.Classes.Servicos.Evento;
using CTe.Classes.Servicos.Tipos;
using CTe.Dacte.Base;
using CTe.Dacte.Fast;
using DFe.Utils;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ConfiguracaoEmail;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Criptografia;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.FusionAdm.CteEletronico.Helpers
{
    public class CTeImpressaoHelper
    {
        public string GeraArquivoXml(CteEmissao emissao)
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

        private string XmlString(CteEmissao emissao)
        {
            if (emissao?.Autorizado != true)
                throw new ArgumentException("CT-e ainda não foi emitida na SEFAZ");

            if (emissao.XmlAutorizado == null)
                throw new ArgumentException("CT-e não possui um XML Autorizado para o DANFE");

            return emissao.XmlAutorizado;
        }

        public void GeraCCe(Cte cte, string xmlEvento, string xmlRetornoEvento)
        {
            var xmlFileNFe = XmlString(cte.CteEmissao);

            GeraArquivoCartaCorrecao(xmlFileNFe, xmlEvento, xmlRetornoEvento);
        }

        private void GeraArquivoCartaCorrecao(string xmlCTe, string xmlEvento, string xmlRetornoEvento)
        {
            var evento = FuncoesXml.XmlStringParaClasse<eventoCTe>(xmlEvento);

            var procEventoCte = new procEventoCTe
            {
                versao = evento.versao == versao.ve200 ? "2.00" : "3.00",
                eventoCTe = evento,
                retEvento = FuncoesXml.XmlStringParaClasse<retEventoCTe>(xmlRetornoEvento)
            };

            var rpt = new DacteFrEvento(
                FuncoesXml.XmlStringParaClasse<cteProc>(xmlCTe),
                procEventoCte);

            rpt.Visualizar();
        }

        public void GeraDanfe(Cte cte)
        {
            var xml = XmlString(cte.CteEmissao);

            var rpt = CriaDacteFrCte(xml, cte.EmissorFiscal.Empresa.LogoMarca);
            rpt.Visualizar();
        }

        private static DacteFrCte CriaDacteFrCte(string xml, byte[] logoMarca)
        {
            var configuracaoDanfeCte = CriaConfiguracaoDanfeCte(logoMarca);

            var rpt = new DacteFrCte(FuncoesXml.XmlStringParaClasse<cteProc>(xml), configuracaoDanfeCte);
            return rpt;
        }

        private static ConfiguracaoDacte CriaConfiguracaoDanfeCte(byte[] logoMarca)
        {
            var configuracaoDanfeCte = new ConfiguracaoDacte
            {
                QuebrarLinhasObservacao = true,
                DocumentoCancelado = false,
                Logomarca = logoMarca,
                Desenvolvedor = string.Empty
            };
            return configuracaoDanfeCte;
        }

        private string GeraArquivoDanfe(CteEmissao emissao, string nomeTemp)
        {
            var arquivoDanfe = Path.Combine(DiretorioAssembly.GetPastaTemp(), $"DANFE-{nomeTemp}-cte.pdf");
            var xml = XmlString(emissao);

            var rpt = CriaDacteFrCte(xml, emissao.Cte.EmissorFiscal.Empresa.LogoMarca);
            rpt.ExportarPdf(arquivoDanfe);
            Thread.Sleep(500);

            return arquivoDanfe;
        }

        public void EnviarLoteXmlAutorizado(FileInfo pacote, IEnumerable<Email> emails, string assunto, string corpoMsg)
        {
            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {

                    var emailBuilder = new EmailBuilder(GetConfigEmail(sessao));

                    emailBuilder
                        .Assunto(assunto)
                        .AddAnexo(GetAnexo(pacote).FullName)
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

        private FileInfo GetAnexo(FileInfo pacoteGerado)
        {
            var anexo = Path.Combine(Path.GetTempPath(), "pacote-de-xml.zip");
            File.Delete(anexo);

            return pacoteGerado.CopyTo(anexo);
        }

        public void EnviaEmail(Cte cte, IEnumerable<Email> emails, string assunto, string corpoMsg)
        {
            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var nomeTemp = Md5Helper.ComputaUnique();

                    var xml = GeraArquivoXml(cte.CteEmissao);
                    var danfe = GeraArquivoDanfe(cte.CteEmissao, nomeTemp);

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

        private static ConfiguracaoEmailDTO GetConfigEmail(ISession sessao)
        {
            var repositorio = new RepositorioComun<ConfiguracaoEmailDTO>(sessao);
            return repositorio.Busca(new UnicaConfiguracaoEmail());
        }
    }
}