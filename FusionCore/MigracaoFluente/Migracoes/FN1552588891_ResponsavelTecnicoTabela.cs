using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1552588891)]
    public class FN1552588891_ResponsavelTecnicoTabela : Migration
    {
        public override void Up()
        {
            Create.Table("responsavel_tecnico")
                .WithColumn("guid").AsGuid().PrimaryKey("pk_responsavel_tecnico")
                .WithColumn("csrt").AsAnsiString(36).NotNullable()
                .WithColumn("id").AsInt32().NotNullable()
                .WithColumn("uf_id").AsByte().NotNullable();

            Create.ForeignKey("fk_responsavel_tecnico__uf").FromTable("responsavel_tecnico").InSchema("dbo")
                .ForeignColumn("uf_id").ToTable("uf").InSchema("dbo").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}