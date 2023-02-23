using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1550324537)]
    public class FA1550324537_AjusteEnderecoParaNfe : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                update
                pessoa_endereco
                set
                bairro = SUBSTRING(bairro, 1, 60),
                numero = SUBSTRING(numero, 1, 60),
                logradouro = SUBSTRING(logradouro, 1, 60),
                complemento = SUBSTRING(complemento, 1, 60)");

            Alter.Table("pessoa_endereco")
                .AlterColumn("bairro").AsAnsiString(60).NotNullable()
                .AlterColumn("numero").AsAnsiString(60).NotNullable()
                .AlterColumn("logradouro").AsAnsiString(60).NotNullable()
                .AlterColumn("complemento").AsAnsiString(60).NotNullable();
        }

        public override void Down()
        {
            
        }
    }
}