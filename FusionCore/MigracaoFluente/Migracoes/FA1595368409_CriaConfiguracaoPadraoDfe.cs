using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1595368409)]
    public class FA1595368409_CriaConfiguracaoPadraoDfe : Migration
    {
        public override void Up()
        {
			Execute.Sql("delete from configuracao_dfe");

            Execute.Sql(@"insert into configuracao_dfe
	            (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	            values (NEWID(), 1, 1, 1, 1, 1)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 1)


                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 2)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 2)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 3)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 3)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 4)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 4)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 5)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 5)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 6)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 6)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 7)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 7)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 8)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 8)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 9)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 9)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 10)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 10)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 11)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 11)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 12)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 12)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 13)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 13)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 14)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 14)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 15)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 15)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 16)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 16)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 17)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 17)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 18)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 18)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 19)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 19)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 20)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 20)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 21)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 21)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 22)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 22)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 23)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 23)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 24)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 24)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 25)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 25)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 26)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 26)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 27)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 27)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 1, 1, 1, 1, 28)

                insert into configuracao_dfe
	                (id, ambienteSefaz, isQrCodeCte, isQrCodeCteOs, isQrCodeMdfe, uf_id)
	                values (NEWID(), 2, 1, 1, 1, 28)");
        }

        public override void Down()
        {
        }
    }
}