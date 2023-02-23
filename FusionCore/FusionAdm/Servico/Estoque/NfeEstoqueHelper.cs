using System;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Servico.Estoque
{
    public static class NfeEstoqueHelper
    {
        public static void CancelamentoNfe(Nfeletronica nfe, UsuarioDTO usuario, ISession sessao)
        {
            try
            {
                var servico = EstoqueServicoAdmFactory.Cria(sessao);

                nfe.Itens.ForEach(i =>
                {
                    if (i.MovimentaEstoque == false && nfe.PedidoInternoSistema == false)
                    {
                        return;
                    }

                    var model = new EstoqueModel(
                        i.Produto,
                        i.Quantidade,
                        usuario,
                        OrigemEventoEstoque.CancelamentoNfe
                    );

                    if (nfe.TipoOperacao == TipoOperacao.Saida)
                    {
                        servico.Acrescentar(model);
                        return;
                    }

                    servico.Descontar(model);
                });
            }
            catch (Exception e)
            {
                throw new EstoqueException("Erro ao inverter o estoque: " + e.Message);
            }
        }

        public static void DenegacaoNfe(Nfeletronica nfe, UsuarioDTO usuario, ISession sessao)
        {
            try
            {
                var servico = EstoqueServicoAdmFactory.Cria(sessao);

                nfe.Itens.ForEach(i =>
                {
                    if (i.MovimentaEstoque == false)
                    {
                        return;
                    }

                    var model = new EstoqueModel(
                        i.Produto,
                        i.Quantidade,
                        usuario,
                        OrigemEventoEstoque.DenegacaoNfe
                    );

                    if (nfe.TipoOperacao == TipoOperacao.Saida)
                    {
                        servico.Acrescentar(model);
                        return;
                    }

                    servico.Descontar(model);
                });
            }
            catch (Exception e)
            {
                throw new EstoqueException("Erro ao inverter o estoque: " + e.Message);
            }
        }
    }
}