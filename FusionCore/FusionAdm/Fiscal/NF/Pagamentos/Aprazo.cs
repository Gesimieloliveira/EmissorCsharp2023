using System;
using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Fiscal.NF.Pagamentos
{
    public class Aprazo : FormaPagamentoNfe
    {
        private readonly IList<ParcelaNfe> _parcelas = new List<ParcelaNfe>();

        private Aprazo()
        {
            //nhibernate
        }

        public Aprazo(
            UsuarioDTO usuario,
            ITipoDocumento tipo,
            IReadOnlyList<ParcelaNfe> parcelas) : this()
        {
            TipoDocumento = tipo;
            CriadoEm = DateTime.Now;
            PossuiParcelamento = true;
            Usuario = usuario;

            foreach (var p in parcelas)
            {
                p.Prazo = this;
                _parcelas.Add(p);

                Valor += p.Valor;
            }
        }

        public override IEnumerable<ParcelaNfe> Parcelas => _parcelas;
        public override ETipoPagamento Especie => ETipoPagamento.CreditoLoja;

        public override string ToString()
        {
            return $"{TextoTextView} em {_parcelas.Count}x";
        }
    }
}