using System;
using System.Collections.Generic;
using FusionCore.CadastroUsuario;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Papeis;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class UsuarioDTO : EntidadeBase<int>, ISincronizavelAdm, IUsuario, IEntidade
    {
        private readonly IList<Papel> _papeis = new List<Papel>();

        public UsuarioDTO()
        {
            AlteradoEm = DateTime.Now;
            CadastradoEm = DateTime.Now;
            Tema = string.Empty;
            VerificaPermissao = new VerificaPermissao(this);
        }

        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public string Login { get; set; }
        public IEnumerable<IPapel> PapeisDoUsuario => _papeis;
        public string Senha { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public DateTime? CadastradoEm { get; private set; }
        public string Tema { get; set; }
        public string Referencia => Id.ToString();
        public bool IsAdmin => Id == 1;
        public bool ApenasFaturamento { get; set; }
        public EntidadeSincronizavel EntidadeSincronizavel { get; set; } = EntidadeSincronizavel.Usuario;

        public override string ToString()
        {
            return Login;
        }

        public VerificaPermissao VerificaPermissao { get; }
    }
}