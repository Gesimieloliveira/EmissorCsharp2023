using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.PerfilCfop
{
    public class UltimoSufixo : IEntidade
    {
        private readonly byte _ultimo;

        public UltimoSufixo(byte ultimo)
        {
            _ultimo = ultimo;
        }

        public byte UsarProximoSufixo()
        {
            return (byte) (_ultimo + 1);
        }
    }
}
