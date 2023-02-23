using System;
using System.Text;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Contratos.Base.Comando;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Comandos.Pdv
{
    public class CriaTabelaVersao : IComando
    {
        public void ExecutaComando(ISession sessao)
        {
            try
            {
                var sql = new StringBuilder();

                sql.Append("SET ANSI_NULLS ON ");
                sql.Append("SET QUOTED_IDENTIFIER ON ");
                sql.Append("SET ANSI_PADDING ON ");
                sql.Append("CREATE TABLE [dbo].[versao]( ");
                sql.Append("[id] [int] NOT NULL, ");
                sql.Append("[versao] [varchar](255) NOT NULL, ");
                sql.Append("CONSTRAINT [pk_versao] PRIMARY KEY CLUSTERED ");
                sql.Append("( ");
                sql.Append("[id] ASC ");
                sql.Append(
                    ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ");
                sql.Append(") ON [PRIMARY] ");
                sql.Append("SET ANSI_PADDING OFF ");

                sessao.CreateSQLQuery(sql.ToString()).ExecuteUpdate();
            }
            catch (Exception e)
            {
                throw new RepositorioExeption("Erro ao criar tabela versão", e);
            }
        }
    }
}
