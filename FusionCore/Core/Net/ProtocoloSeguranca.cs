using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Core.Net
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum ProtocoloSeguranca
    {
        [Description("SSL 3.0")]
        Ssl3 = 0,

        [Description("TLS 1.0")]
        Tls1 = 10,

        [Description("TLS 1.1")]
        Tls11 = 11,

        [Description("TLS 1.2")]
        Tls12 = 12
    }
}
