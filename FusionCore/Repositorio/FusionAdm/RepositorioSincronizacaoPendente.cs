using System.Collections.Generic;
using System.Text;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.Repositorio.Contratos;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioSincronizacaoPendente : Repositorio<SincronizacaoPendente, SincronizacaoPendente>,
        IRepositorioSincronizacaoPendente
    {
        public RepositorioSincronizacaoPendente(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(SincronizacaoPendente sincronizacaoPendente)
        {
            var queryDelete = Sessao.CreateSQLQuery("delete sync_pendente where referencia = :referencia" +
                                                    " and terminalOffline_id = :terminalOffline and entidade = :entidade");

            queryDelete.SetString("referencia", sincronizacaoPendente.Referencia);
            queryDelete.SetByte("terminalOffline", sincronizacaoPendente.TerminalOfflineId);
            queryDelete.SetParameter("entidade", sincronizacaoPendente.EntidadeSincronizavel);

            queryDelete.ExecuteUpdate();


            var query = Sessao.CreateSQLQuery("insert into sync_pendente (referencia, terminalOffline_id, entidade) " +
                                              "values (:referencia, :terminalOffline, :entidade) ");

            query.SetString("referencia", sincronizacaoPendente.Referencia);
            query.SetByte("terminalOffline", sincronizacaoPendente.TerminalOfflineId);
            query.SetParameter("entidade", sincronizacaoPendente.EntidadeSincronizavel);

            query.ExecuteUpdate();
        }

        public void Deletar(SincronizacaoPendente sincronizacaoPendente)
        {
            Sessao.Delete(sincronizacaoPendente);
        }

        public IList<SincronizacaoPendente> BuscaTodosParaSincronizacao(EntidadeSincronizavel sincronizavel,
            byte terminalOfflineId)
        {
            var hql = new StringBuilder("select ");
            hql.Append("sp.Referencia as Referencia, sp.TerminalOfflineId as TerminalOfflineId, sp.EntidadeSincronizavel as EntidadeSincronizavel ");
            hql.Append("from SincronizacaoPendente sp where sp.EntidadeSincronizavel = :entidade ");
            hql.Append("and sp.TerminalOfflineId = :terminalId");

            var query = Sessao.CreateQuery(hql.ToString());

            query.SetParameter("entidade", sincronizavel);
            query.SetParameter("terminalId", terminalOfflineId);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(SincronizacaoPendente)));

            var todos = query.List<SincronizacaoPendente>();

            return todos;
        }

        public void AdicionaTodosUsuariosNaPrimeiraSync(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select u.id, :idTerminal, :entidade from usuario as u ");
            hql.Append(
                "where CAST(u.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.Usuario)
                .SetParameter("entidade2", EntidadeSincronizavel.Usuario)
                .ExecuteUpdate();
        }

        public void AdicionaTodosEstadosUfNaPrimeiraSync(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select u.id, :idTerminal, :entidade from uf as u ");
            hql.Append(
                "where CAST(u.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.EstadoUf)
                .SetParameter("entidade2", EntidadeSincronizavel.EstadoUf)
                .ExecuteUpdate();
        }

        public void AdicionaTodasCidadesNaPrimeiraSync(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select c.id, :idTerminal, :entidade from cidade as c ");
            hql.Append(
                "where CAST(c.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.Cidade)
                .SetParameter("entidade2", EntidadeSincronizavel.Cidade)
                .ExecuteUpdate();
        }

        public void AdicionaTodasEmpresasNaPrimeiraSync(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select e.id, :idTerminal, :entidade from empresa as e ");
            hql.Append(
                "where CAST(e.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.Empresa)
                .SetParameter("entidade2", EntidadeSincronizavel.Empresa)
                .ExecuteUpdate();
        }

        public void AdicionaTodosCfopDaNfceNaPrimeiraSync(byte idTerminal)
        {
            const string subQuerySync =
                "select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal";

            var hql =
                "insert into sync_pendente(referencia, terminalOffline_id, entidade)" +
                $" select c.id, :idTerminal, :entidade from cfop as c where CAST(c.id as varchar(100)) not in ({subQuerySync})";

            var query = Sessao.CreateSQLQuery(hql)
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.Cfop)
                .SetParameter("entidade2", EntidadeSincronizavel.Cfop);

            query.ExecuteUpdate();
        }

        public void AdicionaTodosEmissorFiscalDaNfceNaPrimeiraSync(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select e.id, :idTerminal, :entidade from emissor_fiscal as e ");
            hql.Append("where (e.flagNfce = :flagNfce or e.flagSat = :flagSat) and ");
            hql.Append(
                "CAST(e.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.EmissorFiscal)
                .SetParameter("entidade2", EntidadeSincronizavel.EmissorFiscal)
                .SetByte("flagNfce", 1)
                .SetByte("flagSat", 1)
                .SetByte("idTerminal", idTerminal)
                .ExecuteUpdate();
        }

        public void AdicionaTodasUnidadesMedidasProdutoDaNfceNaPrimeiraSync(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select u.id, :idTerminal, :entidade from produto_unidade as u ");
            hql.Append("where ");
            hql.Append(
                "CAST(u.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.ProdutoUnidade)
                .SetParameter("entidade2", EntidadeSincronizavel.ProdutoUnidade)
                .ExecuteUpdate();
        }

        public void AdicionaTodosProdutoDaNfceNaPrimeiraSync(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select p.id, :idTerminal, :entidade from produto as p ");
            hql.Append("where ");
            hql.Append(
                "CAST(p.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.Produto)
                .SetParameter("entidade2", EntidadeSincronizavel.Produto)
                .ExecuteUpdate();
        }

        public void AdicionaTodasPessoasDaNfceNaPrimeiraSync(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select p.id, :idTerminal, :entidade from pessoa as p ");
            hql.Append("where ");
            hql.Append(
                "CAST(p.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.Pessoa)
                .SetParameter("entidade2", EntidadeSincronizavel.Pessoa)
                .ExecuteUpdate();
        }

        public void AdicionaConfiguracaoDeEmailNaNfceNaPrimeiraSync(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select cp.id, :idTerminal, :entidade from configuracao_email as cp ");
            hql.Append("where ");
            hql.Append(
                "CAST(cp.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.ConfiguracaoEmail)
                .SetParameter("entidade2", EntidadeSincronizavel.ConfiguracaoEmail)
                .ExecuteUpdate();
        }

        public void AdicionaIbptNaNfceNaPrimeiraSync(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select distinct i.codigo, :idTerminal, :entidade from tabela_tributo_ibpt as i ");
            hql.Append("where ");
            hql.Append(
                "CAST(i.codigo as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.Ibpt)
                .SetParameter("entidade2", EntidadeSincronizavel.Ibpt)
                .ExecuteUpdate();
        }

        public void AdicionaTipoDocumento(byte idTipoDocumento)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select p.id, :idTipoDocumento, :entidade from tipo_documento as p ");
            hql.Append("where ");
            hql.Append(
                "CAST(p.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTipoDocumento)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTipoDocumento", idTipoDocumento)
                .SetParameter("entidade", EntidadeSincronizavel.TipoDocumento)
                .SetParameter("entidade2", EntidadeSincronizavel.TipoDocumento)
                .ExecuteUpdate();
        }

        public void AdicionaConfiguracaoFrenteCaixa(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select p.id, :idConfigFrenteCaixa, :entidade from configuracao_frente_caixa as p ");
            hql.Append("where ");
            hql.Append(
                "CAST(p.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idConfigFrenteCaixa)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idConfigFrenteCaixa", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.ConfiguracaoFrenteCaixa)
                .SetParameter("entidade2", EntidadeSincronizavel.ConfiguracaoFrenteCaixa)
                .ExecuteUpdate();
        }

        public void AdicionaConfiguracaoEstoque(byte terminal)
        {
            var hql =
                new StringBuilder();

            hql.Append("INSERT INTO sync_pendente (referencia, terminalOffline_id, entidade) SELECT eq.id, :terminal, :entidade FROM configuracao_estoque as eq ");

            hql.Append("where ");
            hql.Append(
                "CAST(eq.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :terminal)");

            var nonQuery = Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("terminal", terminal)
                .SetInt32("entidade", (int) EntidadeSincronizavel.ConfiguracaoEstoque)
                .SetInt32("entidade2", (int) EntidadeSincronizavel.ConfiguracaoEstoque);

            nonQuery.ExecuteUpdate();
        }

        public void AdicionaConfiguracaoBalanca(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select p.id, :idTerminal, :entidade from configuracao_balanca as p ");
            hql.Append("where ");
            hql.Append(
                "CAST(p.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.Balanca)
                .SetParameter("entidade2", EntidadeSincronizavel.Balanca)
                .ExecuteUpdate();
        }

        public void AdicionaRegraTributacaoSaida(byte idTerminal)
        {
            const string queryDelete = "delete from sync_pendente where terminalOffline_id = :terminal and entidade = :entidade";

            Sessao.CreateSQLQuery(queryDelete)
                .SetByte("terminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.RegraTributacaoSaida)
                .ExecuteUpdate();

            var sb = new StringBuilder();

            sb.AppendLine("insert into sync_pendente(referencia, terminalOffline_id, entidade)");
            sb.AppendLine("select regra.id, :terminal, :entidade from regra_tributacao_saida as regra");

            Sessao.CreateSQLQuery(sb.ToString())
                .SetByte("terminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.RegraTributacaoSaida)
                .ExecuteUpdate();
        }

        public void AdicionaResponsavelTecnico(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select u.guid, :idTerminal, :entidade from responsavel_tecnico as u ");
            hql.Append("where ");
            hql.Append(
                "u.guid not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.ResponsavelTecnico)
                .SetParameter("entidade2", EntidadeSincronizavel.ResponsavelTecnico)
                .ExecuteUpdate();
        }

        public void AdicionaTodasTabelasDePrecoNaPrimeiraSync(byte idTerminal)
        {
            var hql = new StringBuilder();

            hql.Append(
                "insert into sync_pendente (referencia, terminalOffline_id, entidade) select p.id, :idTerminal, :entidade from tabela_preco as p ");
            hql.Append("where ");
            hql.Append(
                "CAST(p.id as varchar(100)) not in (select s.referencia from sync_pendente as s where s.entidade = :entidade2 and s.terminalOffline_id = :idTerminal)");

            Sessao.CreateSQLQuery(hql.ToString())
                .SetByte("idTerminal", idTerminal)
                .SetParameter("entidade", EntidadeSincronizavel.TabelaPreco)
                .SetParameter("entidade2", EntidadeSincronizavel.TabelaPreco)
                .ExecuteUpdate();
        }
    }
}