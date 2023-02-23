using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1616440786)]
    public class FA1616440786_AdicionadoXmlCancelamentoCupomFiscal : Migration
    {
        public override void Up()
        {
            Alter.Table("cupom_fiscal")
                .AddColumn("xmlCancelamento").AsCustom("xml").Nullable();
        }

        public override void Down()
        {
        }
    }
}