using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1616076407)]
    public class FA1616076407_MdfeCriadoEm : Migration
    {
        public override void Up()
        {
            Alter.Table("mdfe").AddColumn("criadoEm").AsDateTime().WithDefaultValue(DateTime.Now).NotNullable();
            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("criadoEm");

            Execute.Sql("update mdfe set mdfe.criadoEm = mdfe.emissaoEm");
        }

        public override void Down()
        {
        }
    }
}