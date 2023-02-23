using System;
using FusionCore.Repositorio.Base;

// ReSharper disable UnusedMember.Local

namespace FusionCore.Relatorios
{
    public class RelatorioProprio : Entidade
    {
        private RelatorioProprio()
        {
            //nhibernate apenas
        }

        public RelatorioProprio(string descricao, string grupo, Template template)
        {
            Id = Guid.NewGuid();
            Descricao = descricao;
            Grupo = grupo;
            Template = template;
        }

        protected override int ReferenciaUnica => Id.GetHashCode();

        public Guid Id { get; private set; }
        public string Descricao { get; private set; }
        public string Grupo { get; private set; }
        public Template Template { get; private set; }

        public void AlterarInformacoes(string descricao, string grupo)
        {
            Descricao = descricao;
            Grupo = grupo;
        }
    }
}