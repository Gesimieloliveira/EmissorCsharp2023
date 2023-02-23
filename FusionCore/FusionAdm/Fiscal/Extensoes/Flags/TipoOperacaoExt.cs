using System;
using FusionCore.Core.Flags;
using NFe.Classes.Informacoes.Identificacao.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class TipoOperacaoExt
    {
        public static TipoNFe ToZeus(this TipoOperacao pagamento)
        {
            switch (pagamento)
            {
                case TipoOperacao.Saida:
                    return TipoNFe.tnSaida;
                case TipoOperacao.Entrada:
                    return TipoNFe.tnEntrada;
            }

            throw new InvalidCastException("TipoOperacao para zeus é inválido");
        }
    }
}