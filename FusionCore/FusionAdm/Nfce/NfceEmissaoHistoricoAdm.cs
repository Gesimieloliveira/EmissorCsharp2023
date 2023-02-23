using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.Nfce
{
    public class NfceEmissaoHistoricoAdm : Entidade
    {
        public int Id { get; set; }
        public NfceAdm Nfce { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
        public bool Finalizou { get; set; }
        public short CodigoAutorizacao { get; set; }
        public string Motivo { get; set; }
        public string ChaveTexto { get; set; }
        public byte CodigoIbgeUf { get; set; }
        public DateTime AnoMes { get; set; }
        public string CnpjEmitente { get; set; }
        public ModeloDocumento ModeloDocumento { get; set; }
        public short Serie { get; set; }
        public long NumeroFiscal { get; set; }
        public TipoEmissao TipoEmissao { get; set; }
        public int CodigoNumerico { get; set; }
        public short DigitoVerificador { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public string JustificativaContingencia { get; set; }
        public DateTime? EntrouEmContingenciaEm { get; set; }
        public DateTime TentouEm { get; set; }
        public Versao Versao { get; set; }
        public string XmlLote { get; set; }
        public bool FalhaReceberLote { get; set; }

        protected override int ReferenciaUnica => Id;
    }
}