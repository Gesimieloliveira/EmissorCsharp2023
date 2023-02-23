using FusionCore.FusionPdv.Sessao.Arquivo;

namespace FusionPdv.Servicos.ArquivoAuxiliar
{
    public class AtualizarNumeroEcf : AtualizarTemplate
    {
        private readonly string _numero;

        public AtualizarNumeroEcf(string numero)
        {
            _numero = numero;
        }

        public override void ManipulaJson(dynamic json)
        {
            json.Agil4.Impressora.Numero = _numero;
        }
    }
}
