using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public class AjusteDiferenciado : EntidadeBase<int>, ISincronizavelAdm
    {
        public int Id { get; set; }
        public TabelaPreco TabelaPreco { get; set; }
        public ProdutoDTO Produto { get; set; }
        public decimal PercentualAjuste { get; set; }
        public decimal NovoPreco => Calcula();
        protected override int ChaveUnica => Id;
        public string Referencia => TabelaPreco.Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.TabelaPreco;

        public ICalculoAjustePreco CriarCalculador()
        {
            return FabricaCalculoPeloTipoAjuste.ObterCalculadoraDeAjuste(TabelaPreco.TipoAjustePreco);
        }

        private decimal Calcula()
        {
            if (PercentualAjuste == 0) return Produto.PrecoVenda;

            var calculador = CriarCalculador();
            var precoCalculado = calculador.Calcular(Produto, PercentualAjuste);

            return precoCalculado;
        }
    }
}