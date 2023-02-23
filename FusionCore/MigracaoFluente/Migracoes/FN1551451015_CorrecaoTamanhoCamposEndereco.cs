using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1551451015)]
    public class FN1551451015_CorrecaoTamanhoCamposEndereco : Migration
    {
        public override void Up()
        {
            Execute.Sql(@" update cliente_endereco set
                bairro = SUBSTRING(bairro, 1, 60),
                numero = SUBSTRING(numero, 1, 60),
                logradouro = SUBSTRING(logradouro, 1, 60),
                complemento = SUBSTRING(complemento, 1, 60)
            ");

            Alter.Table("cliente_endereco")
                .AlterColumn("bairro").AsAnsiString(60).NotNullable()
                .AlterColumn("numero").AsAnsiString(60).NotNullable()
                .AlterColumn("logradouro").AsAnsiString(60).NotNullable()
                .AlterColumn("complemento").AsAnsiString(60).NotNullable();

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