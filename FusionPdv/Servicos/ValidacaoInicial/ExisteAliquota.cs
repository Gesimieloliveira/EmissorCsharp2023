using FusionPdv.Ecf;

namespace FusionPdv.Servicos.ValidacaoInicial
{
    public class ExisteAliquota
    {

        public void Executar()
        {
            var aliquotas = SessaoEcf.EcfFiscal.Aliquotas();

            if (aliquotas.Count == 0)
            {
                throw new ExceptionExisteAliquota("Não existe aliquota cadastrada na ecf.");
            }
        }
    }
}
