using FusionCore.FusionPdv.Sessao.Arquivo;

namespace FusionPdv.Servicos.ArquivoAuxiliar
{
    public class AtualizarMd5 : AtualizarTemplate
    {
        private readonly string _md5;

        public AtualizarMd5(string md5)
        {
            _md5 = md5;
        }


        public override void ManipulaJson(dynamic json)
        {
            json.Agil4.Md5 = _md5;
        }
    }
}
