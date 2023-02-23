using FusionCore.FusionPdv.Sessao.Arquivo;

namespace FusionPdv.Servicos.ArquivoAuxiliar
{
    public class AtualizarGt : AtualizarTemplate
    {
        private readonly string _gt;

        public AtualizarGt(string gt)
        {
            _gt = gt;
        }

        public override void ManipulaJson(dynamic json)
        {
            json.Agil4.Impressora.Gt = _gt;
        }
    }
}
