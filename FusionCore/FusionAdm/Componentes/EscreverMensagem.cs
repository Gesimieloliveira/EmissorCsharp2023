namespace FusionCore.FusionAdm.Componentes
{
    public class EscreverMensagem
    {
        public EscreverMensagem(string assunto, string mensagem, string nomeFantasiaCustomizado)
        {
            Assunto = assunto;
            Mensagem = mensagem;
            NomeFantasiaCustomizado = nomeFantasiaCustomizado;
        }

        public string Assunto { get; }
        public string Mensagem { get; }
        public string NomeFantasiaCustomizado { get; }
    }
}