using System;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class ModalidadeDeterminacaoBcIcmsExt
    {
        public static NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcms ToZeus(this DeterminacaoBcIcms modalidade)
        {
            switch (modalidade)
            {
                case DeterminacaoBcIcms.MargemValorAgregado:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcms.DbiMargemValorAgregado;
                case DeterminacaoBcIcms.Pauta:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcms.DbiPauta;
                case DeterminacaoBcIcms.PrecoTabeladoMax:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcms.DbiPrecoTabelado;
                case DeterminacaoBcIcms.ValorOperacao:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcms.DbiValorOperacao;
            }

            throw new InvalidCastException("Conversão de tipo para Zeus Inválida");
        }
    }
}