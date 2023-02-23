using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1552921037)]
    public class FA1552921037_AlteracoesResponsavelTecnico : Migration
    {
        public override void Up()
        {
            Alter.Table("responsavel_tecnico")
                .AddColumn("isCTe").AsBoolean().WithDefaultValue(false).NotNullable();

            Alter.Table("responsavel_tecnico")
                .AddColumn("isCTeOs").AsBoolean().WithDefaultValue(false).NotNullable();

            Alter.Table("responsavel_tecnico")
                .AddColumn("isMDFe").AsBoolean().WithDefaultValue(false).NotNullable();

            Alter.Table("responsavel_tecnico")
                .AddColumn("isNFCe").AsBoolean().WithDefaultValue(false).NotNullable();

            Alter.Table("responsavel_tecnico")
                .AddColumn("isNFe").AsBoolean().WithDefaultValue(false).NotNullable();

            Delete.DefaultConstraint().OnTable("responsavel_tecnico").InSchema("dbo").OnColumn("isCTe");
            Delete.DefaultConstraint().OnTable("responsavel_tecnico").InSchema("dbo").OnColumn("isCTeOs");
            Delete.DefaultConstraint().OnTable("responsavel_tecnico").InSchema("dbo").OnColumn("isMDFe");
            Delete.DefaultConstraint().OnTable("responsavel_tecnico").InSchema("dbo").OnColumn("isNFCe");
            Delete.DefaultConstraint().OnTable("responsavel_tecnico").InSchema("dbo").OnColumn("isNFe");
        }

        public override void Down()
        {
        }
    }
}