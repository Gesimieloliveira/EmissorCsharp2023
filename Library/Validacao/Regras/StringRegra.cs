namespace FusionLibrary.Validacao.Regras
{
    public class StringRegra : IRegra
    {
        private readonly int _tamanhoMinimio;
        private readonly int _tamanhoMaximo;

        public StringRegra(int tamanhoMinimio, int tamanhoMaximo = int.MaxValue)
        {
            _tamanhoMinimio = tamanhoMinimio;
            _tamanhoMaximo = tamanhoMaximo;
        }

        public bool AplicaRegra(object objeto)
        {
            var length = ((string) objeto)?.Length;
            return !(length <= _tamanhoMinimio || length >= _tamanhoMaximo);
        }
    }
}