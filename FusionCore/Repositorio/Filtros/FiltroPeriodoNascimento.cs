namespace FusionCore.Repositorio.Filtros
{
    public class FiltroPeriodoNascimento
    {
        public FiltroPeriodoNascimento(int mesInicio, int mesFinal)
        {
            MesInicio = mesInicio;
            MesFinal = mesFinal;
        }

        public int MesInicio { get; }
        public int MesFinal { get; }
    }
}