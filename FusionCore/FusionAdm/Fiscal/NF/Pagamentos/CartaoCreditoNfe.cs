using System;
using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Fiscal.NF.Pagamentos
{
    public class CartaoCreditoNfe : FormaPagamentoNfe
    {
        private CartaoCreditoNfe()
        {
            //nhibernate
        }

        public CartaoCreditoNfe(UsuarioDTO usuarioCriacao, decimal valor) : this()
        {
            Usuario = usuarioCriacao;
            Valor = valor;
            CriadoEm = DateTime.Now;
            PossuiParcelamento = false;
            TipoDocumento = null;
        }

        public override IEnumerable<ParcelaNfe> Parcelas { get; } = new List<ParcelaNfe>();
        public override ETipoPagamento Especie { get; } = ETipoPagamento.CartaoCredito;
    }
}