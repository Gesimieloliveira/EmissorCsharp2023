using System;
using FusionCore.Papeis.Enums;

namespace FusionCore.Papeis.Anotacoes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PermissaoDetalhe : Attribute
    {
        public PermissaoDetalhe(string descricao, PermissaoGrupo grupo, PermissaoSubGrupo permissaoSubGrupo)
        {
            Descricao = descricao;
            Grupo = grupo;
            SubGrupo = permissaoSubGrupo;
        }

        public string Descricao { get; set; }
        public PermissaoGrupo Grupo { get; set; }
        public PermissaoSubGrupo SubGrupo { get; set; }
    }
}