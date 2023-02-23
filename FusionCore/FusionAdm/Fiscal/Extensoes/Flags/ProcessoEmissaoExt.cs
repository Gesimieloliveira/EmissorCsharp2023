using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class ProcessoEmissaoExt
    {
        public static NFe.Classes.Informacoes.Identificacao.Tipos.ProcessoEmissao ToZeus(this ProcessoEmissao emissao)
        {
            switch (emissao)
            {
                case ProcessoEmissao.NFeAplicativoContribuinte:
                    return NFe.Classes.Informacoes.Identificacao.Tipos.ProcessoEmissao.peAplicativoContribuinte;
            }

            throw new InvalidCastException("ProcessoEmissao para zeus é inválido");
        }
    }
}