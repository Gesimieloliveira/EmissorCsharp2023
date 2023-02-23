using System;
using System.Reflection;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.EmissorFiscal;
using FusionCore.Repositorio.Base;
using NFe.Classes;
using NFe.Classes.Protocolo;
using NFe.Utils.NFe;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace FusionCore.FusionNfce.Fiscal
{
    public class NfceEmissao : Entidade
    {
        public int NfceId { get; set; }
        public Nfce Nfce { get; set; }
        public NfceEmissorFiscal EmissorFiscal { get; set; }
        public Versao Versao { get; set; }
        public short Serie { get; set; }
        public int NumeroDocumento { get; set; }
        public int CodigoNumerico { get; set; }
        public string Chave { get; set; }
        public string TagId { get; set; }
        public string VersaoAplicativo { get; set; }
        public TipoEmissao TipoEmissao { get; set; }
        public TipoAmbiente TipoAmbiente { get; set; }
        public short CodigoAutorizacao { get; set; }
        public string Protocolo { get; set; }
        public string DigestValue { get; set; }
        public ProcessoEmissao ProcessoEmissao { get; set; }
        public string VersaoAplicativoAutorizacao { get; set; }
        public DateTime? RecebidoEm { get; set; }
        public bool Autorizado { get; set; }
        public string XmlAutorizado { get; set; }
        public string JustificativaContingencia { get; set; }
        public DateTime? EntrouEmContingenciaEm { get; set; }

        private NfceEmissao()
        {
            Autorizado = false;
            ProcessoEmissao = ProcessoEmissao.NFeAplicativoContribuinte;
            VersaoAplicativo = Assembly.GetEntryAssembly().GetName().Version.ToString();
            CodigoNumerico = 0;
            Versao = Versao.V400;
            TipoEmissao = TipoEmissao.Normal;
            TipoAmbiente = TipoAmbiente.Homologacao;
            Protocolo = string.Empty;
            CodigoAutorizacao = default(int);
            DigestValue = string.Empty;
            VersaoAplicativoAutorizacao = string.Empty;
            Chave = string.Empty;
            JustificativaContingencia = string.Empty;
            TagId = string.Empty;
        }

        public NfceEmissao(Nfce nfce, int numeroDocumento, short serie, TipoAmbiente ambiente) : this()
        {
            Nfce = nfce;
            NumeroDocumento = numeroDocumento;
            Serie = serie;
            TipoAmbiente = ambiente;
        }

        public void AutorizaXml(NfceEmissaoHistorico historicoFinalizado)
        {
            TagId = $"NFe{Chave}";
            var recebimento = RecebidoEm ?? DateTime.Now;

            var zeus = new NFe.Classes.NFe().CarregarDeXmlString(historicoFinalizado.XmlEnvio.Valor);
            var nfeProc = new nfeProc();

            nfeProc.NFe = zeus;
            nfeProc.protNFe = new protNFe();
            nfeProc.versao = Versao.GetString();
            nfeProc.protNFe.versao = Versao.GetString();

            nfeProc.protNFe.infProt = new infProt();
            nfeProc.protNFe.infProt.dhRecbto = recebimento;
            nfeProc.protNFe.infProt.tpAmb = TipoAmbiente.ToZeus();
            nfeProc.protNFe.infProt.verAplic = VersaoAplicativoAutorizacao;
            nfeProc.protNFe.infProt.chNFe = Chave;
            nfeProc.protNFe.infProt.digVal = DigestValue;
            nfeProc.protNFe.infProt.cStat = CodigoAutorizacao;
            nfeProc.protNFe.infProt.nProt = Protocolo;

            if (historicoFinalizado.Motivo != null)
            {
                nfeProc.protNFe.infProt.xMotivo = historicoFinalizado.Motivo.Valor;
            }

            XmlAutorizado = nfeProc.ObterXmlString();
        }

        protected override int ReferenciaUnica => NfceId;
    }
}