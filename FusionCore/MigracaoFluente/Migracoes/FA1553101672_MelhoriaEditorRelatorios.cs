using System;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1553101672)]
    public class FA1553101672_MelhoriaEditorRelatorios : Migration
    {
        public override void Up()
        {
            Create.Table("relatorio_template")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey("pk_relatorio_template")
                .WithColumn("template").AsCustom("varbinary(max)").NotNullable()
                .WithColumn("tempGid").AsAnsiString().Nullable();

            Alter.Table("relatorio_customizado").AddColumn("template_id").AsGuid().Nullable();

            Create.ForeignKey("fk_relatoriocustomizado_to_template")
                .FromTable("relatorio_customizado").ForeignColumn("template_id")
                .ToTable("relatorio_template").PrimaryColumn("id");

            Execute.Sql("insert into relatorio_template(id, template, tempGid) select NEWID(), r.arquivoRelatorio, r.guid from relatorio_customizado r;");
            Execute.Sql("update relatorio_customizado set template_id = (select top 1 rt.id from relatorio_template rt where rt.tempGid = guid);");

            Alter.Table("relatorio_customizado").AlterColumn("template_id").AsGuid().NotNullable();

            Delete.Column("tempGid").FromTable("relatorio_template");
            Delete.Column("arquivoRelatorio").FromTable("relatorio_customizado");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}