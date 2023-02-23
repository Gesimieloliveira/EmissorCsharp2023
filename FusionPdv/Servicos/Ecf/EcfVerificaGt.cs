using System.Globalization;
using FusionPdv.Ecf;
using FusionPdv.Servicos.ArquivoAuxiliar;
using FusionPdv.Servicos.ValidacaoInicial;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfVerificaGt
    {
        private readonly string _gt;

        public EcfVerificaGt()
        {
            _gt = new BuscarGt().Executar();
        }

        public void Evalido()
        {
            var gt = SessaoEcf.EcfFiscal.GrandeTotal().ToString(CultureInfo.CurrentCulture);

            var eIgual = gt.Equals(_gt);

            if (!eIgual)
            {
                throw new ExceptionGtEcf("Número de gt da ecf não correponde com o registro na base de dados.");
            }
       
        }
    }
}
