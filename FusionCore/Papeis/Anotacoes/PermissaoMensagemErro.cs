using System;

namespace FusionCore.Papeis.Anotacoes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PermissaoMensagemErro : Attribute
    {
        public string MensagemErroSemPermissao { get; }

        public PermissaoMensagemErro(string mensagemErroSemPermissao)
        {
            MensagemErroSemPermissao = mensagemErroSemPermissao;
        }
    }
}