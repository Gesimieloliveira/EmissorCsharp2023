using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.CteEletronico.Cancelar;
using FusionCore.FusionAdm.CteEletronico.CCe;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using TipoEmissao = FusionCore.FusionAdm.CteEletronico.Flags.TipoEmissao;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class Cte : Entidade, ICancelarCte, ICartaCorrecaoCte
    {
        private Ibpt _ibptCache;
        private CteEmissaoHistorico _historico;
        public int Id { get; set; }
        public CteEmissao CteEmissao { get; set; }
        public PerfilCte PerfilCte { get; set; }
        public PerfilCfopDTO PerfilCfop { get; set; }
        public string NaturezaOperacao { get; set; }
        public Modal Modal { get; set; }
        public TipoServico TipoServico { get; set; }
        public TipoEmissao TipoEmissao { get; set; } = TipoEmissao.Normal;

        public CidadeDTO CidadeInicioOperacao { get; set; }
        public EstadoDTO EstadoInicioOperacao { get; set; }
        public CidadeDTO CidadeFinalOperacao { get; set; }
        public EstadoDTO EstadoFinalOperacao { get; set; }
        public decimal ValorServico { get; private set; }
        public decimal ValorReceber { get; set; }
        public decimal ValorTributoApoximado { get; private set; }
        public string Observacao { get; set; }
        public decimal ValorTotalCarga { get; set; }
        public string NomeProdutoPredominante { get; set; }
        public string CaracteristicaProdutoPredominante { get; set; }
        public TipoCte TipoCte { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFinal { get; set; }
        public TipoPeriodoData TipoPeriodoData { get; set; }
        public TipoPeriodoHora TipoPeriodoHora { get; set; }
        public TipoTomador TipoTomador { get; set; }
        public bool CalcularTotalCargaAutomatico { get; set; }
        public bool Globalizado { get; set; }
        public string ChaveCTeComplementado { get; set; }
        public short SerieEmissao { get; set; }
        public long NumeroFiscalEmissao { get; set; }
        public int CodigoNumericoEmissao { get; set; }
        public CteEmitente CteEmitente { get; set; }
        public CteDestinatario CteDestinatario { get; set; }
        public CteRemetente CteRemetente { get; set; }
        public CteExpedidor CteExpedidor { get; set; }
        public CteRecebedor CteRecebedor { get; set; }
        public CteTomador CteTomador { get; set; }
        public CteRodoviario CteRodoviario { get; set; }
        public CteCancelamento Cancelamento { get; set; }
        public CteImpostoCst CteImpostoCst { get; set; }
        public CteImpostoDifal CteImpostoDifal { get; set; }
        public CteConfigImposto CteConfigImposto { get; set; }
        public CteSubstituicao CteSubstituicao { get; set; }
        public IList<CteDocumentoImpresso> CteDocumentoImpressos { get; set; }
        public IList<CteDocumentoNfe> CteDocumentoNfes { get; set; }
        public IList<CteDocumentoOutro> CteDocumentoOutros { get; set; }
        public IList<CteInfoQuantidadeCarga> CteInfoQuantidadeCargas { get; set; }
        public IList<CteVeiculoTransportado> CteVeiculoTransportados { get; set; }
        public IList<CteDocumentoAnterior> CteDocumentoAnteriores { get; set; }
        public IList<CteComponenteValorPrestacao> CteComponenteValorPrestacaos { get; set; }

        protected override int ReferenciaUnica => Id;

        public bool TemEmissao => CteEmissao != null;
        public decimal? ValorAverbacao { get; set; }
        public DateTime EmissaoEm { get; set; }
        public bool Inutilizado { get; set; }
        public string MotivoInutilizacao { get; set; }
        public string CodigoIbpt { get; private set; }

        public string NumeroFiscal => NumeroFiscalEmissao.ToString();
        public string Chave => _historico != null && _historico.Chave.IsNotNullOrEmpty() ? _historico.Chave : CteEmissao.Chave;
        public string CnpjOuCpf => CteEmitente.Emitente.DocumentoUnicoFormatado;
        public EstadoDTO Estado => CteEmitente.Emitente.EstadoDTO;
        public TipoAmbiente TipoAmbiente => _historico != null ? _historico.AmbienteSefaz : CteEmissao.Ambiente;
        public string Protocolo => CteEmissao.Protocolo;
        public string CnpjCpfEmitente => CteEmitente.Emitente.DocumentoUnicoFormatado;
        public EmissorFiscal EmissorFiscal => PerfilCte.EmissorFiscal;
        public Documento Documento => Documento.CTe;
        public string ChaveCteAnulacao { get; set; }
        public DateTime DeclaracaoEmitidaEm { get; set; }

        public Cte()
        {
            CteRodoviario = new CteRodoviario();
            CteInfoQuantidadeCargas = new List<CteInfoQuantidadeCarga>();
            CteVeiculoTransportados = new List<CteVeiculoTransportado>();
            CteDocumentoImpressos = new List<CteDocumentoImpresso>();
            CteDocumentoNfes = new List<CteDocumentoNfe>();
            CteDocumentoOutros = new List<CteDocumentoOutro>();
            CteDocumentoAnteriores = new List<CteDocumentoAnterior>();
            EmissaoEm = DateTime.Now;
            Inutilizado = false;
            MotivoInutilizacao = string.Empty;
        }

        public Ibpt FetchIbpt()
        {
            if (_ibptCache != null && _ibptCache.Codigo == CodigoIbpt)
            {
                return _ibptCache;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioIbpt(sessao);
                _ibptCache = repositorio.GetPeloNbs(CodigoIbpt);
            }

            return _ibptCache;
        }

        public void InformaValorServico(decimal valorServico)
        {
            if (valorServico == 0)
            {
                ValorTributoApoximado = 0;
                return;
            }

            ValorServico = valorServico;

            var calculo = CalculaValorTributoAproximado();
            ValorTributoApoximado = calculo.Total;
        }

        public void InformaCodigoIbpt(string codigoIbpt)
        {
            CodigoIbpt = string.IsNullOrWhiteSpace(codigoIbpt) ? null : codigoIbpt;

            var calculo = CalculaValorTributoAproximado();
            ValorTributoApoximado = calculo.Total;

        }

        private ImpostoAproximadoIbpt CalculaValorTributoAproximado()
        {
            if (string.IsNullOrWhiteSpace(CodigoIbpt) || ValorServico == 0)
            {
                return new ImpostoAproximadoIbpt(0, 0);
            }

            var ibpt = FetchIbpt();

            if (ibpt == null)
            {
                return new ImpostoAproximadoIbpt(0, 0);
            }

            var bc = new BaseCalculo(ValorServico);
            var estadual = ibpt.ImpostoEstadualAproximado(bc);
            var federal = ibpt.ImpostoFederalAproximado(bc);

            return new ImpostoAproximadoIbpt(estadual, federal);
        }

        public object GetCte()
        {
            return this;
        }

        public string ComputaTextoDeOlhoNoImposto()
        {
            var calculo = CalculaValorTributoAproximado();
            var obs =  $"O valor aproximado de tributos incidentes sobre o preço deste serviço é de R$ {calculo.Total}";

            return obs.ToUpper();
        }

        public bool IsNormal() => TipoCte == TipoCte.Normal || TipoCte == TipoCte.CteDeSubstituicao;

        public bool IsSubstituto() => CteSubstituicao != null;

        public bool IsNaoContemCteSubstituto() => !IsSubstituto();

        public void SetHistorico(CteEmissaoHistorico historico)
        {
            _historico = historico;
        }

        public bool JaFoiAlocadoNumeroFiscal()
        {
            return NumeroFiscalEmissao != 0;
        }
    }
}