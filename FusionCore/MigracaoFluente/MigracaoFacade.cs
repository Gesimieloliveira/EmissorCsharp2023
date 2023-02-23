using System;
using FusionCore.Sessao;

namespace FusionCore.MigracaoFluente
{
    public static class MigracaoFacade
    {
        public static void ThrowExcepionSeVersaoAntiga(ISessaoManager manager)
        {
            using (var sessao = manager.CriaStatelessSession())
            {
                var countTable = sessao
                    .CreateSQLQuery("select count(*) from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'versao_informacao'")
                    .UniqueResult<int>();

                if (countTable == 0)
                {
                    throw  new InvalidOperationException("Banco de dados muito antigo: Entrar em contato com Equipe de Suporte!");
                }

                var countVersoes = sessao
                    .CreateSQLQuery("select count(*) from versao_informacao")
                    .UniqueResult<int>();

                if (countVersoes == 0)
                {
                    throw  new InvalidOperationException("Banco de dados muito antigo: Entrar em contato com Equipe de Suporte!");
                }
            }
        }
    }
}