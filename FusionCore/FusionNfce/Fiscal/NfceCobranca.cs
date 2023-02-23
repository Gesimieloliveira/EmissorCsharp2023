using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.Fiscal
{
    public class NfceCobranca : Entidade
    {
        public NfceCobranca()
        {
            TipoParcela = TipoParcela.DiaFixo;
            DiaParcelaFixa = DateTime.Now.Day;
        }

        public int NfceId { get; set; }
        public Nfce Nfce { get; set; }
        public string NumeroFatura { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorLiquido { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorEntrada { get; set; }
        public TipoParcela TipoParcela { get; set; }
        public int DiaParcelaFixa { get; set; }
        public string Descricao { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public short? CentroLucroId { get; set; }
        public IList<NfceCobrancaDuplicata> CobrancaDuplicatas { get; set; } = new List<NfceCobrancaDuplicata>();
        protected override int ReferenciaUnica => NfceId;

        public void Add(NfceCobrancaDuplicata duplicata)
        {
            CobrancaDuplicatas.Add(duplicata);
        }
    }
}