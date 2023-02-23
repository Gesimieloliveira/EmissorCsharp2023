using FusionCore.Repositorio.Filtros.BuscaProdutoNfce;

namespace FusionNfce.Visao.ConsultaProduto
{
    public class UltimaBuscaEfetuadaDoDia
    {
        public UltimaBuscaEfetuadaDoDia(IOpcaoBuscaProdutoNfce opcaoBusca, string textoBuscado) : this()
        {
            OpcaoBusca = opcaoBusca;
            TextoBuscado = textoBuscado;
        }

        private UltimaBuscaEfetuadaDoDia() { }

        public IOpcaoBuscaProdutoNfce OpcaoBusca { get; }
        public string TextoBuscado { get; }
    }
}