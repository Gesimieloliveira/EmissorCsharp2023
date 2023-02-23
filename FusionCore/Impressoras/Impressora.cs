using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;

namespace FusionCore.Impressoras
{
    public static class Impressora
    {
        public static IEnumerable<string> ObterImpressorasDoComputador() => PrinterSettings.InstalledPrinters.Cast<string>().ToList();
        public static string Padrao => ObterImpressorasDoComputador().FirstOrDefault() ?? string.Empty;
    }
}