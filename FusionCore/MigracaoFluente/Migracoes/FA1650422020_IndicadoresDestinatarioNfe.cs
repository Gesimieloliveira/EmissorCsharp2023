using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1650422020)]
    public class FA1650422020_IndicadoresDestinatarioNfe : Migration
    {
        public override void Up()
        {
            Alter.Table("nfe_destinatario")
                .AddColumn("indicadorOperacaoFinal").AsInt16().Nullable()
                .AddColumn("indicadorPresenca").AsInt16().Nullable()
                .AddColumn("indicadorDestinoOperacao").AsInt16().Nullable();
        }

        public override void Down()
        {
            Delete.Column("indicadorOperacaoFinal")
                .Column("indicadorPresenca")
                .Column("indicadorDestinoOperacao")
                .FromTable("nfe_destinatario");
        }
    }
}