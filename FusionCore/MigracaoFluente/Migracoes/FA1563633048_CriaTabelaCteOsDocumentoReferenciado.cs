using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1563633048)]
    public class FA1563633048_CriaTabelaCteOsDocumentoReferenciado : Migration
    {
        public override void Up()
        {
            Create.Table("cte_os_documento_referenciado")
                .WithColumn("id").AsInt32().Identity().PrimaryKey("pk_cte_os_documento_referenciado")
                .WithColumn("cteOs_id").AsInt32().NotNullable()
                .WithColumn("numero").AsAnsiString(20).NotNullable()
                .WithColumn("serie").AsInt16().Nullable()
                .WithColumn("subSerie").AsInt16().Nullable()
                .WithColumn("emitidaEm").AsDateTime().NotNullable()
                .WithColumn("valor").AsDecimal(15, 2).Nullable();

            Create.ForeignKey("fk_cte_os_documento_referenciado__cte_os")
                .FromTable("cte_os_documento_referenciado")
                .InSchema("dbo").ForeignColumn("cteOs_id")
                .ToTable("cte_os").InSchema("dbo")
                .PrimaryColumn("id");
        }

        public override void Down()
        {
            
        }
    }
}