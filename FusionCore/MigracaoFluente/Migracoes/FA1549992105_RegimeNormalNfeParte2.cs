using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1549992105)]
    public class FA1549992105_RegimeNormalNfeParte2: Migration
    {
        public override void Up()
        {
            Execute.Sql(@"alter table nfe_item_icms add aliquotaIcmsInternoSt decimal(15,4) not null default 0;");
            Execute.Sql(@"alter table nfe_item_icms alter column aliquotaCredito decimal(15,4) not null;");
            Execute.Sql(@"alter table nfe_item_icms alter column aliquotaIcms decimal(15,4) not null;");
            Execute.Sql(@"alter table nfe_item_icms alter column reducaoBcIcms decimal(15,4) not null;");
            Execute.Sql(@"alter table nfe_item_icms alter column aliquotaIcmsSt decimal(15,4) not null;");
            Execute.Sql(@"alter table nfe_item_icms alter column mva decimal(15,4) not null;");
            Execute.Sql(@"alter table nfe_item_icms alter column reducaoBcIcms decimal(15,4) not null;");

            Execute.Sql(@"exec sp_drop_defaultconstraint 'nfe_item_icms', 'calculoManual';");
            Execute.Sql(@"exec sp_drop_defaultconstraint 'nfe_item_ipi', 'calculoManual';");
            Execute.Sql(@"exec sp_drop_defaultconstraint 'nfe_item_pis', 'calculoManual';");
            Execute.Sql(@"exec sp_drop_defaultconstraint 'nfe_item_cofins', 'calculoManual';");

            Execute.Sql(@"alter table nfe_item_icms drop column calculoManual;");
            Execute.Sql(@"alter table nfe_item_ipi drop column calculoManual;");
            Execute.Sql(@"alter table nfe_item_pis drop column calculoManual;");
            Execute.Sql(@"alter table nfe_item_cofins drop column calculoManual;");

            Execute.Sql(@"alter table nfe_item add autoAjustarImposto bit not null default 1;");

            Execute.Sql(@"alter table perfil_nfe_simples_nacional drop column aliquotaFcp;");
            Execute.Sql(@"alter table perfil_nfe_simples_nacional drop column permiteFcp;");
            Execute.Sql(@"alter table perfil_cfop drop column sujeitoIcmsInterstadual;");
        }

        public override void Down()
        {
            
        }
    }
}