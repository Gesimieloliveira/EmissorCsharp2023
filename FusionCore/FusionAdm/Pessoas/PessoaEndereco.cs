using System;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable All

namespace FusionCore.FusionAdm.Pessoas
{
    public class PessoaEndereco : Entidade, ISincronizavelAdm
    {
        private string _logradouro;
        private string _numero;
        private string _bairro;
        private string _cep;
        private CidadeDTO _cidade;

        protected PessoaEndereco()
        {
            //nhibernate
        }

        public PessoaEndereco(
            string logradouro,
            string numero,
            string bairro,
            string cep,
            CidadeDTO cidade) : this()
        {
            Logradouro = logradouro ?? string.Empty;
            Numero = numero ?? string.Empty;
            Bairro = bairro ?? string.Empty;
            Complemento = string.Empty;
            Cep = cep;
            Cidade = cidade;
        }

        protected override int ReferenciaUnica => Id;
        public int Id { get; protected set; }
        public PessoaEntidade Residente { get; set; }

        public virtual string Logradouro
        {
            get { return _logradouro; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException("Logradouro é inválido");

                if (value.Length > 60)
                    throw new InvalidOperationException("Logradouro não pode ter mais que 60 letras");

                _logradouro = value;
            }
        }

        public virtual string Numero
        {
            get { return _numero; }
            set
            {
                if (value?.Length > 60)
                {
                    throw new InvalidOperationException("Número Endereço não pode ter mais que 60 letras");
                }

                _numero = value ?? "S/N";
            }
        }

        public virtual string Bairro
        {
            get { return _bairro; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("Bairro é inválido");
                }

                if (value.Length > 60)
                {
                    throw new InvalidOperationException("Bairro não pode ter mais que 60 letras");
                }

                _bairro = value;
            }
        }

        public virtual string Cep
        {
            get { return _cep; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("CEP é inválido");
                }

                if (value.Length > 8)
                {
                    throw new InvalidOperationException("CEP não pode ter mais que 8 letras");
                }

                _cep = value;
            }
        }

        public virtual CidadeDTO Cidade
        {
            get { return _cidade; }
            set { _cidade = value ?? throw new InvalidOperationException("Cidade Endereço é inválida"); }
        }

        public string Complemento { get; set; }
        public string Referencia => Residente.Referencia;
        public EntidadeSincronizavel EntidadeSincronizavel => Residente.EntidadeSincronizavel;
        public bool Principal { get; set; }
        public bool Entrega { get; set; }
        public bool Outros { get; set; }

        public override string ToString()
        {
            return $"{Logradouro}, {Numero}, {Bairro} / {Cep} / {Cidade}";
        }

        public void Atualizar(PessoaEndereco novo)
        {
            Bairro = novo.Bairro;
            Cep = novo.Cep;
            Cidade = novo.Cidade;
            Complemento = novo.Complemento;
            Logradouro = novo.Logradouro;
            Numero = novo.Numero;
            Residente = novo.Residente;
            Principal = novo.Principal;
            Entrega = novo.Entrega;
            Outros = novo.Outros;
        }
    }
}