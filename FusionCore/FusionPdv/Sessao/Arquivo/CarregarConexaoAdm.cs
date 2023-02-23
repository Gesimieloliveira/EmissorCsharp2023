using System;

namespace FusionCore.FusionPdv.Sessao.Arquivo
{
    public class CarregarConexaoAdm : BuscarTemplate
    {
        public CarregarConexaoAdm(string caminhoArquivoPdv)
        {
            NaoCriptografar = true;
            CaminhoArquivo = caminhoArquivoPdv;
        }

        public override string RetornoDado(dynamic json)
        {
            return json.Agil4.ConexaoAdm.ToString();
        }

        public override void ExceptionValidacaoVazio(string mensagem)
        {
            throw new InvalidOperationException("Arquivo não contém dados.");
        }
    }
}
