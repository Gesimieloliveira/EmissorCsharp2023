using System;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronico.Emitir.EntidadesModels
{
    public class GridDocumentoNotaFiscalImpressaModel : ViewModel
    {
        public int Id { get; set; }
        public short Serie { get; set; }
        public string NumeroRomaneiro { get; set; }
        public string NumeroPedidoNf { get; set; }
        public ModeloNotaFiscal ModeloNotaFiscal { get; set; }
        public string Numero { get; set; }
        public DateTime EmitidaEm { get; set; }
        public decimal ValorBaseCalculoIcms { get; set; }
        public decimal ValorTotalIcms { get; set; }
        public decimal ValorBaseCalculoIcmsSt { get; set; }
        public decimal ValorTotalIcmsSt { get; set; }
        public decimal ValorTotalProduto { get; set; }
        public decimal ValorTotalNf { get; set; }
        public PerfilCfopDTO PerfilCfop { get; set; }
        public decimal PesoTotalEmKg { get; set; }
        public int PinSuframa { get; set; }
        public DateTime? DataPrevistaEntrega { get; set; }
        public CteDocumentoImpresso DocumentoImpresso { get; set; }

        public static GridDocumentoNotaFiscalImpressaModel Cria(CteDocumentoImpresso documentoImpresso)
        {
            var documentoNotaFiscalImpressa = new GridDocumentoNotaFiscalImpressaModel
            {
                EmitidaEm = documentoImpresso.EmitidaEm,
                ModeloNotaFiscal = documentoImpresso.ModeloNotaFiscal,
                Numero = documentoImpresso.Numero,
                NumeroPedidoNf = documentoImpresso.NumeroPedido,
                NumeroRomaneiro = documentoImpresso.NumeroRomaneiro,
                PerfilCfop = documentoImpresso.PerfilCfop,
                PesoTotalEmKg = documentoImpresso.TotalPesoKg,
                PinSuframa = documentoImpresso.PinSuframa,
                Serie = documentoImpresso.Serie,
                ValorBaseCalculoIcms = documentoImpresso.BaseCalculoIcms,
                ValorBaseCalculoIcmsSt = documentoImpresso.BaseCalculoIcmsSt,
                ValorTotalIcms = documentoImpresso.TotalBaseCalculoIcms,
                ValorTotalIcmsSt = documentoImpresso.TotalBaseCalculoIcmsSt,
                ValorTotalNf = documentoImpresso.TotalNf,
                ValorTotalProduto = documentoImpresso.TotalProdutos,
                DataPrevistaEntrega = documentoImpresso.PrevisaoEntregaEm,
                DocumentoImpresso = documentoImpresso
            };


            return documentoNotaFiscalImpressa;
        }
    }
}