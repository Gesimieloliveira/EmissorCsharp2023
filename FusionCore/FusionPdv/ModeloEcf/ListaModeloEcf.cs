using System.Collections.Generic;

namespace FusionCore.FusionPdv.ModeloEcf
{
    public class ListaModeloEcf
    {

        private readonly IList<ModeloEcfTemplate> _modelosEcf = new List<ModeloEcfTemplate>();

        public ListaModeloEcf()
        {
            InicializarModelosEcf();
        }

        private void InicializarModelosEcf()
        {
            _modelosEcf.Add(new SwedaStx());
            _modelosEcf.Add(new Epson());
            _modelosEcf.Add(new BematechMp600());
            _modelosEcf.Add(new BematechMp2000());
            _modelosEcf.Add(new BematechMp2100());
            _modelosEcf.Add(new BematechMp3000());
            _modelosEcf.Add(new BematechMp4000());
            _modelosEcf.Add(new BematechMp4200());
            _modelosEcf.Add(new BematechMp6100());
            _modelosEcf.Add(new DarumaFs600());
            _modelosEcf.Add(new DarumaFs600Usb());
            _modelosEcf.Add(new DarumaFs700L());
            _modelosEcf.Add(new DarumaFs700M());
            _modelosEcf.Add(new DarumaFs700H());
            _modelosEcf.Add(new DarumaFs800I());
            _modelosEcf.Add(new DarumaMach1());
            _modelosEcf.Add(new DarumaMach2());
            _modelosEcf.Add(new DarumaMach3());
            _modelosEcf.Add(new ElginIfMfdFit1E());
            _modelosEcf.Add(new ElginX5());
            _modelosEcf.Add(new Elgin200());
            _modelosEcf.Add(new Elgin300());
            _modelosEcf.Add(new ElginKfiscal());
            _modelosEcf.Add(new ElginZpm());
        }

        public IList<ModeloEcfTemplate> ObterModelosEcf()
        {
            return (IList<ModeloEcfTemplate>) _modelosEcf;
        }
    }
}
