using System.Diagnostics.CodeAnalysis;

namespace FusionCore.FusionAdm.Localidade
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class Pais
    {
        public short Id { get; private set; }
        public string Nome { get; set; }
        public short Bacen { get; set; }

        private Pais()
        {
            //nhibernate
        }

        public Pais(short bacen, string nome)
        {
            Bacen = bacen;
            Nome = nome;
        }
    }
}