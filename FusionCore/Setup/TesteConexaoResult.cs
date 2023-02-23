namespace FusionCore.Setup
{
    public class TesteConexaoResult
    {
        public bool IsValido { get; }
        public string DetalheFalha { get; }
        public static TesteConexaoResult Sucesso => new TesteConexaoResult();

        private TesteConexaoResult()
        {
            IsValido = true;
        }

        public TesteConexaoResult(string erro)
        {
            DetalheFalha = erro;
        }
    }
}