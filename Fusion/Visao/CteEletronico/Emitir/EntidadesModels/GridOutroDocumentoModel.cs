using System;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronico.Emitir.EntidadesModels
{
    public class GridOutroDocumentoModel : ViewModel
    {
        public int Id { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string DescricaoOutros { get; set; }
        public string Numero { get; set; }
        public DateTime? EmitidoEm { get; set; }
        public DateTime? PrevisaoEntregaEm { get; set; }
        public decimal ValorTotal { get; set; }
        public CteDocumentoOutro DocumentoOutro { get; set; }

        public static GridOutroDocumentoModel Cria(CteDocumentoOutro documentoOutro)
        {
            var outroDocumento = new GridOutroDocumentoModel
            {
                TipoDocumento = documentoOutro.TipoDocumento,
                DescricaoOutros = documentoOutro.DescricaoOutro,
                EmitidoEm = documentoOutro.EmitidoEm,
                Numero = documentoOutro.Numero,
                PrevisaoEntregaEm = documentoOutro.PrevisaoEntregaEm,
                ValorTotal = documentoOutro.Valor,
                DocumentoOutro = documentoOutro
            };


            return outroDocumento;
        }
    }
}