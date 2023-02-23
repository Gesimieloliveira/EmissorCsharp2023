using System.Globalization;
using System.IO;
using System.Reflection;
using FusionCore.Relatorios;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios
{
    public class FornecedorTemplate
    {
        private readonly ISessaoManager _sessaoManager;

        public FornecedorTemplate(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public byte[] ObtemTemplate<T>(string resourceFrx) where T : RelatorioBase
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repo = new RepositorioRelatorio(sessao);
                var guid = GuidsDosRelatorios.Obtem(typeof(T));

                if (repo.TentaObterTemplate(guid, out var template))
                {
                    return template.Dados;
                }
            }

            return ObtemBytesFrx(resourceFrx);
        }

        public byte[] ObtemBytesFrx(string resourceFrx)
        {
            if (!resourceFrx.EndsWith(".frx", true, CultureInfo.CurrentCulture))
            {
                resourceFrx = $"{resourceFrx}.frx";
            }

            using (var stream = ObtemStreamFrx(resourceFrx))
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                ms.Position = 0;

                return ms.ToArray();
            }
        }

        public Stream ObtemStreamFrx(string resourceFrx)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resource = $"Fusion.FastReport.Arquivos.{resourceFrx}";
            var stream = assembly.GetManifestResourceStream(resource);

            return stream;
        }
    }
}