using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using DFe.Utils;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ConfiguracaoEmail;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using MDFe.Classes.Retorno;
using MDFe.Damdfe.Base;
using MDFe.Damdfe.Fast;
using NHibernate;
using NHibernate.Util;

// ReSharper disable InconsistentNaming

namespace FusionCore.FusionAdm.MdfeEletronico.Helpers
{
    public class MDFeImpressaoHelper
    {

        public void ImprimirDanfe(MDFeEletronico mdfe)
        {
            if (mdfe.Emissao?.Autorizado != true)
                throw new ArgumentException("MDF-e ainda não foi emitida na SEFAZ");

            if (mdfe.Emissao.XmlAutorizado == null)
                throw new ArgumentException("MDF-e não possui um XML Autorizado para o DANFE");

            var zeusMdfe = FuncoesXml.XmlStringParaClasse<MDFeProcMDFe>(mdfe.Emissao.XmlAutorizado);

            var rpt = new DamdfeFrMDFe(zeusMdfe,
                    new ConfiguracaoDamdfe
                    {
                        Logomarca = mdfe.Emitente.Empresa.LogoMarca,
                        DocumentoEncerrado = mdfe.Status == MDFeStatus.Encerrada,
                        DocumentoCancelado = mdfe.Status == MDFeStatus.Cancelada
                    });

            rpt.Visualizar();
        }

        public void EnviarLoteXml(FileInfo pacote, IEnumerable<Email> emails, string assunto, string corpoMensagem)
        {
            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {

                    var emailBuilder = new EmailBuilder(GetConfigEmail(sessao));

                    emailBuilder
                        .Assunto(assunto)
                        .AddAnexo(GetAnexo(pacote).FullName)
                        .Mensagem(corpoMensagem);

                    emails.ForEach(e => emailBuilder.AddEmail(e));

                    emailBuilder.Enviar();
                }
            }
            catch (SmtpException)
            {
                throw new InvalidOperationException("Não foi possível conectar no SMTP de email");
            }
        }

        public void EnviaEmail(MDFeEletronico mdfe, IEnumerable<Email> emails, string assunto, string mensagem)
        {
            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var xml = ObterStringXml(mdfe.Emissao);
                    var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                    var pdf = new MemoryStream();


                    var mdfeProc = FuncoesXml.XmlStringParaClasse<MDFeProcMDFe>(xml);
                    //var impressor = CriaImpressor(nfeProc, false, mdfe.Emitente.Empresa.LogoMarca);

                    var impressor = new DamdfeFrMDFe(mdfeProc,
                        new ConfiguracaoDamdfe
                        {
                            Logomarca = mdfe.Emitente.Empresa.LogoMarca,
                            DocumentoEncerrado = mdfe.Status == MDFeStatus.Encerrada,
                            DocumentoCancelado = mdfe.Status == MDFeStatus.Cancelada
                        });

                    impressor.ExportarPdf(pdf);

                    var emailBuilder = new EmailBuilder(GetConfigEmail(sessao));

                    emailBuilder.Assunto(assunto)
                        .AddAnexo(xmlStream, $"xml-{mdfe.Emissao.Chave}.xml")
                        .AddAnexo(pdf, $"pdf-{mdfe.Emissao.Chave}.pdf")
                        .Mensagem(mensagem);

                    emails.ForEach(e => emailBuilder.AddEmail(e));

                    emailBuilder.Enviar();
                }
            }
            catch (SmtpException)
            {
                throw new InvalidOperationException("Não foi possível conectar no SMTP de email");
            }
        }

        public string ObterStringXml(MDFeEmissao emissao)
        {
            if (emissao.XmlAutorizado == null)
                throw new ArgumentException("NF-e não possui um XML Autorizado para o DANFE");

            return emissao.XmlAutorizado;
        }

        private FileInfo GetAnexo(FileInfo pacoteGerado)
        {
            var anexo = Path.Combine(Path.GetTempPath(), "pacote-de-xml.zip");
            File.Delete(anexo);

            return pacoteGerado.CopyTo(anexo);
        }

        private static ConfiguracaoEmailDTO GetConfigEmail(ISession sessao)
        {
            var repositorio = new RepositorioComun<ConfiguracaoEmailDTO>(sessao);
            return repositorio.Busca(new UnicaConfiguracaoEmail());
        }
    }
}