using ACBrFramework.ECF;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;

namespace FusionPdv.Acbr.Ecf
{
    public class AcbrEcfConfigurador
    {
        private readonly ACBrECF _acbrEcf;
        private readonly EcfDt _ecfDt;

        public AcbrEcfConfigurador(ACBrECF acbrEcf, EcfDt ecfDt)
        {
            _acbrEcf = acbrEcf;
            _ecfDt = ecfDt;
        }

        public void Configurar()
        {
            _acbrEcf.Modelo = new BuscarModeloAcbr(_ecfDt).Buscar();
            _acbrEcf.Device.Porta = _ecfDt.Porta;
            _acbrEcf.Device.Baud = int.Parse(_ecfDt.Velocidade);
            _acbrEcf.Retentar = false;
           
        }
    }
}
