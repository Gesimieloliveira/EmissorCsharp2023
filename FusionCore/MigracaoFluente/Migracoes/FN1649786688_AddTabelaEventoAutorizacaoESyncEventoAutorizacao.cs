using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1649786688)]
    public class FN1649786688_AddTabelaEventoAutorizacaoESyncEventoAutorizacao : Migration
    {
        public override void Up()
        {
            Create.Table("evento_operacao_autorizada")
             .WithColumn("id").AsGuid().NotNullable().PrimaryKey("pk_evento_operacao_autorizada")
             .WithColumn("dataCriacao").AsDateTime().NotNullable()
             .WithColumn("usuarioLogado_id").AsInt32().NotNullable().ForeignKey("fk_usuario_logado_id", "usuario", "id")
             .WithColumn("usuarioAutorizou_id").AsInt32().NotNullable().ForeignKey("fk_usuario_autorizou_id", "usuario", "id")
             .WithColumn("payload").AsAnsiString(5000).NotNullable()
             .WithColumn("permissao").AsInt32().NotNullable()
             .WithColumn("permissaoTexto").AsAnsiString(255).NotNullable()
             .WithColumn("agregadoAfetado").AsAnsiString(255).NotNullable();

            Create.Table("sync_evento_operacao_autorizada")
                .WithColumn("id").AsGuid().NotNullable();
        }

        public override void Down()
        {
            
        }
    }
}