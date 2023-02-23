using System;
using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.Relatorios
{
    public class Template : Entidade
    {
        private Template()
        {
            //nhibernate
        }

        public Template(Guid gid, byte[] dados)
        {
            Id = gid;
            Dados = dados;
        }

        public Template(byte[] dados) : this()
        {
            Id = Guid.NewGuid();
            Dados = dados;
        }

        protected override int ReferenciaUnica => Id.GetHashCode();

        public Guid Id { get; private set; }
        public byte[] Dados { get; private set; }
    }
}