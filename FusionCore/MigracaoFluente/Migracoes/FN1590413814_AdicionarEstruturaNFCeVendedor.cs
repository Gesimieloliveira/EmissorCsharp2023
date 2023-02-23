using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1590413814)]
    public class FN1590413814_AdicionarEstruturaNFCeVendedor : Migration
    {
        public override void Up()
        {
            Create.Table("vendedor")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey("pk_vendedor")
                .WithColumn("nome").AsAnsiString(255).NotNullable()
                .WithColumn("documentoUnico").AsAnsiString(14).NotNullable();

            Alter.Table("nfce")
                .AddColumn("vendedorId").AsInt32().Nullable()
                .ForeignKey("fk_nfce_vendedor", "vendedor", "id");

        }

        public override void Down()
        {
        }
    }
}