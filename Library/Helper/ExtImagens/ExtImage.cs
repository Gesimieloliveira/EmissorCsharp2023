using System.IO;
using System.Windows.Media.Imaging;
using FusionLibrary.Helper.Conversores;

namespace FusionLibrary.Helper.ExtImagens
{
    public static class ExtImage
    {
        public static byte[] ToByteArray(this System.Drawing.Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        public static BitmapImage ToBitmapImage(this System.Drawing.Image image)
        {
            var bytes = image.ToByteArray();

            return ConverteImage.ByteEmImagem(bytes);
        }
    }
}