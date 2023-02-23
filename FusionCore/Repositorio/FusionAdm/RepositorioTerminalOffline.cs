using System.Collections.Generic;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.TerminalOffline;
using NHibernate;

// ReSharper disable RedundantBoolCompare

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioTerminalOffline : Repositorio<TerminalOffline, byte>
    {
        public RepositorioTerminalOffline(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(TerminalOffline terminalOffline)
        {
            Sessao.SaveOrUpdate(terminalOffline);
            Sessao.Flush();
        }

        public IEnumerable<byte> TodosTerminaisSomenteComId()
        {
            var query = Sessao.QueryOver<TerminalOffline>()
                .Select(e => e.Id)
                .Where(e => e.Ativo == true);

            return query.List<byte>();
        }

        public IEnumerable<TerminalOffline> TodosTerminaisSincronizaveis()
        {
            var queryOver = Sessao.QueryOver<TerminalOffline>()
                .Where(t => t.Ativo == true && t.BindTerminal == string.Empty);

            return queryOver.List<TerminalOffline>();
        }

        public TerminalOffline ConfiguracaoNfce(string bindTerminal)
        {
            var queryOver = Sessao.QueryOver<TerminalOffline>().Where(t => t.BindTerminal == bindTerminal);
            var terminalOffline = queryOver.SingleOrDefault<TerminalOffline>();

            return terminalOffline;
        }

        public void Deletar(TerminalOffline terminalOffline)
        {
            Sessao.Delete(terminalOffline);
        }

        public string GetObservacaoPadrao(byte terminalId)
        {
            var query = Sessao.QueryOver<TerminalOffline>()
                .Select(t => t.Observacao)
                .Where(t => t.Id == terminalId)
                .Take(1);

            return query.SingleOrDefault<string>();
        }

        public bool EmissorJaVinculado(EmissorFiscal emissorFiscal)
        {
            var query = Sessao.QueryOver<EmissorFiscal>()
                .Where(x => x.Id == emissorFiscal.Id && x.TerminalOffline != null);

            return query.RowCount() > 0;
        }

        public bool EmissorParaFaturamento(EmissorFiscal emissorFiscal)
        {
            var query = Sessao.QueryOver<EmissorFiscal>()
                .Where(x => x.Id == emissorFiscal.Id && x.IsFaturamento == true);

            return query.RowCount() > 0;
        }
    }
}