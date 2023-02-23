using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1617307111)]
    public class FA1617307111_CteConfiguracaoAutorizacao : Migration
    {
        public override void Up()
        {
            Create.Table("cte_configuracao_autorizacao")
                .WithColumn("id").AsGuid().PrimaryKey("pk_cte_configuracao_autorizacao")
                .WithColumn("tempoEsperaConsultaRecibo").AsInt32().NotNullable();

            Execute.Sql("insert into cte_configuracao_autorizacao (id, tempoEsperaConsultaRecibo) values ('6F7502F0-D9DF-49D3-BC6F-E52E1A0B942C', 5000)");
        }

        public override void Down()
        {
        }
    }
}