using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1549992104)]
    public class FA1549992104_BetaRegimeNormal : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"alter table nfe alter column entradaSaidaEm datetime null;");

            if (Schema.Table("tributacao_cst").Column("cst").Exists())
            {
                FazerMigracaoRegimeNormalParte1();
            }
        }

        private void FazerMigracaoRegimeNormalParte1()
        {
            Execute.Sql(@"EXEC sp_rename 'tributacao_cst.cst' , 'id', 'COLUMN';");

            Execute.Sql(@"alter table nfe_item_icms alter column situacaoTributariaCsosn_id varchar(3) null;");

            Execute.Sql(@"alter table nfe_item_icms add tributacaoCst_id varchar(3) null;");
            Execute.Sql(@"alter table nfe_item_icms add constraint fk_this_to_tributacaoCst_id foreign key(tributacaoCst_id) references tributacao_cst(id);");
            Execute.Sql(@"update nfe_item_icms set tributacaoCst_id = situacaoTributariaCsosn_id;");
            Execute.Sql(@"alter table nfe_item_icms alter column tributacaoCst_id varchar(3) not null;");

            Execute.Sql(@"alter table nfe_item_icms alter column valorBcFcpSt decimal(15,2) not null;");
            Execute.Sql(@"alter table nfe_item_icms alter column aliquotaFcpSt decimal(15,4) not null;");
            Execute.Sql(@"alter table nfe_item_icms alter column valorFcpSt decimal(15,2) not null;");

            Execute.Sql(@"alter table nfe_item_icms add aliquotaFcp decimal(15,4) not null default 0;");
            Execute.Sql(@"alter table nfe_item_icms add valorBcFcp decimal(15,2) not null default 0;");
            Execute.Sql(@"alter table nfe_item_icms add valorFcp decimal(15,2) not null default 0;");

            Execute.Sql(@"alter table nfe add totalFcp decimal (15,2) not null default 0;");
            Execute.Sql(@"update nfe set totalFcp = coalesce((select sum(nii.valorFcp) from nfe_item_icms nii inner join nfe_item ni  on ni.id = nii.nfeItem_id where ni.nfe_id = nfe.id), 0);");

            Execute.Sql(@"alter table nfe_item_icms drop column permiteFcpSt;");
            Execute.Sql(@"alter table nfe_item_icms drop constraint fk_nfe_item_icms__situacao_tributaria_cson;");
            Execute.Sql(@"alter table nfe_item_icms drop column situacaoTributariaCsosn_id;");
        }

        public override void Down()
        {
            
        }
    }
}