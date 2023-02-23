using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1600626597)]
    public class FA1600626597_NovaEstruturaItemDistribuicaoDfe : Migration
    {
        public override void Up()
        {
            Alter.Table("mde_distribuicao")
                .AddColumn("dataCriacao").AsDateTime().NotNullable().SetExistingRowsTo(DateTime.Now);

            Create.Table("mde_distribuicao_item")
                .WithColumn("id").AsInt32().Identity().PrimaryKey("pk_mde_distribuicao_item")
                .WithColumn("mdeDistribuicao_id").AsInt32().NotNullable()
                .WithColumn("xmlDescompactado").AsXml().NotNullable()
                .WithColumn("nsu").AsInt64().NotNullable()
                .WithColumn("nomeSchema").AsAnsiString(100).NotNullable()
                .WithColumn("tipoDfe").AsInt32().NotNullable()
                .WithColumn("tipoEvento").AsInt32().Nullable()
                .WithColumn("processado").AsByte().NotNullable();

            Create.ForeignKey("fk_mde_distribuicao_item_to_distribuicao_dfe_id")
                .FromTable("mde_distribuicao_item").ForeignColumn("mdeDistribuicao_id")
                .ToTable("mde_distribuicao").PrimaryColumn("id");

        }

        public override void Down()
        {
        }
    }
}