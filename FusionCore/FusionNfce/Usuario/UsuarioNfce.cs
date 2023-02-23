using System.Collections.Generic;
using System.Linq;
using FusionCore.CadastroUsuario;
using FusionCore.FusionNfce.Usuario.Papeis;
using FusionCore.Papeis;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Usuario
{
    public class UsuarioNfce : EntidadeBase<int>, IUsuario
    {
        private readonly IList<PapelNfce> _papeis;

        // ReSharper disable once EmptyConstructor
        public UsuarioNfce()
        {
            _papeis = new List<PapelNfce>();

            VerificaPermissao = new VerificaPermissao(this);
        }

        public UsuarioNfce(UsuarioDTO usuarioDTO) : this()
        {
            Id = usuarioDTO.Id;
            Login = usuarioDTO.Login;
            Senha = usuarioDTO.Senha;
        }

        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Tema { get; set; }
        public bool IsAdmin => Id == 1;
        public VerificaPermissao VerificaPermissao { get; }

        public IEnumerable<IPapel> PapeisDoUsuario => _papeis;

        public bool IsTemPermissao(Permissao permissao)
        {
            if (IsAdmin) return true;

            var permite = false;

            foreach (var papel in _papeis)
            {
                if (papel.Permissoes.Any(x => x.Permissao == permissao))
                {
                    permite = true;
                }
            }

            return permite;
        }

        public override string ToString()
        {
            return $"{Login}";
        }
    }
}