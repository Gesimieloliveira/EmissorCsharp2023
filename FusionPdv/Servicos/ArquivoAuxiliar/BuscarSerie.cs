using FusionCore.FusionPdv.Sessao.Arquivo;
using FusionPdv.Servicos.ValidacaoInicial;

namespace FusionPdv.Servicos.ArquivoAuxiliar
{
    public class BuscarSerie : BuscarTemplate
    {
        public override string RetornoDado(dynamic json)
        {
            return json.Agil4.Impressora.Serie.ToString();
        }

        public override void ExceptionValidacaoVazio(string mensagem)
        {
            throw new ExceptionSerieEcf(mensagem);    
        }
    }
}
