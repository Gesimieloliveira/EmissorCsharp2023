using System.Collections.Generic;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1551114243)]
    public class FN1551114243_InserirTributacoes : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "01"}, {"descricao", "Operação Tributável com Alíquota Básica"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "02"}, {"descricao", "Operação Tributável com Alíquota Diferenciada"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "03"}, {"descricao", "Operação Tributável com Alíquota por Unidade de Medida de Produto"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "04"}, {"descricao", "Operação Tributável Monofásica - Revenda a Alíquota Zero"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "05"}, {"descricao", "Operação Tributável por Substituição Tributária"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "06"}, {"descricao", "Operação Tributável a Alíquota Zero"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "07"}, {"descricao", "Operação Isenta da Contribuição"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "08"}, {"descricao", "Operação Sem Incidência da Contribuição"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "09"}, {"descricao", "Operação com Suspensão da Contribuição"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "49"}, {"descricao", "Outras Operações de Saída"}
                });




            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "01"}, {"descricao", "Operação Tributável com Alíquota Básica"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "02"}, {"descricao", "Operação Tributável com Alíquota Diferenciada"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "03"}, {"descricao", "Operação Tributável com Alíquota por Unidade de Medida de Produto"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "04"}, {"descricao", "Operação Tributável Monofásica - Revenda a Alíquota Zero"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "05"}, {"descricao", "Operação Tributável por Substituição Tributária"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "06"}, {"descricao", "Operação Tributável a Alíquota Zero"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "07"}, {"descricao", "Operação Isenta da Contribuição"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "08"}, {"descricao", "Operação Sem Incidência da Contribuição"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "09"}, {"descricao", "Operação com Suspensão da Contribuição"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "49"}, {"descricao", "Outras Operações de Saída"}
                });



            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "00"}, {"descricao", "Tributado integralmente"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "10"}, {"descricao", "Tributado com cobrança de ICMS por ST"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "101"}, {"descricao", "Tributada pelo Simples Nacional com permissão de crédito"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "102"}, {"descricao", "Tributada pelo Simples Nacional sem permissão de crédito"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "103"}, {"descricao", "Isenção do ICMS no Simples Nacional para faixa de receita bruta"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "20"}, {"descricao", "Redução de base de cálculo"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "201"}, {"descricao", "Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS substituição tributária"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "202"}, {"descricao", "Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS substituição tributária"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "203"}, {"descricao", "Isenção do ICMS no Simples Nacional para faixa de receita bruta e com cobrança do ICMS por substituição tributária"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "30"}, {"descricao", "Isento ou não tributado com cobrança de ICMS ST"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "300"}, {"descricao", "Imune"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "40"}, {"descricao", "Isento"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "400"}, {"descricao", "Não tributada pelo Simples Nacional"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "41"}, {"descricao", "Não tributado"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "50"}, {"descricao", "Com suspensão"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "500"}, {"descricao", "ICMS cobrado anteriormente por substituição tributária"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "51"}, {"descricao", "Com diferimento"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "60"}, {"descricao", "ICMS cobrado anteriormente por ST"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "70"}, {"descricao", "Redução na base de cálculo e cobrança do ICMS por ST"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "90"}, {"descricao", "Outros"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "900"}, {"descricao", "Outros"}, {"regimeTributario", 1}
                });
        }

        public override void Down()
        {
            
        }
    }
}