using System;
using DFe.Utils;
using FusionCore.DFe.XmlCte;
using FusionCore.DFe.XmlCte.XmlCte.Processada;
using FusionCore.FusionAdm.CteEletronico.Flags.Extencoes;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteEmissao
    {
        public int CteId { get; set; }
        public Cte Cte { get; set; }
        public int CodigoNumerico { get; set; }
        public ModeloDocumento Modelo { get; set; }
        public short Serie { get; set; }
        public int Numero { get; set; }
        public DateTime? EmitidaEm { get; set; }
        public byte DigitoVerificador { get; set; }
        public TipoAmbiente Ambiente { get; set; }
        public string Chave { get; set; }
        public bool Autorizado { get; set; }
        public string TagId { get; set; }
        public string XmlAssinado { get; set; }
        public short CodigoAutorizacao { get; set; }
        public DateTimeOffset? RecebidoEm { get; set; } 
        public string VersaoAplicativoAutorizacao { get; set; }
        public string DigestValue { get; set; }
        public string Protocolo { get; set; }
        public string XmlAutorizado { get; set; }
        public string NumeroRecibo { get; set; }
        public string Motivo { get; set; }
        public CteEmissaoStatus StatusConsultaRecibo { get; set; } = CteEmissaoStatus.Vazio;

        private CteEmissao()
        {
            Autorizado = false;
            CodigoNumerico = GetRandom();
            Ambiente = TipoAmbiente.Homologacao;
            Modelo = ModeloDocumento.CTe;
            DigestValue = string.Empty;
            Protocolo = string.Empty;
            VersaoAplicativoAutorizacao = string.Empty;
            NumeroRecibo = string.Empty;
            Motivo = string.Empty;
        }

        public CteEmissao(Cte cte, int numeroAtual, short serie, TipoAmbiente ambiente) : this()
        {
            Cte = cte;
            Numero = numeroAtual;
            Serie = serie;
            Ambiente = ambiente;
        }

        private static int GetRandom()
        {
            var rand = new Random();
            return rand.Next(11111111, 99999999);
        }

        public void AutonrizaXml()
        {
            if (Autorizado == false)
                throw new ArgumentException(
                    "Nao foi possível gerar XML de Autorização devido a CHAVE não estar autorizada");

            var recebimento = RecebidoEm ?? DateTime.Now;

            var cte = FuncoesXml.XmlStringParaClasse<FusionCTe>(XmlAssinado);

            var cteProc = new FusionCTeProcessamento
            {
                CTe = cte,
                Versao = "3.00",
                Protocolo = 
                {
                    InformacaoProtocolo =
                    {
                        Ambiente = Ambiente.ToXml(),
                        VersaoAplicativo = VersaoAplicativoAutorizacao,
                        Chave = Chave,
                        RecebidaEm = recebimento,
                        Protocolo = Protocolo,
                        DigestValue = DigestValue,
                        CodigoStatus = CodigoAutorizacao,
                        Motivo = Motivo
                    }
                }
            };

            XmlAutorizado = FuncoesXml.ClasseParaXmlString(cteProc);
        }
    }
}