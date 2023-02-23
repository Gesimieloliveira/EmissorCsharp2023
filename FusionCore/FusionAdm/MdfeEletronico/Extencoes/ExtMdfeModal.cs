using System;
using FusionCore.FusionAdm.CteEletronico.Flags;
using MDFe.Classes.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico.Extencoes
{
    public static class ExtMdfeModal
    {
        public static MDFeModal ToZeusMdfe(this Modal modal)
        {
            switch (modal)
            {
                case Modal.Rodoviario:
                    return MDFeModal.Rodoviario;
            }

            throw new InvalidOperationException("Modal inválido");
        }
    }
}