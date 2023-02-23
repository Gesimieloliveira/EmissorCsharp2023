using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ACBrFramework;
using FusionCore.FusionPdv.Servico.Estoque;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.Helper.Criptografia;
using FusionPdv.Ecf;
using NHibernate.Util;

namespace FusionPdv.Servicos.Ecf.EstadoEcf
{
    public class VendaOuPagamento : VerificaEstado
    {
        public VendaEcfDt VendaEcfDt { get; set; } = new VendaEcfDt
        {
            UuidVenda = GuuidHelper.Computar("PDV" + DateTime.Now.ToString("G") + SessaoSistema.UsuarioLogado.Login)
        };
  
        protected override void CondicaoDeVerificacao()
        {

            if (Estado != EstadoEcfFiscal.Venda && Estado != EstadoEcfFiscal.Pagamento)
            { 
                return;
            }

            var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao();
            var transacao = sessao.BeginTransaction();

            try
            {
                var vendaRepositorio = new VendaEcfRepositorio(sessao);
                var itemRepositorio = new VendaEcfItemRepositorio(sessao);
                var dt = vendaRepositorio.ObterUltimaVenda();

                if (dt == null)
                {
                    SessaoEcf.EcfFiscal.CancelarCupom();
                    return;
                }

                dt.Cancelado = IntBinario.Sim;
                dt.Status = VendaStatus.Cancelada;

                SessaoEcf.EcfFiscal.CancelarCupom();

                vendaRepositorio.Salvar(dt);


                var itensCancelados =
                    dt.VendaEcfItens.Where(item => item.Cancelado.Equals(VendaItemCancelado.SimCancelado)).ToList();
                var itens =
                    dt.VendaEcfItens.Where(item => item.Cancelado.Equals(VendaItemCancelado.NaoEstaCancelado)).ToList();

                var estoqueServico = EstoqueServicoPdvFactory.CriarEstoqueServico(sessao);

                dt.VendaEcfItens.ForEach(item =>
                {
                    if (item.Cancelado == VendaItemCancelado.SimCancelado)
                        return;

                    item.Cancelado = VendaItemCancelado.SimCancelado;
                    itemRepositorio.Salvar(item);

                    var estoqueModel = new EstoqueModel
                    {
                        OrigemEvento = OrigemEventoEstoque.CancelamentoItemPdv,
                        Produto = item.ProdutoDt,
                        Quantidade = item.Quantidade,
                        Usuario = SessaoSistema.UsuarioLogado
                    };
                    estoqueServico.Acrescentar(estoqueModel);
                });

                sessao.Flush();
                sessao.Clear();

                dt.Cancelado = IntBinario.Nao;
                dt.Status = VendaStatus.Aberta;
                dt.Id = 0;

                VendaEcfDt = dt;

                VendaEcfDt.VendaEcfItens = new List<VendaEcfItemDt>();
                itensCancelados.ForEach(item =>
                {
                    item.Id = 0;
                    item.Cancelado = VendaItemCancelado.SimCancelado;
                    VendaEcfDt.VendaEcfItens.Add(item);
                });

                itens.ForEach(item =>
                {
                    item.Id = 0;
                    item.Cancelado = VendaItemCancelado.NaoEstaCancelado;
                    VendaEcfDt.VendaEcfItens.Add(item);
                });


                VendaEcfDt.VendaEcfItens = VendaEcfDt.VendaEcfItens.OrderByDescending(v => v.NumeroItem).ToList(); 

                transacao.Commit();
            }
            catch (ACBrException ex)
            {
                transacao.Rollback();
                throw new ACBrException(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                transacao.Rollback();
            }
            finally
            {
                sessao.Close();
            }
        }
    }
}
