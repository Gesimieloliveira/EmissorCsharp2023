using FusionCore.Extencoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;

namespace FusionCore.Core.Nfes.Xml.Componentes.Impl
{
    public class XmlCofins
    {
        public XmlCofins(COFINS zcofins)
        {
            if (zcofins.TipoCOFINS is COFINSNT nt)
            {
                Cst = nt.CST.GetCodigo();
            }
            else if (zcofins.TipoCOFINS is COFINSAliq aliq)
            {
                Cst = aliq.CST.GetCodigo();
                Aliquota = aliq.pCOFINS;
                ValorBc = aliq.vBC;
                Valor = aliq.vCOFINS;
            }
            else if (zcofins.TipoCOFINS is COFINSOutr outros)
            {
                Cst = outros.CST.GetCodigo();
                Aliquota = outros.pCOFINS ?? 0.00M;
                ValorBc = outros.vBC ?? 0.00M;
                Valor = outros.vCOFINS ?? 0.00M;
            }
            else if (zcofins.TipoCOFINS is COFINSQtde qtde)
            {
                Cst = qtde.CST.GetCodigo();
            }
        }

        public string Cst { get; }
        public decimal Aliquota { get; }
        public decimal ValorBc { get; }
        public decimal Valor { get; }
    }
}
