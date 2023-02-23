using System;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Contratos.Base.Comando;
using FusionLibrary.Helper.Criptografia;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Comandos.Pdv
{
    public class CadastrarVersao : IComando
    {
        public void ExecutaComando(ISession sessao)
        {
            try
            {
                const string queryString = "INSERT INTO versao(id, versao)" +
                                           "VALUES(:id, :versao);";

                sessao.CreateSQLQuery(queryString)
                    .SetInt64("id", 1)
                    .SetString("versao", SimetricaCrip.Computar("v2@fusionPdv"))
                    .ExecuteUpdate();
            }
            catch (Exception e)
            {
                throw new RepositorioExeption("Erro ao cadastrar versão", e);
            }
        }
    }
}
