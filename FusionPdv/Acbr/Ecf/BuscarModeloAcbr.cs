using System;
using System.Linq;
using ACBrFramework.ECF;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;

namespace FusionPdv.Acbr.Ecf
{
    public class BuscarModeloAcbr
    {
        private readonly EcfDt _ecfDt;
        private readonly string _modelo;

        public BuscarModeloAcbr(EcfDt ecfDt)
        {
            _ecfDt = ecfDt;
        }

        public BuscarModeloAcbr(string modelo)
        {
            _modelo = modelo;
        }


        public ModeloECF Buscar()
        {
            return Enum.GetValues(typeof (ModeloECF))
                .Cast<ModeloECF>().First(modelo => modelo.ToString().Equals(_modelo ?? _ecfDt.ModeloAcbr));
        }
    }
}
