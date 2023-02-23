using System.Windows.Forms;

namespace FusionCore.FusionPdv.Sessao.Arquivo
{
    public class BuscarConexaoPdv : BuscarTemplate
    {
        public BuscarConexaoPdv()
        {
            CaminhoArquivo = Application.StartupPath + "\\Conexoes.agil4";
            NaoCriptografar = true;
        }

        public override string RetornoDado(dynamic json)
        {
            return json.ToString();
        }

        public override void ExceptionValidacaoVazio(string mensagem)
        {
            throw new ExceptionConexaoPdv(mensagem);    
        }
    }
}
