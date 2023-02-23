using System;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtImpostoCsosnZeus
    {
        public static ICMS ToZeus(this INfceImpostoIcms icms)
        {
            return new ICMS
            {
                TipoICMS = GetImposto(icms)
            };
        }

        private static ICMSBasico GetImposto(INfceImpostoIcms icms)
        {
            switch (icms.CST.Id)
            {
                case "102":
                case "103":
                case "300":
                case "400":
                    return GetCsosn102(icms);

                case "500": return GetCsosn500(icms);
                case "60": return GetCst60(icms);
                case "30": return GetCst30(icms);
                case "00": return GetCst00(icms);
                case "90": return GetCst90(icms);
                case "20": return GetCst20(icms);

                case "41":
                case "40":
                    return GetCst40(icms);
                default:
                    throw new InvalidOperationException($"CSOSN {icms.CST} não é compatível com nfc-e");
            }
        }

        private static ICMSBasico GetCst90(INfceImpostoIcms icms)
        {
            return new ICMS90
            {
                orig = icms.OrigemMercadoria.ToZeus(),
                CST = icms.CST.ToCstZeus(),
                pICMS = icms.AliquotaIcms,
                vICMS = icms.ValorIcms,
                vBC = icms.BcIcms,
                pRedBC = icms.ReducaoBcIcms,
                modBC = DeterminacaoBaseIcms.DbiValorOperacao
            };
        }

        private static ICMSBasico GetCst00(INfceImpostoIcms icms)
        {
            return new ICMS00
            {
                orig = icms.OrigemMercadoria.ToZeus(),
                CST = icms.CST.ToCstZeus(),
                pICMS = icms.AliquotaIcms,
                vICMS = icms.ValorIcms,
                vBC = icms.BcIcms,
                modBC = DeterminacaoBaseIcms.DbiValorOperacao
            };
        }

        private static ICMSBasico GetCst20(INfceImpostoIcms icms)
        {
            return new ICMS20
            {
                orig = icms.OrigemMercadoria.ToZeus(),
                CST = icms.CST.ToCstZeus(),
                pICMS = icms.AliquotaIcms,
                vICMS = icms.ValorIcms,
                vBC = icms.BcIcms,
                pRedBC = icms.ReducaoBcIcms,
                modBC = DeterminacaoBaseIcms.DbiValorOperacao
            };
        }

        private static ICMSBasico GetCst40(INfceImpostoIcms icms)
        {
            return new ICMS40
            {
                orig = icms.OrigemMercadoria.ToZeus(),
                CST = icms.CST.ToCstZeus()
            };
        }

        private static ICMSBasico GetCst30(INfceImpostoIcms icms)
        {
            return new ICMS30
            {
                orig = icms.OrigemMercadoria.ToZeus(),
                CST = icms.CST.ToCstZeus()
            };
        }

        private static ICMSBasico GetCst60(INfceImpostoIcms i)
        {
            var cst = new ICMS60
            {
                orig = i.OrigemMercadoria.ToZeus(),
                CST = i.CST.ToCstZeus()
            };

            if (i.Item.Nfce.IndicadorConsumidorFinal != IndicadorOperacaoFinal.ConsumidorFinal)
            {
                cst.vBCSTRet = 0.00M;
                cst.pST = 0.00M;
                cst.vICMSSubstituto = 0.00M;
                cst.vICMSSTRet = 0.00M;
            }

            return cst;
        }

        private static ICMSBasico GetCsosn102(INfceImpostoIcms icms)
        {
            return new ICMSSN102
            {
                orig = icms.OrigemMercadoria.ToZeus(),
                CSOSN = icms.CST.ToCsosnZeus()
            };
        }

        private static ICMSBasico GetCsosn500(INfceImpostoIcms i)
        {
            var icms = new ICMSSN500
            {
                CSOSN = i.CST.ToCsosnZeus(),
                orig = i.OrigemMercadoria.ToZeus()
            };

            if (i.Item.Nfce.IndicadorConsumidorFinal != IndicadorOperacaoFinal.ConsumidorFinal)
            {
                icms.vBCSTRet = 0.00M;
                icms.pST = 0.00M;
                icms.vICMSSubstituto = 0.00M;
                icms.vICMSSTRet = 0.00M;
            }

            return icms;
        }
    }
}