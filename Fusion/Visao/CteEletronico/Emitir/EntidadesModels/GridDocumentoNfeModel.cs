using System;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronico.Emitir.EntidadesModels
{
    public class GridDocumentoNfeModel : ViewModel
    {
        public int Id { get; set; }
        public string ChaveNfe { get; set; }
        public DateTime? PrevisaoEntregaEm { get; set; }
        public int PinSuframa { get; set; }
        public decimal TotalNFe { get; set; }
        public CteDocumentoNfe DocumentoNfe { get; set; }

        public static GridDocumentoNfeModel Cria(CteDocumentoNfe documentoNfe)
        {
            var gridDocumentoNfeModel = new GridDocumentoNfeModel
            {
                Id = documentoNfe.Id,
                ChaveNfe = documentoNfe.Chave,
                PinSuframa = documentoNfe.PinSuframa,
                PrevisaoEntregaEm = documentoNfe.PrevisaoEntregaEm,
                TotalNFe = documentoNfe.Valor,
                DocumentoNfe = documentoNfe
            };

            return gridDocumentoNfeModel;
        }
    }
}