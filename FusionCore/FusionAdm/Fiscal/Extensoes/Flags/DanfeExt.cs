using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class DanfeExt
    {
        public static NFe.Classes.Informacoes.Identificacao.Tipos.TipoImpressao ToZeus(this TipoDanfe danfe)
        {
            switch (danfe)
            {
                case TipoDanfe.NFCe:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.TipoImpressao.tiNFCe;
                case TipoDanfe.NFCeSemMensagemEletroncia:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.TipoImpressao.tiMsgEletronica;
                case TipoDanfe.NormalPaisagem:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.TipoImpressao.tiPaisagem;
                case TipoDanfe.NormalRetrato:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.TipoImpressao.tiRetrato;
                case TipoDanfe.SemGeracaoDanfe:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.TipoImpressao.tiSemGeracao;
                case TipoDanfe.Simplificado:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.TipoImpressao.tiSimplificado;
            }

            throw new InvalidCastException("TipoDanfe para zeus é inválido");
        }
    }
}