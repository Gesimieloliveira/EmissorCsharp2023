using System;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico.Autorizador
{
    public class MdfeAutorizacaoParcela : EntidadeBase<int>
    {

        public MdfeAutorizacaoParcela()
        {
        }


        public int Id { get; set; }
        public MdfeAutorizacaoInformacaoPagamento InformacaoPagamento { get; set; }
        public int Numero { get; set; }
        public DateTime DataDeVencimento { get; set; }
        public decimal Valor { get; set; }
        protected override int ChaveUnica => Id;
    }
}