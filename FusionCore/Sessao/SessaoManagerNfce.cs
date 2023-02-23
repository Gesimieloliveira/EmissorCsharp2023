using System;
using System.Data;
using FusionCore.FusionNfce.Sessao;
using NHibernate;

namespace FusionCore.Sessao
{
    public class SessaoManagerNfce : ISessaoManager
    {
        private ISession _sessaoAberta;
        public bool FactoryIsOpen { get; private set; }

        public string ConnectionString => GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).ConnectionString;

        public void CarregarFactory()
        {
            GerenciaSessaoNfce.GerenciaSessaoNfceInicializa();
            FactoryIsOpen = true;
        }

        public void FecharFactory()
        {
            GerenciaSessaoNfce.FecharSessoes();
            FactoryIsOpen = false;
        }

        public ISession CriaSessao()
        {
            _sessaoAberta = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

            return _sessaoAberta;
        }

        public ISession CriaSessao(IsolationLevel level)
        {
            var sessao = CriaSessao();
            sessao.BeginTransaction(level);

            return sessao;
        }

        public ISession GetSessaoAberta()
        {
            if (_sessaoAberta?.IsOpen == true)
            {
                return _sessaoAberta;
            }

            throw new InvalidOperationException("Nenhum sessão aberta para ser obtida");
        }

        public IStatelessSession CriaStatelessSession()
        {
            return GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrStatelessSession();
        }
    }
}