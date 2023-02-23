using System;
using FusionCore.DFe.XmlCte;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtModal
    {
        public static FusionModalCTe ToXml(this Modal modal)
        {
            switch (modal)
            {
                case Modal.Rodoviario:
                    return FusionModalCTe.Rodoviario;
            }

            throw new ArgumentException("Modal inválido");
        }
    }
}