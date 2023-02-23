using System;
using DFe.Utils;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using MDFe.Classes.Retorno;
using MDFe.Classes.Retorno.MDFeRetRecepcao;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeEmissao
    {
        public int MDFeId { get; set; }
        public MDFeEletronico MDFeEletronico { get; set; }
        public string VersaoLayout { get; set; }
        public string TagId { get; set; }
        public TipoAmbiente Ambiente { get; set; }
        public short Serie { get; set; }
        public int NumeroDocumento { get; set; }
        public int CodigoNumerico { get; set; }
        public byte DigitoVerificador { get; set; }
        public DateTime EmitidoEm { get; set; }
        public MDFeTipoEmissao TipoEmissao { get; set; }
        public MDFeModeloManifesto ModeloManifesto { get; set; }
        public bool Autorizado { get; set; }
        public string XmlAssinado { get; set; }
        public short CodigoAutorizacao { get; set; }
        public string Chave { get; set; }
        public string VersaoAplicativoAutorizacao { get; set; }
        public string DigestValue { get; set; }
        public string Protocolo { get; set; }
        public string XmlAutorizado { get; set; }
        public string NumeroRecibo { get; set; }
        public string Motivo { get; set; }
        public DateTime? RecebidoEm { get; set; }

        public MDFeEmissao()
        {
            Autorizado = false;
            CodigoNumerico = GetRandom();
            Ambiente = TipoAmbiente.Homologacao;
            TipoEmissao = MDFeTipoEmissao.Normal;
            ModeloManifesto = MDFeModeloManifesto.MDFe;
            DigestValue = string.Empty;
            Protocolo = string.Empty;
            VersaoAplicativoAutorizacao = string.Empty;
            NumeroRecibo = string.Empty;
            Motivo = string.Empty;
            VersaoLayout = string.Empty;
            TagId = string.Empty;
            Chave = string.Empty;
        }

        public MDFeEmissao(MDFeEletronico mdfe, int numeroAtual, short serie, TipoAmbiente ambiente) : this()
        {
            MDFeEletronico = mdfe;
            NumeroDocumento = numeroAtual;
            Serie = serie;
            Ambiente = ambiente;
        }

        private static int GetRandom()
        {
            var rand = new Random();
            return rand.Next(11111111, 99999999);
        }

        public void AutonrizaXml(MDFeProtMDFe protMdfe)
        {
            var mdfe = MDFe.Classes.Informacoes.MDFe.LoadXmlString(XmlAssinado);

            var procMdfe = new MDFeProcMDFe
            {
                MDFe = mdfe,
                ProtMDFe = protMdfe,
                Versao = mdfe.InfMDFe.Versao
            };

            XmlAutorizado = FuncoesXml.ClasseParaXmlString(procMdfe);
        }
    }
}