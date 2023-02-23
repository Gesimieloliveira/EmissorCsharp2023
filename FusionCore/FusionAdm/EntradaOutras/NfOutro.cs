using System;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Estadual;

namespace FusionCore.FusionAdm.EntradaOutras
{
    public class NfOutro : Entidade
    {
        public int Id { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public DateTime EmissaoEm { get; set; } = DateTime.Now;
        public DateTime RecebimentoEm { get; set; } = DateTime.Now;
        public ModeloDocumentoOutro ModeloDocumento { get; set; } = ModeloDocumentoOutro.ContaEnergiaEletrica;
        public short Serie { get; set; }
        public int Numero { get; set; }
        public CfopDTO Cfop { get; set; }
        public TipoEmitente TipoEmitente { get; set; } = TipoEmitente.Terceiro;
        public TributacaoIcms Cst { get; set; }

        public decimal ValorTotal { get; set; }
        public decimal BaseCalculoIcms { get; set; }
        public decimal ValorIcms { get; set; }
        public decimal ValorDespesasAcessorias { get; set; }
        public decimal AliquotaIcms { get; set; }

        public decimal BaseCalculoIcmsSt { get; set; }
        public decimal ValorIcmsSt { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal TotalDesconto { get; set; }

        public SituacaoFiscal SituacaoFiscal { get; set; } = SituacaoFiscal.Normal;

        protected override int ReferenciaUnica => Id;
    }
}