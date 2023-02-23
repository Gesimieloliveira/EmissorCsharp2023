using System;
using Fusion.Conversor.Core.Cache;
using Fusion.Conversor.Core.Map;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Exceptions;

// ReSharper disable RedundantTypeSpecificationInDefaultExpression

namespace Fusion.Conversor.Core.Resolvedores.Pessoa
{
    public class ResolvedorEndereco : ResolvedorCacheable<CidadeDTO>
    {
        public ResolvedorEndereco(IStatelessSession session, ArrayCache<CidadeDTO> cache) : base(session, cache)
        {
        }

        public Action<Exception> ErrorHandler { get; set; } = e => throw e;

        public void Resolve(ref PessoaEntidade pessoa, PessoaCsv csv)
        {
            try
            {
                DoResolve(ref pessoa, csv);
            }
            catch (GenericADOException ex)
            {
                throw new Exception($"Erro crítico endereco: {ex.Message}");
            }
            catch (Exception e)
            {
                ErrorHandler?.Invoke(e);
            }
        }

        private void DoResolve(ref PessoaEntidade pessoa, PessoaCsv csv)
        {
            if (!TryResolveCidade(csv, out var cidade))
            {
                throw new InvalidOperationException("Não foi possível resolver a cidade do endereço");
            }

            var logradouro = StringPreparer.Prepare(csv.Logradouro?.ToUpper(), 75);
            var bairro = StringPreparer.Prepare(csv.Bairro?.ToUpper(), 75);
            var cep = StringPreparer.Prepare(StringPreparer.RemoveNaoNumeros(csv.Cep), 8);
            var complemento = StringPreparer.Prepare(csv.ComplementoEndereco?.ToUpper(), 255);
            var numero = StringPreparer.Prepare(StringPreparer.RemoveNaoNumeros(csv.NumeroEndereco), 10);

            var endereco = new PessoaEndereco(logradouro, numero, bairro, cep, cidade)
            {
                Complemento = complemento,
                Residente = pessoa
            };

            Session.Insert(endereco);
        }

        private bool TryResolveCidade(PessoaCsv csv, out CidadeDTO cidade)
        {
            return TryResolveByIbge(csv, out cidade) || TryResolveByNome(csv, out cidade);
        }

        private bool TryResolveByIbge(PessoaCsv csv, out CidadeDTO cidade)
        {
            cidade = default(CidadeDTO);

            if (string.IsNullOrEmpty(csv.IbgeCidade))
            {
                return false;
            }

            var codigoIbge = StringPreparer.RemoveNaoNumeros(csv.IbgeCidade);
            var ibge = IntegerConverter.Converte(codigoIbge);

            if (ibge == 0)
            {
                return false;
            }

            if (Cache.TryGetCache(i => i.CodigoIbge == ibge, out cidade))
            {
                return true;
            }

            cidade = Session.QueryOver<CidadeDTO>()
                .Where(i => i.CodigoIbge == ibge)
                .Take(1)
                .SingleOrDefault<CidadeDTO>();

            if (cidade == null)
            {
                return false;
            }

            Cache.Add(cidade);
            return true;

        }

        private bool TryResolveByNome(PessoaCsv csv, out CidadeDTO cidade)
        {
            cidade = default(CidadeDTO);

            if (string.IsNullOrEmpty(csv.Cidade) || string.IsNullOrWhiteSpace(csv.Uf))
            {
                return false;
            }

            if (Cache.TryGetCache(i => i.Compare(csv.Cidade, csv.Uf), out cidade))
            {
                return true;
            }

            cidade = Session.QueryOver<CidadeDTO>()
                .Where(i => i.Nome == csv.Cidade && i.SiglaUf == csv.Uf)
                .Take(1)
                .SingleOrDefault<CidadeDTO>();

            if (cidade == null)
            {
                return false;
            }

            Cache.Add(cidade);
            return true;
        }
    }
}