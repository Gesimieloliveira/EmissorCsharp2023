using FusionCore.FusionNfce.Uf;
using OpenAC.Net.Sat;

namespace FusionNfce.AutorizacaoSatFiscal
{
    public class AtivarSat
    {
        private readonly OpenSat _acbrSat;

        public AtivarSat(OpenSat acbrSat)
        {
            _acbrSat = acbrSat;
        }

        public void Ativar()
        {
            if(!_acbrSat.Ativo)
                _acbrSat.Ativar();
        }

        public SatResposta Ativar(string cnpj, UfNfce ufNfce)
        {
            Ativar();

            var reposta = _acbrSat.AtivarSAT(1, cnpj, ufNfce.CodigoIbge);

            return reposta;
        }
    }
}