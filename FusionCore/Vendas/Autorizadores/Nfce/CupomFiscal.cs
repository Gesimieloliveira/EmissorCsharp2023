using System;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Preferencias;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Vendas.Faturamentos;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class CupomFiscal : EntidadeBase<int>, IDadosParaImpressaoNfce, IDadosParaEnvioEmailNfce
    {
        private CupomFiscal()
        {
            // nhibernate
            CriadaEm = DateTime.Now;
        }

        public CupomFiscal(FaturamentoVenda venda
            , byte emissorFiscalId
            , int numeroFiscal, short serie
            , int codigoNumerico
            , TipoAmbiente ambienteSefaz
            , UsuarioDTO usuarioCriacao) : this()
        {
            Venda = venda;
            EmissorFiscalId = emissorFiscalId;
            NumeroFiscal = numeroFiscal;
            Serie = serie;
            CodigoNumerico = codigoNumerico;
            AmbienteSefaz = ambienteSefaz;
            UsuarioCriacao = usuarioCriacao;
            Aberto();
        }

        public CupomFiscal(int id, byte emissorFiscalId, int numeroFiscal
            , short serie, int codigoNumerico, TipoEmissao tipoEmissao
            , TipoAmbiente ambienteSefaz, int contingenciaId
            , DateTime emitirEm)
        {
            Id = id;
            EmissorFiscalId = emissorFiscalId;
            NumeroFiscal = numeroFiscal;
            Serie = serie;
            CodigoNumerico = codigoNumerico;
            TipoEmissao = tipoEmissao;
            AmbienteSefaz = ambienteSefaz;
            ContingenciaId = contingenciaId;
            EmitirEm = emitirEm;
        }

        public int Id { get; private set; }
        public string XmlAutorizado => CupomFiscalFinalizado?.XmlAutorizado;
        public bool Cancelada => SituacaoFiscal == SituacaoFiscal.Cancelado;
        public byte[] Logo => Venda.Empresa.LogoMarcaNfce;
        public byte EmissorFiscalId { get; private set; }
        public DateTime CriadaEm { get; private set; }
        public int NumeroFiscal { get; private set; }
        public short Serie { get; private set; }
        public int CodigoNumerico { get; private set; }
        public TipoEmissao TipoEmissao { get; private set; }
        public SituacaoFiscal SituacaoFiscal { get; private set; }
        public TipoAmbiente AmbienteSefaz { get; private set; }
        public string XmlCancelamento { get; private set; }
        public DateTime EmitirEm { get; private set; }
        public UsuarioDTO UsuarioCriacao { get; private set; }
        public FaturamentoVenda Venda { get; private set; }
        public CupomFiscalFinalizado CupomFiscalFinalizado { get; private set; }
        public int? ContingenciaId { get; private set; }

        public void EmissaoNormal()
        {
            TipoEmissao = TipoEmissao.Normal;
        }

        public void AtivarContingencia(int contingenciaId)
        {
            TipoEmissao = TipoEmissao.ContigenciaOfflineNFCe;
            ContingenciaId = contingenciaId;
            AutorizadaSemInternet();
        }

        private void Aberto()
        {
            EmitirEm = DateTime.Now;
            SituacaoFiscal = SituacaoFiscal.Aberta;
        }

        public void Autorizada()
        {
            SituacaoFiscal = SituacaoFiscal.Autorizada;
        }

        public void AutorizadaSemInternet()
        {
            SituacaoFiscal = SituacaoFiscal.AutorizadaSemInternet;
        }

        public void Cancelado(string xmlCancelado)
        {
            SituacaoFiscal = SituacaoFiscal.Cancelado;
            XmlCancelamento = xmlCancelado;
        }

        public void CanceladoSemXml()
        {
            SituacaoFiscal = SituacaoFiscal.Cancelado;
        }

        public void Denegada()
        {
            SituacaoFiscal = SituacaoFiscal.AutorizadaDenegada;
        }

        protected override int ChaveUnica => Id;

        public void AlocaDadosFiscais(EmissorFiscalNFCE emissorFiscal)
        {
            var usaNumeracaoDiferenteContigencia = emissorFiscal.UsaNumeracaoDiferenteContigencia;

            NumeroFiscal = usaNumeracaoDiferenteContigencia && ContingenciaAtiva() ?
                ++emissorFiscal.NumeroAtualContingencia : ++emissorFiscal.NumeroAtual;

            Serie = usaNumeracaoDiferenteContigencia && ContingenciaAtiva() ? 
                emissorFiscal.SerieContingencia : emissorFiscal.Serie;

            CodigoNumerico = GeraCodigoNumerico();
        }

        private bool ContingenciaAtiva()
        {
            return ContingenciaId != null && ContingenciaId != 0;
        }

        private int GeraCodigoNumerico()
        {
            var random = new Random().Next(1, 99999999);

            if (random == NumeroFiscal)
            {
                return GeraCodigoNumerico();
            }

            return random;
        }

        public void ComCupomFinalizado(CupomFiscalFinalizado cupomFiscalFinalizado)
        {
            CupomFiscalFinalizado = cupomFiscalFinalizado;
        }

        public bool NaoEstaCancelada()
        {
            return SituacaoFiscal != SituacaoFiscal.Cancelado;
        }

        public string NumeroChave => NumeroFiscal.ToString("D9");

        public bool IsPodeImprimir => SituacaoFiscal == SituacaoFiscal.Autorizada
                                      || SituacaoFiscal == SituacaoFiscal.AutorizadaSemInternet
                                      || SituacaoFiscal == SituacaoFiscal.Cancelado
                                      || SituacaoFiscal == SituacaoFiscal.AutorizadaDenegada;

        public bool IsPodeBaixarXml => SituacaoFiscal == SituacaoFiscal.Autorizada
                                       || SituacaoFiscal == SituacaoFiscal.Cancelado
                                       || SituacaoFiscal == SituacaoFiscal.AutorizadaDenegada;
                                       
        public bool IsPodeEnviarEmail => IsPodeImprimir;

        public bool IsPodeAlocarNumeracao => SituacaoFiscal == SituacaoFiscal.RejeicaoOffline
                                             || SituacaoFiscal == SituacaoFiscal.Rejeicao;

        public bool ImportadaParaNfe { get; private set; }


        public void ComRejeicaoOffline()
        {
            SituacaoFiscal = SituacaoFiscal.RejeicaoOffline;
        }

        public void ComRejeicao()
        {
            SituacaoFiscal = SituacaoFiscal.Rejeicao;
        }

        public void AlterarEmitirEm(DateTime emitirEm)
        {
            EmitirEm = emitirEm;
        }

        public bool EstaEmHomologacao()
        {
            return AmbienteSefaz == TipoAmbiente.Homologacao;
        }

        public void FoiImportadaParaNfe()
        {
            ImportadaParaNfe = true;
        }
    }
}