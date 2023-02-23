using System.Collections.Generic;
using FusionCore.FusionNfce.Produto;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Produtos
{
    public class ReceberProdutoUnidade : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.ProdutoUnidade;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes,
            ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioProdutoUnidadeAdm = new RepositorioProdutoUnidade(sessaoServidor);
            var repositorioProdutoUnidadeNfce = new RepositorioProdutoUnidadeNfce(sessaoNfce);

            pendentes.ForEach(sp =>
            {
                var produtoUnidade = repositorioProdutoUnidadeAdm.GetPeloId(int.Parse(sp.Referencia));


                if (produtoUnidade != null)
                    repositorioProdutoUnidadeNfce.Salvar(new ProdutoUnidadeNfce
                    {
                        Id = produtoUnidade.Id,
                        Sigla = produtoUnidade.Sigla,
                        PodeFracionar = produtoUnidade.PodeFracionar == 1,
                        SolicitaTotal = produtoUnidade.SolicitaTotalPdv
                    });

                SincronizacaoPendentesADeletar.Add(sp);

                sessaoServidor.Flush();
                sessaoServidor.Clear();
                sessaoNfce.Flush();
                sessaoNfce.Clear();
            });
        }
    }
}