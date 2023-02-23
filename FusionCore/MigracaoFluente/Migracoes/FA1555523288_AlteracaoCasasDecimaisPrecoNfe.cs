using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1555523288)]
    public class FA1555523288_AlteracaoCasasDecimaisPrecoNfe : Migration
    {
        public override void Up()
        {
            Alter.Table("nfe_item_mercadoria").AlterColumn("valorUnitario").AsDecimal(21, 10).NotNullable();
            Alter.Table("nfe_item_mercadoria").AlterColumn("porcentagemDesconto").AsDecimal(21, 10).NotNullable();
            Alter.Table("nfe_item_mercadoria").AlterColumn("valorUnitarioComDesconto").AsDecimal(21, 10).NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}