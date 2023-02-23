using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1569329642)]
    public class FA1569329642_AlteraEmissaoHistoricoMdfe : Migration
    {
        public override void Up()
        {
            Alter.Table("mdfe_emissao_historico")
                .AddColumn("numeroRecibo").AsAnsiString(15).NotNullable()
                .AddColumn("xmlLote").AsCustom("xml").Nullable();
            
        }

        public override void Down()
        {
        }
    }
}