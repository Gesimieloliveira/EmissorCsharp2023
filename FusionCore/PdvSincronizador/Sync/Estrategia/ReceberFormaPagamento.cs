using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Buscas.Sincronizacao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate.Util;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class ReceberFormaPagamento : SincronizacaoBase
    {
        public override string Tag { get; } = "receber-forma-pagamento-ecf";

        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            var formas = ObterFormas();
            if (formas.Count == 0)
                return;

            var repositorio = new FormaDePagamentoRepositorio(SessaoPdv);
            var formasDoPdv = repositorio.BuscaTodos();

            SessaoPdv.Clear();

            formas.ForEach(
                forma =>
                {
                    var formaPdv = (FormaPagamentoEcfDt) formasDoPdv.Where(f => f.Id == forma.Id).FirstOrNull();
                    var formaPagamento = CriarFormaPagamento(forma, formaPdv);
                    repositorio.Salvar(formaPagamento);
                });
        }

        private static FormaPagamentoEcfDt CriarFormaPagamento(PdvFormaPagamentoDTO forma,
            FormaPagamentoEcfDt formaPdv)
        {
            return new FormaPagamentoEcfDt
            {
                Id = forma.Id,
                Nome = forma.Nome,
                AlteradoEm = forma.AlteradoEm,
                CodigoEcf = formaPdv?.CodigoEcf ?? string.Empty
            };
        }

        private IList<PdvFormaPagamentoDTO> ObterFormas()
        {
            var repositorio = new RepositorioComun<PdvFormaPagamentoDTO>(SessaoAdm);
            return repositorio.Busca(new BuscaFormaPagamento());
        }
    }
}