using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1628017143)]
    public class FA1628017143_InseridoColunaObservacaoNaTabelaNfceItem : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce_item")
                .AddColumn("observacao").AsString(500).Nullable().SetExistingRowsTo(string.Empty);
        }

        public override void Down()
        {
            
        }
    }
}