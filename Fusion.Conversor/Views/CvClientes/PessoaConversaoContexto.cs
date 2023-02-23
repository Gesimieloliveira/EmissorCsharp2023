using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fusion.Conversor.Core.Cache;
using Fusion.Conversor.Core.Csv;
using Fusion.Conversor.Core.Map;
using Fusion.Conversor.Core.Repositorios.CustomQueries;
using Fusion.Conversor.Core.Resolvedores;
using Fusion.Conversor.Core.Resolvedores.Pessoa;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.TextEncoding;
using FusionLibrary.VisaoModel;

namespace Fusion.Conversor.Views.CvClientes
{
    public sealed class PessoaConversaoContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;

        public PessoaConversaoContexto(ISessaoManager sessaoManager)
        {
            FileEncoding = TipoEncoding.Default;
            _sessaoManager = sessaoManager;
        }

        public string CsvPath
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public IEnumerable<PessoaCsv> ListaDeDados
        {
            get => GetValue<IList<PessoaCsv>>();
            set => SetValue(value);
        }

        public bool ManterCodigo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IgnorarErroDocumento
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IgnorarErroEndereco
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public TipoEncoding FileEncoding
        {
            get => GetValue<TipoEncoding>();
            set => SetValue(value);
        }

        public bool ImportarIsEnabled
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool ImportarRegistroSemDocumento
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void CarregarCsv()
        {
            if (!File.Exists(CsvPath))
            {
                throw new InvalidOperationException("Arquivo CSV não localizado!");
            }

            var csv = new CsvFile(CsvPath, FileEncoding);
            var reader = new PessoaReader(csv);

            ListaDeDados = reader.ReadAll();
        }

        public void FazerImportacao()
        {
            ThrowExceptionSeModeloInvalido();

            using (var sessao = _sessaoManager.CriaStatelessSession())
            using (var transaction = sessao.BeginTransaction())
            {
                var cidades = sessao.QueryOver<CidadeDTO>().List();

                CustomQuery.ActiveInsertIdentity(sessao, CustomQuery.TbPessoa);

                var resolvedorPessoa = new ResolvedorPessoa(sessao);
                var resolvedorEndereco = new ResolvedorEndereco(sessao, new ArrayCache<CidadeDTO>(cidades));
                var resolvedorEmail = new ResolvedorEmail(sessao);
                var resolvedorTelefone = new ResolvedorTelefone(sessao);
                var resolvedorCodigo = new ResolvedorCodigo();

                resolvedorPessoa.ImportarSemDocumento = ImportarRegistroSemDocumento;
                resolvedorPessoa.IgorarErroDocumento = IgnorarErroDocumento;

                if (IgnorarErroEndereco)
                {
                    resolvedorEndereco.ErrorHandler = null;
                }

                var sequencia = TabelaCliente.UltimoCodigo(sessao) + 1;

                PessoaCsv currentItem = null;

                try
                {
                    foreach (var row in ListaDeDados)
                    {
                        currentItem = row;

                        if (string.IsNullOrEmpty(row.Nome) || row.Nome.Length <= 2)
                        {
                            continue;
                        }

                        var clienteId = ManterCodigo
                            ? resolvedorCodigo.Resolve(row.Codigo)
                            : sequencia++;

                        if (resolvedorPessoa.Resolve(row, clienteId, out var entidade))
                        {
                            resolvedorEndereco.Resolve(ref entidade, row);
                            resolvedorEmail.Resolve(ref entidade, row);

                            resolvedorTelefone.Resolve(ref entidade, row.DescricaoTelefone, row.Telefone);
                            resolvedorTelefone.Resolve(ref entidade, row.DescricaoTelefone2, row.Telefone2);
                            resolvedorTelefone.Resolve(ref entidade, row.DescricaoTelefone3, row.Telefone3);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    sessao.Close();

                    using (var newSession = _sessaoManager.CriaStatelessSession())
                    {
                        CustomQuery.ResetIdentity<PessoaEntidade>(newSession, nameof(PessoaEntidade.Id));
                        CustomQuery.ResetIdentity<PessoaEmail>(newSession, nameof(PessoaEmail.Id));
                        CustomQuery.ResetIdentity<PessoaTelefone>(newSession, nameof(PessoaTelefone.Id));
                        CustomQuery.ResetIdentity<PessoaEndereco>(newSession, nameof(PessoaEndereco.Id));
                    }

                    throw new Exception($"Falha ao processar o item: {currentItem}", e);
                }
            }
        }

        private void ThrowExceptionSeModeloInvalido()
        {
            if (ListaDeDados?.Any() != true)
            {
                throw new InvalidOperationException("Nenhuma linha para ser importada.");
            }
        }

        public void Clear()
        {
            ImportarIsEnabled = false;
            ListaDeDados = new List<PessoaCsv>();
            CsvPath = string.Empty;
        }
    }
}