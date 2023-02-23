using System;
using System.Collections.Generic;
using FusionCore.CadastroUsuario;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.ControleCaixa.Repositorios
{
    public class RepositorioLancamentoAvulso : RepositorioBase
    {
        public RepositorioLancamentoAvulso(ISession sessao) : base(sessao)
        {
        }

        public LancamentoAvulsoCaixa BuscarPeloId(Guid id)
        {
            return Sessao.Get<LancamentoAvulsoCaixa>(id);
        }

        public void Persistir(LancamentoAvulsoCaixa lancamento)
        {
            ThrowExceptionSeNaoExisteTransacao();
            Sessao.Persist(lancamento);
            Sessao.Flush();
        }

        public IEnumerable<LancamentoAvulsoCaixaDTO> ListarLancamentos()
        {
            LancamentoAvulsoCaixaDTO a = null;
            IUsuario tbUsuario = null;

            var query = Sessao.QueryOver<LancamentoAvulsoCaixa>()
                .JoinAlias(i => i.UsuarioCriacao, () => tbUsuario)
                .SelectList(list => list
                    .Select(i => i.Id).WithAlias(() => a.Id)
                    .Select(i => i.TipoOperacao).WithAlias(() => a.TipoOperacao)
                    .Select(i => i.ValorOperacao).WithAlias(() => a.ValorOperacao)
                    .Select(i => i.DataCriacao).WithAlias(() => a.DataCriacao)
                    .Select(i => i.Motivo).WithAlias(() => a.Motivo)
                    .Select(i => i.LocalEvento).WithAlias(() => a.LocalEvento)
                    .Select(() => tbUsuario.Login).WithAlias(() => a.NomeOperador)
                );

            query.TransformUsing(Transformers.AliasToBean<LancamentoAvulsoCaixaDTO>());

            return query.List<LancamentoAvulsoCaixaDTO>();
        }

        public void Alterar(LancamentoAvulsoCaixa lancamento)
        {
            Sessao.Update(lancamento);
            Sessao.Flush();
        }
    }
}