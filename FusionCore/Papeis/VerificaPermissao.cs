using System;
using System.Linq;
using FusionCore.CadastroUsuario;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Anotacoes;
using FusionCore.Papeis.Enums;
using FusionCore.Permissoes;

namespace FusionCore.Papeis
{
    public class VerificaPermissao
    {
        private readonly IUsuario _usuario;

        public VerificaPermissao(IUsuario usuarioVerificar)
        {
            _usuario = usuarioVerificar;
        }

        public bool IsTemPermissao(Permissao permissao)
        {
            if (_usuario.IsAdmin) return true;

            var permite = false;

            foreach (var papel in _usuario.PapeisDoUsuario)
            {
                if (papel.Permissoes.Any(x => x.Permissao == permissao))
                {
                    permite = true;
                }
            }

            return permite;
        }

        public bool IsQualquerPermissao(params Permissao[] permissaos)
        {
            foreach (var permissao in permissaos)
            {
                if (IsTemPermissao(permissao)) return true;
            }

            return false;
        }

        public void IsTemPermissaoThrow(Permissao permissao)
        {
            if (IsTemPermissao(permissao))
            {
                return;
            }

            var mensagemErro = ObterMensagemDeErro(permissao);

            if (mensagemErro.IsNullOrEmpty())
            {
                throw new PermissaoException($"Oops... permissão insuficiente ({permissao})!");
            }
            
            throw new PermissaoException(mensagemErro);
        }

        private string ObterMensagemDeErro(Permissao permissao)
        {
            var fi = permissao.GetType().GetField(permissao.ToString());

            var attributes =
                (PermissaoMensagemErro[])fi.GetCustomAttributes(
                    typeof(PermissaoMensagemErro),
                    false);

            if (!attributes.Any()) return string.Empty;

            var mensagemErro = string.Format(
                attributes[0]?.MensagemErroSemPermissao
                ?? throw new InvalidOperationException("Erro ao obter mensagem de erro da permissão")
                , _usuario.Login
            );

            return mensagemErro;
        }
    }
}