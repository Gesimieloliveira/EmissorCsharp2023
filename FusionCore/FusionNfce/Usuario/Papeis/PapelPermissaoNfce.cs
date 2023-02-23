using System;
using FusionCore.Papeis;
using FusionCore.Papeis.Enums;

namespace FusionCore.FusionNfce.Usuario.Papeis
{
    public class PapelPermissaoNfce : IPermissaoPapel
    {
        private PapelPermissaoNfce()
        {
            //nhiberante
        }

        public PapelPermissaoNfce(
            Guid id, 
            PapelNfce papel, 
            Permissao permissao, 
            string permisaoString) : this()
        {
            Id = id;
            Papel = papel;
            Permissao = permissao;
            PermissaoString = permisaoString;
        }

        public Guid Id { get; set; }
        public PapelNfce Papel { get; set; }
        public Permissao Permissao { get; set; }
        public string PermissaoString { get; set; }
    }
}