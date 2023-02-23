using System.IO;
using FusionCore.FusionAdm.Fiscal.Flags;
using ZeusVersao = DFe.Classes.Flags.VersaoServico;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class VersaoExt
    {
        public static ZeusVersao ToZeus(this Versao versao)
        {
            switch (versao)
            {
                case Versao.V310:
                    return ZeusVersao.Versao310;
                case Versao.V400:
                    return ZeusVersao.Versao400;
                default:
                    throw new InvalidDataException("Versão não disponivels");
            }
        }

        public static string GetString(this Versao versao)
        {
            switch (versao)
            {
                case Versao.V310:
                    return "3.10";
                case Versao.V400:
                    return "4.00";
            }

            return "3.10";
        }
    }
}