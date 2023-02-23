using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace Fusion.WhiteLabel
{
    public static class MarcaWhiteLabel
    {
        private static dynamic _configs;
        private static string _cnpj;
        public static string NomeSoftware { get; }
        public static string TituloMenuGestor { get; }
        public static ImageSource ImgTituloX48 { get; set; }
        public static ImageSource ImgLoginGestor { get; }
        public static ImageSource ImgMarcaGestor { get; }
        public static ImageSource ImgLoginNfce { get; }
        public static ImageSource ImgLoginPdv { get; }
        public static Uri CorSoftware { get; }

        static MarcaWhiteLabel()
        {
            LoadConfigs();

            NomeSoftware = _configs["NomeSoftware"];
            TituloMenuGestor = _configs["TituloMenuGestor"];
            ImgTituloX48 = ObterImagem("ImgTituloX48");
            ImgLoginGestor = ObterImagem("ImgLoginGestor");
            ImgMarcaGestor = ObterImagem("ImgMarcaGestor");
            ImgLoginPdv = ObterImagem("ImgLoginNfce");
            ImgLoginNfce = ObterImagem("ImgLoginPdv");
            CorSoftware = ObterCor("CorSoftware");
        }

        private static void LoadConfigs()
        {
            var asb = Assembly.GetExecutingAssembly();
            var wlFile = Path.Combine(Path.GetDirectoryName(asb.Location) ?? string.Empty, "wlcfg");
            if (File.Exists(wlFile))
            {
                _cnpj = File.ReadAllText(wlFile)?.Trim();
            }

            _cnpj = _cnpj ?? "0001";

            using (var configs = asb.GetManifestResourceStream($"Fusion.WhiteLabel.Marcas._{_cnpj}.marca.json"))
            {
                if (configs == null)
                {
                    throw new InvalidOperationException($"Não foi possível configurar a Marca do Sitema para o CNPJ {_cnpj}");
                }

                var serializer = new JsonSerializer();
                var values = serializer.Deserialize<dynamic>(new JsonTextReader(new StreamReader(configs)));
                _configs = values;
            }
        }

        private static ImageSource ObterImagem(string key)
        {
            var asb = Assembly.GetExecutingAssembly();

            using (var img = asb.GetManifestResourceStream($"Fusion.WhiteLabel.Marcas._{_cnpj}.{_configs[key]}"))
            {
                if (img == null)
                {
                    throw new InvalidOperationException("Não foi possível obter a Imagem da Marca");
                }

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = img;
                bmp.EndInit();

                return bmp;
            }
        }

        private static Uri ObterCor(string cor)
        {
            return new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/light.{_configs[cor]}.xaml");
        }
    }

}