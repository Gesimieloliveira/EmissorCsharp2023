using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1551293508)]
    public class FA1551293508_AdicionaTabelaPisCofinsNfce : Migration
    {
        public override void Up()
        {
            Create.Table("nfce_item_pis")
                .WithColumn("nfceItem_id").AsInt32().PrimaryKey("pk_nfce_item_pis")
                .WithColumn("situacaoTributariaPis_id").AsAnsiString(2).NotNullable()
                .WithColumn("aliquota").AsDecimal(15, 4).NotNullable()
                .WithColumn("baseCalculo").AsDecimal(15, 2).NotNullable()
                .WithColumn("valor").AsDecimal(15, 2).NotNullable();

            Create.Table("nfce_item_cofins")
                .WithColumn("nfceItem_id").AsInt32().PrimaryKey("pk_nfce_item_cofins")
                .WithColumn("situacaoTributariaCofins_id").AsAnsiString(2).NotNullable()
                .WithColumn("aliquota").AsDecimal(15, 4).NotNullable()
                .WithColumn("baseCalculo").AsDecimal(15, 2).NotNullable()
                .WithColumn("valor").AsDecimal(15, 2).NotNullable();

            Create.ForeignKey("fk_nfce_item_pis__nfce_item_icms")
                .FromTable("nfce_item_pis").InSchema("dbo").ForeignColumn("nfceItem_id")
                .ToTable("nfce_item").InSchema("dbo").PrimaryColumn("id");

            Create.ForeignKey("fk_nfce_item_pis__situacao_tributaria_pis")
                .FromTable("nfce_item_pis").InSchema("dbo").ForeignColumn("situacaoTributariaPis_id")
                .ToTable("situacao_tributaria_pis").InSchema("dbo").PrimaryColumn("id");


            Create.ForeignKey("fk_nfce_item_cofins__nfce_item_icms")
                .FromTable("nfce_item_cofins").InSchema("dbo").ForeignColumn("nfceItem_id")
                .ToTable("nfce_item").InSchema("dbo").PrimaryColumn("id");

            Create.ForeignKey("fk_nfce_item_cofins__situacao_tributaria_cofins")
                .FromTable("nfce_item_cofins").InSchema("dbo").ForeignColumn("situacaoTributariaCofins_id")
                .ToTable("situacao_tributaria_cofins").InSchema("dbo").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}