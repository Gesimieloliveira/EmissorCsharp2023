using System;
using System.Xml;
using DFe.Utils;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Helpers.DocumentoXml;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Base;
using NFe.Classes;
using NFe.Classes.Protocolo;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class CupomFiscalHistorico : EntidadeBase<int>
    {
        private CupomFiscalHistorico()
        {
            // nhibernate
            CriadoEm = DateTime.Now;
        }

        public CupomFiscalHistorico(CupomFiscal cupomFiscal
            , int numeroFiscal, short serie
            , int codigoNumerico, string chave
            , TipoAmbiente ambienteSefaz) : this()
        {
            CupomFiscal = cupomFiscal;
            NumeroFiscal = numeroFiscal;
            Serie = serie;
            CodigoNumerico = codigoNumerico;
            Chave = chave;
            AmbienteSefaz = ambienteSefaz;
        }

        public int Id { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public int NumeroFiscal { get; private set; }
        public short Serie { get; private set; }
        public int CodigoNumerico { get; private set; }
        public TipoAmbiente AmbienteSefaz { get; private set; }
        public DateTime TentouEm { get; private set; }
        public bool FalhaEnvioLote { get; private set; }
        public bool Finalizado { get; private set; }
        public string Chave { get; private set; }
        public string RespostaLote { get; private set; }
        public string Envio { get; private set; }
        public string Resposta { get; private set; }
        public CupomFiscal CupomFiscal { get; private set; }

        public void FalhouNoEnvioDeLote()
        {
            FalhaEnvioLote = true;
        }

        public void FoiFinalizado()
        {
            Finalizado = true;
        }

        public void ComRespostaLote(string xml)
        {
            RespostaLote = xml;
        }

        public void ComXmlEnvio(string xml)
        {
            Envio = xml;
        }

        protected override int ChaveUnica => Id;

        public void TentativaEm(DateTime tentouEm)
        {
            TentouEm = tentouEm;
        }

        public bool PossuiRecibo()
        {
            return !string.IsNullOrWhiteSpace(GetRecibo());
        }

        public string GetRecibo()
        {
            if (RespostaLote.IsNullOrEmpty()) return null;

            var xmlHelper = new XmlHelper(RespostaLote);
            return xmlHelper.GetValueFromElement("nRec", "infRec").GetValueOrEmpty();
        }

        public void ProcessarRespotaLote(XmlNode xmlLote)
        {
            var xmlHelper = new XmlHelper(xmlLote.OuterXml);
            var statusLote = xmlHelper.GetValueFromElement("cStat", "retConsReciNFe").GetValueOrDefault<int>();
            var cStat = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            Resposta = xmlLote.OuterXml;
            Finalizado = statusLote == 104 || cStat == 100;
        }

        public void ProcessarRespostaSincrona(XmlNode xmlSincrono)
        {
            var xmlHelper = new XmlHelper(xmlSincrono.OuterXml);
            var statusLote = xmlHelper.GetValueFromElement("cStat", "retEnviNFe").GetValueOrDefault<int>();
            var cStat = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            Resposta = xmlSincrono.OuterXml;
            Finalizado = statusLote == 104 || cStat == 100;
        }

        public void ProcessarRespostaConsultaProtocolo(XmlNode xmlConsulta)
        {

        }

        public bool IsRejeicao999()
        {
            var xmlHelper = new XmlHelper(Resposta);
            var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            return status == 999;
        }

        public string GetTextoRejeicao()
        {
            if (Resposta == null)
                return "Emissão não possui detalhes de autorização/rejeição";

            var xmlHelper = new XmlHelper(Resposta);

            var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();
            var motivo = xmlHelper.GetValueFromElement("xMotivo", "infProt").GetValueOrDefault<string>();

            if (status != 0)
                return $"{status} - {motivo}";

            status = xmlHelper.GetValueFromElement("cStat").GetValueOrDefault<int>();
            motivo = xmlHelper.GetValueFromElement("xMotivo").GetValueOrDefault<string>();

            return $"{status} - {motivo}";
        }

        public void ProcessarRespostaPelaChave(XmlNode respostaProtocolo)
        {
            Resposta = respostaProtocolo.OuterXml;
            Finalizado = true;
        }

        public bool Autorizado()
        {
            if (Finalizado == false || Resposta == null)
                return false;

            var xmlHelper = new XmlHelper(Resposta);
            var statusAtorizacao = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            return statusAtorizacao == 100;
        }

        public string ObterProtocolo()
        {
            if (Resposta == null) return null;

            return new XmlHelper(Resposta)
                .GetValueFromElement("nProt", "infProt")
                .GetValueOrEmpty();
        }

        public DateTime ObterDataReciboEm()
        {
            if (Resposta == null)
                throw new InvalidOperationException("Xml autorização não possui data recebimento");

            return new XmlHelper(Resposta)
                .GetValueFromElement("dhRecbto", "infProt")
                .GetValueOrDefault<DateTime>();
        }

        public string MontarXmlProcessado()
        {
            try
            {
                var xmlHelper = new XmlHelper(Resposta);
                var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();
                var verAplic = xmlHelper.GetValueFromElement("verAplic", "infProt").GetValueOrDefault<string>();
                var xMotivo = xmlHelper.GetValueFromElement("xMotivo", "infProt").GetValueOrDefault<string>();

                var nfe = FuncoesXml.XmlStringParaClasse<global::NFe.Classes.NFe>(Envio);

                var nfeProc = new nfeProc
                {
                    versao = "4.00",
                    NFe = nfe,
                    protNFe = new protNFe
                    {
                        versao = "4.00",
                        infProt = new infProt
                        {
                            tpAmb = AmbienteSefaz.ToZeus(),
                            chNFe = Chave,
                            dhRecbto = ObterDataReciboEm(),
                            nProt = ObterProtocolo(),
                            digVal = ObterDigestValue(),
                            cStat = status,
                            verAplic = verAplic,
                            xMotivo = xMotivo
                        }
                    }
                };

                return FuncoesXml.ClasseParaXmlString(nfeProc);
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Falha ao criar XML da Nota Autorizada: " + ex.Message, ex);
            }
        }

        private string ObterDigestValue()
        {
            if (Resposta == null) return null;

            return new XmlHelper(Resposta)
                .GetValueFromElement("digVal", "infProt")
                .GetValueOrEmpty();
        }

        public bool Denegado()
        {
            if (Finalizado == false || Resposta == null)
                return false;

            var xmlHelper = new XmlHelper(Resposta);
            var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            return status == 302 || status == 301 || status == 303;
        }

        public void ThrowInvalidOperationOutrasRejeicoes()
        {
            if (Autorizado() || Denegado()) return;

            throw new InvalidOperationException(GetTextoRejeicao());
        }

        public void AtualizaCupomFiscal(CupomFiscal cupomFiscal)
        {
            CupomFiscal = cupomFiscal;
        }

        public void AtualizarXmlEnvio(string xmlEnvio)
        {
            Envio = xmlEnvio;
            Chave = FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(xmlEnvio).ObterChaveNfeZeus();
        }

        public void AlteraChave(string chave)
        {
            Chave = chave;
        }

        public void NaoFinalizado()
        {
            Finalizado = false;
        }
    }
}