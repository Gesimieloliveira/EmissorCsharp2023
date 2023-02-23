using System;
using System.Diagnostics.CodeAnalysis;
using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.PedidoVenda
{
    public class NegociacaoParcela : Entidade
    {
        [SuppressMessage("ReSharper", "EmptyConstructor")]
        public NegociacaoParcela()
        {
            Id = 0;
        }

        public NegociacaoParcela(short numero, DateTime vencimento, decimal valor)
        {
            Numero = numero;
            Vencimento = vencimento;
            Valor = valor;
        }

        public int Id { get; private set; }
        protected override int ReferenciaUnica => Id;
        public Negociacao Negociacao { get; private set; }
        public short Numero { get; private set; }
        public DateTime Vencimento { get; private set; }
        public decimal Valor { get; private set; }

        public void AnexarNegociacao(Negociacao negociacao)
        {
            Negociacao = negociacao;
        }
    }
}