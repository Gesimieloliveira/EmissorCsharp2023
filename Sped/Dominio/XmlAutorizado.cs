using DFe.Utils;
using FusionCore.DFe.XmlCte.XmlCte.Processada;

namespace Sped.Dominio
{
    public class XmlAutorizado
    {
        public string Chave { get; set; }
        public string Xml { get; set; }
        public short SituacaoCodigo { get; set; }
        public FusionCTeProcessamento FusionCTe => FuncoesXml.XmlStringParaClasse<FusionCTeProcessamento>(Xml);

        public bool IsCancelado()
        {
            return SituacaoCodigo == 135 || SituacaoCodigo == 136 || SituacaoCodigo == 134;
        }
    }
}