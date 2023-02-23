using System;
using System.Data;
using FusionCore.FusionAdm.Sessao;
using NHibernate;

namespace FusionCore.Sessao
{
    public class SessaoManagerAdm : ISessaoManager
    {
        private ISession _sessaoAtual;
        public bool FactoryIsOpen { get; private set; }

        public string ConnectionString => SessaoHelperFactory.GetConnectionString();

        public void CarregarFactory()
        {
            SessaoHelperFactory.CarregarAdm();
            FactoryIsOpen = true;
        }

        public void FecharFactory()
        {
            SessaoHelperFactory.Fechar();
            FactoryIsOpen = false;
        }

        public ISession CriaSessao(IsolationLevel level)
        {
            _sessaoAtual = SessaoHelperFactory.AbrirSessaoAdm();
            _sessaoAtual.BeginTransaction(level);

            return _sessaoAtual;
        }

        public ISession GetSessaoAberta()
        {
            if (_sessaoAtual?.IsOpen == false)
            {
                throw new ApplicationException("Gerenciador de sessão não encontrou uma sessão aberta");
            }

            return _sessaoAtual;
        }

        public IStatelessSession CriaStatelessSession()
        {
            return SessaoHelperFactory.SessaoStatelessAdm();
        }

        public ISession CriaSessao()
        {
            _sessaoAtual = SessaoHelperFactory.AbrirSessaoAdm();
            return _sessaoAtual;
        }
    }
}