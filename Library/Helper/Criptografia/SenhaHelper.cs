namespace FusionLibrary.Helper.Criptografia
{
    public class SenhaHelper
    {
        public static string CriptografarSenha(string senha)
        {
            return Sha1Helper.Computar(senha);
        }

        public static bool SenhaIgual(string senha, string senhaHash)
        {
            return senhaHash == Sha1Helper.Computar(senha);
        }
    }
}