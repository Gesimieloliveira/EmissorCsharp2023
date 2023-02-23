using FusionPdv.Ecf;
using FusionPdv.Servicos.Ecf;
using FusionPdv.Servicos.Ecf.EstadoEcf;

namespace FusionPdv.Servicos.ValidacaoInicial.AbrirVenda
{
    public class ValidacaoAbrirVenda
    {
        private EstadoEcfFiscal _estadoEcf;

        public void Executar()
        {
            new EstadoEcf(_estadoEcf).ValidarEstadoDoEcf();
            new EcfVerificaSerie().Existe();
            new EcfVerificaGt().Evalido();
        }

        public void Executar(EstadoEcfFiscal estadoEcf)
        {
            _estadoEcf = estadoEcf;
            Executar();
        }
    }
}
