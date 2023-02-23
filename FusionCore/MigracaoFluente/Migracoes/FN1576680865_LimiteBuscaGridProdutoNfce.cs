using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1576680865)]
    public class FN1576680865_LimiteBuscaGridProdutoNfce : Migration
    {
        public override void Up()
        {
            Alter.Table("preferencia_terminal")
                .AddColumn("limiteBuscaGirdProduto")
                .AsInt32().NotNullable().WithDefaultValue(500);

            Delete.DefaultConstraint().OnTable("preferencia_terminal")
                .InSchema("dbo").OnColumn("limiteBuscaGirdProduto");
        }

        public override void Down()
        {
        }
    }
}