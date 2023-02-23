using FusionLibrary.Helper.Criptografia;

namespace FusionCore.Helpers.Maquina
{
    public static class IdMaquinaProvider
    {
        public static string Computa()
        {
            var mk = ChaveMaquinaUkProvider.ProvideUk();
            var mkSha1 = Sha1Helper.Computar(mk);

            return mkSha1;
        }
    }
}