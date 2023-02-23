using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1551453531)]
    public class FN1551453531_CorrecaoGerarFinanceiroSemCentroLucroNfce : Migration
    {
        public override void Up()
        {
            Alter.Table("tipo_documento")
                .AddColumn("registraFinanceiro").AsBoolean().NotNullable().SetExistingRowsTo(false);

            Alter.Table("documento_receber")
                .AlterColumn("centroLucro_id").AsInt16().Nullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}