using System.IO;
using System.Reflection;
using System.Text;

namespace Dev.GerarClasseMigracao
{
    public static class LocalAssets
    {
        public static string ObterMigracaoTemplate()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var stream = assembly.GetManifestResourceStream("Dev.GerarClasseMigracao.Template.Migracao_Template.txt");

            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}