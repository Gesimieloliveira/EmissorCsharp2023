using System.IO;
using DFe.DocumentosEletronicos.ManipuladorDeXml;
using FusionCore.Helpers.Ambiente;

namespace FusionCore.FusionAdm.Acbr
{
    public static class AcbrMonitorPersistXml
    {
        private static readonly string CaminhoArquivoAcbrEndereco;

        static AcbrMonitorPersistXml()
        {
            CaminhoArquivoAcbrEndereco = Path.Combine(DiretorioAssembly.GetPastaConfig(), "AcbrEndereco.xml");
        }

        public static void Persistir(AcbrEndereco acbrEndereco)
        {
            FuncoesXml.ClasseParaArquivoXml(acbrEndereco, CaminhoArquivoAcbrEndereco);
        }

        public static AcbrEndereco LoadConfigAcbrEndereco()
        {
            try
            {
                return FuncoesXml.ArquivoXmlParaClasse<AcbrEndereco>(CaminhoArquivoAcbrEndereco);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }
    }
}