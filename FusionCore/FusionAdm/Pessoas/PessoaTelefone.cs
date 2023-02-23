using System;
using FusionCore.Repositorio.Base;
using static System.String;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.FusionAdm.Pessoas
{
    public sealed class PessoaTelefone : Entidade
    {
        private string _descricao;
        private string _numero;
        protected override int ReferenciaUnica => Id;
        public int Id { get; private set; }
        public PessoaEntidade Contato { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Descri��o do telefone � inv�lida");
                }

                _descricao = value;
            }
        }

        public string Numero
        {
            get => _numero;
            set
            {
                if (IsNullOrWhiteSpace(value) || value.Length < 10 || value.Length > 11)
                {
                    throw new ArgumentException("N�mero do telefone � inv�lido");
                }

                _numero = value;
            }
        }

        private PessoaTelefone()
        {
            //nhibernate
        }

        public PessoaTelefone(string descricao, string numero) : this()
        {
            Descricao = descricao.ToUpper();
            Numero = numero;
        }

        public void Update(PessoaTelefone telefone)
        {
            Contato = telefone.Contato;
            Numero = telefone.Numero;
            Descricao = telefone.Descricao;
        }
    }
}