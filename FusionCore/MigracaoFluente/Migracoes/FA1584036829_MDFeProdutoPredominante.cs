using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1584036829)]
    public class FA1584036829_MDFeProdutoPredominante : Migration
    {
        public override void Up()
        {
            Alter.Table("mdfe").AddColumn("tipoCargaProdutoPredominante").AsByte().NotNullable().WithDefaultValue(0);
            Alter.Table("mdfe").AddColumn("nomeProdutoPredominante").AsAnsiString(120).NotNullable().WithDefaultValue(string.Empty);
            Alter.Table("mdfe").AddColumn("codigoBarrasProdutoPredominante").AsAnsiString(14).NotNullable().WithDefaultValue(string.Empty);
            Alter.Table("mdfe").AddColumn("ncmProdutoPredominante").AsAnsiString(8).NotNullable().WithDefaultValue(string.Empty);

            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("tipoCargaProdutoPredominante");
            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("nomeProdutoPredominante");
            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("codigoBarrasProdutoPredominante");
            Delete.DefaultConstraint().OnTable("mdfe").InSchema("dbo").OnColumn("ncmProdutoPredominante");
        }

        public override void Down()
        {
        }
    }
}