using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1630007636)]
    public class FN1630007636_InseridoTabelaPreferenciaMaquina : Migration
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