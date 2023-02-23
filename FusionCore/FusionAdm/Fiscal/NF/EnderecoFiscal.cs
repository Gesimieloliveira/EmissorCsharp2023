using System;
using System.IO;
using System.Text.RegularExpressions;
using FusionCore.FusionAdm.Fiscal.Contratos.Componentes;
using FusionLibrary.Validacao;
using FusionLibrary.Validacao.Regras;
using JetBrains.Annotations;

// ReSharper disable MemberCanBePrivate.Global

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class EnderecoFiscal : IEnderecoFiscal
    {
        private string _logradouro;
        private string _numero;
        private string _complemento;
        private string _bairro;
        private string _telefone;
        private ILocalizacaoFiscal _localizacao;
        public string Cep { get; set; }

        public string Complemento
        {
            get { return _complemento; }
            set { _complemento = value ?? string.Empty; }
        }

        public string Bairro
        {
            get { return _bairro; }
            set { _bairro = value ?? string.Empty; }
        }

        private static string HidrataTelefone(string telefone)
        {
            var regex = new Regex(@"[^\d]");

            var telefo = regex.Replace(telefone ?? string.Empty, string.Empty);

            return telefo;
        }

        public string Telefone
        {
            get { return HidrataTelefone(_telefone); }
            set { _telefone = HidrataTelefone(value); }
        }

        public string Logradouro
        {
            get { return _logradouro; }
            set
            {
                if (new StringRegra(2).NaoValido(value))
                    throw new InvalidDataException("Logradouro não é válido; minimo 3 caracteres.");

                _logradouro = value;
            }
        }

        public string Numero
        {
            get { return _numero; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = "S/N";

                _numero = value;
            }
        }

        public ILocalizacaoFiscal Localizacao
        {
            get { return _localizacao; }
            set
            {
                if (value == null) throw new ArgumentException("LocalizaçãoFiscal não pode ser nulo");
                _localizacao = value;
            }
        }

        protected EnderecoFiscal()
        {
            Telefone = string.Empty;
        }

        public EnderecoFiscal(
            string cep,
            [NotNull] string logradouro,
            [NotNull] string numero,
            [NotNull] string bairro,
            [NotNull] string complemento,
            [NotNull] ILocalizacaoFiscal localizacao,
            [NotNull] string telefone) : this()
        {
            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Bairro = bairro;
            Complemento = complemento;
            Localizacao = localizacao;
            Telefone = telefone;
        }
    }
}