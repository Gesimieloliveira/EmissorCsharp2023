using System;
using FusionCore.FusionAdm.Financeiro.Flags;
using OpenAC.Net.Core.Extensions;

namespace FusionCore.FusionAdm.Financeiro
{
    public class DocumentoPagarLancamento
    {
        public int Id { get; set; }
        public DocumentoPagar DocumentoPagar { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public TipoLancamento TipoLancamento { get; set; }
        public string TipoLancamentoTexto { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? DataEstorno { get; private set; }
        public bool Estornado { get; private set; }

        public static DocumentoPagarLancamento Cria(
            string descricao, 
            decimal valor, 
            TipoLancamento tipoLancamento,
            DocumentoPagar documentoPagar
        ) {
            var docLancamento = new DocumentoPagarLancamento
            {
                Descricao = descricao,
                Valor = valor,
                TipoLancamento = tipoLancamento,
                DocumentoPagar = documentoPagar,
                TipoLancamentoTexto = tipoLancamento.GetDescription(),
                CriadoEm = DateTime.Now
            };

            return docLancamento;
        }

        public void Estornar()
        {
            if (Estornado)
            {
                throw new InvalidOperationException("Lançamento já foi estornado!");
            }

            Estornado = true;
            DataEstorno = DateTime.Now;

            switch (TipoLancamento)
            {
                case TipoLancamento.Pagamento:
                    DocumentoPagar.EstornarPagamento(Valor);
                    break;
                case TipoLancamento.Juro:
                    DocumentoPagar.EstornarJuros(Valor);
                    break;
                case TipoLancamento.Desconto:
                    DocumentoPagar.EstornarDesconto(Valor);
                    break;
                case TipoLancamento.AjusteParaMais:
                    DocumentoPagar.ValorAjustado -= Valor;
                    break;
                case TipoLancamento.AjusteParaMenos:
                    DocumentoPagar.ValorAjustado -= Valor;
                    break;
            }
        }
    }
}