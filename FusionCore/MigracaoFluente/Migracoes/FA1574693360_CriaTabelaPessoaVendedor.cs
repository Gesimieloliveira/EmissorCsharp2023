using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1574693360)]
    public class FA1574693360_CriaTabelaPessoaVendedor : Migration
    {
        public override void Up()
        {
            Create.Table("pessoa_vendedor").WithColumn("pessoa_id").AsInt32()
                .NotNullable().PrimaryKey("pk_pessoa_vendedor__pessoa")
                .ForeignKey("fk_pessoa_vendedor__pessoa", "pessoa", "id");
        }

        public override void Down()
        {
        }
    }
}