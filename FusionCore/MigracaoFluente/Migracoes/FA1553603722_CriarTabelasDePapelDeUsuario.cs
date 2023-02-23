using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1553603722)]
    public class FA1553603722_CriarTabelasDePapelDeUsuario : Migration
    {
        public override void Up()
        {
            Create.Table("usuario_papel")
                .WithColumn("usuario_id").AsInt32().NotNullable()
                .WithColumn("papel_id").AsGuid().NotNullable();

            Create.PrimaryKey("pk_usuario_papel__papel").OnTable("usuario_papel")
                .Columns("usuario_id", "papel_id");

            Create.Table("papel")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey("pk_papel")
                .WithColumn("descricao").AsAnsiString(60).NotNullable();

            Create.Table("papel_permissao")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey("pk_papel_permissao")
                .WithColumn("papel_id").AsGuid().NotNullable()
                .WithColumn("permissao_numero_enum").AsInt32().NotNullable()
                .WithColumn("permissao_descricao_enum").AsAnsiString(255);

            Create.ForeignKey("fk_usuario_papel__usuario")
                .FromTable("usuario_papel").InSchema("dbo").ForeignColumn("usuario_id")
                .ToTable("usuario").InSchema("dbo").PrimaryColumn("id");

            Create.ForeignKey("fk_usuario_papel__papel")
                .FromTable("usuario_papel").InSchema("dbo").ForeignColumn("papel_id")
                .ToTable("papel").InSchema("dbo").PrimaryColumn("id");

            Create.ForeignKey("fk_papel_permissao__papel")
                .FromTable("papel_permissao").InSchema("dbo").ForeignColumn("papel_id")
                .ToTable("papel").InSchema("dbo").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}