using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fusion.Conversor.Core.Cache;
using Fusion.Conversor.Core.Csv;
using Fusion.Conversor.Core.Helpers;
using Fusion.Conversor.Core.Map;
using Fusion.Conversor.Core.Repositorios;
using Fusion.Conversor.Core.Repositorios.CustomQueries;
using Fusion.Conversor.Core.Resolvedores;
using Fusion.Conversor.Core.Resolvedores.Produto;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.TextEncoding;
using FusionCore.Tributacoes.Federal;
using FusionCore.Tributacoes.Regras;
using FusionLibrary.VisaoModel;

namespace Fusion.Conversor.Views.CvProdutos
{
    public class ProdutoConversaoContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private readonly DecimalConverter _decimalConverter = new DecimalConverter();
        private readonly IntegerConverter _integerConverter = new IntegerConverter();
        private readonly StringPreparer _stringPreparer = new StringPreparer();

        public ProdutoConversaoContexto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
            FileEncoding = TipoEncoding.Default;
        }

        public TipoEncoding FileEncoding
        {
            get => GetValue<TipoEncoding>();
            set => SetValue(value);
        }

        public string CsvPath
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool ManterCodigo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public RegraTributacaoSaida RegraSaidaPadrao
        {
            get => GetValue<RegraTributacaoSaida>();
            set => SetValue(value);
        }

        public bool ForcarRegraSaida
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public TributacaoIpi IpiPadrao
        {
            get => GetValue<TributacaoIpi>();
            set => SetValue(value);
        }

        public bool ForcarIpi
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public TributacaoPis PisPadrao
        {
            get => GetValue<TributacaoPis>();
            set => SetValue(value);
        }

        public bool ForcarPis
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public TributacaoCofins CofinsPadrao
        {
            get => GetValue<TributacaoCofins>();
            set => SetValue(value);
        }

        public bool ForcarCofins
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string SeparadorDecimal
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool ImportarEstoque
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public IEnumerable<RegraTributacaoSaida> ListaDeRegrasSaidas
        {
            get => GetValue<IEnumerable<RegraTributacaoSaida>>();
            set => SetValue(value);
        }

        public IEnumerable<TributacaoPis> ListaDePis
        {
            get => GetValue<IEnumerable<TributacaoPis>>();
            set => SetValue(value);
        }

        public IEnumerable<TributacaoIpi> ListaDeIpi
        {
            get => GetValue<IEnumerable<TributacaoIpi>>();
            set => SetValue(value);
        }

        public IEnumerable<TributacaoCofins> ListaDeCofins
        {
            get => GetValue<IEnumerable<TributacaoCofins>>();
            set => SetValue(value);
        }

        public IEnumerable<ProdutoCsv> ListaDeProdutos
        {
            get => GetValue<IList<ProdutoCsv>>();
            private set => SetValue(value);
        }

        public bool ImportarIsEnabled
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void CarregarListas()
        {
            using (var sessao = _sessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioProdutoConversao(sessao);

                ListaDeRegrasSaidas = repositorio.RegrasDeSaida();
                ListaDeIpi = repositorio.TributacoesSaidaIpi();
                ListaDePis = repositorio.TributacoesSaidaPis();
                ListaDeCofins = repositorio.TributacoesSaidaCofins();
            }

            RegraSaidaPadrao = ListaDeRegrasSaidas.FirstOrDefault(i => i.Id == 1);
            IpiPadrao = ListaDeIpi.FirstOrDefault(i => i.Codigo == "99");
            PisPadrao = ListaDePis.FirstOrDefault(i => i.Id == "49");
            CofinsPadrao = ListaDeCofins.FirstOrDefault(i => i.Id == "49");
        }

        public void CarregarCsv()
        {
            if (!File.Exists(CsvPath))
            {
                throw new InvalidOperationException("Arquivo CSV não localiado!");
            }

            var csv = new CsvFile(CsvPath, FileEncoding);
            var reader = new ProdutoReader(csv);

            ListaDeProdutos = reader.ReadAll();
        }

        public void FazerImportacao()
        {
            ThrowExceptionSeModeloInvalido();

            _decimalConverter.SeparadorDecimal = SeparadorDecimal;
            _decimalConverter.PositivarNegativo = true;

            DoImportar();
        }

        private void DoImportar()
        {
            var now = DateTime.Now;

            using (var sessao = _sessaoManager.CriaStatelessSession())
            using (var transaction = sessao.BeginTransaction())
            {
                CustomQuery.ActiveInsertIdentity(sessao, CustomQuery.TbProduto);

                var resolvedorUnidade = new ResolvedorUnidade(sessao, new ArrayCache<ProdutoUnidadeDTO>());
                var resolvedorGrupo = new ResolvedorGrupo(sessao, new ArrayCache<ProdutoGrupoDTO>());
                var resolvedorNcm = new ResolvedorNcm(sessao, new ArrayCache<NcmDTO>());
                var resolvedorPis = new ResolvedorPis(sessao, new ArrayCache<TributacaoPis>());
                var resolvedorCofins = new ResolvedorCofins(sessao, new ArrayCache<TributacaoCofins>());
                var resolvedorIpi = new ResolvedorIpi(sessao, new ArrayCache<TributacaoIpi>());
                var resolvedorCst = new ResolvedorRegraSaida(sessao, new ArrayCache<RegraTributacaoSaida>());
                var resolvedorEnquadramentoIpi = new ResolvedorEnquadramentoIpi(sessao, new ArrayCache<EquadramentoIpi>());
                var resolvedorAlias = new ResolvedorAlias(sessao, new ArrayCache<ProdutoAlias>());
                var resolvedorIdentity = new ResolvedorCodigo();

                resolvedorPis.ForcarUsoDefault(ForcarPis);
                resolvedorCofins.ForcarUsoDefault(ForcarCofins);
                resolvedorIpi.ForcarUsoDefault(ForcarIpi);
                resolvedorCst.ForcarUsoDefault(ForcarRegraSaida);

                var ncmPadrao = sessao.Get<NcmDTO>("00");
                var unidadePadrao = sessao.Get<ProdutoUnidadeDTO>(1);
                var grupoPadrao = sessao.Get<ProdutoGrupoDTO>(1);
                var enqIpiPadrao = sessao.Get<EquadramentoIpi>("999");

                var sequencia = TabelaProduto.UltimoCodigo(sessao) + 1;

                ProdutoCsv currentItem = null;

                try
                {
                    foreach (var item in ListaDeProdutos)
                    {
                        currentItem = item;

                        if (string.IsNullOrEmpty(item.Nome) || item.Nome.Length <= 2)
                        {
                            continue;
                        }

                        var ncm = resolvedorNcm.Resolve(item.Ncm, ncmPadrao);

                        var produtoId = ManterCodigo
                            ? resolvedorIdentity.Resolve(item.Codigo)
                            : sequencia++;

                        var entidade = new ProdutoDTO
                        {
                            Id = produtoId,
                            Ativo = true,
                            ProdutoUnidadeDTO = resolvedorUnidade.Resolve(item.SiglaUnidade, unidadePadrao),
                            ProdutoGrupoDTO = resolvedorGrupo.Resolve(item.Grupo, grupoPadrao),
                            Nome = item.Nome.ToUpper(),
                            PrecoCompra = _decimalConverter.Converte(item.PrecoCompra, 4),
                            PrecoVenda = _decimalConverter.Converte(item.PrecoVenda, 4),
                            OrigemMercadoria = OrigemMercadoria.Nacional,
                            Ncm = ncm.Id,
                            Cest = _stringPreparer.IsValid(item.Cest) ? item.Cest : ncm.Cest,
                            AliquotaIcms = _decimalConverter.Converte(item.AliquotaCst, 2),
                            PercentualMva = _decimalConverter.Converte(item.PercentualMva, 2),
                            ReducaoIcms = _decimalConverter.Converte(item.ReducaoIcms, 2),
                            Pis = resolvedorPis.Resolve(item.CodigoPis, PisPadrao),
                            AliquotaPis = _decimalConverter.Converte(item.AliquotaPis, 2),
                            Cofins = resolvedorCofins.Resolve(item.CodigoCofins, CofinsPadrao),
                            AliquotaCofins = _decimalConverter.Converte(item.AliquotaCofins, 2),
                            SituacaoTributariaIpi = resolvedorIpi.Resolve(item.CodigoIpi, IpiPadrao),
                            AliquotaIpi = _decimalConverter.Converte(item.AliquotaIpi, 2),
                            CadastradoEm = now,
                            AlteradoEm = now,
                            ReferenciaInterna = _stringPreparer.Prepare(item.Referencia?.ToUpper(), 30),
                            Observacao = _stringPreparer.Prepare(item.Observacao?.ToUpper(), maxLength: 5000),
                            MargemLucro = _decimalConverter.Converte(item.MargemLucro, 2),
                            CodigoBalanca = _integerConverter.Converte(item.CodigoBalanca),
                            PrecoCusto = _decimalConverter.Converte(item.PrecoCusto, 2),
                            CodigoAnp = _stringPreparer.Prepare(item.CodigoAnp, 9),
                            RegraTributacaoSaida = resolvedorCst.Resolve(item.CodigoCst, RegraSaidaPadrao),
                            EnquadramentoIpi = resolvedorEnquadramentoIpi.Resolve(item.CodigoEnquadramentoIpi, enqIpiPadrao),
                        };

                        sessao.Insert(entidade);

                        if (!string.IsNullOrEmpty(item.CodigoBarra))
                        {
                            var alias = resolvedorAlias.Resolve(entidade, item.CodigoBarra);
                            sessao.Insert(alias);
                        }

                        var quantidade = ImportarEstoque 
                            ? _decimalConverter.Converte(item.Estoque, 4) 
                            : 0.00M;

                        TabelaEstoque.InsertEstoque(sessao, entidade.Id, quantidade);
                        TabelaEstoque.RegistrarEvento(sessao, entidade.Id, quantidade);
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    sessao.Close();

                    using (var newSession = _sessaoManager.CriaStatelessSession())
                    {
                        CustomQuery.ResetIdentity<ProdutoDTO>(newSession, nameof(ProdutoDTO.Id));
                        CustomQuery.ResetIdentity<ProdutoAlias>(newSession, nameof(ProdutoAlias.Id));
                        CustomQuery.ResetIdentity<EstoqueEventoDTO>(newSession, nameof(EstoqueEventoDTO.Id));
                    }

                    throw new Exception($"Falha no item: {currentItem}", e);
                }
            }
        }

        private void ThrowExceptionSeModeloInvalido()
        {
            if (ListaDeProdutos?.Any() != true)
            {
                throw new InvalidOperationException("Nenhum produto para ser importado.");
            }

            if (string.IsNullOrEmpty(SeparadorDecimal))
            {
                throw new InvalidOperationException("Preciso que informe qual separador decimal está os valores.");
            }
        }

        public void Clear()
        {
            ImportarIsEnabled = false;
            ListaDeProdutos = new List<ProdutoCsv>();
            CsvPath = string.Empty;
        }
    }
}