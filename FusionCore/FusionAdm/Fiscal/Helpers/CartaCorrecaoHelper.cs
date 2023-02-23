using System;
using System.IO;
using System.Text;
using DFe.Utils;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.CCe;
using NFe.Classes;
using NFe.Classes.Servicos.Consulta;
using NFe.Classes.Servicos.Evento;
using NFe.Danfe.Base.NFe;
using NFe.Danfe.Fast.NFe;

namespace FusionCore.FusionAdm.Fiscal.Helpers
{
    public static class CartaCorrecaoHelper
    {
        private static procEventoNFe GerarArquivoXML(CartaCorrecaoNfe cce)
        {
            if (string.IsNullOrWhiteSpace(cce.XmlRetorno) || string.IsNullOrWhiteSpace(cce.XmlEnvio))
            {
                throw new InvalidOperationException("Que pena! CC-e não possui XML válido para gerar o PDF");
            }

            var envEvento = FuncoesXml.XmlStringParaClasse<envEvento>(cce.XmlEnvio);
            var retEnvEvento = FuncoesXml.XmlStringParaClasse<retEnvEvento>(cce.XmlRetorno);

            return new procEventoNFe
            {
                evento = envEvento.evento[0],
                versao = envEvento.versao,
                retEvento = retEnvEvento.retEvento[0]
            };
        }

        public static MemoryStream GerarXmlMemoryStream(CartaCorrecaoNfe cce)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(FuncoesXml.ClasseParaXmlString(GerarArquivoXML(cce))));
        }

        public static MemoryStream GeraPdfEmMemoria(CartaCorrecaoNfe cce, EmissaoFinalizadaNfe emissao)
        {
            var pdf = new MemoryStream();
            var xmlNfe = DanfeNfeHelper.ObterStringXml(emissao);
            var xmlCce = GerarArquivoXML(cce);

            var nfeProc = FuncoesXml.XmlStringParaClasse<nfeProc>(xmlNfe);

            var danfe = new DanfeFrEvento(nfeProc, xmlCce, new ConfiguracaoDanfeNfe(null, false, false, true));
            danfe.ExportarPdf(pdf);

            return pdf;
        }

        public static void GerarPDF(CartaCorrecaoNfe cce, EmissaoFinalizadaNfe emissao)
        {
            var xmlNfe = DanfeNfeHelper.ObterStringXml(emissao);
            var xmlCce = GerarArquivoXML(cce);

            var nfeProc = FuncoesXml.XmlStringParaClasse<nfeProc>(xmlNfe);

            var danfe = new DanfeFrEvento(nfeProc, xmlCce, new ConfiguracaoDanfeNfe(null, false, false, true));
            danfe.Visualizar();
        }
    }
}