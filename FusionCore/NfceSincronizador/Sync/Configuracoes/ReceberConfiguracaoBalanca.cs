using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionNfce.ConfiguracaoBalanca;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace FusionCore.NfceSincronizador.Sync.Configuracoes
{
    public class ReceberConfiguracaoBalanca : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.Balanca;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioBalancaServidor = new RepositorioBalanca(sessaoServidor);
            var repositorioBalancaTerminal = new RepositorioBalancaNfce(sessaoNfce);

            if (!pendentes.Any()) return;

            var balancaServidor = repositorioBalancaServidor.BuscarUnicaBalanca();
            var balancaTerminal = BalancaNfce.Cria(balancaServidor);

            repositorioBalancaTerminal.SalvarOuAtualizar(balancaTerminal);
            SincronizacaoPendentesADeletar.Add(pendentes[0]);
        }
    }
}