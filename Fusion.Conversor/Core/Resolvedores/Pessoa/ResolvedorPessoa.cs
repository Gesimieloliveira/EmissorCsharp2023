using System;
using Fusion.Conversor.Core.Map;
using Fusion.Conversor.Core.Repositorios.CustomQueries;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Basico;
using FusionCore.Helpers.DocumentoUnico;
using NHibernate;
using NHibernate.Exceptions;

namespace Fusion.Conversor.Core.Resolvedores.Pessoa
{
    public class ResolvedorPessoa : Resolvedor
    {
        private readonly DocumentoUnicoHelper _helperDocumento = new DocumentoUnicoHelper();
        private readonly DateTime _now = DateTime.Now;

        public ResolvedorPessoa(IStatelessSession session) : base(session)
        {
        }

        public bool ImportarSemDocumento { get; set; }
        public bool IgorarErroDocumento { get; set; }

        public bool Resolve(PessoaCsv csv, int clienteId, out PessoaEntidade entidade)
        {
            try
            {
                var mae = csv.NomeMae?.ToUpper();
                var pai = csv.NomePai?.ToUpper();
                var nome = csv.Nome.ToUpper();

                entidade = new PessoaEntidade(nome)
                {
                    Id = clienteId,
                    AlteradoEm = _now,
                    NomeMae = StringPreparer.Prepare(mae),
                    NomePai = StringPreparer.Prepare(pai)
                };

                if (ResolveTipoPessoa(ref entidade, csv))
                {
                    if (PessoaJaCadastrada(entidade))
                    {
                        return false;
                    }

                    Session.Insert(entidade);

                    TabelaCliente.InsertCliente(Session, entidade, csv.Observacao);

                    if (csv.EhFornecedor == "1")
                    {
                        TabelaFornecedor.InsertFornecedor(Session, entidade);
                    }

                    return true;
                }
            }
            catch (GenericADOException e)
            {
                throw new Exception($"Erro crítico pessoa: {e.Message}");
            }

            return false;
        }

        private bool ResolveTipoPessoa(ref PessoaEntidade pessoa, PessoaCsv csv)
        {
            try
            {
                if (!string.IsNullOrEmpty(csv.Cnpj))
                {
                    ResolvePessoaJuridica(ref pessoa, csv, csv.Cnpj);
                    return true;
                }

                if (!string.IsNullOrEmpty(csv.Cpf))
                {
                    ResolvePessoaFisica(ref pessoa, csv, csv.Cpf);
                    return true;
                }

                if (_helperDocumento.TamanhoCnpj(csv.CpfOuCnpj))
                {
                    ResolvePessoaJuridica(ref pessoa, csv, csv.CpfOuCnpj);
                    return true;
                }

                if (_helperDocumento.TamanhoCpf(csv.CpfOuCnpj))
                {
                    ResolvePessoaFisica(ref pessoa, csv, csv.CpfOuCnpj);
                    return true;
                }

                throw new InvalidOperationException($"Pessoa {csv} não possui CPF ou CNPJ");
            }
            catch (Exception)
            {
                if (IgorarErroDocumento == false)
                {
                    throw;
                }
            }

            if (ImportarSemDocumento == false)
            {
                return false;
            }

            ResolvePessoaFisica(ref pessoa, csv, string.Empty);
            return true;
        }

        private bool PessoaJaCadastrada(PessoaEntidade entidade)
        {
            if (entidade.Tipo == PessoaTipo.Fisica && entidade.PossuiCpf())
            {
                var q = Session.QueryOver<PessoaEntidade>()
                    .Where(i => i.Cpf == entidade.Cpf);

                return q.RowCount() > 0;
            }

            if (entidade.Tipo == PessoaTipo.Juridica && entidade.PossuiCnpj())
            {
                var q = Session.QueryOver<PessoaEntidade>()
                    .Where(i => i.Cnpj == entidade.Cnpj);

                return q.RowCount() > 0;
            }

            return false;
        }

