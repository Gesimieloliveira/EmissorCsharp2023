using System;
using ACBrFramework;
using FusionCore.FusionPdv.Servico.Estoque;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;
using FusionPdv.Ecf;
using FusionPdv.ManipulaValor;
using NHibernate;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfVender
    {
        private readonly VendaEcfItemDt _vendaEcfItem;
        private readonly int _ultimoItem;
        private EstadoEcf.EstadoEcf _estadoEcf;

        public EcfVender(VendaEcfItemDt vendaEcfItem, int ultimoItem)
        {
            _vendaEcfItem = vendaEcfItem;
            _ultimoItem = ultimoItem;
        }

        private void Valida()
        {
            if (_estadoEcf != null)
            {
                _estadoEcf.ValidarEstadoDoEcf();
                return;
            }
            _estadoEcf = new EstadoEcf.EstadoEcf(SessaoEcf.EcfFiscal.Estado());
            _estadoEcf.ValidarEstadoDoEcf();
        }

        public void VendeItem()
        {
            Valida();
            SalvarItem();
        }

        private void SalvarItem()
        {
            if (_vendaEcfItem.Id != 0) return;

            _vendaEcfItem.VendaEcfDt.QuantidadeItens = _ultimoItem + 1;
            _vendaEcfItem.NumeroItem = _ultimoItem + 1;
            _vendaEcfItem.IcmsEcf = _vendaEcfItem.IcmsEcf.Trim();
            _vendaEcfItem.SiglaUnidadeProduto = _vendaEcfItem.SiglaUnidadeProduto.Trim();
            _vendaEcfItem.CodigoBarra = _vendaEcfItem.CodigoBarra ?? string.Empty;
            _vendaEcfItem.AlteradoEm = DateTime.Now;
            _vendaEcfItem.EcfDt = _vendaEcfItem.VendaEcfDt.EcfDt;
            _vendaEcfItem.SerieEcf = _vendaEcfItem.EcfDt.Serie;
            _vendaEcfItem.PrecoUnitario = new TruncaArredonda(_vendaEcfItem.PrecoUnitario).Executa();
            _vendaEcfItem.TotalizadorParcial = string.Empty;
            _vendaEcfItem.Cst = string.Empty;
            _vendaEcfItem.Coo = _vendaEcfItem.VendaEcfDt.Coo;
            _vendaEcfItem.Ccf = _vendaEcfItem.VendaEcfDt.Ccf;
            _vendaEcfItem.Total = new TruncaArredonda(_vendaEcfItem.Total).Executa();
            _vendaEcfItem.Quantidade = new TruncaArredonda(_vendaEcfItem.Quantidade, 3).ExecutaComQuantidadeDecimal();

            SalvarItemNoBanco();
        }

        private void SalvarItemNoBanco()
        {
            using (var sessao = GerenciaSessao.ObterSessao(SessaoPdv.FabricaDois).AbrirSessao())
            {
                var transacao = sessao.BeginTransaction();

                try
                {
                    var itemRepositorio = new VendaEcfItemRepositorio(sessao);

                    itemRepositorio.Salvar(_vendaEcfItem);
                    MovimentarEstoque(_vendaEcfItem, sessao);

                    LancaItemNaEcf();

                    transacao.Commit();
                }
                catch (ACBrException)
                {
                    transacao?.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    transacao?.Rollback();
                    throw new RepositorioExeption("Erro ao salvar item no banco de dados.", ex);
                }
            }
        }

        private static void MovimentarEstoque(VendaEcfItemDt vendaEcfItem, ISession sessao)
        {
            var estoqueModel = new EstoqueModel
            {
                OrigemEvento = OrigemEventoEstoque.ItemPdv,
                Produto = vendaEcfItem.ProdutoDt,
                Quantidade = vendaEcfItem.Quantidade,
                Usuario = SessaoSistema.UsuarioLogado
            };

            var estoqueServico = EstoqueServicoPdvFactory.CriarEstoqueServico(sessao);
            estoqueServico.Descontar(estoqueModel);
        }

        private void LancaItemNaEcf()
        {
            SessaoEcf.EcfFiscal.VendeItem(
                _vendaEcfItem.GetCodigoParaEcf(),
                _vendaEcfItem.GetNomeProdutoParaEcf(),
                _vendaEcfItem.GetIcmsParaEcf(),
                _vendaEcfItem.GetQuantidadeParaEcf(),
                _vendaEcfItem.GetPrecoUnitarioParaEcf(),
                _vendaEcfItem.GetSiglaUnidadeParaEcf());
        }
    }
}