using System;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionPdv.Modelos.FormaPagamento;

namespace FusionPdv.Modelos.Pagamento
{
    public class PagamentoAjuste : IPagamento
    {
        public PagamentoAjuste()
        {
            FormaPagamento = new AjusteSaldo();
        }

        public decimal Valor { get; set; }

        public IFormaPagamento FormaPagamento { get; set; }
        public bool Pagou
        {
            get { throw new ArgumentException("Não pode ser utilizado"); }
            set { throw new ArgumentException("Não pode ser utilizado"); }
        }

        public void Calcula(VendaEcfDt vendaEcfDt)
        {
            if (Valor > vendaEcfDt.TotalCupom)
            {
                vendaEcfDt.Acrescimo = Valor - vendaEcfDt.TotalCupom;

                vendaEcfDt.TotalFinal = Valor;

                vendaEcfDt.Desconto = 0;
            }
            else
            {
                var desconto = vendaEcfDt.TotalCupom - Valor;
                var totalFinal = vendaEcfDt.TotalCupom - desconto;

                if (totalFinal < 0 || totalFinal < vendaEcfDt.TotalRecebido)
                    throw new PagamentoNegativoException("Ajuste final inválido, valor ficara negativo ou é menor que o total recebido.");

                vendaEcfDt.Desconto = desconto;
                vendaEcfDt.TotalFinal = totalFinal;
                vendaEcfDt.Acrescimo = 0;
            }
        }
    }
}
