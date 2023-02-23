using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1556658389)]
    public class FA1556658389_AlteracaoEnderecoDestinatarioDoFaturamento : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update faturamento_destinatario set 
                bairro = substring(bairro, 1, 60),
                logradouro = substring(logradouro, 1, 60),
                complemento = substring(complemento, 1, 60)
            ");

            Alter.Table("faturamento_destinatario")
                .AlterColumn("bairro").AsAnsiString(60).NotNullable()
                .AlterColumn("numero").AsAnsiString(60).NotNullable()
                .AlterColumn("logradouro").AsAnsiString(60).NotNullable()
                .AlterColumn("complemento").AsAnsiString(60).NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}