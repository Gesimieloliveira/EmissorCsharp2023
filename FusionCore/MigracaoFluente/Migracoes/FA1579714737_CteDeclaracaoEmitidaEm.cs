using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1579714737)]
    public class FA1579714737_CteDeclaracaoEmitidaEm : Migration
    {
        public override void Up()
        {
            Alter.Table("cte").AddColumn("declaracaoEmitidaEm").AsDateTime().WithDefaultValue(DateTime.Now)
                .NotNullable();

            Delete.DefaultConstraint().OnTable("cte").InSchema("dbo").OnColumn("declaracaoEmitidaEm");
        }

        public override void Down()
        {
        }
    }
}