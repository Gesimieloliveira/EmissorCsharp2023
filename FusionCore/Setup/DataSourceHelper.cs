using System.Text;

namespace FusionCore.Setup
{
    public static class DataSourceHelper
    {
        public static string GetDataSource(this IConexaoCfg cfg)
        {
            var sb = new StringBuilder($"{cfg.Servidor}");

            if (string.IsNullOrWhiteSpace(cfg.Instancia))
            {
                sb.Append($",{cfg.Porta}");

                return sb.ToString();
            }

            sb.Append($"\\{cfg.Instancia}");

            return sb.ToString();
        }
    }
}