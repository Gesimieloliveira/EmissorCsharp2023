using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1576630542)]
    public class FN1576630542_DeletarIdsRemotos : Migration
    {
        public override void Up()
        {
            Delete.Column("idRemoto").FromTable("nfce");
            Delete.Column("idRemoto").FromTable("nfce_item");
            Delete.Column("idRemoto").FromTable("nfce_item_icms");
            Delete.Column("idRemoto").FromTable("nfce_forma_pagamento");
            Delete.Column("idRemoto").FromTable("finaliza_emissao_sat");
            Delete.Column("idRemoto").FromTable("nfce_contingencia");
            Delete.Column("idRemoto").FromTable("nfce_emissao");
            Delete.Column("idRemoto").FromTable("nfce_destinatario");
            Delete.Column("idRemoto").FromTable("nfce_emitente");
            Delete.Column("idRemoto").FromTable("nfce_cancelamento");
            Delete.Column("idRemoto").FromTable("cancelamento_sat");
            Delete.Column("idRemoto").FromTable("nfce_emissao_historico");
            Delete.Column("idRemoto").FromTable("historico_envio_sat");
        }

        public override void Down()
        {
        }
    }
}