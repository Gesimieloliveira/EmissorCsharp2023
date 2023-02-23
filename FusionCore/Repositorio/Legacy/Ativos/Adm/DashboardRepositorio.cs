using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace FusionCore.Repositorio.Legacy.Ativos.Adm
{
    public class DashboardRepositorio
    {
        private readonly ISession _sessao;

        public DashboardRepositorio(ISession sessao)
        {
            _sessao = sessao;
        }

        public int QuantidadeProdutos()
        {
            try
            {
                var query = _sessao.Query<ProdutoDTO>();
                return query.Count();
            }
            catch (Exception e)
            {
                throw new RepositorioExeption(e);
            }
        }

        public int QuanitdadeClientes()
        {
            try
            {
                var query = _sessao.QueryOver<Cliente>();

                return query.RowCount();
            }
            catch (Exception e)
            {
                throw new RepositorioExeption(e);
            }
        }

        public int QuantidadeVendasPdv()
        {
            try
            {
                var query = _sessao.Query<PdvVendaDTO>().Where(v => v.Cancelado == IntBinario.Nao);
                return query.Count();
            }
            catch (Exception e)
            {
                throw new RepositorioExeption(e);
            }
        }

        public IList<PdvVendaDTO> Ultimas10Vendas()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append("SELECT ")
                    .Append(
                        "p.Coo AS Coo, p.SerieEcf AS SerieEcf, p.TotalCupom AS TotalCupom, p.TotalFinal AS TotalFinal ")
                    .Append("FROM PdvVendaDTO p ")
                    .Append("WHERE p.Cancelado = 0 ORDER BY p.Id DESC");

                var listaPdvVendaEcf = _sessao.CreateQuery(sql.ToString())
                    .SetMaxResults(10)
                    .SetResultTransformer(Transformers.AliasToBean(typeof (PdvVendaDTO)))
                    .List<PdvVendaDTO>();

                return listaPdvVendaEcf;
            }
            catch (Exception ex)
            {
                throw new RepositorioExeption(ex);
            }
        }

        public decimal TicketMedioVendas()
        {
            const string sql = "SELECT sum(p.TotalFinal) FROM PdvVendaDTO p WHERE p.Cancelado = 0";
            var totalFinal = _sessao.CreateQuery(sql).UniqueResult<decimal>();

            if (totalFinal == 0)
                return 0;

            const string sqlItem = "SELECT sum(i.Quantidade) FROM PdvVendaItemDTO i WHERE i.Cancelado = ?";
            var query = _sessao.CreateQuery(sqlItem).SetParameter(0, VendaItemCancelado.NaoEstaCancelado);
            var qtdeItens = query.UniqueResult<decimal>();

            if (qtdeItens == 0)
                return 0;

            return totalFinal/qtdeItens;
        }

        public int QuantidadeNfesAutorizadas()
        {
            var query = _sessao.QueryOver<EmissaoFinalizadaNfe>();
            var count = query.RowCount();

            return count;
        }

        public int QuantidadeNfesPendentes()
        {
            Nfeletronica nfe = null;
            EmissaoFinalizadaNfe emissao = null;

            var query = _sessao.QueryOver(() => nfe)
                .JoinAlias(() => nfe.Finalizacao, () => emissao, JoinType.LeftOuterJoin)
                .Where(() => emissao.Nfe == null);

            var count = query.RowCount();

            return count;
        }
    }
}