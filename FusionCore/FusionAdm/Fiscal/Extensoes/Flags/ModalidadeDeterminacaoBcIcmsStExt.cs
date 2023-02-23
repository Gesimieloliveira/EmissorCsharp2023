using System;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class ModalidadeDeterminacaoBcIcmsStExt
    {
        public static NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcmsSt ToZeus(this DeterminacaoBcIcmsSt modalidade)
        {
            switch (modalidade)
            {
                case DeterminacaoBcIcmsSt.ListaNegativa:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcmsSt.DbisListaNegativa;
                case DeterminacaoBcIcmsSt.ListaNeutra:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcmsSt.DbisListaNeutra;
                case DeterminacaoBcIcmsSt.ListaPositiva:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcmsSt.DbisListaPositiva;
                case DeterminacaoBcIcmsSt.MargemValorAgregado:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcmsSt.DbisMargemValorAgregado;
                case DeterminacaoBcIcmsSt.Pauta:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcmsSt.DbisPauta;
                case DeterminacaoBcIcmsSt.PrecoTabeladoOuMaximoSugerido:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.DeterminacaoBaseIcmsSt.DbisPrecoTabelado;
            }

            throw new InvalidCastException("Conversão de tipo para Zeus Inválida");
        }
    }
}