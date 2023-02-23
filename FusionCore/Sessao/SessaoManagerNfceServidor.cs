using System;
using System.Data;
using FusionCore.FusionNfce.Sessao;
using NHibernate;

namespace FusionCore.Sessao
{
    public class SessaoManagerNfceServidor : ISessaoManager
    {
        public bool FactoryIsOpen { get; private set; }

        public string ConnectionString => GerenciaSessaoNfce.StringConexaoServidor;

        public void CarregarFactory()
        {
            FactoryIsOpen = true;
        }

        public void FecharFactory()
        {
            FactoryIsOpen = false;
        }

        public ISession CriaSessao()
        {
            return GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoServerNfce).AbrirSessao();
        }

        public ISession CriaSessao(IsolationLevel level)
        {
            var sessao = CriaSessao();
            sessao.BeginTransaction(level);

            return sessao;
        }

        public ISession GetSessaoAberta()
        {
            throw new NotImplementedException();
        }

        public IStatelessSession CriaStatelessSession()
        {
            return GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoServerNfce).AbrStatelessSession();
        }
    }
}