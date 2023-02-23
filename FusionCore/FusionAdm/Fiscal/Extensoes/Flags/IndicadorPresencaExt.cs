using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class IndicadorPresencaExt
    {
        public static NFe.Classes.Informacoes.Identificacao.Tipos.PresencaComprador ToZeus(this IndicadorComprador indicador)
        {
            switch (indicador)
            {
                case IndicadorComprador.EntregaDomicilio:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.PresencaComprador.pcEntregaDomicilio;
                case IndicadorComprador.Internet:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.PresencaComprador.pcInternet;
                case IndicadorComprador.NaoSeAplica:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.PresencaComprador.pcNao;
                case IndicadorComprador.Outros:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.PresencaComprador.pcOutros;
                case IndicadorComprador.Presencial:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.PresencaComprador.pcPresencial;
                case IndicadorComprador.TeleAtendimento:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.PresencaComprador.pcTeleatendimento;
            }

            throw new InvalidCastException("IndicadorPresenca para zeus é inválido");
        }
    }
}