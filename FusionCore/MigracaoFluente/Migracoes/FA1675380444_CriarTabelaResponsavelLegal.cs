using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1675380444)]
    public class FA1675380444_CriarTabelaResponsavelLegal : Migration
    {
        public override void Up()
        {
            Create.Table("responsavel_legal")
                .WithColumn("id").AsGuid().PrimaryKey("pk_responsavel_legal")
                .WithColumn("razaoSocial").AsAnsiString(250).NotNullable()
                .WithColumn("estadoUf_id").AsInt16().NotNullable()
                .WithColumn("cnpj").AsAnsiString(14).NotNullable()
                .WithColumn("inscricaoEstadual").AsAnsiString(14).NotNullable()
                .WithColumn("email").AsAnsiString(255).NotNullable()
                .WithColumn("telefone").AsAnsiString(20).NotNullable();
        }

        public override void Down()
        {
        }
    }
}