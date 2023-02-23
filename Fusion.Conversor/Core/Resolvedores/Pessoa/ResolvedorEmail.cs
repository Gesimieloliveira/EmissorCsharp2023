using System;
using Fusion.Conversor.Core.Map;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Pessoas;
using NHibernate;
using NHibernate.Exceptions;

namespace Fusion.Conversor.Core.Resolvedores.Pessoa
{
    public class ResolvedorEmail : Resolvedor
    {
        public ResolvedorEmail(IStatelessSession session) : base(session)
        {
        }

        public void Resolve(ref PessoaEntidade entidade, PessoaCsv csv)
        {
            if (string.IsNullOrEmpty(csv.Email))
            {
                return;
            }

            try
            {
                var email = new Email(csv.Email);
                var pessoaEmail = new PessoaEmail(email) {Pessoa = entidade};

                Session.Insert(pessoaEmail);
            }
            catch (GenericADOException e)
            {
                throw new Exception($"Erro crítico email: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}