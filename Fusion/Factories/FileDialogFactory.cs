using Microsoft.Win32;

namespace Fusion.Factories
{
    public static class FileDialogFactory
    {
        public static OpenFileDialog CriaDialogXml()
        {
            return new OpenFileDialog
            {
                Filter = @"Arquivos XML|*.xml",
                CheckFileExists = true
            };
        }

        public static OpenFileDialog CriaDialogFRX()
        {
            return new OpenFileDialog
            {
                Filter = @"Arquivos FRX|*.frx",
                CheckFileExists = true
            };
        }
    }
}