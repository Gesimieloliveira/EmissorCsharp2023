using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1609853363)]
    public class FA1609853363_AdicionarNovosAtributosEndereco : Migration
    {
        public override void Up()
        {
            Alter.Table("pessoa_endereco")
                .AddColumn("principal").AsBoolean().WithDefaultValue(false).NotNullable();

            Alter.Table("pessoa_endereco")
                .AddColumn("entrega").AsBoolean().WithDefaultValue(false).NotNullable();

            Alter.Table("pessoa_endereco")
                .AddColumn("outros").AsBoolean().WithDefaultValue(true).NotNullable();

            Delete.DefaultConstraint().OnTable("pessoa_endereco").InSchema("dbo").OnColumn("principal");
            Delete.DefaultConstraint().OnTable("pessoa_endereco").InSchema("dbo").OnColumn("entrega");
            Delete.DefaultConstraint().OnTable("pessoa_endereco").InSchema("dbo").OnColumn("outros");

        }

        public override void Down()
        {
        }
    }
}