using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.PedidoVenda
{
    public class Negociacao : Entidade
    {
        private IList<NegociacaoParcela> _parcelas;

        private Negociacao()
        {
            CriadoEm = DateTime.Now;
            _parcelas = new List<NegociacaoParcela>();
        }

        public int Id { get; private set; }
        protected override int ReferenciaUnica => Id;
        public PedidoVenda Pedido { get; private set; }
        public ETipoPagamento Especie { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public UsuarioDTO CriadoPor { get; private set; }
        public ITipoDocumento TipoDocumento { get; private set; }
        public decimal Valor { get; private set; }
        public bool PossuiParcelas { get; private set; }

        public IEnumerable<NegociacaoParcela> Parcelas => _parcelas;

        public override string ToString()
        {
            var descricao = Especie.GetDescription();

            switch (Especie)
            {
                case ETipoPagamento.CreditoLoja:
                    return $"{descricao} {_parcelas.Count}x";
                default:
                    return descricao;
            }
        }

        public static Negociacao CriarNoDinheiro(decimal valor, UsuarioDTO criadoPor)
        {
            return new Negociacao
            {
                CriadoPor = criadoPor,
                PossuiParcelas = false,
                Especie = ETipoPagamento.Dinheiro,
                TipoDocumento = null,
                Valor = valor
            };
        }

        public static Negociacao CriarNoPrazo(
            IList<NegociacaoParcela> parcelas,
            ITipoDocumento tipoDocumento,
            UsuarioDTO usuario)
        {
            var negociacao = new Negociacao
            {
                CriadoPor = usuario,
                PossuiParcelas = true,
                TipoDocumento = tipoDocumento,
                Valor = parcelas.Sum(i => i.Valor),
                Especie = ETipoPagamento.CreditoLoja
            };

            foreach (var p in parcelas)
            {
                p.AnexarNegociacao(negociacao);
            }

            negociacao._parcelas = parcelas;

            return negociacao;
        }

        public static Negociacao CriarNoCartaoCredito(decimal valor, UsuarioDTO criadoPor)
        {
            return new Negociacao
            {
                CriadoPor = criadoPor,
                PossuiParcelas = false,
                Especie = ETipoPagamento.CartaoCredito,
                TipoDocumento = null,
                Valor = valor
            };
        }

        public static Negociacao CriarNoCartaoDebito(decimal valor, UsuarioDTO criadoPor)
        {
            return new Negociacao
            {
                CriadoPor = criadoPor,
                PossuiParcelas = false,
                Especie = ETipoPagamento.CartaoDebito,
                TipoDocumento = null,
                Valor = valor
            };
        }

        public void AnexarPedido(PedidoVenda pedido)
        {
            if (Pedido != null)
            {
                throw new InvalidOperationException("Negociação já anexada a outro pedido.");
            }

            Pedido = pedido;
        }
    }
}