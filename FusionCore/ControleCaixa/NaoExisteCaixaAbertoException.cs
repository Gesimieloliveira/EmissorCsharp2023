namespace FusionCore.ControleCaixa
{
    public class NaoExisteCaixaAbertoException : ControleCaixaException
    {
        public NaoExisteCaixaAbertoException(string message) : base(message)
        {
        }
    }
}