using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1570464818)]
    public class FA1570464818_CriacaoEstruturaContaCaixaUserStory184 : Migration
    {
        private decimal _somaSaldoCaixa;
        private const int TipoEntrada = 0;
        private const int TipoSaida = 1;

        public override void Up()
        {
            CriarTabelaContaCaixaLoja();
            MigrarFluxoDeCaixa();
        }

        private void CriarTabelaContaCaixaLoja()
        {
            Create.Table("conta_caixa_fluxo")
                .WithColumn("id").AsGuid().PrimaryKey("pk_conta_caixa_fluxo")
                .WithColumn("fluxo").AsInt64().NotNullable().Identity()
                .WithColumn("dataCriacao").AsDateTime().NotNullable()
                .WithColumn("dataOperacao").AsDateTime().NotNullable()
                .WithColumn("usuario_id").AsInt32().NotNullable()
                .WithColumn("tipoOperacao").AsInt16().NotNullable()
                .WithColumn("totalOperacao").AsDecimal(15, 2).NotNullable()
                .WithColumn("saldoAtual").AsDecimal(20, 2).NotNullable()
                .WithColumn("historico").AsAnsiString(255).NotNullable()
                .WithColumn("caixaIndividual_id").AsGuid().Nullable();

            Create.ForeignKey("fk_conta_caixa_fluxo_to_caixa_individual")
                .FromTable("conta_caixa_fluxo").ForeignColumn("caixaIndividual_id")
                .ToTable("caixa_individual").PrimaryColumn("id");
        }

        private void MigrarFluxoDeCaixa()
        {
            Execute.WithConnection((con, tran) =>
            {
                _somaSaldoCaixa = 0;

                var inserts = new List<string>();

                IncluirInsertsDeAbertura(inserts, con, tran);
                IncluirInsertsDeFechamento(inserts, con, tran);

                foreach (var insert in inserts)
                {
                    var cmd = con.CreateCommand();

                    cmd.Transaction = tran;
                    cmd.CommandText = insert;

                    cmd.ExecuteNonQuery();
                }
            });
        }

        private void IncluirInsertsDeAbertura(ICollection<string> inserts, IDbConnection con, IDbTransaction tran)
        {
            var queryCaixas = con.CreateCommand();

            queryCaixas.Transaction = tran;
            queryCaixas.CommandText = "select ci.id, ci.dataAbertura, ci.usuario_id, ci.saldoInicial from caixa_individual ci order by ci.dataAbertura";

            var reader = queryCaixas.ExecuteReader();

            const string insertTxt =
                "insert into conta_caixa_fluxo(id, dataCriacao, dataOperacao, usuario_id, tipoOperacao, totalOperacao, saldoAtual, historico, caixaIndividual_id) " +
                "values('{0}', '{1}', '{2}', {3}, {4}, {5}, {6}, '{7}', '{8}')";

            while (reader.Read())
            {
                var totalOperacao = decimal.Negate(reader.GetDecimal(3));

                _somaSaldoCaixa += totalOperacao;

                var insertCmd = string.Format(
                    insertTxt,
                    Guid.NewGuid(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000"),
                    reader.GetDateTime(1).ToString("yyyy-MM-dd HH:mm:ss.000"),
                    reader.GetInt32(2),
                    TipoSaida,
                    totalOperacao.ToString(new NumberFormatInfo {NumberDecimalSeparator = "."}),
                    _somaSaldoCaixa.ToString(new NumberFormatInfo {NumberDecimalSeparator = "."}),
                    "abertura de caixa saldo inicial",
                    reader.GetGuid(0)
                );

                inserts.Add(insertCmd);
            }

            reader.Close();
        }

        private void IncluirInsertsDeFechamento(ICollection<string> inserts, IDbConnection con, IDbTransaction tran)
        {
            var queryCaixas = con.CreateCommand();

            queryCaixas.Transaction = tran;
            queryCaixas.CommandText = "select ci.id, ci.dataFechamento, ci.usuario_id, ci.saldoInformado from caixa_individual ci where ci.estado = 1 order by ci.dataFechamento";

            var reader = queryCaixas.ExecuteReader();

            const string insertTxt =
                "insert into conta_caixa_fluxo(id, dataCriacao, dataOperacao, usuario_id, tipoOperacao, totalOperacao, saldoAtual, historico, caixaIndividual_id) " +
                "values('{0}', '{1}', '{2}', {3}, {4}, {5}, {6}, '{7}', '{8}')";

            while (reader.Read())
            {
                var totalOperacao = reader.GetDecimal(3);

                _somaSaldoCaixa += totalOperacao;

                var insertCmd = string.Format(
                    insertTxt,
                    Guid.NewGuid(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000"),
                    reader.GetDateTime(1).ToString("yyyy-MM-dd HH:mm:ss.000"),
                    reader.GetInt32(2),
                    TipoSaida,
                    totalOperacao.ToString(new NumberFormatInfo {NumberDecimalSeparator = "."}),
                    _somaSaldoCaixa.ToString(new NumberFormatInfo {NumberDecimalSeparator = "."}),
                    "fechamento de caixa individual",
                    reader.GetGuid(0)
                );

                inserts.Add(insertCmd);
            }

            reader.Close();
        }

        public override void Down()
        {
        }
    }
}