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
                    {"id", "01"}, {"descricao", "Opera��o Tribut�vel com Al�quota B�sica"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "02"}, {"descricao", "Opera��o Tribut�vel com Al�quota Diferenciada"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "03"}, {"descricao", "Opera��o Tribut�vel com Al�quota por Unidade de Medida de Produto"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "04"}, {"descricao", "Opera��o Tribut�vel Monof�sica - Revenda a Al�quota Zero"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "05"}, {"descricao", "Opera��o Tribut�vel por Substitui��o Tribut�ria"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "06"}, {"descricao", "Opera��o Tribut�vel a Al�quota Zero"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "07"}, {"descricao", "Opera��o Isenta da Contribui��o"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "08"}, {"descricao", "Opera��o Sem Incid�ncia da Contribui��o"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "09"}, {"descricao", "Opera��o com Suspens�o da Contribui��o"}
                });

            Insert.IntoTable("situacao_tributaria_cofins").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "49"}, {"descricao", "Outras Opera��es de Sa�da"}
                });




            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "01"}, {"descricao", "Opera��o Tribut�vel com Al�quota B�sica"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "02"}, {"descricao", "Opera��o Tribut�vel com Al�quota Diferenciada"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "03"}, {"descricao", "Opera��o Tribut�vel com Al�quota por Unidade de Medida de Produto"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "04"}, {"descricao", "Opera��o Tribut�vel Monof�sica - Revenda a Al�quota Zero"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "05"}, {"descricao", "Opera��o Tribut�vel por Substitui��o Tribut�ria"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "06"}, {"descricao", "Opera��o Tribut�vel a Al�quota Zero"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "07"}, {"descricao", "Opera��o Isenta da Contribui��o"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "08"}, {"descricao", "Opera��o Sem Incid�ncia da Contribui��o"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "09"}, {"descricao", "Opera��o com Suspens�o da Contribui��o"}
                });

            Insert.IntoTable("situacao_tributaria_pis").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "49"}, {"descricao", "Outras Opera��es de Sa�da"}
                });



            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "00"}, {"descricao", "Tributado integralmente"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "10"}, {"descricao", "Tributado com cobran�a de ICMS por ST"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "101"}, {"descricao", "Tributada pelo Simples Nacional com permiss�o de cr�dito"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "102"}, {"descricao", "Tributada pelo Simples Nacional sem permiss�o de cr�dito"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "103"}, {"descricao", "Isen��o do ICMS no Simples Nacional para faixa de receita bruta"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "20"}, {"descricao", "Redu��o de base de c�lculo"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "201"}, {"descricao", "Tributada pelo Simples Nacional com permiss�o de cr�dito e com cobran�a do ICMS substitui��o tribut�ria"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "202"}, {"descricao", "Tributada pelo Simples Nacional sem permiss�o de cr�dito e com cobran�a do ICMS substitui��o tribut�ria"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "203"}, {"descricao", "Isen��o do ICMS no Simples Nacional para faixa de receita bruta e com cobran�a do ICMS por substitui��o tribut�ria"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "30"}, {"descricao", "Isento ou n�o tributado com cobran�a de ICMS ST"}, {"regimeTributario", 3}
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
                    {"id", "400"}, {"descricao", "N�o tributada pelo Simples Nacional"}, {"regimeTributario", 1}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "41"}, {"descricao", "N�o tributado"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "50"}, {"descricao", "Com suspens�o"}, {"regimeTributario", 3}
                });

            Insert.IntoTable("tributacao_cst").InSchema("dbo")
                .Row(new Dictionary<string, object>
                {
                    {"id", "500"}, {"descricao", "ICMS cobrado anteriormente por substitui��o tribut�ria"}, {"regimeTributario", 1}
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
                    {"id", "70"}, {"descricao", "Redu��o na base de c�lculo e cobran�a do ICMS por ST"}, {"regimeTributario", 3}
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