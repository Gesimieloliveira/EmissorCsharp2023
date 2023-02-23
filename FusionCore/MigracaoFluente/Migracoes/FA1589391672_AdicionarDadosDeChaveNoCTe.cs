using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1589391672)]
    public class FA1589391672_AdicionarDadosDeChaveNoCTe : Migration
    {
        public override void Up()
        {
            Alter.Table("cte").AddColumn("serieEmissao").AsInt16().NotNullable().WithDefaultValue(0);
            Alter.Table("cte").AddColumn("numeroFiscalEmissao").AsInt64().NotNullable().WithDefaultValue(0);
            Alter.Table("cte").AddColumn("codigoNumericoEmissao").AsInt32().NotNullable().WithDefaultValue(0);

            Delete.DefaultConstraint().OnTable("cte").InSchema("dbo").OnColumn("serieEmissao");
            Delete.DefaultConstraint().OnTable("cte").InSchema("dbo").OnColumn("numeroFiscalEmissao");
            Delete.DefaultConstraint().OnTable("cte").InSchema("dbo").OnColumn("codigoNumericoEmissao");
        }

        public override void Down()
        {
        }
    }
}