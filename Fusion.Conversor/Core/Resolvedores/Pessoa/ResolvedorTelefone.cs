using System;
using FusionCore.FusionAdm.Pessoas;
using NHibernate;
using NHibernate.Exceptions;

namespace Fusion.Conversor.Core.Resolvedores.Pessoa
{
    public class ResolvedorTelefone : Resolvedor
    {
        public ResolvedorTelefone(IStatelessSession session) : base(session)
        {
        }

        public void Resolve(ref PessoaEntidade entidade, string descricao, string numero)
        {
            if (string.IsNullOrEmpty(numero))
            {
                return;
            }

            try
            {
                var telefone = StringPreparer.RemoveNaoNumeros(numero);
                var descContato = string.IsNullOrWhiteSpace(descricao) ? "TELEFONE" : descricao;

                var pessoaTelefone = new PessoaTelefone(descContato, telefone)
                {
                    Contato = entidade
                };

                Session.Insert(pessoaTelefone);
            }
            catch (GenericADOException ex)
            {
                throw new Exception($"Erro crítico telefone: {ex.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}