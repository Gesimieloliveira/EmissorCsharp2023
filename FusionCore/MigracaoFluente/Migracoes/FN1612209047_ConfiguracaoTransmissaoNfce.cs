using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1612209047)]
    public class FN1612209047_ConfiguracaoTransmissaoNfce : Migration
    {
        public override void Up()
        {
            Create.Table("configuracao_transmissao_nfce")
                .WithColumn("guid").AsGuid().NotNullable().PrimaryKey("pk_configuracao_transmissao_nfce")
                .WithColumn("transmissao").AsByte().NotNullable();

            Execute.Sql("insert into configuracao_transmissao_nfce (guid, transmissao)" +
                        " values ('6A6D8E7A-8181-4D4A-8CF6-D78A4F7D6B4B', 1)");
        }

        public override void Down()
        {
        }
    }
}