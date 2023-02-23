using System;
using FusionCore.Helpers.Ambiente;

namespace FusionCore.FusionAdm.Emissores.Flags
{
    public enum Modelo
    {
        Emulador,
        Bematech,
        Dimep,
        Elgin,
        Gertec,
        Sweda,
        Urano,
        Tanca,
        Kryptus,
        Nitere,
        Daruma,
        ControlId,
        Nenhum
    }

    public static class ExtModelo
    {
        public static string CaminhoDll(this Modelo modelo)
        {
            var caminhoPasta = ManipulaPasta.LocalSistema() + @"\SATdll\";

            if (modelo == Modelo.Nenhum)
                return string.Empty;

            switch (modelo)
            {
                case Modelo.Bematech:
                    return caminhoPasta + @"Bematech\BemaSAT32.dll";
                case Modelo.Dimep:
                    return caminhoPasta + @"Dimep\dllsat.dll";
                case Modelo.Elgin:
                    return caminhoPasta + @"Elgin\dllsat.dll";
                case Modelo.Emulador:
                    return caminhoPasta + @"Emulador\SAT.dll";
                case Modelo.Gertec:
                    return caminhoPasta + @"Gertec\GERSAT.dll";
                case Modelo.Kryptus:
                    return caminhoPasta + @"Kryptus\SAT.dll";
                case Modelo.Sweda:
                    return caminhoPasta + @"Sweda\SATDLL.dll";
                case Modelo.Tanca:
                    return caminhoPasta + @"Tanca\SAT.dll";
                case Modelo.Urano:
                    return caminhoPasta + @"Urano\SAT.dll";
                case Modelo.Nitere:
                    return caminhoPasta + @"Nitere\dllsat.dll";
                case Modelo.Daruma:
                    return caminhoPasta + @"Daruma\dllsat.dll";
                case Modelo.ControlId:
                    return caminhoPasta + @"ControlId\libsatid.dll";
                default:
                    throw new InvalidOperationException("Não existe esse modelo de sat");
            }
        }
    }
}