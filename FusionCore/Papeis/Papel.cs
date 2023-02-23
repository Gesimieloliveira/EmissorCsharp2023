using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.Papeis
{
    public class Papel : EntidadeBase<Guid>, ISincronizavelAdm, IPapel
    {
        private readonly IList<PapelPermissao> _permissoes;
        private readonly IList<UsuarioDTO> _usuarios;

        public Papel()
        {
            _permissoes = new List<PapelPermissao>();
            _usuarios = new List<UsuarioDTO>();
        }

        public Guid Id { get; set; } = Guid.Empty;
        public string Descricao { get; set; }
        public IEnumerable<IPermissaoPapel> Permissoes => _permissoes;
        public IEnumerable<UsuarioDTO> Usuarios => _usuarios;
        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.Papel;
        protected override Guid ChaveUnica => Id;

        public override string ToString()
        {
            return $"{Descricao}";
        }

        public void AddUsuario(UsuarioDTO usuario)
        {
            _usuarios.Add(usuario);
        }

        public void RemoverUsuario(UsuarioDTO usuario)
        {
            _usuarios.Remove(usuario);
        }

        public void AddPermissao(PapelPermissao permissao)
        {
            _permissoes.Add(permissao);
        }

        public void RemoverPermissao(Permissao permissao)
        {
            for (var i = 0; i < _permissoes.Count; i++)
            {
                if (_permissoes[i].Permissao != permissao)
                {
                    continue;
                }

                _permissoes.RemoveAt(i);
                break;
            }
        }

        public void RemoverTodasAsPermissoes()
        {
            _permissoes.Clear();
        }

        public bool Existe(Permissao permissao)
        {
            return _permissoes.Any(i => i.Permissao == permissao);
        }
    }
}