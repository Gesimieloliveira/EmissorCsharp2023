using System;
using System.Collections.Generic;
using FusionCore.Excecoes.Sessao;
using FusionCore.Repositorio.Legacy.Base.Helper;
using FusionCore.Repositorio.Legacy.Contratos.Base.Sessao;
using NHibernate.Util;

namespace FusionCore.FusionNfce.Sessao
{
    public static class GerenciaSessaoNfce
    {
        private static readonly Dictionary<string, ISessaoHelper> Sessoes = new Dictionary<string, ISessaoHelper>();
        public static string StringConexaoServidor { get; set; }

        public const string SessaoVenda = nameof(SessaoNfce);
        public const string SessaoServerNfce = nameof(SessaoServerNfce);

        public static void GerenciaSessaoInicializaTodasConexoes()
        {
            InicializaSessaoLocal();
            InicializaSessaoServidor();
        }

        private static void InicializaSessaoServidor()
        {
            try
            {
                var sessaoServidorNfce = new SessaoServerNfce();
                StringConexaoServidor = sessaoServidorNfce.ConnectionString;
                AddSessao(nameof(SessaoServerNfce), sessaoServidorNfce);
            }
            catch (SessaoHelperException ex)
            {
                throw new ConexaoInvalidaException(ex.Message, ex);
            }
            catch (TypeInitializationException ex)
            {
                throw new ConexaoInvalidaException("Não existe conexão valida para o servidor.", ex);
            }
        }

        private static void InicializaSessaoLocal()
        {
            try
            {
                FecharSessoes();

                AddSessao(nameof(SessaoNfce), new SessaoNfce());
            }
            catch (SessaoHelperException ex)
            {
                throw new ConexaoInvalidaException(ex.Message, ex);
            }
            catch (TypeInitializationException ex)
            {
                throw new ConexaoInvalidaException("Não existe conexão valida para a conexão local.", ex);
            }
        }

        public static void GerenciaSessaoNfceInicializa()
        {
            InicializaSessaoLocal();
        }

        public static void FecharSessoes()
        {
            try
            {
                Sessoes.ForEach(s => { s.Value.Fechar(); });
            }
            catch
            {
                // ignored
            }

            Sessoes.Clear();
        }

        private static void AddSessao(string chave, ISessaoHelper sessaoHelper)
        {
            Sessoes.Add(chave, sessaoHelper);
        }

        public static ISessaoHelper ObterSessao(string chave)
        {
            if (chave == nameof(SessaoServerNfce) && !Sessoes.ContainsKey(chave))
            {
                InicializaSessaoServidor();
            }

            return Sessoes[chave];
        }
    }
}