using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1587989002)]
    public class FA1587989002_AdicionarGrupoRepasseIcmsSPorAnp : Migration
    {
        public override void Up()
        {
            Alter.Table("produto_codigo_anp").AddColumn("grupoRepasseInterestadualST").AsBoolean().NotNullable()
                .WithDefaultValue(false);

            Execute.Sql(@"update produto_codigo_anp set produto_codigo_anp.grupoRepasseInterestadualST = 1
                where id in('210203001', '320101001', '320101002', '320102002', '320102001', '320102003', '820101032'
                , '820101027', '820101004', '820101005', '820101022', '820101031', '820101030', '820101018'
                , '820101020', '820101021', '420105001', '420101005', '420101004', '420102005', '820101003'
                , '820101012', '420106002', '830101001', '420301004', '420202001', '420301001', '510101002'
                , '510102002', '510201001', '510201003', '510301003', '510103001', '510301001', '820101026'
                , '320102005', '320201001', '320103001', '220102001', '320301001', '320103002', '820101019'
                , '820101014', '820101006', '820101016', '820101015', '820101025', '820101017', '820101013'
                , '420102004', '420104001', '820101033', '820101034', '420106001', '820101011', '510102001'
                , '420301002', '410103001', '410101001', '410102001', '430101004', '510101001')");

            Delete.DefaultConstraint().OnTable("produto_codigo_anp").InSchema("dbo").OnColumn("grupoRepasseInterestadualST");


        }

        public override void Down()
        {
        }
    }
}