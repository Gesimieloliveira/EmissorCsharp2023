using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1551296640)]
    public class FA1551296640_CorrecaoEventosEstoqueSemRegistroInicial : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                @"insert into produto_estoque_evento(produto_id, tipoEvento, tipoEventoTexto, origemEvento, origemEventoTexto, origemEventoDetalhe, estoqueAtual, movimento, estoqueFuturo, usuario_id, cadastradoEm) 
                    select pe.produto_id, 1, 'Entrada', 1, 'SaldoInicial', 'Saldo inicial do produto', 0, pe.estoque, pe.estoque, 1, pe.alteradoEm  
		            from produto_estoque pe
		            where pe.produto_id not in (select evento.produto_id from produto_estoque_evento evento group by evento.produto_id);"
            );
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}