using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1649764749)]
    public class FA1649764749_AdicionarInicioEFimVigenciaNcm : Migration
    {
        public override void Up()
        {
            Alter.Table("tabela_ncm")
                .AlterColumn("descricao")
                .AsCustom("text").NotNullable();

            Alter.Table("tabela_ncm")
                .AddColumn("inicioVigencia").AsDateTime().Nullable();

            Alter.Table("tabela_ncm")
                .AddColumn("fimVigencia").AsDateTime().Nullable();

            Alter.Table("tabela_ncm")
                .AddColumn("vencido")
                .AsBoolean().WithDefaultValue(false).NotNullable();

            Delete.DefaultConstraint().OnTable("tabela_ncm")
                .InSchema("dbo").OnColumn("vencido");
        }

        public override void Down()
        {
        }
    }
}