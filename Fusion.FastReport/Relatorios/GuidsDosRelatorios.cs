using System;
using System.Collections.Generic;
using Fusion.FastReport.Relatorios.Fixos;
using Fusion.FastReport.Relatorios.Sistema;
using Fusion.FastReport.Relatorios.Sistema.FaturamentoMei;
using Fusion.FastReport.Relatorios.Sistema.Financeiro;

namespace Fusion.FastReport.Relatorios
{
    public static class GuidsDosRelatorios
    {
        private static readonly IDictionary<string, Guid> Guids;

        static GuidsDosRelatorios()
        {
            Guids = new Dictionary<string, Guid>
            {
                {nameof(RImpressaoPedidoVenda), new Guid("6F13483C-68A5-4B54-B718-E7AF1835F409")},
                {nameof(RImpressaoFaturamentoA4), new Guid("E0D47A90-6618-464B-9DF0-9CEA06C92D30")},
                {nameof(RImpressaoFaturamento80), new Guid("70E0667E-B213-403D-B958-651D9F404450")},
                {nameof(RAnaliseLucroPorItem), new Guid("3879968D-8D51-4686-B1BD-30450FC1C391")},
                {nameof(RAniversariantes), new Guid("99FE48E9-10E1-462E-A3DF-3A88957A56CD")},
                {nameof(RComprasDetalhado), new Guid("F016F491-C298-419B-8FBB-08F665AD1B95")},
                {nameof(RContagemEstoque), new Guid("2E6E7675-EB11-4E8E-9F2E-E829D5002663")},
                {nameof(RDanfeNfce), new Guid("DAF232C0-8CC1-4A5E-81DC-31079551B471")},
                {nameof(RDanfeNfce58mm), new Guid("6886F307-0AE7-4198-B1C5-4AA09867091B")},
                {nameof(RDanfeNfceA4), new Guid("0A01E317-69A4-4805-83FB-40DECA188CD6")},
                {nameof(RItensVendidos), new Guid("EE7EC3E6-F49D-42A8-A920-F4D0DEEAB33C")},
                {nameof(RIventarioEstoque), new Guid("F2A03DF6-0D57-4F24-986E-3859FBB2B85F")},
                {nameof(RListagemProdutoEstoque), new Guid("E13CEB92-57B6-4802-B2A1-F2D0F5C4639A")},
                {nameof(RListagemProdutoTributacao), new Guid("165987BE-5196-4E15-B4F1-6B44E011C0D6")},
                {nameof(RVendasTrasmitidasNaNfce), new Guid("1AF53099-185D-4886-A747-DB2F2245074F")},
                {nameof(RVendasDoFaturamento), new Guid("9423F2A9-6F77-44D6-961B-A594F8CC2D97")},
                {nameof(RModeloEtiqueta), new Guid("DDFD51B7-E2B4-4CD3-8826-789621EA3CB0")},
                {nameof(RPromissoriaCarne), new Guid("EF70651D-D96A-4F27-B12C-DD108B39144F")},
                {nameof(RPromissoria), new Guid("0B9BF92F-242B-421B-84F9-8FA70B62A787")},
                {nameof(RDocumentoPagar), new Guid("A04A84FE-A299-4462-A171-ACF4C220D83F")},
                {nameof(RDocumentoReceber), new Guid("735B8978-6248-487E-9DAF-3F121A541A00")},
                {nameof(RRecibo), new Guid("CABA6750-B629-49C7-8492-D1FB7189F198")},
                {nameof(RProdutosComNcmVencido), new Guid("379BE72A-43F0-4263-A47E-46C75DAB4384")},
                {nameof(RNcmVencido), new Guid("A5706A61-90E8-41B9-9BF4-201A4C42A06E")},
            };
        }

        public static Guid Obtem(Type type)
        {
            var nome = type.Name;

            if (Guids.TryGetValue(nome, out var guid))
            {
                return guid;
            }
            
            throw new InvalidOperationException("Relatório ainda não possui template para edição");
        }
    }
}