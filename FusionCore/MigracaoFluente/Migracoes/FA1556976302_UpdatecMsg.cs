using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1556976302)]
    public class FA1556976302_UpdatecMsg : Migration
    {
        public override void Up()
        {
            Execute.Sql("update nfe_emissao set xmlAutorizado = replace(cast(xmlAutorizado as varchar(max)), '<cMsg xmlns:p4=\"http://www.w3.org/2001/XMLSchema-instance\" p4:nil=\"true\"/>','') where recebidoEm > '2019-01-01';");
        }

        public override void Down()
        {
        }
    }
}