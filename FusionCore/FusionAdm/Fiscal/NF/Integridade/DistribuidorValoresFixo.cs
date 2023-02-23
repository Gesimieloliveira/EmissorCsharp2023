using System.Linq;
using static System.Decimal;

namespace FusionCore.FusionAdm.Fiscal.NF.Integridade
{
    internal static class DistribuidorValoresFixo
    {
        public static void Distribuir(Nfeletronica nfe)
        {
            if (!nfe.Itens.Any())
            {
                return;
            }

            var totalDosItens = nfe.Itens.Where(x => x.AutoCalcularTotaisItem == true).Sum(i => i.TotalItem);

            var fatorDesconto = CalcularFator(nfe.ValorDescontoFixo, totalDosItens);
            var fatorDespesas = CalcularFator(nfe.ValorDespesasFixa, totalDosItens);
            var fatorFrete = CalcularFator(nfe.ValorFreteFixo, totalDosItens);
            var fatorSeguro = CalcularFator(nfe.ValorSeguroFixo, totalDosItens);

            foreach (var i in nfe.Itens.Where(x => x.AutoCalcularTotaisItem == true))
            {
                i.ValorDescontoFixoRateio = Round(i.TotalItem * fatorDesconto, 2);
                i.ValorDespesasFixaRateio = Round(i.TotalItem * fatorDespesas, 2);
                i.ValorFreteFixoRateio = Round(i.TotalItem * fatorFrete, 2);
                i.ValorSeguroFixoRateio = Round(i.TotalItem * fatorSeguro, 2);
            }

            DistribuirRestanteUltimoItem(nfe);
        }

        private static decimal CalcularFator(decimal valor, decimal totalDosItens)
        {
            if (totalDosItens == 0)
            {
                return 0;
            }

            return valor / totalDosItens;
        }

        private static void DistribuirRestanteUltimoItem(Nfeletronica nfe)
        {
            var ultimoItem = nfe.Itens.LastOrDefault(x => x.AutoCalcularTotaisItem == true);

            if (ultimoItem == null) return;

            var somaDescontoFixo = nfe.Itens.Sum(i => i.ValorDescontoFixoRateio);
            var somaFreteFixo = nfe.Itens.Sum(i => i.ValorFreteFixoRateio);
            var somaSeguroFixo = nfe.Itens.Sum(i => i.ValorSeguroFixoRateio);
            var somaDespesasFixa = nfe.Itens.Sum(i => i.ValorDespesasFixaRateio);

            ultimoItem.ValorDescontoFixoRateio += Round(nfe.ValorDescontoFixo - somaDescontoFixo, 2);
            ultimoItem.ValorFreteFixoRateio += Round(nfe.ValorFreteFixo - somaFreteFixo, 2);
            ultimoItem.ValorSeguroFixoRateio += Round(nfe.ValorSeguroFixo - somaSeguroFixo, 2);
            ultimoItem.ValorDespesasFixaRateio += Round(nfe.ValorDespesasFixa - somaDespesasFixa, 2);
        }
    }
}