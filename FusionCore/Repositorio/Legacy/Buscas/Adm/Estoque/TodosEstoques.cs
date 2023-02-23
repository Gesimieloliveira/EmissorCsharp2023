using System.Collections.Generic;
using System.Linq;
using System.Text;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Estoque
{
    public class TodosEstoques : IBuscaListagem<ProdutoEstoqueDTO>
    {
        public IList<ProdutoEstoqueDTO> Busca(ISession sessao)
        {
            var sql = new StringBuilder("SELECT ");
            sql.Append("PE.ProdutoDTO.Id AS Id, PE.ProdutoDTO.Nome AS Nome, ");
            sql.Append("PE.ProdutoDTO.PrecoCompra AS PrecoCompra, PE.ProdutoDTO.PrecoVenda AS PrecoVenda, PE.Estoque AS Estoque, ");
            sql.Append("PE.ProdutoDTO.ReferenciaInterna as ReferenciaInterna ");
            sql.Append("FROM ProdutoEstoqueDTO PE");

            var produtosObject = sessao.CreateQuery(sql.ToString())
                .List<object>();

            var produtos = (from object[] objeto in produtosObject
                select new ProdutoEstoqueDTO
                {
                    ProdutoDTO = new ProdutoDTO
                    {
                        Id = int.Parse(objeto[0].ToString()),
                        Nome = (string) objeto[1],
                        PrecoCompra = decimal.Parse(objeto[2].ToString()),
                        PrecoVenda = decimal.Parse(objeto[3].ToString()),
                        ReferenciaInterna = (string) objeto[5]
                    },
                    Estoque = decimal.Parse(objeto[4].ToString())
                }).ToList();

            return produtos;
        }
    }
}