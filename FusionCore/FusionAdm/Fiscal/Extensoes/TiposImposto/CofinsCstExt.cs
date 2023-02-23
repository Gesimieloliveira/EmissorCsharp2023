using FusionCore.FusionAdm.Fiscal.FlagsImposto;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.TiposImposto
{
    public static class CofinsCstExt
    {
        public static string GetCodigoCst(this CofinsCst cst)
        {
            var code = ((int) cst).ToString("00");
            return code;
        }

        public static bool Isento(this CofinsCst cst)
        {
            return false;
        }
    }
}