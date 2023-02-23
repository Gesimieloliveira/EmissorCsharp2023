using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1553014075)]
    public class FA1553014075_CTeOSAdicionarTipoEmissao : Migration
    {
        public override void Up()
        {
            Alter.Table("cte_os").AddColumn("tipoEmissao").AsByte().WithDefaultValue(1).NotNullable();

            Delete.DefaultConstraint().OnTable("cte_os").InSchema("dbo").OnColumn("tipoEmissao");
        }

        public override void Down()
        {
        }
    }
}