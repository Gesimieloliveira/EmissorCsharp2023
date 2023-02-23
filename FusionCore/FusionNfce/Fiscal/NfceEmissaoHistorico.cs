using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.TyneTypes;
using FusionCore.Helpers.CloneSerializer;
using FusionCore.Helpers.DocumentoXml;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.Fiscal
{
    public class NfceEmissaoHistorico : Entidade
    {
        private readonly int _id;
        private readonly Nfce _nfce;
        private readonly XmlEnvio _xmlEnvio;
        private readonly XmlRetorno _xmlRetorno;
        private readonly Finalizou _finalizou = Finalizou.Default;
        private readonly CodigoAutorizacao _codigoAutorizacao = CodigoAutorizacao.Default;
        private readonly Motivo _motivo = Motivo.Default;
        private readonly Chave _chave;
        private readonly ChaveTexto _chaveTexto;
        private readonly TipoAmbiente _ambienteSefaz;
        private readonly Contingencia _contingencia;
        private readonly TentouEm _tentouEm = new TentouEm(DateTime.Now);
        private readonly Versao _versao;
        private readonly string _xmlLote;
        private readonly bool _falhaReceberLote;

        // ReSharper disable once UnusedMember.Local
        private NfceEmissaoHistorico() { }

        private NfceEmissaoHistorico(int id, Nfce nfce, XmlEnvio xmlEnvio, XmlRetorno xmlRetorno, 
            Chave chave, TipoAmbiente ambienteSefaz, Contingencia contingencia, ChaveTexto chaveTexto, 
            Versao versao, CodigoAutorizacao codigoAutorizacao, Finalizou finalizou,
            TentouEm tentouEm, Motivo motivo, string xmlLote, bool falhaReceberLote)
        {
            _id = id;
            _nfce = nfce;
            _xmlEnvio = xmlEnvio;
            _xmlRetorno = xmlRetorno;
            _chave = chave;
            _ambienteSefaz = ambienteSefaz;
            _contingencia = contingencia;
            _chaveTexto = chaveTexto;
            _versao = versao;
            _codigoAutorizacao = codigoAutorizacao;
            _finalizou = finalizou;
            _tentouEm = tentouEm;
            _motivo = motivo;
            _xmlLote = xmlLote;
            _falhaReceberLote = falhaReceberLote;
        }

        public int Id => _id;

        public Nfce Nfce => _nfce;

        public XmlEnvio XmlEnvio => _xmlEnvio;

        public XmlRetorno XmlRetorno => _xmlRetorno;

        public Finalizou Finalizou => _finalizou;

        public CodigoAutorizacao CodigoAutorizacao => _codigoAutorizacao;

        public Motivo Motivo => _motivo;

        public Chave Chave => _chave;

        public TipoAmbiente AmbienteSefaz => _ambienteSefaz;

        public Contingencia Contingencia => _contingencia;

        public TentouEm TentouEm => _tentouEm;

        public ChaveTexto ChaveTexto => _chaveTexto;

        public Versao Versao => _versao;

        public string XmlLote => _xmlLote;

        public bool FalhaReceberLote => _falhaReceberLote;

        protected override int ReferenciaUnica => Id;

        public Builder ToBuilder()
        {
            return new Builder(this, false);
        }

        public Builder ToBuilder(bool naoCopiar)
        {
            return new Builder(this, naoCopiar);
        }

        public sealed class Builder
        {
            public Builder()
            {
            }

            public Builder(NfceEmissaoHistorico nfceEmissaoHistorico) : this(nfceEmissaoHistorico, false) { }

            public Builder(NfceEmissaoHistorico nfceEmissaoHistorico, bool naoCopiar)
            {
                var nfceEmissaoHistoricoClone = nfceEmissaoHistorico.CopiaProfunda(null, naoCopiar);
                Id = nfceEmissaoHistoricoClone.Id;
                Finalizou = nfceEmissaoHistoricoClone.Finalizou;
                Motivo = nfceEmissaoHistoricoClone.Motivo;
                XmlEnvio = nfceEmissaoHistoricoClone.XmlEnvio;
                XmlRetorno = nfceEmissaoHistoricoClone.XmlRetorno;
                CodigoAutorizacao = nfceEmissaoHistoricoClone.CodigoAutorizacao;
                Nfce = nfceEmissaoHistoricoClone.Nfce;
                Chave = nfceEmissaoHistoricoClone.Chave;
                AmbienteSefaz = nfceEmissaoHistoricoClone.AmbienteSefaz;
                Contingencia = nfceEmissaoHistoricoClone.Contingencia;
                TentouEm = nfceEmissaoHistoricoClone.TentouEm;
                ChaveTexto = nfceEmissaoHistoricoClone.ChaveTexto;
                Versao = nfceEmissaoHistoricoClone.Versao;
                XmlLote = nfceEmissaoHistoricoClone.XmlLote;
                FalhaReceberLote = nfceEmissaoHistoricoClone.FalhaReceberLote;
            }

            private int Id { get; set; }
            private Nfce Nfce { get; set; }
            private XmlEnvio XmlEnvio { get; set; }
            private XmlRetorno XmlRetorno { get; set; }
            private Finalizou Finalizou { get; set; } = Finalizou.Default;
            private CodigoAutorizacao CodigoAutorizacao { get; set; } = CodigoAutorizacao.Default;
            private Motivo Motivo { get; set; } = Motivo.Default;
            private Chave Chave { get; set; }
            private TipoAmbiente AmbienteSefaz { get; set; }
            private Contingencia Contingencia { get; set; }
            private TentouEm TentouEm { get; set; }
            private ChaveTexto ChaveTexto { get; set; }
            private Versao Versao { get; set; }
            private string XmlLote { get; set; }
            private bool FalhaReceberLote { get; set; }

            public Builder ComNfce(Nfce nfce)
            {
                Nfce = nfce;
                return this;
            }

            public Builder ComXmlDeEnvio(XmlEnvio xmlEnvio)
            {
                XmlEnvio = xmlEnvio;
                return this;
            }

            public Builder ComXmlDeRetorno(XmlRetorno xmlRetorno)
            {
                XmlRetorno = xmlRetorno;
                return this;
            }

            public Builder Finalizar()
            {
                Finalizou = new Finalizou(true);
                return this;
            }

            public Builder ComCodigoDeAutorizacao(CodigoAutorizacao codigoAutorizacao)
            {
                CodigoAutorizacao = codigoAutorizacao;
                return this;
            }

            public Builder ComMotivo(Motivo motivo)
            {
                Motivo = motivo;
                return this;
            }

            public Builder ComChave(Chave chave)
            {
                Chave = chave;
                return this;
            }

            public Builder ComAmbiente(TipoAmbiente tipoAmbiente)
            {
                AmbienteSefaz = tipoAmbiente;
                return this;
            }

            public Builder ComContingencia(Contingencia contingencia)
            {
                Contingencia = contingencia;
                return this;
            }

            public Builder ComTentouEm(TentouEm tentouEm)
            {
                TentouEm = tentouEm;
                return this;
            }

            public Builder ComChaveTexto(ChaveTexto chaveTexto)
            {
                ChaveTexto = chaveTexto;
                return this;
            }

            public Builder ComVersao(Versao versao)
            {
                Versao = versao;
                return this;
            }

            public Builder ComId(int id)
            {
                Id = id;
                return this;
            }

            public Builder NaoFinalizou()
            {
                Finalizou = Finalizou.Default;
                return this;
            }

            public static implicit operator NfceEmissaoHistorico(Builder builder)
            {
                var nfceEmissaoHistorico = new NfceEmissaoHistorico(builder.Id, builder.Nfce,
                    builder.XmlEnvio, builder.XmlRetorno, builder.Chave, builder.AmbienteSefaz, builder.Contingencia, builder.ChaveTexto,
                    builder.Versao, builder.CodigoAutorizacao, builder.Finalizou, builder.TentouEm, builder.Motivo, builder.XmlLote, builder.FalhaReceberLote);

                return nfceEmissaoHistorico;
            }

            public Builder ComXmlLote(string xmlLote)
            {
                XmlLote = xmlLote;
                return this;
            }

            public Builder ComFalhaReceberLote(bool falhaReceberLote)
            {
                FalhaReceberLote = falhaReceberLote;
                return this;
            }
        }


        public string ObterRecibo()
        {
            if (XmlLote == null) return null;

            var xmlHelper = new XmlHelper(XmlLote);
            return xmlHelper.GetValueFromElement("nRec", "infRec").GetValueOrEmpty();
        }
    }
}