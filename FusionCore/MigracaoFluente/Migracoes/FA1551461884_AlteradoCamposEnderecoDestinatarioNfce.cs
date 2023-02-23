using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1551461884)]
    public class FA1551461884_AlteradoCamposEnderecoDestinatarioNfce : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update nfce_destinatario set 
                bairro = substring(bairro, 1, 60),
                logradouro = substring(logradouro, 1, 60),
                complemento = substring(complemento, 1, 60)
            ");

            Alter.Table("nfce_destinatario")
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