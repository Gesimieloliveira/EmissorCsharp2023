using System.Collections.Generic;
using System.Data;
using System.Globalization;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1599342388)]
    public class FA1599342388_CorrecaoSobraDescontoFaturamento : Migration
    {
        public override void Up()
        {
            Execute.WithConnection((con, tran) =>
            {
                var query = @"with analise as (
	                    select 
		                    v.id,
		                    totalDesconto, 
		                    (select sum(i.totalDescontoFixo) from faturamento_produto i where i.faturamentoVenda_id = v.id) as totalDescontoFixoItem
	                    from faturamento_venda v
                    )
                    select 
	                    a.id,
	                    a.totalDesconto,
	                    a.totalDescontoFixoItem,
	                    a.totalDesconto - a.totalDescontoFixoItem as diferenca
                    from analise a
                    where a.totalDesconto != a.totalDescontoFixoItem";

                var command = con.CreateCommand();

                command.CommandText = query;
                command.Transaction = tran;

                var reader = command.ExecuteReader();
                var updates = new List<string>();

                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var sobra = reader.GetDecimal(3);

                    updates.Add(CriarUpdateCorrecao(id, sobra));
                }

                reader.Close();

                ExecutarUpdatesCorrecao(updates, con, tran);
            });
        }

        private string CriarUpdateCorrecao(int id, decimal sobra)
        {
            const string comando =
                "update faturamento_produto set totalDescontoFixo = (totalDescontoFixo + {0}) " +
                "where id = (select max(i.id) from faturamento_produto i where i.faturamentoVenda_id = {1})";

            var sobraTexto = sobra.ToString(new NumberFormatInfo() {NumberDecimalSeparator = "."});

            return string.Format(comando, sobraTexto, id);
        }

        private void ExecutarUpdatesCorrecao(IEnumerable<string> updates, IDbConnection con, IDbTransaction tran)
        {
            foreach (var cmdTxt in updates)
            {
                var cmd = con.CreateCommand();
                cmd.Transaction = tran;
                cmd.CommandText = cmdTxt;
                cmd.ExecuteNonQuery();
            }
        }

        public override void Down()
        {
        }
    }
}