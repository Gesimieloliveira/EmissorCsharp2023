using System;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Contratos.Base.Comando;
using FusionLibrary.Helper.Criptografia;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Comandos.Adm.Usuario
{
    public class CadastraAdmin : IComando
    {
        public void ExecutaComando(ISession sessao)
        {
            try
            {
                const string queryString = "SET IDENTITY_INSERT usuario ON;" +
                                           "INSERT INTO usuario(id, login, senha, cadastradoEm, alteradoEm, tema)" +
                                           "VALUES(:id, :login, :senha, :cadastradoEm, :alteradoEm, :tema);" +
                                           "SET IDENTITY_INSERT usuario OFF;";

                sessao.CreateSQLQuery(queryString)
                    .SetInt64("id", 1)
                    .SetString("login", "admin")
                    .SetString("senha", SenhaHelper.CriptografarSenha("agil4"))
                    .SetDateTime("cadastradoEm", DateTime.Now)
                    .SetDateTime("alteradoEm", DateTime.Now)
                    .SetString("tema", string.Empty)
                    .ExecuteUpdate();
            }
            catch (Exception e)
            {
                throw new RepositorioExeption("Erro ao cadastrar usuário admin", e);
            }
        }
    }
}