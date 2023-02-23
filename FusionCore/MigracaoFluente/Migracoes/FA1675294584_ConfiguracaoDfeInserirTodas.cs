using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1675294584)]
    public class FA1675294584_ConfiguracaoDfeInserirTodas : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"delete from configuracao_dfe
                          GO");
            Execute.Sql(@"INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'5867c430-b6c1-4e10-afbb-029ffd25f880', 2, 1, 1, 1, 24)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'd74c6e49-670d-4d7c-ad59-03ffa6f1a329', 2, 1, 1, 1, 27)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'bf16563f-d80a-439c-a68d-057eebd61ac3', 1, 1, 1, 1, 27)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'e1a98391-4bb5-4e72-83c5-093f47af6595', 2, 1, 1, 1, 21)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'7edc4bfd-6e6b-470f-b424-0a081a47f517', 2, 1, 1, 1, 17)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'b9f4a04d-5cce-494e-94e7-128f40c5fd52', 1, 1, 1, 1, 26)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'ba335e9c-821b-4bc3-8070-1699884bb531', 1, 1, 1, 1, 12)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'e1f8c874-dfb7-441b-9827-18047c23177b', 2, 1, 1, 1, 28)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'ef409b9b-788a-4c74-a5b0-264ca1073db5', 1, 1, 1, 1, 21)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'7722cda1-c372-4468-971c-2917d1e9fe72', 1, 1, 1, 1, 22)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'1ee2bc29-b0cd-43ea-b2ea-342221c3cf2d', 2, 1, 1, 1, 7)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'3f464706-da0a-40be-a084-38bf572cb39c', 2, 1, 1, 1, 11)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'b7401c1c-d44c-4e70-95d4-39cc4afdf667', 1, 1, 1, 1, 13)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'63306ee4-0ece-4f7f-9ad1-44a67d7f216c', 1, 1, 1, 1, 5)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'f9ef42e7-dd4e-4d28-854f-4e52e0d78d5d', 2, 1, 1, 1, 22)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'd61711e4-15dd-4fee-b936-5a017fcc5d79', 2, 1, 1, 1, 14)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'9535f5a1-291f-4abf-b3e6-61775ce6b3d5', 2, 1, 1, 1, 4)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'59e3a449-c09c-46d8-b325-65a3a9f07a98', 2, 1, 1, 1, 20)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'303865e4-e5ae-44a1-8091-66954cafe2ac', 1, 1, 1, 1, 3)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'71d2c083-7a0e-4647-bf29-699e4102d7be', 1, 1, 1, 1, 17)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'25ae8332-6313-45de-97a1-71e87e2762ad', 1, 1, 1, 1, 6)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'66a50288-9456-4000-9832-7a06ac6ed338', 1, 1, 1, 1, 18)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'97df4371-3cfb-43b9-988e-7f87b4c1d9a4', 1, 1, 1, 1, 14)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'808e0719-6ec5-40b8-9421-825230658c32', 2, 1, 1, 1, 18)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'4e816b6c-5bdf-476e-9ad8-8301cb3ef4f7', 2, 1, 1, 1, 26)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'9c2bc9ee-7a81-402b-8ac6-83b78fd5411e', 2, 1, 1, 1, 8)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'85430ff4-1865-455c-8635-84c263909572', 2, 1, 1, 1, 9)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'e6a40006-a399-463f-b022-867084da9692', 2, 1, 1, 1, 13)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'5623aa80-9d6a-42f9-9808-88c3947f6d31', 2, 1, 1, 1, 23)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'2b8369be-e005-4017-9a1f-8a22db16e591', 1, 1, 1, 1, 9)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'7167eadb-a020-4c12-a7d5-8d6bbe5dd1ff', 2, 1, 1, 1, 3)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'50c0b644-c3da-49fe-b918-8f0d1c86bb2f', 2, 1, 1, 1, 19)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'bf0550d6-846e-4062-999f-925ed54ced5a', 2, 1, 1, 1, 2)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'3dcd5696-f57f-4d77-9176-9712627c9c90', 2, 1, 1, 1, 6)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'01528a6d-1aae-442e-b27c-97baca5a85ce', 1, 1, 1, 1, 23)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'1746d82d-d054-4f56-b4f3-9ca84396804e', 1, 1, 1, 1, 16)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'c53faa58-db32-4962-a424-9eb314119a47', 1, 1, 1, 1, 25)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'07a3f05d-1e98-4289-9133-9fdb1b549025', 1, 1, 1, 1, 8)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'3a41e3c6-81df-4921-9d0c-a0c6872f1b68', 2, 1, 1, 1, 25)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'd7f2b57c-54be-4571-b5be-a45e7050858d', 1, 1, 1, 1, 28)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'3286c1ad-33fd-4866-bf8e-a560640ed6ef', 1, 1, 1, 1, 4)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'8837e35c-d3e6-4500-b991-c0ca1ee4ef2c', 2, 1, 1, 1, 10)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'e0c83063-336f-4a53-9ced-c34939057cec', 1, 1, 1, 1, 15)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'2c28693f-24ec-4efc-b81a-c58d86bb29f7', 2, 1, 1, 1, 12)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'371c492f-4c0d-4676-a5df-c8e87fa2629f', 1, 1, 1, 1, 1)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'12d7daca-e139-4edb-9e9d-d1697d701446', 1, 1, 1, 1, 10)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'30213273-d05d-4b2e-be6b-d574812e1e42', 2, 1, 1, 1, 15)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'03012788-eb2b-40d9-938c-da31dd5986b7', 2, 1, 1, 1, 1)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'c3ec0980-a181-45ea-86d5-dec231f929da', 1, 1, 1, 1, 24)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'060dda6d-f465-4f86-8975-e2086d693f8d', 2, 1, 1, 1, 5)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'a060eed0-327a-4fbe-8056-e94975ab27da', 2, 1, 1, 1, 16)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'101fa9b5-f060-4fee-b450-f27daa892e97', 1, 1, 1, 1, 11)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'a545a6c7-b197-4268-ae8d-f29fe148f27a', 1, 1, 1, 1, 20)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'196acb34-dcbb-4c64-9e98-f3bcc876cd2a', 1, 1, 1, 1, 7)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'555ced07-dc3e-4cc5-82c9-f5d0326f742c', 1, 1, 1, 1, 19)
                INSERT [dbo].[configuracao_dfe] ([id], [ambienteSefaz], [isQrCodeCte], [isQrCodeCteOs], [isQrCodeMdfe], [uf_id]) VALUES (N'dd5f2c9f-86c0-47ca-a602-f8aae2638d40', 1, 1, 1, 1, 2)
                GO");
        }

        public override void Down()
        {
        }
    }
}