using System.ComponentModel;
using System.Text;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.TextEncoding
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoEncoding
    {
        [Description("Default")]
        Default = 1,

        [Description("UTF8")]
        UTF8 = 2,

        [Description("ASCII")]
        ASCII = 3,

        [Description("Unicode")]
        Unicode = 4
    }

    public static class TipoEncodingExt
    {
        public static Encoding ToSystemEncoding(this TipoEncoding tipoEncoding)
        {
            switch (tipoEncoding)
            {
                case TipoEncoding.Default: return Encoding.Default;
                case TipoEncoding.UTF8: return Encoding.UTF8;
                case TipoEncoding.ASCII: return Encoding.ASCII;
                case TipoEncoding.Unicode: return Encoding.Unicode;
                default:return Encoding.Default;
            }
        }
    }
}