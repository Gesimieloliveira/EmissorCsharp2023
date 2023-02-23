using System;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.Papeis
{
    public class PapelPermissao : EntidadeBase<Guid>, ISincronizavelAdm, IPermissaoPapel
    {
        private PapelPermissao()
        {
            //nhiberante
        }

        public PapelPermissao(Papel papel, Permissao permissao) : this()
        {
            Id = Guid.NewGuid();
            Papel = papel;
            Permissao = permissao;
            PermissaoString = permissao.ToString();
        }

        public Guid Id { get; private set; }
        public Papel Papel { get; private set; }
        public Permissao Permissao { get; private set; }
        public string PermissaoString { get; private set; }
        public string Referencia => Papel.Referencia;
        public EntidadeSincronizavel EntidadeSincronizavel => Papel.EntidadeSincronizavel;
        protected override Guid ChaveUnica => Id;
    }
}