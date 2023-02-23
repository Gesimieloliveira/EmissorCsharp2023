using System;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Estadual;

namespace FusionCore.FusionAdm.EntradaOutras
{
    public class NfCteEntrada : Entidade
    {
        public int Id { get; set; }

        public EmpresaDTO EmpresaTomador { get; set; }
        public ModeloDocumentoCteEntrada ModeloDocumento { get; set; } = ModeloDocumentoCteEntrada.Cte;
        public SituacaoFiscal SituacaoFiscal { get; set; } = SituacaoFiscal.Normal;
        public DateTime EmissaoEm { get; set; } = DateTime.Now;
        public DateTime UtilizacaoEm { get; set; } = DateTime.Now;
        public short Serie { get; set; }
        public short Subserie { get; set; }
        public int Numero { get; set; }
        public CfopDTO Cfop { get; set; }

        protected override int ReferenciaUnica => Id;
        public TributacaoCst IcmsCst { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal BaseCalculoIcms { get; set; }
        public decimal ValorIcms { get; set; }
    }
}