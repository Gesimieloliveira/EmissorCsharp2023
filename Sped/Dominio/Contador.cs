using System;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Sped.Dominio
{
    [Serializable]
    public class Contador : ViewModel
    {
        private string _nome;
        private string _cpf;
        private string _crc;
        private string _cnpjContabilidade;
        private string _cep;
        private string _logradouro;
        private string _numero;
        private string _complemento;
        private string _bairro;
        private string _telefone;
        private string _fax;
        private string _email;
        private CidadeDTO _cidade;

        public string Nome
        {
            get { return _nome; }
            set
            {
                if (value == _nome) return;
                _nome = value;
                PropriedadeAlterada();
            }
        }

        public string Cpf
        {
            get { return _cpf; }
            set
            {
                if (value == _cpf) return;
                _cpf = value;
                PropriedadeAlterada();
            }
        }

        public string Crc
        {
            get { return _crc; }
            set
            {
                if (value == _crc) return;
                _crc = value;
                PropriedadeAlterada();
            }
        }

        public string CnpjContabilidade
        {
            get { return _cnpjContabilidade; }
            set
            {
                if (value == _cnpjContabilidade) return;
                _cnpjContabilidade = value;
                PropriedadeAlterada();
            }
        }

        public string Cep
        {
            get { return _cep; }
            set
            {
                if (value == _cep) return;
                _cep = value;
                PropriedadeAlterada();
            }
        }

        public string Logradouro
        {
            get { return _logradouro; }
            set
            {
                if (value == _logradouro) return;
                _logradouro = value;
                PropriedadeAlterada();
            }
        }

        public string Numero
        {
            get { return _numero; }
            set
            {
                if (value == _numero) return;
                _numero = value;
                PropriedadeAlterada();
            }
        }

        public string Complemento
        {
            get { return _complemento; }
            set
            {
                if (value == _complemento) return;
                _complemento = value;
                PropriedadeAlterada();
            }
        }

        public string Bairro
        {
            get { return _bairro; }
            set
            {
                if (value == _bairro) return;
                _bairro = value;
                PropriedadeAlterada();
            }
        }

        public string Telefone
        {
            get { return _telefone; }
            set
            {
                if (value == _telefone) return;
                _telefone = value;
                PropriedadeAlterada();
            }
        }

        public string Fax
        {
            get { return _fax; }
            set
            {
                if (value == _fax) return;
                _fax = value;
                PropriedadeAlterada();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (value == _email) return;
                _email = value;
                PropriedadeAlterada();
            }
        }

        public CidadeDTO Cidade
        {
            get { return _cidade; }
            set
            {
                if (Equals(value, _cidade)) return;
                _cidade = value;
                PropriedadeAlterada();
            }
        }
    }
}