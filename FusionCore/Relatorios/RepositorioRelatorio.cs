using System;
using System.Collections.Generic;
using NHibernate;

namespace FusionCore.Relatorios
{
    public class RepositorioRelatorio
    {
        private readonly ISession _sessao;

        public RepositorioRelatorio(ISession sessao)
        {
            _sessao = sessao;
        }

        public IEnumerable<RelatorioProprio> ObtemTodos()
        {
            var q = _sessao.QueryOver<RelatorioProprio>();

            var resultado = q.List<RelatorioProprio>();

            return resultado;
        }

        public void SalvarTemplate(Template template)
        {
            _sessao.SaveOrUpdate(template);
            _sessao.Flush();
        }

        public bool TentaObterTemplate(Guid id, out Template template)
        {
            template = _sessao.Get<Template>(id);

            return template != null;
        }

        public void SalvarRelatorio(RelatorioProprio relatorio)
        {
            ThrowExceptionSeNaoExistirTranscao();

            _sessao.SaveOrUpdate(relatorio.Template);
            _sessao.SaveOrUpdate(relatorio);

            _sessao.Flush();
        }

        private void ThrowExceptionSeNaoExistirTranscao()
        {
            if (!_sessao.Transaction.IsActive)
            {
                throw new TransactionException("Preciso de uma transação ativa no banco de dados");
            }
        }

        public void ExcluirTemplatePeloId(Guid id)
        {
            ThrowExceptionSeNaoExistirTranscao();

            var template = _sessao.Get<Template>(id);

            if (template == null)
            {
                return;
            }

            _sessao.Delete(template);
            _sessao.Flush();
        }

        public void ExcluirRelatorio(RelatorioProprio relatorio)
        {
            ThrowExceptionSeNaoExistirTranscao();

            _sessao.Delete(relatorio);
            _sessao.Delete(relatorio.Template);

            _sessao.Flush();
        }
    }
}