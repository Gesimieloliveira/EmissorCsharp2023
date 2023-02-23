using System;
using FusionCore.Tributacoes.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Tipos
{
    public static class ConversorTributacaoCst
    {
        public static Csticms ToCstIcms(this TributacaoCst tributacao)
        {
            switch (tributacao.Id)
            {
                case "00": return Csticms.Cst00;
                case "10": return Csticms.Cst10;
                case "20": return Csticms.Cst20;
                case "30": return Csticms.Cst30;
                case "40": return Csticms.Cst40;
                case "41": return Csticms.Cst41;
                case "50": return Csticms.Cst50;
                case "51": return Csticms.Cst51;
                case "60": return Csticms.Cst60;
                case "70": return Csticms.Cst70;
                case "90": return Csticms.Cst90;
            }

            throw new InvalidOperationException($"Não foi possível utilizar o CST {tributacao.Id}");
        }
    }
}