using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionAdm.Nfce.SatFiscal;
using FusionCore.FusionNfce.Extencoes;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.SatFiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace FusionCore.NfceSincronizador.Sync
{
    public class EnviarNfce : ISincronizavelPadrao
    {
        public void RealizarSincronizacao()
        {
            Sincroniza();
        }

        private void Sincroniza()
        {
            var todasNfces = BuscarTodasNfceASincronizar();

            foreach (var nfce in todasNfces)
            {
                var sessaoServidor = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao();
                var sessaoNfce = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

                var transacaoServidor = sessaoServidor.BeginTransaction();
                var transacaoNfce = sessaoNfce.BeginTransaction();

                var repositorioNfce = new RepositorioNfce(sessaoNfce);
                var repositorioNfceAdm = new RepositorioNfceAdm(sessaoServidor);
                var repositorioMaloteAdm = new RepositorioMalote(sessaoServidor);

                using (repositorioNfce)
                using (repositorioNfceAdm)
                using (transacaoNfce)
                using (transacaoServidor)
                {
                    if (IgnoraNessaSincronizacao(nfce))
                    {
                        return;
                    }

                    if (NaoTemEmissao(nfce, repositorioNfce, sessaoServidor, sessaoNfce, transacaoServidor, transacaoNfce)) return;

                    var nfceAdmParaDeletar = BuscarNfcePorUuid(nfce.Uuid);

                    var contingenciaId = 0;

                    if (nfceAdmParaDeletar != null)
                    {
                        if (nfceAdmParaDeletar.Contingencia != null)
                            contingenciaId = nfceAdmParaDeletar.Contingencia.Id;

                        repositorioNfceAdm.Deletar(nfceAdmParaDeletar);
                    }

                    var nfceAdm = nfce.ToAdm();

                    nfceAdm.Malote = repositorioMaloteAdm.BuscarMalotePorOrigemUuid(nfce.UuidVenda);
                    nfceAdm.Contingencia = ResolveContingencia(nfce.Contingencia, repositorioNfceAdm, contingenciaId);

                    ResolverHistoricos(sessaoServidor, sessaoNfce, nfceAdm, repositorioNfce, nfce);

                    repositorioNfceAdm.Salvar(nfceAdm);

                    nfce.Sincronizado = true;
                    repositorioNfce.Salvar(nfce);

                    sessaoServidor.Flush();
                    sessaoNfce.Flush();

                    transacaoServidor.Commit();
                    transacaoNfce.Commit();
                }
            }
        }

        private static bool NaoTemEmissao(Nfce nfce,
            IRepositorioNfce repositorioNfce,
            ISession sessaoServidor,
            ISession sessaoNfce,
            ITransaction transacaoServidor,
            ITransaction transacaoNfce)
        {
            if (nfce.Emissao == null && nfce.FinalizaEmissaoSat == null)
            {
                nfce.Sincronizado = true;
                repositorioNfce.Salvar(nfce);

                sessaoServidor.Flush();
                sessaoNfce.Flush();

                transacaoServidor.Commit();
                transacaoNfce.Commit();

                return true;
            }

            return false;
        }

        private void ResolverHistoricos(ISession sessaoServidor,
            ISession sessaoNfce,
            NfceAdm nfceAdm,
            RepositorioNfce repositorioNfce,
            Nfce nfce)
        {
            sessaoServidor.Flush();
            sessaoNfce.Flush();
            nfceAdm.HistoricoEnvioSatAdmLista = ResolveHistoricoSat(repositorioNfce, nfce, nfceAdm);
            nfceAdm.HistoricoEnvioNfceAdmLista = ResolverHistoricoNfce(repositorioNfce, nfce, nfceAdm);
            sessaoServidor.Clear();
            sessaoNfce.Clear();
        }

        private IList<HistoricoEnvioSatAdm> ResolveHistoricoSat(RepositorioNfce repositorioNfce,
            Nfce nfce,
            NfceAdm nfceAdm)
        {
            var historicoSatNfce = repositorioNfce.BuscarHistoricoEnvioSats(nfce);

            return historicoSatNfce.Select(h => h.ToAdm(nfceAdm)).ToList();
        }

        private IList<NfceEmissaoHistoricoAdm> ResolverHistoricoNfce(RepositorioNfce repositorioNfce, Nfce nfce, NfceAdm nfceAdm)
        {
            var historicoNfce = repositorioNfce.BuscarHistoricoEmissao(nfce);

            return historicoNfce.Select(h => h.ToAdm(nfceAdm)).ToList();
        }

        private NfceContingenciaAdm ResolveContingencia(
            NfceContingencia nfceContingencia
            , RepositorioNfceAdm repositorioNfceAdm
            , int contingenciaId)
        {
            if (nfceContingencia == null) return null;

            var contingenciaAdmBuscada = repositorioNfceAdm.BuscarContingencia(contingenciaId);

            if (contingenciaAdmBuscada != null)
                return contingenciaAdmBuscada;

            return nfceContingencia.ToAdm();
        }


        private NfceAdm BuscarNfcePorUuid(string nfceUuid)
        {
            var repositorio = CriarRepositorioAdm();

            using (repositorio)
            {
                return repositorio.BuscarNfcePeloUuid(nfceUuid);
            }
        }

        private IEnumerable<Nfce> BuscarTodasNfceASincronizar()
        {
            var sessaoNfce = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

            var repositorioNfce = new RepositorioNfce(sessaoNfce);

            using (repositorioNfce)
            {
                return repositorioNfce.BuscarTodasNfceSincronivaveis();
            }
        }

        private static RepositorioNfceAdm CriarRepositorioAdm()
        {
            var repositorioNfceAdm =
                new RepositorioNfceAdm(GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao());
            return repositorioNfceAdm;
        }

        private static RepositorioNfce CriaRepositorioNfce()
        {
            var repositorioNfce = new RepositorioNfce(GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao());
            return repositorioNfce;
        }

        private IEnumerable<HistoricoEnvioSat> BuscarHistoricoEnvioSat(Nfce nfce)
        {
            var repositorio = CriaRepositorioNfce();

            using (repositorio)
            {
                return repositorio.BuscarHistoricoEnvioSats(nfce);
            }
        }

        private bool IgnoraNessaSincronizacao(Nfce nfce)
        {
            if (nfce == null) return true;
            if (nfce.Status == Status.Aberta) return true;
            if (nfce.Status == Status.PendenteOffline) return true;

            return false;
        }
    }
}
 