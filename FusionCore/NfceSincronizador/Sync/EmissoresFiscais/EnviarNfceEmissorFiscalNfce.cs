using System.Collections.Generic;
using FusionCore.FusionNfce.EmissorFiscal;
using FusionCore.FusionNfce.Extencoes;
using FusionCore.FusionNfce.Sessao;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;

namespace FusionCore.NfceSincronizador.Sync.EmissoresFiscais
{
    public class EnviarNfceEmissorFiscalNfce : ISincronizavelPadrao
    {
        private void Sincroniza()
        {
            var todosEmissoresSincronizaveis = BuscarTodosEmissoresSincornizaveis();

            foreach (var emissorFiscalSincronizavel in todosEmissoresSincronizaveis)
            {
                var sessaoServidor = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao();
                var sessaoNfce = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

                var transacaoServidor = sessaoServidor.BeginTransaction();
                var transacaoNfce = sessaoNfce.BeginTransaction();

                var repositorioEmissorNfce = new RepositorioEmissorFiscalNfce(sessaoNfce);
                var repositorioEmissorAdm = new RepositorioEmissorFiscal(sessaoServidor);

                using (repositorioEmissorNfce)
                using (repositorioEmissorAdm)
                using (transacaoNfce)
                using (transacaoServidor)
                {                    
                    var emissorAdmTeste = repositorioEmissorAdm.GetPeloId(emissorFiscalSincronizavel.Id);
                    sessaoServidor.Evict(emissorAdmTeste);

                    var emissorNfce = repositorioEmissorNfce.GetPeloId(emissorFiscalSincronizavel.Id);

                    if (emissorNfce.EmissorFiscalNfce != null && emissorAdmTeste.EmissorFiscalNfce != null)
                    {
                        emissorAdmTeste.EmissorFiscalNfce.NumeroAtual = emissorNfce.EmissorFiscalNfce.NumeroAtual;
                        emissorAdmTeste.EmissorFiscalNfce.NumeroAtualContingencia = emissorNfce.EmissorFiscalNfce.NumeroAtualContingencia;


                        emissorAdmTeste.EntidadeSincronizavel = EntidadeSincronizavel.NaoSincronizar;
                        emissorAdmTeste.EmissorFiscalSat = null;

                        repositorioEmissorAdm.SalvarEmissorFiscalNfce(emissorAdmTeste.EmissorFiscalNfce);
                    }

                    if (emissorNfce.EmissorFiscalSat != null && emissorAdmTeste.EmissorFiscalSat != null)
                    {
                        var emissorAdm = emissorNfce.EmissorFiscalSat.ToAdm();

                        emissorAdm.EntidadeSincronizavel = EntidadeSincronizavel.NaoSincronizar;
                        emissorAdm.EmissorFiscal.EmissorFiscalNfce = null;

                        repositorioEmissorAdm.SalvarEmissorFiscalSat(emissorAdm);
                    }

                    repositorioEmissorNfce.SalvarENaoSincronizar(emissorNfce);

                    sessaoServidor.Flush();
                    sessaoServidor.Clear();
                    sessaoNfce.Flush();
                    sessaoNfce.Clear();

                    transacaoServidor.Commit();
                    transacaoNfce.Commit();
                }
            }
            
            new ReceberEmissorFiscal().RealizarSincronizacao();
        }

        private IEnumerable<NfceEmissorFiscal> BuscarTodosEmissoresSincornizaveis()
        {
            var sessaoNfce = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

            var repositorioNfce = new RepositorioEmissorFiscalNfce(sessaoNfce);

            using (repositorioNfce)
            {
                return repositorioNfce.BuscarTodosEmissoresSincronivaveis();
            }
        }

        public void RealizarSincronizacao()
        {
            Sincroniza();
        }
    }
}