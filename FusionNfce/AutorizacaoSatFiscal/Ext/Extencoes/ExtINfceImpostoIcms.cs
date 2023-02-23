using System;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using OpenAC.Net.Sat;
using OpenAC.Net.Sat.Interfaces;

namespace FusionNfce.AutorizacaoSatFiscal.Ext.Extencoes
{
    public static class ExtINfceImpostoIcms
    {
        public static OrigemMercadoria ObterOrigemMercadoriaSAT(this FusionCore.FusionAdm.Fiscal.FlagsImposto.OrigemMercadoria origem)
        {
            switch (origem)
            {
                case FusionCore.FusionAdm.Fiscal.FlagsImposto.OrigemMercadoria.Nacional:
                    return OrigemMercadoria.Nacional;
                case FusionCore.FusionAdm.Fiscal.FlagsImposto.OrigemMercadoria.EstrangeiraImportacaoDireta:
                    return OrigemMercadoria.EstrangeiraImportacaoDireta;
                case FusionCore.FusionAdm.Fiscal.FlagsImposto.OrigemMercadoria.EstrangeiraImportacaoDiretaSemSimilar:
                    return OrigemMercadoria.EstrangeiraImportacaoDiretaSemSimilar;
                case FusionCore.FusionAdm.Fiscal.FlagsImposto.OrigemMercadoria.EstrangeiraInterna:
                    return OrigemMercadoria.EstrangeiraAdquiridaBrasil;
                case FusionCore.FusionAdm.Fiscal.FlagsImposto.OrigemMercadoria.EstrangeiraInternaSemSimilar:
                    return OrigemMercadoria.EstrangeiraAdquiridaBrasilSemSimilar;
                case FusionCore.FusionAdm.Fiscal.FlagsImposto.OrigemMercadoria.NacionalConformidadeBasicas:
                    return OrigemMercadoria.NacionalProcessosBasicos;
                case FusionCore.FusionAdm.Fiscal.FlagsImposto.OrigemMercadoria.NacionalImportacaoInferior40:
                    return OrigemMercadoria.NacionalConteudoImportacaoInferiorIgual40;
                case FusionCore.FusionAdm.Fiscal.FlagsImposto.OrigemMercadoria.NacionalImportacaoSuperior40:
                    return OrigemMercadoria.NacionalConteudoImportacaoSuperior40;
                case FusionCore.FusionAdm.Fiscal.FlagsImposto.OrigemMercadoria.NacionalImportacaoSuperior70:
                    return OrigemMercadoria.NacionalConteudoImportacaoSuperior70;
                default:
                    throw new InvalidOperationException("Opção não existe");
            }
        }


        public static ICFeImposto ObterImpostoSAT(this INfceImpostoIcms impostoCsosn, NfceItem item)
        {
            switch (impostoCsosn.CodigoCsosn)
            {
                case "102":
                case "300":
                case "400":
                case "500":
                    return new ImpostoIcms
                    {
                        Icms = new ImpostoIcmsSn102
                        {
                            Csosn = impostoCsosn.CodigoCsosn,
                            Orig = impostoCsosn.OrigemMercadoria.ObterOrigemMercadoriaSAT()
                        }
                    };
                case "900":
                    return new ImpostoIcms
                    {
                        Icms = new ImpostoIcmsSn900
                        {
                            Orig = impostoCsosn.OrigemMercadoria.ObterOrigemMercadoriaSAT(),
                            Csosn = impostoCsosn.CodigoCsosn,
                            PIcms = item.Produto.AliquotaIcms
                        }
                    };
                case "00":
                case "20":
                case "90":
                    return new ImpostoIcms
                    {
                        Icms = new ImpostoIcms00
                        {
                            Cst = impostoCsosn.CodigoCsosn,
                            Orig = impostoCsosn.OrigemMercadoria.ObterOrigemMercadoriaSAT(),
                            PIcms = impostoCsosn.AliquotaIcms
                        }
                    };
                case "40":
                case "41":
                case "60":
                    return new ImpostoIcms
                    {
                        Icms = new ImpostoIcms40
                        {
                            Cst = impostoCsosn.CodigoCsosn,
                            Orig = impostoCsosn.OrigemMercadoria.ObterOrigemMercadoriaSAT()
                        }
                    };
                
                default:
                    throw new InvalidOperationException("Não existe essa tributação para simples nacional");
            }
        }
    }
}