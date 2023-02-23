using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1551113391)]
    public class FN1551113391_CriaTabelasTributarias : Migration
    {
        public override void Up()
        {
            Create.Table("situacao_tributaria_cofins")
                .WithColumn("id").AsAnsiString(2).PrimaryKey("pk_situacao_tributaria_cofins").NotNullable()
                .WithColumn("descricao").AsAnsiString(255).NotNullable();

            Create.Table("situacao_tributaria_pis")
                .WithColumn("id").AsAnsiString(2).PrimaryKey("pk_situacao_tributaria_pis").NotNullable()
                .WithColumn("descricao").AsAnsiString(255).NotNullable();

            Create.Table("tributacao_cst")
                .WithColumn("id").AsAnsiString(3).PrimaryKey("pk_tributacao_cst").NotNullable()
                .WithColumn("descricao").AsAnsiString(255).NotNullable()
                .WithColumn("regimeTributario").AsByte().NotNullable();
        }

        public override void Down()
        {
            
        }
    }
}