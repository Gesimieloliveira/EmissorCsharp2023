using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using DFe.DocumentosEletronicos.ManipuladorDeXml;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Autorizacao;
using FusionCore.FusionAdm.Fiscal.NF.CCe;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ConfiguracaoEmail;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using NFe.Classes;
using NFe.Danfe.Base.NFe;
using NFe.Danfe.Fast.NFe;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Fiscal.Helpers
{
    public static class DanfeNfeHelper
    {
        public static string ObterStringXml(EmissaoFinalizadaNfe emissao)
        {
            if (emissao.XmlAutorizado == null)
                throw new ArgumentException("NF-e não possui um XML Autorizado para o DANFE");

            return emissao.XmlAutorizado.Replace("[", "{").Replace("]", "}");
        }

        public static void GeraDanfe(Nfeletronica nfe)
        {
            var xmlFile = ObterStringXml(nfe.Finalizacao);
            GeraArquivoDanfe(xmlFile, nfe.Cancelamento != null, nfe.Emitente.Empresa.LogoMarca);
        }

        public static void GeraPreVisualizacaoDanfe(Nfeletronica nfe, ISessaoManager sessionManager)
        {
            var chave = ChaveSefazHelper.GerarChave(new ComponentesChaveNfe(nfe, TipoEmissao.Normal));
            var dummy = new EmissaoDanfeDummy(chave);
            var zeusNfe = nfe.ToZeus(dummy, sessionManager);

            var danfe = CriaImpressor(zeusNfe, nfe.Emitente.Empresa.LogoMarca);

            danfe.Visualizar();
        }

        public static void GeraArquivoDanfe(string arquivoXml, bool cancelada, byte[] logoMarca)
        {
            var nfe = FuncoesXml.XmlStringParaClasse<nfeProc>(arquivoXml.Replace("[", "{").Replace("]", "}"));
            var danfe = CriaImpressor(nfe, cancelada, logoMarca);

            danfe.Visualizar();
        }

        private static DanfeFrNfe CriaImpressor(nfeProc nfe, bool cancelada, byte[] logo)
        {
            return new DanfeFrNfe(nfe, new ConfiguracaoDanfeNfe(logo, true, cancelada, true)
            {
                DecimaisQuantidadeItem = 4,
                DecimaisValorUnitario = 4
            });
        }

        private static DanfeFrNfe CriaImpressor(NFe.Classes.NFe nfe, byte[] logo)
        {
            return new DanfeFrNfe(nfe, new ConfiguracaoDanfeNfe(logo, true, false, true)
            {
                DecimaisQuantidadeItem = 4,
                DecimaisValorUnitario = 4
            }, string.Empty);
        }

        public static void EnviaEmail(Nfeletronica nfe, IEnumerable<Email> emails, string assunto, string mensagem)
        {
            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var xml = ObterStringXml(nfe.Finalizacao);
                    var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                    var pdf = new MemoryStream();


                    var nfeProc = FuncoesXml.XmlStringParaClasse<NFe.Classes.nfeProc>(xml);
                    var impressor = CriaImpressor(nfeProc, false, nfe.Emitente.Empresa.LogoMarca);

                    impressor.ExportarPdf(pdf);

                    var emailBuilder = new EmailBuilder(GetConfigEmail(sessao));

                    emailBuilder.Assunto(assunto)
                        .AddAnexo(xmlStream, $"xml-{nfe.NumeroChave}.xml")
                        .AddAnexo(pdf, $"pdf-{nfe.NumeroChave}.pdf")
                        .Mensagem(mensagem);

                    EnviaCCe(nfe, sessao, emailBuilder);

                    emails.ForEach(e => emailBuilder.AddEmail(e));

                    emailBuilder.Enviar();
                }
            }
            catch (SmtpException)
            {
                throw new InvalidOperationException("Não foi possível conectar no SMTP de email");
            }
        }

        private static void EnviaCCe(Nfeletronica nfe, ISession sessao, EmailBuilder emailBuilder)
        {
            var repositorioCCe = new RepositorioCCe(sessao);
            var cartasCorrecoes = repositorioCCe.BuscaPelaNfe(nfe).OrderBy(x => x.SequenciaEvento).ToList();

            if (cartasCorrecoes.Count == 0) return;

            var cceXmls = CriaXmlsCCe(cartasCorrecoes);
            var ccePdfs = CriaPdfsCCe(cartasCorrecoes, nfe);

            var posicao = 0;

            foreach (var memoryStream in ccePdfs)
            {
                emailBuilder.AddAnexo(memoryStream, $"pdf-cce-{++posicao}_${nfe.NumeroChave}.pdf");
            }

            foreach (var cceXml in cceXmls)
            {
                emailBuilder.AddAnexo(cceXml, $"xml-cce-{++posicao}_${nfe.NumeroChave}.xml");
            }
        }

        private static IEnumerable<MemoryStream> CriaPdfsCCe(IEnumerable<CartaCorrecaoNfe> cartasCorrecoes, Nfeletronica nfeletronica)
        {
            return cartasCorrecoes.Select(x => CartaCorrecaoHelper.GeraPdfEmMemoria(x, nfeletronica.Finalizacao))
                .ToList();
        }

        private static IEnumerable<MemoryStream> CriaXmlsCCe(IEnumerable<CartaCorrecaoNfe> cartasCorrecoes)
        {
            return cartasCorrecoes.Select(CartaCorrecaoHelper.GerarXmlMemoryStream).ToList();
        }

        public static void EnviaEmail(string xml, IEnumerable<Email> emails, string assunto, string mensagem)
        {
            var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            var pdf = new MemoryStream();

            var nfeProc = FuncoesXml.XmlStringParaClasse<NFe.Classes.nfeProc>(xml);
            var impressor = CriaImpressor(nfeProc, false, null);

            impressor.ExportarPdf(pdf);

            EmailBuilder emailBuilder;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                emailBuilder = new EmailBuilder(GetConfigEmail(sessao));
            }

            var chaveNfe = nfeProc.NFe.infNFe.Id.Substring(3, 44);

            emailBuilder.Assunto(assunto)
                .AddAnexo(xmlStream, $"xml-{chaveNfe}.xml")
                .AddAnexo(pdf, $"pdf-{chaveNfe}.pdf")
                .Mensagem(mensagem);

            emails.ForEach(e => emailBuilder.AddEmail(e));

            emailBuilder.Enviar();
        }

        private static ConfiguracaoEmailDTO GetConfigEmail(ISession sessao)
        {
            var repositorio = new RepositorioComun<ConfiguracaoEmailDTO>(sessao);
            return repositorio.Busca(new UnicaConfiguracaoEmail());
        }
    }
}