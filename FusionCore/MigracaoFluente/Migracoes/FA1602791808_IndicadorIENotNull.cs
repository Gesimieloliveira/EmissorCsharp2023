using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1602791808)]
    public class FA1602791808_IndicadorIENotNull : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update pessoa
				set pessoa.indicadorIe = 9
				where
				pessoa.cnpj != ''
				and pessoa.inscricaoEstadual = '';");

            Delete.DefaultConstraint().OnTable("pessoa")
                .InSchema("dbo").OnColumn("indicadorIe");

            Alter.Column("indicadorIe").OnTable("pessoa")
                .InSchema("dbo").AsByte().NotNullable();
        }

        public override void Down()
        {
        }
    }
}