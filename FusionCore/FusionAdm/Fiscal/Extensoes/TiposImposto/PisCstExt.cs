using FusionCore.FusionAdm.Fiscal.FlagsImposto;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.TiposImposto
{
    public static class PisCstExt
    {
        public static string GetCodigoCst(this PisCst cst)
        {
            var code = ((int) cst).ToString("00");
            return code;
        }
    }
}