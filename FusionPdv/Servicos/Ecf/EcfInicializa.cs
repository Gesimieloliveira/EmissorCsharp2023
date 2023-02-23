using System;
using ACBrFramework.ECF;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionPdv.Acbr;
using FusionPdv.Acbr.Ecf;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfInicializa
    {
        private readonly ACBrECF _acbrEcf;
        private readonly AcbrEcfConfigurador _acbrEcfConfigurador;


        public EcfInicializa(EcfDt ecfDt)
        {
            _acbrEcf = AcbrFactory.ObterAcbrEcf();
            _acbrEcfConfigurador = new AcbrEcfConfigurador(_acbrEcf, ecfDt);
        }

        public EcfInicializa()
        {
            _acbrEcf = AcbrFactory.ObterAcbrEcf();
            var ecfDt = new ObterEcfEmUso().Buscar();
            _acbrEcfConfigurador = new AcbrEcfConfigurador(_acbrEcf, ecfDt);
        }

        public void Iniciar()
        {
            _acbrEcfConfigurador.Configurar();
            try
            {
                _acbrEcf.Ativar();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
            
        }
    }
}
