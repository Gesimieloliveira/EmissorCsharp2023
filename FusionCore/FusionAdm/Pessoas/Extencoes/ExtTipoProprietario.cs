using System;
using FusionCore.FusionAdm.CteEletronico.Flags;

namespace FusionCore.FusionAdm.Pessoas.Extencoes
{
    public static class ExtTipoProprietario
    {
        public static string ToCte(this TipoPropriedadeVeiculo proprietario)
        {
            switch (proprietario)
            {
                case TipoPropriedadeVeiculo.Proprio:
                return "P";
                case TipoPropriedadeVeiculo.Terceiro:
                return "T";
            }

            throw new InvalidOperationException("Proprietario inválido");
        }
    }
}