using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public class TabelaPreco : EntidadeBase<int>, ISincronizavelAdm, ITabelaPreco
    {
        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public string Descricao { get; set; }
        public TipoAjustePreco TipoAjustePreco { get; set; }
        public decimal PercentualAjuste { get; set; }
        public bool ApenasItensDaLista { get; set; }
        public bool Status { get; set; }

        public IList<AjusteDiferenciado> AjusteDiferenciadoLista { get; set; } = new List<AjusteDiferenciado>();

        public void JaExisteProduto(ProdutoDTO produtoDTO)
        {
            if (AjusteDiferenciadoLista.Any(x => x.Produto.Equals(produtoDTO)))
                throw new InvalidOperationException("Produto já foi adicionado");
        }

        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.TabelaPreco;

        public decimal CalcularNovoPreco(int produtoId, decimal precoAtual)
        {
            var calculador = FabricaCalculoPeloTipoAjuste.ObterCalculadoraDeAjuste(TipoAjustePreco);
            var ajusteDiferenciado = AjusteDiferenciadoLista.SingleOrDefault(i => i.Produto.Id == produtoId);

            if (ajusteDiferenciado != null)
                return calculador.Calcular(precoAtual, ajusteDiferenciado.PercentualAjuste);

            return ApenasItensDaLista
                ? precoAtual
                : calculador.Calcular(precoAtual, PercentualAjuste);
        }
    }
}