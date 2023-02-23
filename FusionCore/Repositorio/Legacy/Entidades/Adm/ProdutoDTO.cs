using System;
using System.Collections.Generic;
using FusionCore.Core.Estoque;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.IdGenerator;
using FusionCore.Tributacoes.Federal;
using FusionCore.Tributacoes.Regras;

// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class ProdutoDTO : Entidade, ISincronizavelAdm, IProdutoSelecionado, IEntidadeIdentity, IProdutoTabelaPreco
    {
        private readonly IList<ProdutoRegraTributacao> _regrasInterstaduais = new List<ProdutoRegraTributacao>();

        public ProdutoDTO()
        {
            Ativo = true;
            CadastradoEm = DateTime.Now;
            AlteradoEm = DateTime.Now;
            CodigoDfe = string.Empty;
        }

        public int Id { get; set; }
        public int ProdutoId => Id;
        public string Nome { get; set; }
        public ProdutoLocalizacao Localizacao { get; set; }
        public ProdutoUnidadeDTO ProdutoUnidadeDTO { get; set; }
        public ProdutoGrupoDTO ProdutoGrupoDTO { get; set; }
        public decimal PrecoCompra { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal MargemLucro { get; set; }
        public decimal PrecoVenda { get; set; }
        public bool Ativo { get; set; }
        public string Ncm { get; set; }
        public string Cest { get; set; }
        public OrigemMercadoria OrigemMercadoria { get; set; }
        public RegraTributacaoSaida RegraTributacaoSaida { get; set; }
        public decimal AliquotaIcms { get; set; }
        public decimal ReducaoIcms { get; set; }
        public decimal PercentualMva { get; set; }
        public TributacaoPis Pis { get; set; }
        public TributacaoCofins Cofins { get; set; }
        public decimal AliquotaPis { get; set; }
        public decimal AliquotaCofins { get; set; }
        public TributacaoIpi SituacaoTributariaIpi { get; set; }
        public decimal AliquotaIpi { get; set; }
        public DateTime? CadastradoEm { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string ReferenciaInterna { get; set; }
        public string Observacao { get; set; }
        public IEnumerable<ProdutoRegraTributacao> RegrasInterstaduais => _regrasInterstaduais;
        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.Produto;
        public int CodigoBalanca { get; set; }
        public IList<ProdutoAlias> ProdutosAlias { get; set; } = new List<ProdutoAlias>();
        protected override int ReferenciaUnica => Id;
        public bool IsNovo => Id == 0;
        public string CodigoAnp { get; set; }
        public bool IsIndicadorEscalaRelevante { get; set; } = true;
        public string CnpjFabricante { get; set; } = string.Empty;
        public EquadramentoIpi EnquadramentoIpi { get; set; }
        public bool IsControlaIndicadorEscala { get; set; }

        public decimal PercentualGlpPetroleo { get; set; }
        public decimal PercentualGasNacional { get; set; }
        public decimal PercentualGasImportador { get; set; }
        public decimal ValorDePartida { get; set; }
        public string CodigoDfe { get; set; }
        public ProdutoUnidadeDTO ProdutoUnidadeTributavel { get; set; }
        public decimal QuantidadeUnidadeTributavel { get; set; }
        public bool UsarObservacaoNoItemFiscal { get; set; } = false;

        public void AddAlias(string alias, bool isCean = true)
        {
            ProdutosAlias.Add(new ProdutoAlias(alias, isCean) {Produto = this});
        }

        public ProdutoDTO CarregaProduto()
        {
            return this;
        }

        public override string ToString()
        {
            return $"{Id} - {Nome}";
        }

        public bool NcmInvalidoParaNfe()
        {
            return Ncm.Length != 8;
        }

        public ProdutoCodigoAnp CarregaAnp()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var anp = new RepositorioProduto(sessao).BuscaCodigoAnp(CodigoAnp);

                if (anp == null)
                {
                    throw new InvalidOperationException($"Não consegui localizar descrição para esse Código ANP: {CodigoAnp}");
                }

                return anp;
            }
        }

        public void NaoAtivoThrowInvalidOperation()
        {
            if (Ativo == false)
            {
                throw new InvalidOperationException("Produto não está ativo");
            }
        }
    }
}