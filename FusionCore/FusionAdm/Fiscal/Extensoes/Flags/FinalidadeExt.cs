using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class FinalidadeExt
    {
        public static NFe.Classes.Informacoes.Identificacao.Tipos.FinalidadeNFe ToZeus(this FinalidadeEmissao finalidade)
        {
            switch (finalidade)
            {
                case FinalidadeEmissao.Devolucao:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.FinalidadeNFe.fnDevolucao;
                case FinalidadeEmissao.Ajuste:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.FinalidadeNFe.fnAjuste;
                case FinalidadeEmissao.Complementar:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.FinalidadeNFe.fnComplementar;
                case FinalidadeEmissao.Normal:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.FinalidadeNFe.fnNormal;
            }

            throw new InvalidCastException("FinalidadeEmissao para zeus é inválido");
        }
    }
}