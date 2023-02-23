using System;
using System.Collections.Generic;
using System.Data;
using FusionCore.ControleCaixa.Individual;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.NfceSincronizador.ControleCaixa
{
    public class ServicoEnviarDadosDoCaixaParaServidor
    {
        private readonly SessaoSyncFactory _sessaoFactory;

        public ServicoEnviarDadosDoCaixaParaServidor(SessaoSyncFactory sessaoFactory)
        {
            _sessaoFactory = sessaoFactory;
        }

        public void EnviarLancamentosAvulsos()
        {
            IEnumerable<SyncLancamentoCaixa> lancamentosPendentes;

            using (var sessao = _sessaoFactory.CriarSessaoLocal())
            {
                var repositorio = new RepositorioSincronizacaoCaixa(sessao);
                lancamentosPendentes = repositorio.ObterLançamentosPendentes();
            }

            foreach (var sync in lancamentosPendentes)
            {
                SincronizarLancamentoPendente(sync);
            }
        }

        private void SincronizarLancamentoPendente(SyncLancamentoCaixa sync)
        {
            using (var sessaoLocal = _sessaoFactory.CriarSessaoLocal(IsolationLevel.ReadCommitted))
            using (var sessaoServidor = _sessaoFactory.CriarSessaoServidor(IsolationLevel.ReadCommitted))
            {
                var usuario = sessaoServidor.Get<UsuarioDTO>(sync.Lancamento.UsuarioCriacao.Id);
                var empresa = sessaoServidor.Get<EmpresaDTO>(sync.Lancamento.Empresa.Id);

                sync.Lancamento.AlterarTipo(usuario);
                sync.Lancamento.AlterarTipo(empresa);

                sessaoServidor.SaveOrUpdate(sync.Lancamento);

                sessaoLocal.Evict(sync);
                sessaoLocal.Delete(sync);

                sessaoServidor.Transaction.Commit();
                sessaoLocal.Transaction.Commit();
            }
        }

        public void EnviarCaixasIndividuais()
        {
            IEnumerable<SyncCaixaIndividual> caixasPendentes;

            using (var sessaoLocal = _sessaoFactory.CriarSessaoLocal())
            {
                var repositorio = new RepositorioSincronizacaoCaixa(sessaoLocal);
                caixasPendentes = repositorio.ObterCaixasPendentes();
            }

            foreach (var sync in caixasPendentes)
            {
                SincronizarCaixaPendente(sync);
            }
        }

        private void SincronizarCaixaPendente(SyncCaixaIndividual sync)
        {
            using (var sessaoLocal = _sessaoFactory.CriarSessaoLocal(IsolationLevel.ReadCommitted))
            using (var sessaoServidor = _sessaoFactory.CriarSessaoServidor(IsolationLevel.ReadCommitted))
            {
                var servicoContaCaixa = new ServicoContaCaixa(sessaoServidor);

                sync.Caixa.AlterarTipo(sessaoServidor.Get<UsuarioDTO>(sync.Caixa.Usuario.Id));
                sessaoServidor.SaveOrUpdate(sync.Caixa);

                var fluxosDoCaixa = sessaoLocal.QueryOver<Fluxo>().Where(i => i.Caixa == sync.Caixa).List();

                foreach (var fluxo in fluxosDoCaixa)
                {
                    fluxo.Usuario = sessaoServidor.Get<UsuarioDTO>(fluxo.Usuario.Id);
                    fluxo.Caixa = sync.Caixa;

                    sessaoServidor.SaveOrUpdate(fluxo);
                }

                if (servicoContaCaixa.PrecisaRegistrarAberturaDoCaixa(sync.Caixa))
                {
                    servicoContaCaixa.RegistrarAberturaCaixaLoja(sync.Caixa);
                }

                if (servicoContaCaixa.PrecisaRegistrarFechamentoDoCaixa(sync.Caixa))
                {
                    servicoContaCaixa.RegistrarFechamentoEmCaiaLoja(sync.Caixa);
                }

                sessaoLocal.Evict(sync);
                sessaoLocal.Delete(sync);

                sessaoServidor.Transaction.Commit();
                sessaoLocal.Transaction.Commit();
            }
        }
    }
}