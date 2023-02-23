namespace FusionCore.Helpers.AssemblyUtils.Leitura
{
    public class NomeAssembly : IRegraLeitura
    {
        public string Ler(System.Reflection.Assembly assembly)
        {
            var nome = assembly.GetName().Name;
            return nome;
        }
    }
}