        private void ResolvePessoaJuridica(ref PessoaEntidade pessoa, PessoaCsv csv, string docCnpj)
        {
            var cnpj = new Cnpj(docCnpj);
            var nomeFantasia = string.IsNullOrEmpty(csv.NomeFantasia) ? csv.Nome : csv.NomeFantasia;
            var ie = ResolveInscricaoEstadual(csv.InscricaoEstadual);
            var im = StringPreparer.RemoveNaoNumeros(csv.InscricaoMunicipal) ?? string.Empty;

            ResolveIndicadorIEPessoaJuridica(pessoa, ie, cnpj, csv.IndicadorIE);

            cnpj.ThrowExcetpionSeInvalido();

            pessoa.ComoPessoaJuridica(nomeFantasia, cnpj, pessoa.IndicadorIEDestinatario, ie, im);
        }

        private void ResolveIndicadorIEPessoaJuridica(PessoaEntidade pessoa,
            string ie,
            Cnpj cnpj,
            string csvIndicadorIE)
        {
            ValidaIndicadorIe(csvIndicadorIE);

            if (SetandoIndicadorIeSeExistir(pessoa, csvIndicadorIE)) return;

            pessoa.IndicadorIEDestinatario = IndicadorIE.ContribuinteIcms;

            if (ie == "ISENTO")
                pessoa.IndicadorIEDestinatario = IndicadorIE.Isento;

            if (cnpj.Valor != "" && ie == string.Empty)
            {
                pessoa.IndicadorIEDestinatario = IndicadorIE.NaoContribuinte;
            }
        }

        private static bool SetandoIndicadorIeSeExistir(PessoaEntidade pessoa, string csvIndicadorIE)
        {
            if (csvIndicadorIE != null)
            {
                switch (csvIndicadorIE)
                {
                    case "1":
                        pessoa.IndicadorIEDestinatario = IndicadorIE.ContribuinteIcms;
                        return true;
                    case "2":
                        pessoa.IndicadorIEDestinatario = IndicadorIE.Isento;
                        return true;
                    case "9":
                        pessoa.IndicadorIEDestinatario = IndicadorIE.NaoContribuinte;
                        return true;
                }

                throw new InvalidOperationException(
                    "Valor invalido do indicador IE , aceitos apenas, 1 = contribuinte icms, 2 = isento, 9 = não contribuinte");
            }

            return false;
        }

        private string ResolveInscricaoEstadual(string ie)
        {
            if (string.IsNullOrEmpty(ie))
            {
                return string.Empty;
            }

            if (ie.ToUpper().Contains("ISENTO"))
            {
                return string.Empty;
            }

            return StringPreparer.RemoveNaoNumeros(ie);
        }

        private void ResolvePessoaFisica(ref PessoaEntidade pessoa, PessoaCsv csv, string docCpf)
        {
            var cpf = new Cpf(docCpf);
            var rg = new DocumentoRg(csv.Rg, csv.OrgaoRg);
            var nascimento = DateTimeHelper.Parse(csv.NascidoEm);
            var ie = ResolveInscricaoEstadual(csv.InscricaoEstadual);

            ResolveIndicadorIeCpf(pessoa, ie, cpf, csv.IndicadorIE);

            cpf.ThrowExcetionSeInvalido();

            pessoa.ComoPessoaFisica(cpf, rg, PessoaSexo.SexoOutros, pessoa.IndicadorIEDestinatario, ie, nascimento);
        }

        private void ResolveIndicadorIeCpf(PessoaEntidade pessoa,
            string ie,
            Cpf cpf,
            string csvIndicadorIE)
        {
            ValidaIndicadorIe(csvIndicadorIE);

            if (SetandoIndicadorIeSeExistir(pessoa, csvIndicadorIE)) return;

            pessoa.IndicadorIEDestinatario = IndicadorIE.NaoContribuinte;

            if (ie == "ISENTO")
                pessoa.IndicadorIEDestinatario = IndicadorIE.Isento;

            if (cpf.Valor != string.Empty && ie != string.Empty)
            {
                pessoa.IndicadorIEDestinatario = IndicadorIE.ContribuinteIcms;
            }

            if (cpf.Valor != "" && ie == null)
                pessoa.IndicadorIEDestinatario = IndicadorIE.NaoContribuinte;
        }

        private void ValidaIndicadorIe(string csvIndicadorIE)
        {
            if (csvIndicadorIE == null) return;

            if (csvIndicadorIE == "1" || csvIndicadorIE == "2" || csvIndicadorIE == "9") return;

            throw new InvalidOperationException("Indicador IE inválido, valores aceito são" +
                                                "1 = contribuinte icms, 2 = isento, 9 = não contribuinte");
        }
    }
}