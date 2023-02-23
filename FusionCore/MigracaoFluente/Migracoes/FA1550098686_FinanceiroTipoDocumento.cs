using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1550098686)]
    public class FA1550098686_FinanceiroTipoDocumento : Migration
    {
        public override void Up()
        {
            Alter.Table("tipo_documento")
                .AddColumn("registraFinanceiro").AsBoolean().NotNullable().WithDefaultValue(0);

            Alter.Table("pagamento_especie")
                .AddColumn("tipoDocumento_id").AsInt16().Nullable();

            Create.ForeignKey("fk_especie_to_tipodocumento")
                .FromTable("pagamento_especie").ForeignColumn("tipoDocumento_id")
                .ToTable("tipo_documento").PrimaryColumn("id");

            Execute.Sql("update pagamento_especie set tipoDocumento_id = (select top 1 p.tipoDocumento_id from pagamento_parcela p where p.pagamentoEspecie_id = pagamento_especie.id);");
            
            Delete.ForeignKey("fk_parcela_to_tipodocumento").OnTable("pagamento_parcela");
            Delete.Column("tipoDocumento_id").FromTable("pagamento_parcela");

            Execute.Sql("update tipo_documento set registraFinanceiro = (select (case when count(*) >= 1 then 1 else 0 end) from documento_receber)");
        }

        public override void Down()
        {
            
        }
    }
}