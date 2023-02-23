using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class FormaPagamentoExt
    {
        public static NFe.Classes.Informacoes.Identificacao.Tipos.IndicadorPagamento ToZeus(this FormaPagamento pagamento)
        {
            switch (pagamento)
            {
                case FormaPagamento.Aprazo:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.IndicadorPagamento.ipPrazo;
                case FormaPagamento.Avista:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.IndicadorPagamento.ipVista;
                case FormaPagamento.Outros:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.IndicadorPagamento.ipOutras;
            }

            throw new InvalidCastException("FormaPagamento para zeus é inválido");
        }
    }
}