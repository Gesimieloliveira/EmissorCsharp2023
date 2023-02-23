using FusionCore.FusionPdv.Sessao.Arquivo;

namespace FusionPdv.Servicos.ArquivoAuxiliar
{
    public class AtualizarSerieEcf : AtualizarTemplate
    {
        private readonly string _serie;

        public AtualizarSerieEcf(string serie)
        {
            _serie = serie;
        }


        public override void ManipulaJson(dynamic json)
        {
            json.Agil4.Impressora.Serie = _serie;
        }
    }
}
