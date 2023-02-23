using System;
using System.Collections.Generic;
using FusionCore.Papeis;

namespace FusionCore.FusionNfce.Usuario.Papeis
{
    public class PapelNfce : IPapel
    {
        private readonly IList<PapelPermissaoNfce> _permissoes;
        private readonly IList<UsuarioNfce> _usuarios;

        private PapelNfce()
        {
            _permissoes = new List<PapelPermissaoNfce>();
            _usuarios = new List<UsuarioNfce>();
        }

        public static PapelNfce CriarApartir(Papel papel)
        {
            var novo = new PapelNfce
            {
                Id = papel.Id,
                Descricao = papel.Descricao
            };

            foreach (var permissaoPapel in papel.Permissoes)
            {
                var p = (PapelPermissao) permissaoPapel;

                novo._permissoes.Add(new PapelPermissaoNfce(p.Id, novo, p.Permissao, p.PermissaoString));
            }

            foreach (var u in papel.Usuarios)
            {
                novo._usuarios.Add(new UsuarioNfce(u));
            }

            return novo;
        }

        public Guid Id { get; set; } = Guid.Empty;
        public string Descricao { get; set; }
        public IEnumerable<IPermissaoPapel> Permissoes => _permissoes;
        public IEnumerable<UsuarioNfce> Usuarios => _usuarios;
    }
}