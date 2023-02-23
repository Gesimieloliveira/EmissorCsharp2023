using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Nfce.SatFiscal;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.FusionNfce;
using FusionCore.FusionNfce.Extencoes;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.FusionNfce.Preferencias;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Flags;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Nfce
{
    public class NfceAdm : EntidadeBase<int>, IDadosParaEnvioEmailNfce
    {
        private IList<NfceItemAdm> _itens = new List<NfceItemAdm>();
        private IList<FormaPagamentoNfceAdm> _formaPagamentos;

        private NfceAdm()
        {
            //forca o uso do método stático para criação
        }

        public int Id { get; set; }
        public string XmlAutorizado => Emissao != null ? Emissao.XmlAutorizado : null;
        public ModeloDocumento Modelo { get; set; }
        public string NaturezaOperacao { get; set; }
        public DateTime EmitidaEm { get; set; }
        public DateTime EntradaSaidaEm { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public TipoDanfe TipoDanfe { get; set; }
        public FinalidadeEmissao FinalidadeEmissao { get; set; }
        public DestinoOperacao DestinoOperacao { get; set; }
        public IndicadorOperacaoFinal IndicadorConsumidorFinal { get; set; }
        public IndicadorComprador IndicadorComprador { get; set; }
        public ModalidadeFrete ModalidadeFrete { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public string InformacaoAdicional { get; set; }
        public decimal TotalBaseCalculo { get; set; }
        public decimal TotalIcms { get; set; }
        public decimal TotalBaseCalculoCofins { get; set; }
        public decimal TotalCofins { get; set; }
        public decimal TotalBaseCalculoPis { get; set; }
        public decimal TotalPis { get; set; }
        public decimal TotalIcmsDesonerado { get; set; }
        public decimal TotalBaseCalculoSt { get; set; }
        public decimal TotalSt { get; set; }
        public byte TerminalOfflineId { get; set; }
        public decimal ValorTributoAproximado { get; set; }
        public string Uuid { get; set; }
        public Malote Malote { get; set; }
        public Vendedor Vendedor { get; set; }

        public Status Status { get; set; }
        public decimal TotalProdutosServicos { get; set; }
        public decimal TotalDesconto { get; set; }
        public decimal TotalNfce { get; set; }
        public decimal TotalAcrescimo { get; set; }
        public DateTime CriadoEm { get; set; }
        public int CodigoNumerico { get; set; }
        public int NumeroFiscal { get; set; }
        public short Serie { get; set; }
        public bool Denegada { get; set; }
        public bool Cancelada => Status == Status.Cancelada;
        public byte[] Logo => Emitente.Empresa.LogoMarca;
        public decimal Troco { get; set; }
        public bool ImportadaParaNfe { get; set; }

        public NfceEmissaoAdm Emissao { get; set; }
        public NfceDestinatarioAdm Destinatario { get; set; }
        public NfceEmitenteAdm Emitente { get; set; }
        public NfceCancelamentoAdm Cancelamento { get; set; }
        public CancelamentoSatAdm CancelamentoSat { get; set; }
        public NfceContingenciaAdm Contingencia { get; set; }
        public TabelaPreco TabelaPreco { get; set; }


        public string Observacao { get; set; }
        public string UuidVenda { get; set; }
        public UsuarioDTO UsuarioCriacao { get; private set; }
        public RegimeTributario RegimeTributario { get; set; } = RegimeTributario.SimplesNacional;

        public IList<HistoricoEnvioSatAdm> HistoricoEnvioSatAdmLista { get; set; }  = new List<HistoricoEnvioSatAdm>();
        public IList<NfceEmissaoHistoricoAdm> HistoricoEnvioNfceAdmLista { get; set; } = new List<NfceEmissaoHistoricoAdm>();

        public static NfceAdm Cria(FusionNfce.Fiscal.Nfce nfce)
        {
            var nfceAdm = new NfceAdm
            {
                Id = 0,
                DestinoOperacao = nfce.DestinoOperacao,
                EmitidaEm = nfce.EmitidaEm,
                EntradaSaidaEm = nfce.EntradaSaidaEm,
                FinalidadeEmissao = nfce.FinalidadeEmissao,
                FormaPagamento = nfce.FormaPagamento,
                IndicadorComprador = nfce.IndicadorComprador,
                IndicadorConsumidorFinal = nfce.IndicadorConsumidorFinal,
                InformacaoAdicional = nfce.InformacaoAdicional,
                NaturezaOperacao = nfce.NaturezaOperacao,
                TipoDanfe = nfce.TipoDanfe,
                TipoOperacao = nfce.TipoOperacao,
                Troco = nfce.Troco,
                TotalSt = nfce.TotalSt,
                TotalIcmsDesonerado = nfce.TotalIcmsDesonerado,
                TotalIcms = nfce.TotalIcms,
                TotalBaseCalculoPis = nfce.TotalBaseCalculoPis,
                TotalPis = nfce.TotalPis,
                TotalBaseCalculoCofins = nfce.TotalBaseCalculoCofins,
                TotalCofins = nfce.TotalCofins,
                TotalBaseCalculoSt = nfce.TotalBaseCalculoSt,
                TotalBaseCalculo = nfce.TotalBaseCalculo,
                Modelo = nfce.Modelo,
                ModalidadeFrete = nfce.ModalidadeFrete,
                TerminalOfflineId = nfce.TerminalOfflineId,
                ValorTributoAproximado = nfce.ValorTributoAproximado,
                Uuid = nfce.Uuid,
                Status = nfce.Status,
                TotalDesconto = nfce.TotalDesconto,
                TotalNfce = nfce.TotalNfce,
                TotalProdutosServicos = nfce.TotalProdutosServicos,
                Observacao = nfce.Observacao,
                UuidVenda = nfce.UuidVenda,
                CriadoEm = nfce.CriadoEm,
                TotalAcrescimo = nfce.TotalAcrescimo,
                UsuarioCriacao = nfce.UsuarioCriacao.ToAdm(),
                TabelaPreco = nfce.TabelaPreco.ToAdm(),
                RegimeTributario = nfce.RegimeTributario,
                NumeroFiscal = nfce.NumeroFiscal,
                Serie = nfce.Serie,
                CodigoNumerico = nfce.CodigoNumerico,
                TipoEmissao = nfce.TipoEmissao,
                Denegada = nfce.Denegada
            };

            var emissorFiscalAdm = nfce.Emissao?.EmissorFiscal.ToAdm() ?? nfce.FinalizaEmissaoSat?.EmissorFiscal.ToAdm();
            var emissaoSatAdm = nfce.FinalizaEmissaoSat.ToAdm(nfceAdm, emissorFiscalAdm);
            
            nfceAdm.Emissao = nfce.Emissao.ToAdm(nfceAdm, emissorFiscalAdm) ?? emissaoSatAdm.ToAdmConverteFinalizacaoSatParaNfceEmissao();

            var empresaAdm = nfce.Emitente?.Empresa.ToAdm();
            var emitenteAdm = nfce.Emitente.ToAdm(nfceAdm, empresaAdm);
            nfceAdm.Emitente = emitenteAdm;

            var listaNfceItensAdm = nfce.ObterTodosItens().Select(item => item.ToAdm(nfceAdm)).ToList();
            nfceAdm.AddItens(listaNfceItensAdm);

            var listaNfceFormaPagamentoAdm = nfce.ObterOsPagamentos().Select(item => item.ToAdm(nfceAdm)).ToList();
            nfceAdm.AddFormasDePagamentos(listaNfceFormaPagamentoAdm);

            var destinatarioAdm = nfce.Destinatario.ToAdm(nfceAdm);
            nfceAdm.Destinatario = destinatarioAdm;

            var cancelamentoAdm = nfce.Cancelamento.ToAdm(nfceAdm);
            nfceAdm.Cancelamento = cancelamentoAdm;

            var cancelamentoSatAdm = nfce.CancelamentoSat.ToAdm(nfceAdm);
            nfceAdm.CancelamentoSat = cancelamentoSatAdm;

            var vendedor = nfce.Vendedor.ToAdm();
            nfceAdm.Vendedor = vendedor;

            return nfceAdm;
        }

        public TipoEmissao TipoEmissao { get; set; }

        public IList<NfceItemAdm> ObterOsItens()
        {
            return Itens?.Where(i => i.Cancelado == false).ToList();
        }

        public IList<NfceItemAdm> ObterOsItensCancelados()
        {
            return Itens.Where(i => i.Cancelado).ToList();
        }

        public IList<NfceItemAdm> ObterTodosItens()
        {
            return Itens.ToList();
        }

        public void AddItens(IEnumerable<NfceItemAdm> itens)
        {
            if (Itens == null) Itens = new List<NfceItemAdm>();

            itens.ForEach(Itens.Add);
        }

        public IList<FormaPagamentoNfceAdm> ObterOsPagamentos()
        {
            return _formaPagamentos;
        }

        public void AddFormasDePagamentos(List<FormaPagamentoNfceAdm> formaPagamentos)
        {
            if (_formaPagamentos == null) _formaPagamentos = new List<FormaPagamentoNfceAdm>();

            formaPagamentos.ForEach(_formaPagamentos.Add);
        }

        public void AddFormasDePagamento(FormaPagamentoNfceAdm formaPagamentoNfceAdm)
        {
            if (_formaPagamentos == null) _formaPagamentos = new List<FormaPagamentoNfceAdm>();
            _formaPagamentos.Add(formaPagamentoNfceAdm);
        }

        protected override int ChaveUnica => Id;

        public IList<NfceItemAdm> Itens
        {
            get { return _itens; }
            set
            {
                _itens = value;
            }
        }

        public string NumeroChave => Emissao.Chave;

        public void CriadoPorUsuario(UsuarioDTO usuario)
        {
            UsuarioCriacao = usuario;
        }
    }
}