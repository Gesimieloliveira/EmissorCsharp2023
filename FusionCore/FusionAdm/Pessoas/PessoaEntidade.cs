using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Helpers.Hidratacao;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.IdGenerator;
using static System.String;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionAdm.Pessoas
{
    public sealed class PessoaEntidade : Entidade, ISincronizavelAdm, IEntidadeIdentity
    {
        public static class Expressions
        {
            public static readonly Expression<Func<PessoaEntidade, object>> Telefone = x => x.Telefones;
            public static readonly Expression<Func<PessoaEntidade, object>> Email = x => x.Emails;
        }

        private IList<PessoaTelefone> _telefones = new List<PessoaTelefone>();
        private IList<PessoaEndereco> _enderecos = new List<PessoaEndereco>();
        private IList<PessoaEmail> _emails = new List<PessoaEmail>();
        private string _nome;
        private string _inscricaoEstadual = Empty;
        private string _inscricaoMunicipal = Empty;
        private string _nomeFantasia = Empty;
        private string _nomeMae = Empty;
        private string _nomePai = Empty;
        private string _documentoExterior = Empty;

        private PessoaEntidade()
        {
            //nhibernate
        }

        public PessoaEntidade(string nome) : this()
        {
            Nome = nome;
        }

        public int Id { get; set; }

        public PessoaTipo Tipo { get; protected set; }
        protected override int ReferenciaUnica => Id;

        public string Nome
        {
            get => _nome;
            set
            {
                if (IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nome da pessoa é inválido");

                _nome = value;
            }
        }

        public string NomeFantasia
        {
            get => _nomeFantasia;
            set
            {
                if (Tipo == PessoaTipo.Juridica && IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Nome Fantasia não é valido para tipo de pessoa Jurídica");
                }

                _nomeFantasia = value ?? Empty;
            }
        }

        public Cpf Cpf { get; protected set; } = Cpf.Vazio;
        public Cnpj Cnpj { get; protected set; } = Cnpj.Vazio;
        public DocumentoRg Rg { get; protected set; } = DocumentoRg.Vazio;

        public string InscricaoMunicipal
        {
            get => _inscricaoMunicipal;
            protected set => _inscricaoMunicipal = value;
        }

        public string InscricaoEstadual
        {
            get => _inscricaoEstadual;
            protected set => _inscricaoEstadual = value;
        }

        public IndicadorIE IndicadorIEDestinatario { get; set; }

        public string DocumentoExterior
        {
            get => _documentoExterior;
            protected set => _documentoExterior = value ?? Empty;
        }

        public DateTime? NascidoEm { get; protected set; }
        public PessoaSexo Sexo { get; protected set; } = PessoaSexo.SexoNaoInformado;

        public string NomeMae
        {
            get => _nomeMae;
            set => _nomeMae = value ?? Empty;
        }

        public string NomePai
        {
            get => _nomePai;
            set => _nomePai = value ?? Empty;
        }

        public IReadOnlyList<PessoaTelefone> Telefones => _telefones.ToList();
        public IReadOnlyList<PessoaEndereco> Enderecos => _enderecos.ToList();
        public IReadOnlyList<PessoaEmail> Emails => _emails.ToList();
        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.Pessoa;
        public DateTime AlteradoEm { get; set; } = DateTime.Now;
        public Cliente Cliente { get; set; }
        public Transportadora Transportadora { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public Vendedor Vendedor { get; set; }
        public bool Ativo { get; set; } = true;

        public void ComoPessoaFisica(Cpf cpf,
            DocumentoRg rg,
            PessoaSexo sexo,
            IndicadorIE indicador,
            string inscricaoEstadual = null,
            DateTime? nascidoEm = null)
        {
            Tipo = PessoaTipo.Fisica;
            Cpf = cpf ?? Cpf.Vazio;
            Rg = rg ?? DocumentoRg.Vazio;
            Sexo = sexo;
            NascidoEm = nascidoEm;
            InscricaoEstadual = inscricaoEstadual.TrimOrEmpty();
            IndicadorIEDestinatario = indicador;

            ColocarPessoaJuridicaEmBranco();
            ColocarPessoaExteriorEmBranco();
        }

        private void ColocarPessoaJuridicaEmBranco()
        {
            Cnpj = Cnpj.Vazio;
            NomeFantasia = Empty;
        }

        private void ColocarPessoaExteriorEmBranco()
        {
            DocumentoExterior = Empty;
        }

        public void ComoPessoaJuridica(string nomeFantasia, Cnpj cnpj, IndicadorIE indicador, string ie = null, string im = null)
        {
            Tipo = PessoaTipo.Juridica;
            NomeFantasia = nomeFantasia?.TrimOrEmpty() ?? Empty;
            Cnpj = cnpj ?? Cnpj.Vazio;
            InscricaoEstadual = ie?.TrimOrEmpty() ?? Empty;
            IndicadorIEDestinatario = indicador;
            InscricaoMunicipal = im?.TrimOrEmpty() ?? Empty;

            ColocarPessoaExteriorEmBranco();
            ColocarPessoaFisicaEmBranco();
        }

        private void ColocarPessoaFisicaEmBranco()
        {
            Cpf = Cpf.Vazio;
            Rg = DocumentoRg.Vazio;
            Sexo = PessoaSexo.SexoNaoInformado;
            NascidoEm = null;
            NomeMae = Empty;
            NomePai = Empty;
        }

        public void ComoPessoaExterior(string documentoExterior)
        {
            Tipo = PessoaTipo.Extrangeiro;
            DocumentoExterior = documentoExterior?.TrimOrEmpty() ?? Empty;

            ColocarPessoaJuridicaEmBranco();
            ColocarPessoaFisicaEmBranco();
        }

        public void ComTelefones(IEnumerable<PessoaTelefone> telefones)
        {
            _telefones = new List<PessoaTelefone>(telefones);
        }

        public void ComEnderecos(IEnumerable<PessoaEndereco> enderecos)
        {
            _enderecos = new List<PessoaEndereco>(enderecos);
        }

        public void ComEmails(IEnumerable<PessoaEmail> emails)
        {
            _emails = new List<PessoaEmail>(emails);
        }

        public void AdicionarEndereco(PessoaEndereco endereco)
        {
            endereco.Residente = this;
            if (_enderecos.All(end => end.Logradouro.ToUpper() != endereco.Logradouro.ToUpper()))
            {
                _enderecos.Add(endereco);
            }
            
        }

        public void AdicionarEmail(PessoaEmail email)
        {
            email.Pessoa = this;
            _emails.Add(email);
        }

        public void AdicionarTelefone(PessoaTelefone telefone)
        {
            telefone.Contato = this;
            _telefones.Add(telefone);
        }

        public bool PossuiCpf()
        {
            return !IsNullOrWhiteSpace(Cpf?.Valor);
        }

        public bool PossuiCnpj()
        {
            return !IsNullOrWhiteSpace(Cnpj?.Valor);
        }

        public bool PossuiDocumentoUnico()
        {
            return PossuiCnpj() || PossuiCpf();
        }

        public bool NaoPossuiDocumentoUnico()
        {
            return !PossuiDocumentoUnico();
        }

        public void ThrowInvalidOperationInativa()
        {
            if (Ativo == false)
                throw new InvalidOperationException("Pessoa Inativa");
        }
    }
}