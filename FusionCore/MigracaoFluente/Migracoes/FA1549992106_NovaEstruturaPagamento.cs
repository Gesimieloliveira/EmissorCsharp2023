using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1549992106)]
    public class FA1549992106_NovaEstruturaPagamento : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"alter table documento_receber alter column centroLucro_id smallint null;");

            Execute.Sql(@"create table pagamento(
	            id uniqueidentifier not null,
	            numero int not null identity(1,1),
	            criadoEm datetime not null,
	            criadoPor_id int not null,
	            valor decimal(15,2) not null,
	            constraint pk_pagamento primary key(id)
            );");

            Execute.Sql(@"create table pagamento_especie(
	            id int not null identity(1,1),
	            pagamento_id uniqueidentifier not null,
	            especie tinyint not null,
	            valor decimal(15,2) not null,
	            possuiParcelas bit not null,
	            constraint pk_pagamento_especie primary key(id),
	            constraint fk_especie_to_pagamento foreign key (pagamento_id) references pagamento(id)
            );");

            Execute.Sql(@"create table pagamento_parcela(
	            id int not null identity(1,1),
	            pagamentoEspecie_id int not null,
	            tipoDocumento_id smallint not null,
	            numero smallint not null,
	            vencimento datetime not null,
	            valor decimal(15,2),
	            constraint pk_pagamento_parcela primary key(id),
	            constraint fk_parcela_to_especie foreign key (pagamentoEspecie_id) references pagamento_especie(id),
	            constraint fk_parcela_to_tipodocumento foreign key (tipoDocumento_id) references tipo_documento(id)
            );");
        }

        public override void Down()
        {
            
        }
    }
}