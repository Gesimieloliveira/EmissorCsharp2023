using System.Collections.Generic;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.TabelasPrecos
{
    public class ReceberTabelaPreco : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.TabelaPreco;
        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioServidor = new RepositorioTabelaPreco(sessaoServidor);
            var repositorioLocal = new RepositorioTabelaPrecoNfce(sessaoNfce);

            foreach (var sincronizacaoPendente in pendentes)
            {
                var tabelaPreco = repositorioServidor.GetPeloId(int.Parse(sincronizacaoPendente.Referencia));

                SincronizacaoPendentesADeletar.Add(sincronizacaoPendente);

                if (tabelaPreco == null) return;

                DeletaAjustes(sessaoNfce, repositorioLocal, tabelaPreco);

                var tabelaPrecoNfce = new TabelaPrecoNfce(tabelaPreco);

                repositorioLocal.Salva(tabelaPrecoNfce);
            }
        }

        private static void DeletaAjustes(ISession sessaoNfce,
            RepositorioTabelaPrecoNfce repositorioLocal,
            TabelaPreco tabelaPreco)
        {
            var tabelaADeletar = repositorioLocal.GetPeloId(tabelaPreco.Id);

            if (tabelaADeletar == null) return;

            tabelaADeletar.AjusteDiferenciadoLista.ForEach(repositorioLocal.Deleta);

            sessaoNfce.Flush();
            sessaoNfce.Clear();
        }
    }
}