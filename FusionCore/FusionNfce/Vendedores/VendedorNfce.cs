using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.Vendedores
{
    public class VendedorNfce : EntidadeBase<int>
    {
        public VendedorNfce() { }
        public VendedorNfce(Vendedor vendedorEncontrado)
        {
            CopiaInformacoes(vendedorEncontrado);
        }

        private void CopiaInformacoes(Vendedor vendedorEncontrado)
        {
            Id = vendedorEncontrado.Id;
            Nome = vendedorEncontrado.Nome;
            DocumentoUnico = vendedorEncontrado.GetDocumentoUnico();
            Ativo = vendedorEncontrado.Pessoa.Ativo;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string DocumentoUnico { get; set; }
        public bool Ativo { get; set; }
        protected override int ChaveUnica => Id;
    }
}