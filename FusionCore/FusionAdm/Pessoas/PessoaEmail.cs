using FusionCore.FusionAdm.Componentes;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.Pessoas
{
    public sealed class PessoaEmail : Entidade, ISincronizavelAdm
    {
        public int Id { get; private set; }
        public PessoaEntidade Pessoa { get; set; }
        protected override int ReferenciaUnica => Id;
        public Email Email { get; set; }

        private PessoaEmail()
        {
            //nhibernate
        }

        public PessoaEmail(Email email) : this()
        {
            Email = email;
        }

        public string Referencia => Pessoa.Referencia;
        public EntidadeSincronizavel EntidadeSincronizavel => Pessoa.EntidadeSincronizavel;
    }
}