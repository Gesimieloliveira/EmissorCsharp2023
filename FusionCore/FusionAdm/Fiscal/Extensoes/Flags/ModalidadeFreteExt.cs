using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class ModalidadeFreteExt
    {
        public static NFe.Classes.Informacoes.Transporte.ModalidadeFrete ToZeus(this ModalidadeFrete frete)
        {
            switch (frete)
            {
                case ModalidadeFrete.PorContaDestintario:
                    return NFe.Classes.Informacoes.Transporte.ModalidadeFrete.mfContaDestinatario;
                case ModalidadeFrete.PorContaEmitente:
                    return NFe.Classes.Informacoes.Transporte.ModalidadeFrete.mfContaEmitenteOumfContaRemetente;
                case ModalidadeFrete.PorContaTerceiros:
                    return NFe.Classes.Informacoes.Transporte.ModalidadeFrete.mfContaTerceiros;
                case ModalidadeFrete.SemFrete:
                    return NFe.Classes.Informacoes.Transporte.ModalidadeFrete.mfSemFrete;
            }

            throw new InvalidCastException("Conversão de tipo para Zeus Inválida");
        }
    }
}