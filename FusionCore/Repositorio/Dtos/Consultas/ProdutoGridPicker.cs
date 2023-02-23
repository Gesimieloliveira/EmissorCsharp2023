using FusionCore.Core.Estoque;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class ProdutoGridPicker : Entidade, IProdutoAutoComplete, IProdutoTabelaPreco
    {
        private ProdutoDTO _produtoCarregado;

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Referencia { get; set; }
        public decimal PrecoCompra { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal PrecoOriginal { get; set; }
        public decimal Estoque { get; set; }

        protected override int ReferenciaUnica => Id;
        public int ProdutoId => Id;

        public override string ToString()
        {
            return $"{Id} - {Nome}";
        }

        public ProdutoDTO CarregaProduto()
        {
            if (_produtoCarregado != null)
            {
                return _produtoCarregado;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repostirio = new RepositorioProduto(sessao);
                _produtoCarregado = repostirio.GetPeloId(Id);
            }

            return _produtoCarregado;
        }
    }
}