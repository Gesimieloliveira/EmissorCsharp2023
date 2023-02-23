namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class TokenNfce
    {
        public TokenNfce(int token, string csc)
        {
            Token = token;
            Csc = csc;
        }

        public int Token { get; }
        public string Csc { get; }

        public string ObterToken()
        {
            return Token.ToString("D6");
        }
    }
}