using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.EstoqueEvento
{
    public class EventosPorPeriodo : IBuscaListagem<EstoqueEventoDTO>
    {
        private readonly DateTime _final;
        private readonly DateTime _inicio;

        public EventosPorPeriodo(DateTime inicio, DateTime final)
        {
            _inicio = inicio;
            _final = final;
        }

        public EventosPorPeriodo(DateTime inicio)
            : this(inicio, DateTime.Now)
        {
        }

        public IList<EstoqueEventoDTO> Busca(ISession sessao)
        {
            var statement = new StringBuilder("SELECT ");
            statement.Append("EE.ProdutoDTO.Nome AS Nome, ");
            statement.Append("EE.OrigemEventoDetalhe AS OrigemEventoDetalhe, ");
            statement.Append("EE.TipoEventoTexto AS TipoEventoTexto, ");
            statement.Append("EE.EstoqueAtual AS EstoqueAtual, ");
            statement.Append("EE.Movimento AS Movimento, ");
            statement.Append("EE.EstoqueFuturo AS EstoqueFuturo ");
            statement.Append("FROM EstoqueEventoDTO AS EE ");
            statement.Append("WHERE EE.CadastradoEm >= :dataInicial ");
            statement.Append("AND EE.CadastradoEm <= :dataFinal");

            var resultadoQuery = sessao.CreateQuery(statement.ToString())
                .SetDateTime("dataInicial", _inicio)
                .SetDateTime("dataFinal", _final)
                .List<object>();

            var eventos = (from object[] objeto in resultadoQuery
                select new EstoqueEventoDTO
                {
                    ProdutoDTO = new ProdutoDTO
                    {
                        Nome = (string) objeto[0]
                    },
                    OrigemEventoDetalhe = (string) objeto[1],
                    TipoEventoTexto = (string) objeto[2],
                    EstoqueAtual = decimal.Parse(objeto[3].ToString()),
                    Movimento = decimal.Parse(objeto[4].ToString()),
                    EstoqueFuturo = decimal.Parse(objeto[5].ToString())
                }).ToList();


            return eventos;
        }
    }
}