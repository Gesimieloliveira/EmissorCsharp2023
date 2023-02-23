using System;
using System.Reflection;
using System.Xml;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Helpers.DocumentoXml;
using FusionCore.Helpers.Log;
using log4net;

namespace FusionCore.FusionAdm.Fiscal.NF.Autorizacao
{
    public class RespostaAutorizacao
    {
        private static readonly ILog _log = FusionLog.GetLogger(MethodBase.GetCurrentMethod());

        public int CodigoSolicitacao { get; private set; }
        public string TextoSolicitacao { get; private set; }
        public TipoAmbiente Ambiente { get; private set; }
        public string VersaoAplicacao { get; private set; }
        public string Chave { get; private set; }
        public string NumeroProtocolo { get; private set; }
        public DateTime? DataHoraRecebimento { get; private set; }
        public string DigestValue { get; set; }
        public short CodigoAutorizacao { get; private set; }
        public string TextoAutorizacao { get; private set; }
        public XmlNode XmlProtocolo { get; private set; }
        public bool Autorizado => CodigoAutorizacao == 100 ||
            CodigoAutorizacao == 150
            || CodigoAutorizacao == 110;

        public bool Denegado => CodigoAutorizacao == 302 
                                || CodigoAutorizacao == 301 
                                || CodigoAutorizacao == 303;

        public bool Rejeicao999 => CodigoAutorizacao == 999 || CodigoSolicitacao == 999;
        public bool Rejeicao => EUmaRejeicaoComum();
        public bool LoteEmProcessamento => CodigoSolicitacao == 105;

        private bool EUmaRejeicaoComum()
        {
            return Autorizado == false
                   && Denegado == false
                   && Rejeicao999 == false;
        }

        public RespostaAutorizacao()
        {
        }

        public RespostaAutorizacao(int codigoSolicitado, string textoSolicitado)
        {
            CodigoSolicitacao = codigoSolicitado;
            TextoSolicitacao = textoSolicitado;
        }

        public static RespostaAutorizacao Load(XmlNode xml)
        {
            _log.Info($"Load => xml => {xml}");

            var resposta = new RespostaAutorizacao();

            var h = new XmlHelper(xml.OuterXml);
            _log.Info($"Load => xml.OuterXml => {xml.OuterXml}");

            resposta.XmlProtocolo = xml;
            _log.Info($"Load => resposta.XmlProtocolo => {xml}");

            resposta.CodigoSolicitacao = h.GetValueFromElement("cStat", "retEnviNFe").GetValueOrDefault<int>();
            if (resposta.CodigoSolicitacao == 0)
                resposta.CodigoSolicitacao = h.GetValueFromElement("cStat", "retConsReciNFe").GetValueOrDefault<int>();
            _log.Info($"Load => resposta.CodigoSolicitacao => {resposta.CodigoSolicitacao}");

            resposta.TextoSolicitacao = h.GetValueFromElement("xMotivo", "retEnviNFe").GetValueOrEmpty();
            if (string.IsNullOrEmpty(resposta.TextoSolicitacao))
                resposta.TextoSolicitacao = h.GetValueFromElement("xMotivo", "retConsReciNFe").GetValueOrEmpty();
            _log.Info($"Load => resposta.TextoSolicitacao => {resposta.TextoSolicitacao}");

            resposta.Ambiente = h.GetValueFromElement("tpAmb", "infProt").GetValueOrDefault<TipoAmbiente>();
            _log.Info($"Load => resposta.Ambiente => {resposta.Ambiente}");

            resposta.VersaoAplicacao = h.GetValueFromElement("verAplic", "infProt").GetValueOrEmpty();
            _log.Info($"Load => resposta.VersaoAplicacao => {resposta.VersaoAplicacao}");

            resposta.Chave = h.GetValueFromElement("chNfe", "infProt").GetValueOrEmpty();
            _log.Info($"Load => resposta.Chave => {resposta.Chave}");

            resposta.NumeroProtocolo = h.GetValueFromElement("nProt", "infProt").GetValueOrEmpty();
            _log.Info($"Load => resposta.NumeroProtocolo => {resposta.NumeroProtocolo}");

            resposta.DigestValue = h.GetValueFromElement("digVal", "infProt").GetValueOrEmpty();
            _log.Info($"Load => resposta.DigestValue => {resposta.DigestValue}");

            resposta.CodigoAutorizacao = h.GetValueFromElement("cStat", "infProt").GetValueOrDefault<short>();
            _log.Info($"Load => resposta.CodigoAutorizacao => {resposta.CodigoAutorizacao}");

            resposta.TextoAutorizacao = h.GetValueFromElement("xMotivo", "infProt").GetValueOrEmpty();
            _log.Info($"Load => resposta.TextoAutorizacao => {resposta.TextoAutorizacao}");

            var dataReceb = h.GetValueFromElement("dhRecbto", "infProt");
            _log.Info($"Load => dataReceb => {dataReceb}");

            if (dataReceb.HasValue)
            {
                resposta.DataHoraRecebimento = dataReceb.GetValueOrDefault<DateTime>();
                _log.Info($"Load => resposta.DataHoraRecebimento => {resposta.DataHoraRecebimento}");
            }

            return resposta;
        }
    }
}