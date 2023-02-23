using System.Collections.Generic;
using FusionCore.Papeis;

namespace FusionCore.CadastroUsuario
{
    public interface IUsuario
    {
        int Id { get; }
        string Login { get; }
        bool IsAdmin { get; }
        IEnumerable<IPapel> PapeisDoUsuario { get; }
        VerificaPermissao VerificaPermissao { get; }
    }
}