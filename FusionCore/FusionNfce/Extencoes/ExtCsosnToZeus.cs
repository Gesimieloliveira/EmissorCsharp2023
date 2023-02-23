using System;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Tributacoes.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtCsosnToZeus
    {

        public static Csticms ToCstZeus(this TributacaoCstNfce cst)
        {
            switch (cst.Id)
            {
                case "00": return Csticms.Cst00;
                case "20": return Csticms.Cst20;
                case "30": return Csticms.Cst30;
                case "40": return Csticms.Cst40;
                case "41": return Csticms.Cst41;
                case "60": return Csticms.Cst60;
                case "90": return Csticms.Cst90;
            }

            throw new InvalidCastException($"Não foi possível utilizar o CST: {cst}");
        }

        public static Csosnicms ToCsosnZeus(this TributacaoCstNfce cst)
        {
            switch (cst.Id)
            {
                case "101": return Csosnicms.Csosn101;
                case "102": return Csosnicms.Csosn102;
                case "103": return Csosnicms.Csosn103;
                case "201": return Csosnicms.Csosn201;
                case "202": return Csosnicms.Csosn202;
                case "203": return Csosnicms.Csosn203;
                case "300": return Csosnicms.Csosn300;
                case "400": return Csosnicms.Csosn400;
                case "500": return Csosnicms.Csosn500;
                case "900": return Csosnicms.Csosn900;
            }

            throw new InvalidCastException($"Não foi possível utilizar o CSOSN: {cst}");
        }

        public static Csticms ToCstZeus(this TributacaoIcms cst)
        {
            switch (cst.Codigo)
            {
                case "00": return Csticms.Cst00;
                case "20": return Csticms.Cst20;
                case "30": return Csticms.Cst30;
                case "40": return Csticms.Cst40;
                case "41": return Csticms.Cst41;
                case "60": return Csticms.Cst60;
                case "90": return Csticms.Cst90;
            }

            throw new InvalidCastException($"Não foi possível utilizar o CST: {cst}");
        }
    }
}