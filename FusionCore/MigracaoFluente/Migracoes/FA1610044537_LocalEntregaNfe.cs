using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1610044537)]
    public class FA1610044537_LocalEntregaNfe : Migration
    {
        public override void Up()
        {
            Create.Table("nfe_local_entrega")
                .WithColumn("nfe_id").AsInt32().NotNullable()
                    .ForeignKey("fk_nfe_local_entrega__nfe", "nfe", "id")
                .WithColumn("pessoaEndereco_id").AsInt32().NotNullable()
                    .ForeignKey("fk_nfe_local_entrega__pessoa", "pessoa_endereco", "id");
        }

        public override void Down()
        {
        }
    }
}