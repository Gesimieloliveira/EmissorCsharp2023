using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1629893772)]
    public class FA1629893772_InseridoTabelaPreferenciaMaquina : Migration
    {
        public override void Up()
        {
            Create.Table("preferencia_maquina")
               .WithColumn("id").AsGuid().NotNullable().PrimaryKey("pk_preferencia_maquina")
               .WithColumn("idMaquina").AsAnsiString(200).NotNullable()
               .WithColumn("chave").AsAnsiString(200).NotNullable()
               .WithColumn("valor").AsAnsiString(6000).NotNullable();

            Create.UniqueConstraint("uk_chave").OnTable("preferencia_maquina").Columns("idMaquina", "chave");
                
        }

        public override void Down()
        {
            
        }
    }
}