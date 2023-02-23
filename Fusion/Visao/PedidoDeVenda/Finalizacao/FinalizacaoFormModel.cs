using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PedidoDeVenda.Finalizacao
{
    public class FinalizacaoFormModel : ViewModel
    {
        public PedidoVenda PedidoVenda { get; }

        public FinalizacaoFormModel(PedidoVenda pedidoVenda)
        {
            PedidoVenda = pedidoVenda;
        }

        public event EventHandler<PedidoVenda> FinalizacaoConcluida;
        public event EventHandler<PedidoVenda> FinalizacaoRemovida;

        public void FinalizacaoPedido()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                PedidoVenda.Finalizar(new List<Negociacao>());

                new RepositorioPedidoVenda(sessao).SalvarAlteracoes(PedidoVenda);   

                transacao.Commit();
            }

            OnFinalizacaoConcluida(PedidoVenda);
            OnFechar();
        }

        public void AbrirPedido()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                PedidoVenda.AbrirPedidoNovamente();

                new RepositorioPedidoVenda(sessao).SalvarAlteracoes(PedidoVenda);

                transacao.Commit();
            }

            OnFinalizacaoRemovida(PedidoVenda);
            OnFechar();
        }

        protected virtual void OnFinalizacaoConcluida(PedidoVenda e)
        {
            FinalizacaoConcluida?.Invoke(this, e);
        }

        protected virtual void OnFinalizacaoRemovida(PedidoVenda e)
        {
            FinalizacaoRemovida?.Invoke(this, e);
        }
    }
}