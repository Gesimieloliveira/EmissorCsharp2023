using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1553274812)]
    public class FA1553274812_CorrecaoTamanhoColunaTemplateRelatorio : Migration
    {
        public override void Up()
        {
            Alter.Table("relatorio_template")
                .AlterColumn("template").AsCustom("varbinary(max)").NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}