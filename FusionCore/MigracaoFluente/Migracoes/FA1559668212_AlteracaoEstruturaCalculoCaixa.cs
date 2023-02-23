using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1559668212)]
    public class FA1559668212_AlteracaoEstruturaCalculoCaixa : Migration
    {
        public override void Up()
        {
            Alter.Table("caixa_calculo")
                .AddColumn("resultado").AsDecimal(15, 2).NotNullable().SetExistingRowsTo(0);

            Execute.Sql("update caixa_calculo set resultado = saldoAtual - (select ci.saldoInicial from caixa_individual ci where ci.id = caixaIndividual_id)");
        }

        public override void Down()
        {
        }
    }
}