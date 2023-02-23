using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1612210014)]
    public class FA1612210014_LotesHistoricoNfceAdm : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce_emissao_historico")
                .AddColumn("xmlLote").AsCustom("xml")
                .Nullable();

            Alter.Table("nfce_emissao_historico")
                .AddColumn("falhaReceberLote").AsBoolean()
                .NotNullable().WithDefaultValue(false);

            Delete.DefaultConstraint().OnTable("nfce_emissao_historico")
                .InSchema("dbo").OnColumn("falhaReceberLote");
        }

        public override void Down()
        {
        }
    }
}