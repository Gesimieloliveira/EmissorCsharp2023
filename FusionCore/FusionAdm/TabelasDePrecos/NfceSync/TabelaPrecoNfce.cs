using System.Collections.Generic;
using FusionCore.FusionNfce.Produto;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.TabelasDePrecos.NfceSync
{
    public class TabelaPrecoNfce : EntidadeBase<int>, ITabelaPreco
    {
        public TabelaPrecoNfce() {}

        public TabelaPrecoNfce(TabelaPreco tabelaPreco)
        {
            CopiaInformacoes(tabelaPreco);
        }

        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public string Descricao { get; set; }
        public TipoAjustePreco TipoAjustePreco { get; set; }
        public decimal PercentualAjuste { get; set; }
        public bool ApenasItensDaLista { get; set; }
        public bool Status { get; set; }

        public IList<AjusteDiferenciadoNfce> AjusteDiferenciadoLista { get; set; } = new List<AjusteDiferenciadoNfce>();


        private void CopiaInformacoes(TabelaPreco tabelaPreco)
        {
            Id = tabelaPreco.Id;
            Descricao = tabelaPreco.Descricao;
            TipoAjustePreco = tabelaPreco.TipoAjustePreco;
            PercentualAjuste = tabelaPreco.PercentualAjuste;
            ApenasItensDaLista = tabelaPreco.ApenasItensDaLista;
            Status = tabelaPreco.Status;

            foreach (var ajusteDiferenciado in tabelaPreco.AjusteDiferenciadoLista)
            {
                AjusteDiferenciadoLista.Add(new AjusteDiferenciadoNfce
                {
                    PercentualAjuste = ajusteDiferenciado.PercentualAjuste,
                    Id = ajusteDiferenciado.Id,
                    Produto = new ProdutoNfce
                    {
                        Id = ajusteDiferenciado.Produto.Id
                    },
                    TabelaPreco = this
                });
            }
        }
    }
}