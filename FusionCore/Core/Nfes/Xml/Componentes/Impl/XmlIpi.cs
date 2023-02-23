using FusionCore.Extencoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;

namespace FusionCore.Core.Nfes.Xml.Componentes.Impl
{
    public class XmlIpi
    {
        public XmlIpi(IPI ipi)
        {
            if (ipi.TipoIPI is IPINT ipiNt)
            {
                Cst = ipiNt.CST.GetCodigo();
            }
            else if (ipi.TipoIPI is IPITrib ipiTribu)
            {
                Cst = ipiTribu.CST.GetCodigo();
                Aliquota = ipiTribu.pIPI ?? 0.00M;
                ValorBc = ipiTribu.vBC ?? 0.00M;
                Valor = ipiTribu.vIPI ?? 0.00M;
            }
        }

        public string Cst { get; }
        public decimal Aliquota { get; }
        public decimal ValorBc { get; }
        public decimal Valor { get; }
    }

    public class XmlPis
    {
        public XmlPis(PIS zpis)
        {
            if (zpis.TipoPIS is PISNT pisNt)
            {
                Cst = pisNt.CST.GetCodigo();
            }
            else if (zpis.TipoPIS is PISAliq pisAliq)
            {
                Cst = pisAliq.CST.GetCodigo();
                Aliquota = pisAliq.pPIS;
                ValorBc = pisAliq.vBC;
                Valor = pisAliq.vPIS;
            }
            else if (zpis.TipoPIS is PISOutr pisOutr)
            {
                Cst = pisOutr.CST.GetCodigo();
                Aliquota = pisOutr.pPIS ?? 0.00M;
                ValorBc = pisOutr.vBC ?? 0.00M;
                Valor = pisOutr.vPIS ?? 0.00M;
            }
            else if (zpis.TipoPIS is PISQtde pisQtde)
            {
                Cst = pisQtde.CST.GetCodigo();
            }
        }

        public string Cst { get; }
        public decimal Aliquota { get; }
        public decimal ValorBc { get; }
        public decimal Valor { get; }
    }
}