using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1593111711)]
    public class FA1593111711_EstruturaEventoPagamentoMDFe : Migration
    {
        public override void Up()
        {
            Create.Table("mdfe_evento_pagamento")
                .WithColumn("id").AsInt32().Identity().PrimaryKey("pk_mdfe_evento_pagamento").NotNullable()
                .WithColumn("mdfe_id").AsInt32().NotNullable()
                .ForeignKey("fk_mdfe_evento_pagamento__mdfe", "mdfe", "id")
                .WithColumn("quantidadeViagens").AsInt32().NotNullable()
                .WithColumn("numeroReferenciaViagem").AsInt32().NotNullable();

            Create.Table("mdfe_informacao_pagamento")
                .WithColumn("id").AsInt32().Identity().PrimaryKey("pk_mdfe_informacao_pagamento").NotNullable()
                .WithColumn("mdfeEventoPagamento_id").AsInt32().NotNullable().ForeignKey("fk_mdfe_evento_pagamento__mdfe_informacao_pagamento"
                    , "mdfe_evento_pagamento"
                    , "id")
                .WithColumn("nomeContratante").AsAnsiString(60).NotNullable()
                .WithColumn("documentoUnico").AsAnsiString(20).NotNullable()
                .WithColumn("valorContrato").AsDecimal(13, 2).NotNullable()
                .WithColumn("indicadorFormaPagamento").AsByte().NotNullable()
                .WithColumn("contaBancaria").AsAnsiString(5).NotNullable()
                .WithColumn("agenciaBancaria").AsAnsiString(10).NotNullable()
                .WithColumn("cnpjIpef").AsAnsiString(14)
                .WithColumn("informarApenasCnpjIpef").AsBoolean().NotNullable();

            Create.Table("mdfe_componente_pagamento_frete")
                .WithColumn("id").AsInt32().Identity().PrimaryKey("pk_mdfe_componente_pagamento_frete").NotNullable()
                .WithColumn("mdfeInformacaoPagamento_id").AsInt32().NotNullable().ForeignKey("fk_mdfe_informacao_pagamento__mdfe_componente_pagamento_frete"
                    , "mdfe_informacao_pagamento"
                    , "id")
                .WithColumn("tipoComponente").AsByte().NotNullable()
                .WithColumn("valor").AsDecimal(13,2).NotNullable()
                .WithColumn("descricao").AsAnsiString(60).NotNullable();

            Create.Table("mdfe_pagamento_parcela")
                .WithColumn("id").AsInt32().Identity().NotNullable().PrimaryKey("pk_mdfe_pagamento_parcela")
                .WithColumn("mdfeInformacaoPagamento_id").AsInt32().NotNullable().ForeignKey(
                    "fk_mdfe_informacao_pagamento__mdfe_pagamento_parcela",
                    "mdfe_informacao_pagamento",
                    "id")
                .WithColumn("numero").AsInt32().NotNullable()
                .WithColumn("dataVencimento").AsDateTime().NotNullable()
                .WithColumn("valor").AsDecimal(13, 2).NotNullable();
        }

        public override void Down()
        { 
        }
    }
}