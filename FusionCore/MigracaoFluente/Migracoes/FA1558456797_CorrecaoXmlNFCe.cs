using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1558456797)]
    public class FA1558456797_CorrecaoXmlNFCe : Migration
    {
        public override void Up()
        {
            Execute.Sql("update nfce_emissao set xmlAutorizado = replace(cast(xmlAutorizado as varchar(max)), '<cMsg xmlns:p4=\"http://www.w3.org/2001/XMLSchema-instance\" p4:nil=\"true\"/>','') where recebidoEm > '2019-01-01';");
        }

        public override void Down()
        {
        }
    }
}