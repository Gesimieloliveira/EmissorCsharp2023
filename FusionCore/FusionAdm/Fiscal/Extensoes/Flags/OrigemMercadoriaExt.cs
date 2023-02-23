using System;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class OrigemMercadoriaExt
    {
        public static NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.OrigemMercadoria ToZeus(this OrigemMercadoria origem)
        {
            switch (origem)
            {
                case OrigemMercadoria.EstrangeiraImportacaoDireta:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.OrigemMercadoria.OmEstrangeiraImportacaoDireta;
                case OrigemMercadoria.EstrangeiraImportacaoDiretaSemSimilar:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.OrigemMercadoria.OmEstrangeiraImportacaoDiretaSemSimilar;
                case OrigemMercadoria.EstrangeiraInterna:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.OrigemMercadoria.OmEstrangeiraAdquiridaBrasil;
                case OrigemMercadoria.EstrangeiraInternaSemSimilar:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.OrigemMercadoria.OmEstrangeiraAdquiridaBrasilSemSimilar;
                case OrigemMercadoria.Nacional:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.OrigemMercadoria.OmNacional;
                case OrigemMercadoria.NacionalConformidadeBasicas:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.OrigemMercadoria.OmNacionalProcessosBasicos;
                case OrigemMercadoria.NacionalImportacaoInferior40:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.OrigemMercadoria.OmNacionalConteudoImportacaoInferiorIgual40;
                case OrigemMercadoria.NacionalImportacaoSuperior40:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.OrigemMercadoria.OmNacionalConteudoImportacaoSuperior40;
                case OrigemMercadoria.NacionalImportacaoSuperior70:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.OrigemMercadoria.OmNacionalConteudoImportacaoSuperior70;
            }

            throw new InvalidCastException("Conversão de tipo para Zeus Inválida");
        }

        public static MotorTributarioNet.Flags.OrigemMercadoria ToMotorTributario(this OrigemMercadoria origem)
        {
            switch (origem)
            {
                case OrigemMercadoria.Nacional:
                    return MotorTributarioNet.Flags.OrigemMercadoria.Nacional;
                case OrigemMercadoria.EstrangeiraImportacaoDireta:
                    return MotorTributarioNet.Flags.OrigemMercadoria.EstrangeiraImportacaoDireta;
                case OrigemMercadoria.EstrangeiraInterna:
                    return MotorTributarioNet.Flags.OrigemMercadoria.EstrangeiraInterna;
                case OrigemMercadoria.NacionalImportacaoSuperior40:
                    return MotorTributarioNet.Flags.OrigemMercadoria.NacionalImportacaoSuperior40;
                case OrigemMercadoria.NacionalConformidadeBasicas:
                    return MotorTributarioNet.Flags.OrigemMercadoria.NacionalConformidadeBasicas;
                case OrigemMercadoria.NacionalImportacaoInferior40:
                    return MotorTributarioNet.Flags.OrigemMercadoria.NacionalImportacaoInferior40;
                case OrigemMercadoria.EstrangeiraImportacaoDiretaSemSimilar:
                    return MotorTributarioNet.Flags.OrigemMercadoria.EstrangeiraImportacaoDiretaSemSimilar;
                case OrigemMercadoria.EstrangeiraInternaSemSimilar:
                    return MotorTributarioNet.Flags.OrigemMercadoria.EstrangeiraInternaSemSimilar;
                case OrigemMercadoria.NacionalImportacaoSuperior70:
                    return MotorTributarioNet.Flags.OrigemMercadoria.NacionalImportacaoSuperior70;
                default:
                    throw new ArgumentOutOfRangeException(nameof(origem), origem, null);
            }
        }
    }
}