using System;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.Fiscal.NF.CCe
{
    public sealed class CartaCorrecaoNfe
    {
        public int Id { get; set; }
        public Nfeletronica Nfe { get; set; }
        public DateTime? OcorreuEm { get; set; }
        public string Correcao { get; set; }
        public int StatusRetorno { get; set; }
        public string Protocolo { get; set; }
        public byte SequenciaEvento { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }

        private CartaCorrecaoNfe()
        {
            StatusRetorno = 0;
            SequenciaEvento = 0;
            OcorreuEm = DateTime.Now;
            Protocolo = string.Empty;
            XmlEnvio = null;
            XmlRetorno = null;
        }

        public CartaCorrecaoNfe(Nfeletronica nfe, string correcao) :
            this()
        {
            Nfe = nfe;
            Correcao = correcao;
        }
    }
}