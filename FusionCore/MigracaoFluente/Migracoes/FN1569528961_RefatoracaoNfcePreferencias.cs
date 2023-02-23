using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1569528961)]
    public class FN1569528961_RefatoracaoNfcePreferencias : Migration
    {
        public override void Up()
        {
            Delete.Table("nfce_preferencia");

            Create.Table("preferencia_terminal")
                .WithColumn("id").AsGuid().PrimaryKey("pk_nfce_preferencia")
                .WithColumn("solicitaInformacaoItem").AsBoolean().NotNullable()
                .WithColumn("visualizaAntesDeImprimir").AsBoolean().NotNullable()
                .WithColumn("nomeImpressora").AsAnsiString(255).NotNullable();

            Delete.Column("preVisualizarImpressao").FromTable("configuracao_terminal");
            Delete.Column("impressora").FromTable("configuracao_terminal");
        }

        public override void Down()
        {
        }
    }
}