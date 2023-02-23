using OpenAC.Net.Sat;

namespace FusionNfce.AutorizacaoSatFiscal
{
    public class AssociarAssinaturaSat
    {
        private OpenSat _acbrSat;

        public AssociarAssinaturaSat(OpenSat acbrSat)
        {
            _acbrSat = acbrSat;
        }

        public SatResposta Associar(string cnpjAgil4VinculadoEmpresa, string assinatura)
        {
            return _acbrSat.AssociarAssinatura(cnpjAgil4VinculadoEmpresa, assinatura);
        }

    }
}