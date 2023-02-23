using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1560345921)]
    public class FN1560345921_AlteracaoEstruturaCalculoCaixa : Migration
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