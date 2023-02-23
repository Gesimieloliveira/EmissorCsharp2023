using FusionCore.FusionAdm.Fiscal.FlagsImposto;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.TiposImposto
{
    public static class CsosnCstExt
    {
        public static string GetCodigoCst(this CsosnCst cst)
        {
            var codigo = ((int)cst).ToString("000");
            return codigo;
        }
    }
}