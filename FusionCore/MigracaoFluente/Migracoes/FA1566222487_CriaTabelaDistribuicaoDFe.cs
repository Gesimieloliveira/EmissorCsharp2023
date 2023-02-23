using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1566222487)]
    public class FA1566222487_CriaTabelaDistribuicaoDFe : Migration
    {
        public override void Up()
        {
            Create.Table("mde_distribuicao")
                .WithColumn("id").AsInt32().Identity().PrimaryKey("pk_mde_distribuicao")
                .WithColumn("documentoUnicoInteressado").AsAnsiString(14).NotNullable()
                .WithColumn("ultimoNsuPesquisado").AsAnsiString(15).NotNullable()
                .WithColumn("maiorNsu").AsAnsiString(15).NotNullable()
                .WithColumn("xml").AsCustom("xml").NotNullable();
        }

        public override void Down()
        {
            
        }
    }
}