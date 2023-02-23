using System;
using System.Collections.Generic;
using FusionCore.FusionNfce.Pagamento;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Hidratacao;
using NFe.Classes.Informacoes.Pagamento;
using NHibernate.Util;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtFormaPagamentoNfce
    {
        public static detPag ToZeus(this FormaPagamentoNfce formaPagamento)
        {
            var detPag = new detPag();

            const string descontoOuAcrescimo = "10";
            const string dinheiro = "01";
            const string crediario = "03";
            const string cartaoDebito = "07";
            const string cartaoCredito = "08";
            const string cartaoTef = "09";
            const string outros = "99";
            const string pix = "11";

            switch (formaPagamento.IdFormaPagamento)
            {
                case dinheiro:
                    detPag.tPag = FormaPagamento.fpDinheiro;
                    detPag.vPag = formaPagamento.ValorPagamento;
                    break;

                case outros:
                    detPag.tPag = FormaPagamento.fpOutro;
                    detPag.vPag = formaPagamento.ValorPagamento;
                    detPag.xPag = formaPagamento.DescricaoOutros;
                    break;

                case cartaoCredito:
                    detPag.tPag = FormaPagamento.fpCartaoCredito;
                    detPag.vPag = formaPagamento.ValorPagamento;
                    AdicionaInformacoesCartao(formaPagamento, detPag);
                    break;

                case cartaoDebito:
                    detPag.tPag = FormaPagamento.fpCartaoDebito;
                    detPag.vPag = formaPagamento.ValorPagamento;
                    AdicionaInformacoesCartao(formaPagamento, detPag);
                    break;

                case crediario:
                    detPag.tPag = FormaPagamento.fpCreditoLoja;
                    detPag.vPag = formaPagamento.ValorPagamento;
                    break;

                case cartaoTef: 
                    detPag.tPag = FormaPagamento.fpCartaoCredito;
                    detPag.vPag = formaPagamento.ValorPagamento;
                    detPag.card = new card
                    {
                        tpIntegra = TipoIntegracaoPagamento.TipNaoIntegrado,
                        tBand = BandeiraCartao.bcOutros
                    };
                    break;

                case descontoOuAcrescimo:
                    return null;

                case pix:
                    detPag.tPag = FormaPagamento.fpPagamentoInstantaneoPIX;
                    detPag.vPag = formaPagamento.ValorPagamento;
                    break;

            }

            return detPag;
        }

        private static void AdicionaInformacoesCartao(FormaPagamentoNfce formaPagamento, detPag detPag)
        {
            detPag.card = new card
            {
                tpIntegra = TipoIntegracaoPagamento.TipNaoIntegrado,
                tBand = ConverteCartao(formaPagamento.Bandeira)
            };

            if (SessaoSistemaNfce.Preferencia.NaoSolicitaDadosCartaoPos()) return;

            detPag.card.CNPJ = formaPagamento.CnpjCredenciadora.TrimOrNull();
            detPag.card.cAut = formaPagamento.NumeroAprovacao.TrimOrNull();
        }

        private static BandeiraCartao? ConverteCartao(string bandeira)
        {
            if (SessaoSistemaNfce.Preferencia.NaoSolicitaDadosCartaoPos()) return BandeiraCartao.bcOutros;

            switch (bandeira)
            {
                case "Visa":
                    return BandeiraCartao.bcVisa;
                case "Mastercard":
                    return BandeiraCartao.bcMasterCard;
                case "AmericanExpress":
                    return BandeiraCartao.bcAmericanExpress;
                case "Sorocred":
                    return BandeiraCartao.bcSorocred;
                case "DinersClub":
                    return BandeiraCartao.bcDinersClub;
                case "Elo":
                    return BandeiraCartao.Elo;
                case "Hipercard":
                    return BandeiraCartao.Hipercard;
                case "Aura":
                    return BandeiraCartao.Aura;
                case "Cabal":
                    return BandeiraCartao.Cabal;
                case "CalCard":
                    return BandeiraCartao.CalCard;
                case "Credz":
                    return BandeiraCartao.Credz;
                case "Discover":
                    return BandeiraCartao.Discover;
                case "GoodCard":
                    return BandeiraCartao.GoodCard;
                case "GreenCard":
                    return BandeiraCartao.GreenCard;
                case "Hiper":
                    return BandeiraCartao.Hiper;
                case "JcB":
                    return BandeiraCartao.JcB;
                case "Mais":
                    return BandeiraCartao.Mais;
                case "MaxVan":
                    return BandeiraCartao.MaxVan;
                case "Policard":
                    return BandeiraCartao.Policard;
                case "RedeCompras":
                    return BandeiraCartao.RedeCompras;
                case "Sodexo":
                    return BandeiraCartao.Sodexo;
                case "ValeCard":
                    return BandeiraCartao.ValeCard;
                case "Verocheque":
                    return BandeiraCartao.Verocheque;
                case "VR":
                    return BandeiraCartao.VR;
                case "Ticket":
                    return BandeiraCartao.Ticket;
                case "Outros":
                    return BandeiraCartao.bcOutros;
                case "Nemnhum":
                    return null;
                    
            }

            throw new ArgumentException($"{bandeira} inválida na conversão do fusion para nfc-e do zeus");
        }


        public static IList<pag> ToZeus(this IEnumerable<FormaPagamentoNfce> formasDePagamentos)
        {
            var pagamentos = new List<pag>
            {
                new pag
                {
                    detPag = new List<detPag>()
                }
            };

            formasDePagamentos.ForEach(f =>
            {
                var detPag = f.ToZeus();

                if (detPag == null) return;

                pagamentos[0].detPag.Add(detPag);
            });

            return pagamentos;
        } 
    }
}