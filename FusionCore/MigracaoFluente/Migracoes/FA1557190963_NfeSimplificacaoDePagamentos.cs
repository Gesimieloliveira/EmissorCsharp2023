using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1557190963)]
    public class FA1557190963_NfeSimplificacaoDePagamentos : Migration
    {
        public override void Up()
        {
            Delete.Column("centroLucro_id").FromTable("nfe_forma_pagamento");
            Delete.Column("valorEntrada").FromTable("nfe_forma_pagamento");
            Delete.Column("historico").FromTable("nfe_forma_pagamento");

            Create.ForeignKey("fk_nfe_forma_pagamento_to_tipo_documento")
                .FromTable("nfe_forma_pagamento").ForeignColumn("tipoDocumento_id")
                .ToTable("tipo_documento").PrimaryColumn("id");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}