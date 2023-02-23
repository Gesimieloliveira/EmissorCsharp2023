using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Componentes;

// ReSharper disable VirtualMemberNeverOverridden.Global

namespace FusionCore.FusionAdm.Pessoas
{
    public abstract class PessoaExtensao
    {
        public virtual PessoaEntidade Pessoa { get; set; }
        public virtual string Nome => Pessoa.Nome;
        public virtual string NomeFantasia => Pessoa.NomeFantasia;
        public virtual string InscricaoEstadual => Pessoa.InscricaoEstadual;
        public virtual string DocumentoExterior => Pessoa.DocumentoExterior;
        public virtual PessoaTipo Tipo => Pessoa.Tipo;
        public virtual Cpf Cpf => Pessoa.Cpf;
        public virtual Cnpj Cnpj => Pessoa.Cnpj;
        public virtual DocumentoRg Rg => Pessoa.Rg;
        public virtual IReadOnlyList<PessoaTelefone> Telefones => Pessoa.Telefones;
        public virtual IReadOnlyList<PessoaEndereco> Enderecos => Pessoa.Enderecos;
        public virtual DateTime AlteradoEm => Pessoa.AlteradoEm;
        public string Documento => Tipo == PessoaTipo.Fisica ? Cpf.Valor : Cnpj.Valor;
    }
}