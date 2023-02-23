using System;
using FusionCore.FusionAdm.CteEletronico.Flags;
using MDFe.Classes.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico.Extencoes
{
    public static class ExtMdfeTipoCarroceria
    {
        public static MDFeTpCar ToZeusMdfe(this TipoCarroceria tipoCarroceria)
        {
            switch (tipoCarroceria)
            {
                case TipoCarroceria.NaoAplicavel:
                    return MDFeTpCar.NaoAplicavel;

                case TipoCarroceria.Abera:
                    return MDFeTpCar.Aberta;

                case TipoCarroceria.FechadaBau:
                    return MDFeTpCar.FechadaBau;

                case TipoCarroceria.Graneleira:
                    return MDFeTpCar.Granelera;

                case TipoCarroceria.PortaContainer:
                    return MDFeTpCar.PortaContainer;

                case TipoCarroceria.Sider:
                    return MDFeTpCar.Sider;
            }

            throw new InvalidOperationException("Tipo Carroceria inválida");
        }
    }
}