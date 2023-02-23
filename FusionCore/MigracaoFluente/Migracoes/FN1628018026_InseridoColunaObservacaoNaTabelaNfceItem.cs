using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1628018026)]
    public class FN1628018026_InseridoColunaObservacaoNaTabelaNfceItem : Migration
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