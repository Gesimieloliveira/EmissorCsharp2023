using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class DestinoOperacaoExt
    {
        public static NFe.Classes.Informacoes.Identificacao.Tipos.DestinoOperacao ToZeus(this DestinoOperacao operacao)
        {
            switch (operacao)
            {
                case DestinoOperacao.Interna:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.DestinoOperacao.doInterna;
                case DestinoOperacao.Interestadual:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.DestinoOperacao.doInterestadual;
                case DestinoOperacao.Exterior:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.DestinoOperacao.doExterior;
            }

            throw new InvalidCastException("DestinoOperação para zeus é inválido");
        }
    }
}