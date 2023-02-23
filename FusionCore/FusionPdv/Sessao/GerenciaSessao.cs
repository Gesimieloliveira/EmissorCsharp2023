using System;
using System.Collections.Generic;
using FusionCore.Excecoes.Sessao;
using FusionCore.Repositorio.Legacy.Base.Helper;
using FusionCore.Repositorio.Legacy.Contratos.Base.Sessao;
using NHibernate.Util;

namespace FusionCore.FusionPdv.Sessao
{
    public static class GerenciaSessao
    {
        private static readonly Dictionary<string, ISessaoHelper> Sessoes = new Dictionary<string, ISessaoHelper>();

        public static void GerenciaSessaoInicializar()
        {
            try
            {
                LimparSessoes();

                AddSessao(nameof(SessaoPdv), new SessaoPdv());
                AddSessao(SessaoPdv.FabricaDois, new SessaoPdv());
                AddSessao(SessaoPdv.FabricaTres, new SessaoPdv());
                AddSessao(nameof(SessaoTransmissaoLocalEcf), new SessaoTransmissaoLocalEcf());
            }
            catch (SessaoHelperException ex)
            {
                throw new ConexaoInvalidaException(ex.Message, ex);
            }
            catch (TypeInitializationException ex)
            {
                throw new ConexaoInvalidaException("Não existe conexão valida para o servidor.", ex);
            }

            try
            {
                AddSessao(nameof(SessaoAdm), new SessaoAdm());
                AddSessao(nameof(SessaoTransmissaoServerAdmEcf), new SessaoTransmissaoServerAdmEcf());
            }
            catch
            {
                // ignored    
            }
        }

        private static void LimparSessoes()
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
            return Sessoes[chave];
        }
    }
}