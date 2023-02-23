using System.Text.RegularExpressions;

namespace FusionLibrary.Validacao.Regras
{
    public class ArquivoExtensaoZip : IRegra
    {
        public bool AplicaRegra(object objeto)
        {
            var caminhoCompleto = (string) objeto;

            if (string.IsNullOrWhiteSpace(caminhoCompleto))
                return false;

            var regex = new Regex(@"^.+(.zip)$", RegexOptions.IgnoreCase);

            return regex.IsMatch(caminhoCompleto);
        }
    }
}