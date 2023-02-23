using System;
using FusionCore.Core.Estoque;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Servico.Estoque;
using NHibernate.Event;

namespace FusionCore.FusionAdm.Listeners
{
    public class EstoqueListener : 
        IPreInsertEventListener, 
        IPreDeleteEventListener, 
        IPreUpdateEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            if (!(@event.Entity is IMovimentavel movimento) || movimento.MovimentaEstoque == false)
            {
                return false;
            }

            if (!@event.Session.TransactionInProgress)
            {
                throw new Exception("Necessário uma transação para executar movimentacao de estoque");
            }

            var servico = EstoqueServicoAdmFactory.Cria(@event.Session);
            var model = movimento.CriaMovimentoInclusao();

            if (model.IsReservaEstoque && model.Inverso)
            {
                servico.DescontarEstoqueComReserva(model);
                return false;
            }

            if (model.Inverso && model.IsReservaEstoque == false)
            {
                servico.Descontar(model);
                return false;
            }

            if (model.Inverso && model.IsReservaEstoque)
            {
                servico.AcrescentarEstoqueComReserva(model);
                return false;
            }

            servico.Acrescentar(model);
            return false;
        }

        public bool OnPreDelete(PreDeleteEvent @event)
        {
            if (!(@event.Entity is IMovimentavel movimento) || movimento.MovimentaEstoque == false)
            {
                return false;
            }

            if (!@event.Session.TransactionInProgress)
            {
                throw new Exception("Necessário uma transação para executar movimentacao de estoque");
            }

            var servico = EstoqueServicoAdmFactory.Cria(@event.Session);
            var model = movimento.CriaMovimentoRemocao();

            if (model.IsReservaEstoque && model.Inverso)
            {
                servico.AcrescentarEstoqueComReserva(model);
                return false;
            }

            if (model.Inverso && model.IsReservaEstoque == false)
            {
                servico.Acrescentar(model);
                return false;
            }

            if (model.Inverso && model.IsReservaEstoque)
            {
                servico.DescontarEstoqueComReserva(model);
                return false;
            }

            servico.Descontar(model);
            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            if (@event.Entity is ItemNfe item && item.MovimentaEstoque)
            {
                if (!@event.Session.TransactionInProgress)
                {
                    throw new Exception("Necessário uma transação de dados para alteração em item da nfe");
                }

                ControladorEstoqueItemNfe.OnPreUpdate(item, @event.Session);
                return false;
            }

            return false;
        }
    }
}