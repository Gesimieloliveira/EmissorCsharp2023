using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FusionCore.FusionAdm.ContingenciaSefaz;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.TyneTypes;
using FusionCore.Helpers.DocumentoXml;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using Remotion.Linq.Parsing;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.FusionAdm.Fiscal.NF.Autorizacao
{
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public class EmissaoNfe : Entidade, IEmissaoXml
    {
        public int NfeId { get; private set; }
        public int Id { get; private set; }
        protected override int ReferenciaUnica => Id;
        public EmpresaDTO Empresa { get; private set; }
        public EmissorFiscal EmissorFiscal { get; private set; }
        public string Cnpj { get; private set; }
        public string Cpf { get; private set; }
        public ChaveSefaz Chave { get; private set; }
        public bool FalhaReceberLote { get; set; }
        public bool Finalizada { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlLote { get; set; }
        public string XmlAutorizacao { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public DateTime? EnviadoEm { get; set; }
        public TipoAmbiente Ambiente { get; private set; }
        public string CodigoNumericoString => Chave.GetCodigoNumerico().ToString("D8");
        public int DigitoChave => Chave.Dv;
        public TipoEmissao TipoEmissao { get; private set; }
        public string VersaoDocumento { get; } = "4.00";
        public string TagId => "NFe" + Chave.Chave;
        public DateTime? InicioContingencia { get; private set; }
        public string MotivoContingencia { get; private set; } = string.Empty;

        private EmissaoNfe()
        {
            //nhibernate
        }

        public EmissaoNfe(Nfeletronica nfe, EmissorFiscal emissor, TipoAmbiente ambiente, ChaveSefaz chave) : this()
        {
            NfeId = nfe.Id;
            Empresa = nfe.Emitente.Empresa;
            Cnpj = nfe.Emitente.Cnpj;
            Cpf = nfe.Emitente.Cpf;
            CriadoEm = DateTime.Now;
            EmissorFiscal = emissor;
            TipoEmissao = TipoEmissao.Normal;
            Ambiente = ambiente;
            Chave = chave;
        }

        public string GetRecibo()
        {
            if (XmlLote == null) return null;

            var xmlHelper = new XmlHelper(XmlLote);
            return xmlHelper.GetValueFromElement("nRec", "infRec").GetValueOrEmpty();
        }

        public string GetProtocolo()
        {
            if (XmlAutorizacao == null) return null;

            return new XmlHelper(XmlAutorizacao)
                .GetValueFromElement("nProt", "infProt")
                .GetValueOrEmpty();
        }

        public string GetDigestValue()
        {
            if (XmlAutorizacao == null) return null;

            return new XmlHelper(XmlAutorizacao)
                .GetValueFromElement("digVal", "infProt")
                .GetValueOrEmpty();
        }

        public DateTime GetDataRecebimento()
        {
            if (XmlAutorizacao == null)
                throw new InvalidOperationException("Xml autorização não possui data recebimento");

            return new XmlHelper(XmlAutorizacao)
                .GetValueFromElement("dhRecbto", "infProt")
                .GetValueOrDefault<DateTime>();
        }

        public bool HouveTentativaAutorizacao()
        {
            return FalhaReceberLote || EnviadoEm != null || XmlAutorizacao != null;
        }

        public bool PossuiRecibo()
        {
            return !string.IsNullOrWhiteSpace(GetRecibo());
        }

        public void ProcessarRespotaLote(XmlNode resposta)
        {
            var xmlHelper = new XmlHelper(resposta.OuterXml);
            var statusLote = xmlHelper.GetValueFromElement("cStat", "retConsReciNFe").GetValueOrDefault<int>();
            var cStat = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            XmlAutorizacao = resposta.OuterXml;
            Finalizada = statusLote == 104 || cStat == 100;
        }

        public void ProcessarRespostaPelaChave(XmlNode resposta)
        {
            XmlAutorizacao = resposta.OuterXml;
            Finalizada = true;
        }

        public bool IsAutorizadoUsoDaEmissao()
        {
            if (Finalizada == false || XmlAutorizacao == null)
                return false;

            var xmlHelper = new XmlHelper(XmlAutorizacao);
            var statusAtorizacao = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            return statusAtorizacao == 100;
        }

        public bool IsDenegadoUsoDaEmissao()
        {
            if (Finalizada == false || XmlAutorizacao == null)
                return false;

            var xmlHelper = new XmlHelper(XmlAutorizacao);
            var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            return status == 302 || status == 301 || status == 303;
        }

        public bool IsRejeicao999()
        {
            var xmlHelper = new XmlHelper(XmlAutorizacao);
            var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            return status == 999;
        }

        public string GetTextoRejeicao()
        {
            if (XmlAutorizacao == null)
                return "Emissão não possui detalhes de autorização/rejeição";

            var xmlHelper = new XmlHelper(XmlAutorizacao);

            var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();
            var motivo = xmlHelper.GetValueFromElement("xMotivo", "infProt").GetValueOrDefault<string>();

            if (status != 0)
                return $"{status} - {motivo}";

            status = xmlHelper.GetValueFromElement("cStat").GetValueOrDefault<int>();
            motivo = xmlHelper.GetValueFromElement("xMotivo").GetValueOrDefault<string>();

            return $"{status} - {motivo}";
        }

        public int GetStatusRejeicao()
        {
            if (XmlAutorizacao == null)
                return 0;

            var xmlHelper = new XmlHelper(XmlAutorizacao);

            var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

            if (status != 0)
                return status;

            status = xmlHelper.GetValueFromElement("cStat").GetValueOrDefault<int>();

            return status;
        }

        public void DefinirContingencia(ContingenciaNfe contingencia)
        {
            TipoEmissao = contingencia.TipoEmissao;
            InicioContingencia = contingencia.IniciadaEm;
            MotivoContingencia = contingencia.Justificativa;
        }

        public bool ContingenciaAtivada()
        {
            return InicioContingencia != null;
        }
    }
}