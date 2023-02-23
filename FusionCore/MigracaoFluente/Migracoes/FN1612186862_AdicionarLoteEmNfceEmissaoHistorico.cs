using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1612186862)]
    public class FN1612186862_AdicionarLoteEmNfceEmissaoHistorico : Migration
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