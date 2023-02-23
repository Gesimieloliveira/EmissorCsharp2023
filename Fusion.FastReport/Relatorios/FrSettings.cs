using System;
using System.Reflection;
using System.Text;
using FastReport.Design;
using Fr = FastReport;

namespace Fusion.FastReport.Relatorios
{
    public class FrSettings
    {
        private Fr.EnvironmentSettings _settings;
        private Action<Guid, byte[]> _customSaveAction;
        private Guid _reportId;

        private FrSettings()
        {
            _settings = new Fr.EnvironmentSettings();
            _settings.DesignerSettings.DesignerClosed += DesignCloseHandler;
        }

        public static FrSettings Instancia { get; } = new FrSettings();

        private void DesignCloseHandler(object sender, EventArgs e)
        {
            _settings.CustomSaveReport -= CustomSaveHandler;
            _customSaveAction = null;
            _reportId = Guid.Empty;
        }

        private void CustomSaveHandler(object sender, OpenSaveReportEventArgs e)
        {
            var reportBytes = Encoding.UTF8.GetBytes(e.Report.ReportResourceString);

            _customSaveAction?.Invoke(_reportId, reportBytes);
        }

        public void RegistrarSalvamentoCustomizado(Guid id, Action<Guid, byte[]> action)
        {
            _customSaveAction = action;
            _reportId = id;
            _settings.DesignerSettings.CustomSaveReport += CustomSaveHandler;
        }

        public static void ConfigurarIdiomaBrasil()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resource = "Fusion.FastReport.Assets.Portuguese-(Brazil).frl";
            var stream = assembly.GetManifestResourceStream(resource);

            Fr.Utils.Res.LoadLocale(stream);
        }
    }
}