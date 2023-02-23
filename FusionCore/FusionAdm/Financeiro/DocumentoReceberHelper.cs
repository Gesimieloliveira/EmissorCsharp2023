using System;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.FusionAdm.Financeiro
{
    public static class DocumentoReceberHelper
    {
        private static decimal TaxaJuros
        {
            get
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioConfiguracaoFinanceiro(sessao);
                    var cfg = repositorio.BuscarUnico();

                    return cfg.TaxaDeJurosMensal;
                }
            }
        }

        public static decimal CalcularJurosPendente(IDocumentoReceber documento)
        {
            if (documento.EstaVencido == false)
            {
                return 0.00M;
            }

            var taxaDiaria = TaxaJuros / 30;

            var dataBase = documento.UltimoCalculoJuros ?? documento.Vencimento;
            var qtdeDias = (DateTime.Today - dataBase.Date).Days;
            var jurosCalculado = (documento.ValorRestante * (taxaDiaria * qtdeDias) / 100);

            return decimal.Round(jurosCalculado, 2);
        }

        public static decimal CorrigirValorRestante(IDocumentoReceber documento)
        {
            if (documento.ValorRestante <= 0)
            {
                return documento.ValorRestante;
            }

            var jurosCalculado = CalcularJurosPendente(documento);

            return decimal.Round(documento.ValorRestante + jurosCalculado, 2);
        }

        public static int CalcularQtdeDiasVencidos(IDocumentoReceber documento)
        {
            if (documento.EstaVencido)
            {
                return (DateTime.Today - documento.Vencimento).Days;
            }

            return 0;
        }
    }
}