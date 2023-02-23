using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public sealed class Cobranca : Entidade
    {
        public Cobranca()
        {
            TipoParcela = TipoParcela.DiaFixo;
            DiaParcelaFixa = DateTime.Now.Day;
        }

        public int NfeId { get; set; }
        public Nfeletronica Nfe { get; set; }
        public string NumeroFatura { get; set; }
        public decimal ValorOriginal { get; private set; }
        public decimal ValorLiquido { get; private set; }
        public decimal ValorDesconto { get; private set; }
        public decimal ValorEntrada { get; set; }
        public TipoParcela TipoParcela { get; set; }
        public int DiaParcelaFixa { get; set; }
        public string Descricao { get; set; }
        public ITipoDocumento TipoDocumento { get; set; }
        public CentroLucro CentroLucro { get; set; }
        public IList<CobrancaDuplicata> CobrancaDuplicatas { get; private set; } = new List<CobrancaDuplicata>();
        protected override int ReferenciaUnica => NfeId;

        public void Add(CobrancaDuplicata duplicata)
        {
            ValorOriginal += duplicata.Valor;
            ValorLiquido += duplicata.Valor;

            CobrancaDuplicatas.Add(duplicata);
        }
    }
}