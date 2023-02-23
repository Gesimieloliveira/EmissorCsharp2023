using System;
using FusionCore.FusionAdm.MdfeEletronico.Autorizador;
using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionCore.Repositorio.Base;

namespace Fusion.Visao.MdfeEletronico.IncluirPagamento
{
    public class ParcelaDTO : EntidadeBase<int>
    {
        public DateTime VencimentoEm { get; set; }

        public decimal Valor { get; set; }
        public int Numero { get; set; }
        public MdfeParcela MdfeParcela { get; set; }
        protected override int ChaveUnica => MdfeParcela?.Id ?? 0;
        public MdfeAutorizacaoParcela MdfeParcelaAba { get; set; }
    }
}