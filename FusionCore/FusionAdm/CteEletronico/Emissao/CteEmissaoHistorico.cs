using System;
using System.Xml;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Helpers.DocumentoXml;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Base;
using TipoEmissao = FusionCore.FusionAdm.CteEletronico.Flags.TipoEmissao;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteEmissaoHistorico : EntidadeBase<int>
    {
        public int Id { get; set; }
        public Cte Cte { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public TipoEmissao TipoEmissao { get; set; }
        public string Chave { get; set; }
        public bool Finalizada { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
        public DateTime CriadaEm { get; set; }
        public DateTime? EnviadaEm { get; set; }
        public string NumeroRecibo { get; set; }
        public string XmlLote { get; set; }
        public bool FalhaReceberLote { get; set; }
        public CteEmissaoStatus StatusConsultaRecibo { get; set; } = CteEmissaoStatus.Vazio;
        public short CodigoAutorizacao { get; set; }

        protected override int ChaveUnica => Id;
        public DateTime? RecebidoEm { get; set; }
        public string Motivo { get; set; } = string.Empty;

        public byte ObterDigitoVerificador()
        {
            var digitoVerificador = Chave.Substring(Chave.Length - 1, 1);
            return Convert.ToByte(digitoVerificador);
        }

        public bool HouveTentativaAutorizacao()
        {
            return FalhaReceberLote || EnviadaEm != null || XmlLote != null;
        }

        public bool PossuiRecibo()
        {
            return NumeroRecibo.IsNotNullOrEmpty();
        }

        public bool IsAutorizadoUsoDaEmissao()
        {
            if (Finalizada == false || XmlRetorno == null)
                return false;

            var xmlHelper = new XmlHelper(XmlRetorno);
            var statusAtorizacao = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            return statusAtorizacao == 100 || statusAtorizacao == 150 || statusAtorizacao == 110;
        }

        public bool IsDenegadoUsoDaEmissao()
        {
            if (Finalizada == false || XmlRetorno == null)
                return false;

            var xmlHelper = new XmlHelper(XmlRetorno);
            var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            return status == 204;
        }

        public bool IsRejeicao999()
        {
            var xmlHelper = new XmlHelper(XmlRetorno);
            var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            return status == 999;
        }

        public string GetTextoRejeicao()
        {
            if (XmlRetorno == null)
                return "Emissão não possui detalhes de autorização/rejeição";

            var xmlHelper = new XmlHelper(XmlRetorno);

            var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();
            var motivo = xmlHelper.GetValueFromElement("xMotivo", "infProt").GetValueOrDefault<string>();

            if (status != 0)
                return $"{status} - {motivo}";

            status = xmlHelper.GetValueFromElement("cStat").GetValueOrDefault<int>();
            motivo = xmlHelper.GetValueFromElement("xMotivo").GetValueOrDefault<string>();

            return $"{status} - {motivo}";
        }

        public void ProcessarRespotaLote(XmlNode respostaConsulta)
        {
            var xmlHelper = new XmlHelper(respostaConsulta.OuterXml);
            var statusLote = xmlHelper.GetValueFromElement("cStat", "retConsReciCTe").GetValueOrDefault<int>();
            var cStat = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();
            var dhRecbto = xmlHelper.GetValueFromElement("dhRecbto", "infProt").GetValueOrDefault<DateTime?>();

            RecebidoEm = dhRecbto;
            XmlRetorno = respostaConsulta.OuterXml;
            Finalizada = statusLote == 104 || cStat == 100;
            Motivo = GetTextoRejeicao();
        }

        public void ProcessarRespostaPelaChave(XmlNode respostaChave)
        {
            XmlRetorno = respostaChave.OuterXml;
            Finalizada = true;
        }

        public string ObterProtocolo()
        {
            var xmlHelper = new XmlHelper(XmlRetorno);

            return xmlHelper.GetValueFromElement("nProt", "infProt").GetValueOrDefault<string>();
        }

        public string ObterDigestValue()
        {
            var xmlHelper = new XmlHelper(XmlRetorno);

            return xmlHelper.GetValueFromElement("digVal", "infProt").GetValueOrDefault<string>();
        }

        public string ObterVersaoAplicativoAutorizacao()
        {
            var xmlHelper = new XmlHelper(XmlRetorno);

            return xmlHelper.GetValueFromElement("verAplic", "infProt").GetValueOrDefault<string>();
        }
    }
}