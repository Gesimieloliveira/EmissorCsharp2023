namespace FusionCore.Setup
{
    public class RestoreConfig
    {
        public string ArquivoBak { get; }
        public string BancoDados { get; }

        public RestoreConfig(string arquivoBak, string bancoDados)
        {
            BancoDados = bancoDados;
            ArquivoBak = arquivoBak;
        }
    }
}