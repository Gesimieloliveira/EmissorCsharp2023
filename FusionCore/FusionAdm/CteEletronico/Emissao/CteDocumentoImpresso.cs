using System;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteDocumentoImpresso
    {
        public int Id { get; set; }
        public Cte Cte { get; set; }
        public PerfilCfopDTO PerfilCfop { get; set; }
        public string NumeroRomaneiro { get; set; }
        public string NumeroPedido { get; set; }
        public ModeloNotaFiscal ModeloNotaFiscal { get; set; }
        public short Serie { get; set; }
        public string Numero { get; set; }
        public DateTime EmitidaEm { get; set; }
        public decimal BaseCalculoIcms { get; set; }
        public decimal TotalBaseCalculoIcms { get; set; }
        public decimal BaseCalculoIcmsSt { get; set; }
        public decimal TotalBaseCalculoIcmsSt { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal TotalNf { get; set; }
        public decimal TotalPesoKg { get; set; }
        public int PinSuframa { get; set; }
        public DateTime? PrevisaoEntregaEm { get; set; }


    }
}