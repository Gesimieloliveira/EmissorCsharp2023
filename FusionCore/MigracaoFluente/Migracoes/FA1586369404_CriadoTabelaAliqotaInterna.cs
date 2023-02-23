using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1586369404)]
    public class FA1586369404_CriadoTabelaAliqotaInterna : Migration
    {
        public override void Up()
        {
            const string tabela = "aliquota_interna";

            Create.Table(tabela)
                .WithColumn("id").AsGuid().PrimaryKey("pk_aliquota_interna")
                .WithColumn("aliquota").AsDecimal(5, 2).NotNullable()
                .WithColumn("aliquotaFcp").AsDecimal(5, 2).NotNullable()
                .WithColumn("uf_id").AsByte().NotNullable();


            var ac = new
            {
                id = "DE945B89-E75A-4599-BC38-171381FDD398",
                aliquota = 17.00,
                aliquotaFcp = 0.00,
                uf_id = 1
            };

            var al = new
            {
                id = "C073554F-CC0F-4632-994A-5F1E0E4638D1",
                aliquota = 17.00,
                aliquotaFcp = 1.00,
                uf_id = 2
            };

            var am = new
            {
                id = "F887C8EA-3E8B-4AA1-AC22-3047A579356C",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 3
            };

            var ap = new
            {
                id = "BEBD61D9-3838-495A-9FB0-10D4A686E2BF",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 4
            };

            var ba = new
            {
                id = "63656F32-DB72-4E37-ABEC-93AFFCDE416D",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 5
            };

            var ce = new
            {
                id = "D38C0C4E-73B0-4422-A27E-8DA0A641E083",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 6
            };

            var df = new
            {
                id = "201A443D-2553-4853-8A66-6C8FA275A7A1",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 7
            };

            var es = new
            {
                id = "C2C275C5-98BB-43A5-83B0-C6617C52F326",
                aliquota = 17.00,
                aliquotaFcp = 0.00,
                uf_id = 8
            };

            var go = new
            {
                id = "26028A95-9ADE-429D-A52C-67CBA5829BD8",
                aliquota = 17.00,
                aliquotaFcp = 0.00,
                uf_id = 10
            };

            var ma = new
            {
                id = "C917D2D1-0FB3-45A3-8EF3-BA957B333453",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 11
            };

            var mt = new
            {
                id = "891F1BDB-36A6-45D4-A621-9AE223305D27",
                aliquota = 17.00,
                aliquotaFcp = 0.00,
                uf_id = 14
            };

            var ms = new
            {
                id = "1FBBF468-D1D5-415D-AA48-F63223A8E126",
                aliquota = 17.00,
                aliquotaFcp = 0.00,
                uf_id = 13
            };

            var mg = new
            {
                id = "34E88D9D-05C6-4A9B-8605-076D792B4F1E",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 12
            };

            var pa = new
            {
                id = "35313A2B-5303-4F0A-988D-476E84B5ADF6",
                aliquota = 17.00,
                aliquotaFcp = 0.00,
                uf_id = 15
            };

            var pb = new
            {
                id = "AA0245E1-9FF6-4ECC-B448-956D9362E259",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 16
            };

            var pr = new
            {
                id = "35362583-8218-42CE-83DE-4BA7504AE49A",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 19
            };

            var pe = new
            {
                id = "0CC1D3D8-3035-42D2-B8E8-1DA9A035DAE8",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 17
            };

            var pi = new
            {
                id = "70539952-A10D-4FD7-834D-C20FCAC9B88A",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 18
            };

            var rn = new
            {
                id = "9C00BA55-4895-4CC0-9B0B-030B5E6D2CE3",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 21
            };

            var rs = new
            {
                id = "7B93F65C-BE3C-48DC-A9D9-F30A74528B33",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 24
            };

            var rj = new
            {
                id = "7A52A407-6D86-4A5B-8433-01454FFD092B",
                aliquota = 18.00,
                aliquotaFcp = 2.00,
                uf_id = 20
            };

            var ro = new
            {
                id = "07D9B2C7-53D5-4C3D-A06F-60071075D8C0",
                aliquota = 17.50,
                aliquotaFcp = 0.00,
                uf_id = 22
            };

            var rr = new
            {
                id = "B89F54CD-597C-4CCD-8548-319D76C0DE5D",
                aliquota = 17.00,
                aliquotaFcp = 0.00,
                uf_id = 23
            };

            var sc = new
            {
                id = "A6A99A78-189F-4137-B7D7-AF2E003DEC3B",
                aliquota = 17.00,
                aliquotaFcp = 0.00,
                uf_id = 25
            };

            var sp = new
            {
                id = "6176F221-C74F-4765-81C8-6C0D8FEEB761",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 27
            };

            var se = new
            {
                id = "E5D9F95F-080E-4C79-98BA-CCF18583AFFD",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 26
            };

            var to = new
            {
                id = "D49D802C-DBD7-4B07-A0BC-438C83D7F1C0",
                aliquota = 18.00,
                aliquotaFcp = 0.00,
                uf_id = 28
            };

            Insert.IntoTable(tabela).InSchema("dbo").Row(ac);
            Insert.IntoTable(tabela).InSchema("dbo").Row(al);
            Insert.IntoTable(tabela).InSchema("dbo").Row(am);
            Insert.IntoTable(tabela).InSchema("dbo").Row(ap);
            Insert.IntoTable(tabela).InSchema("dbo").Row(ba);
            Insert.IntoTable(tabela).InSchema("dbo").Row(ce);
            Insert.IntoTable(tabela).InSchema("dbo").Row(df);
            Insert.IntoTable(tabela).InSchema("dbo").Row(es);
            Insert.IntoTable(tabela).InSchema("dbo").Row(go);
            Insert.IntoTable(tabela).InSchema("dbo").Row(ma);
            Insert.IntoTable(tabela).InSchema("dbo").Row(mt);
            Insert.IntoTable(tabela).InSchema("dbo").Row(ms);
            Insert.IntoTable(tabela).InSchema("dbo").Row(mg);
            Insert.IntoTable(tabela).InSchema("dbo").Row(pa);
            Insert.IntoTable(tabela).InSchema("dbo").Row(pb);
            Insert.IntoTable(tabela).InSchema("dbo").Row(pr);
            Insert.IntoTable(tabela).InSchema("dbo").Row(pe);
            Insert.IntoTable(tabela).InSchema("dbo").Row(pi);
            Insert.IntoTable(tabela).InSchema("dbo").Row(rn);
            Insert.IntoTable(tabela).InSchema("dbo").Row(rs);
            Insert.IntoTable(tabela).InSchema("dbo").Row(rj);
            Insert.IntoTable(tabela).InSchema("dbo").Row(ro);
            Insert.IntoTable(tabela).InSchema("dbo").Row(rr);
            Insert.IntoTable(tabela).InSchema("dbo").Row(sc);
            Insert.IntoTable(tabela).InSchema("dbo").Row(sp);
            Insert.IntoTable(tabela).InSchema("dbo").Row(se);
            Insert.IntoTable(tabela).InSchema("dbo").Row(to);
        }

        public override void Down()
        {
        }
    }
}