using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1562719216)]
    public class FA1562719216_PadronizacaoTamanhoCamposPessoa : Migration
    {
        public override void Up()
        {
            Alter.Table("pessoa_endereco")
                .AlterColumn("logradouro").AsAnsiString(60).NotNullable()
                .AlterColumn("numero").AsAnsiString(60).NotNullable()
                .AlterColumn("bairro").AsAnsiString(60).NotNullable()
                .AlterColumn("complemento").AsAnsiString(60).NotNullable();

            Execute.Sql("update pedido_destinatario set logradouro = substring(logradouro,1,60)");
            Execute.Sql("update pedido_destinatario set complemento = substring(complemento,1,60)");
            Execute.Sql("update pedido_destinatario set bairro = substring(bairro,1,60)");

            Alter.Table("pedido_destinatario")
                .AlterColumn("logradouro").AsAnsiString(60).NotNullable()
                .AlterColumn("numero").AsAnsiString(60).NotNullable()
                .AlterColumn("bairro").AsAnsiString(60).NotNullable()
                .AlterColumn("complemento").AsAnsiString(60).NotNullable();

            Alter.Table("nfe_destinatario")
                .AlterColumn("cep").AsAnsiString(8).NotNullable()
                .AlterColumn("nome").AsAnsiString(255).NotNullable()
                .AlterColumn("email").AsAnsiString(255).NotNullable();
        }

        public override void Down()
        {
        }
    }
}