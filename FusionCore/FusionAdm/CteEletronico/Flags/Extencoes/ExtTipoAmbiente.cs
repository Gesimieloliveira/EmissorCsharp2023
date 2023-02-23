using System;
using FusionCore.DFe.XmlCte;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtTipoAmbiente
    {
        public static FusionTipoAmbienteCTe ToXml(this TipoAmbiente ambiente)
        {
            switch (ambiente)
            {
                case TipoAmbiente.Producao:
                    return FusionTipoAmbienteCTe.Producao;
                case TipoAmbiente.Homologacao:
                    return FusionTipoAmbienteCTe.Homologacao;
            }

            throw new ArgumentException("Os unicos tipos aceitaveis são Produção é Homologação.");
        }

        public static string ToNome(this TipoAmbiente ambiente)
        {
            switch (ambiente)
            {
                case TipoAmbiente.Producao:
                    return "Produção";
                case TipoAmbiente.Homologacao:
                    return "Homologação";
            }

            throw new ArgumentException("Os unicos tipos aceitaveis são Produção é Homologação.");
        }
    }
}