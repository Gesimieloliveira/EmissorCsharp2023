using System;
using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Fiscal.NF.Pagamentos
{
    public class DinheiroNfe : FormaPagamentoNfe
    {
        private DinheiroNfe()
        {
            //nhibernate
        }

        public DinheiroNfe(UsuarioDTO usuarioCriacao, decimal valor) : this()
        {
            Valor = valor;
            CriadoEm = DateTime.Now;
            PossuiParcelamento = false;
            TipoDocumento = null;
            Usuario = usuarioCriacao;
        }

        public override IEnumerable<ParcelaNfe> Parcelas { get; } = new List<ParcelaNfe>();
        public override ETipoPagamento Especie => ETipoPagamento.Dinheiro;

        public override string ToString()
        {
            return TextoTextView;
        }
    }
}