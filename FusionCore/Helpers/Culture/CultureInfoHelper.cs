using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace FusionCore.Helpers.Culture
{
    public static class CultureInfoHelper
    {
        public static void DefineDefaultCultureInfo()
        {
            const string cultura = "pt-BR";
            var culturaInformacao = new CultureInfo(cultura);

            CultureInfo.DefaultThreadCurrentCulture = culturaInformacao;
            CultureInfo.DefaultThreadCurrentUICulture = culturaInformacao;
            Thread.CurrentThread.CurrentCulture = culturaInformacao;
            Thread.CurrentThread.CurrentUICulture = culturaInformacao;

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }
    }
}