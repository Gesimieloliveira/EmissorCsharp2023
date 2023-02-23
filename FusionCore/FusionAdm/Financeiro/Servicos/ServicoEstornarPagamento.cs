using System;
using System.Data;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Facades;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;

namespace FusionCore.FusionAdm.Financeiro.Servicos
{
    public class ServicoEstornarPagamento
    {
        private readonly ISessaoManager _sessaoManager;

        public ServicoEstornarPagamento(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public int LancamentoId { get; set; }
        public UsuarioDTO Usuario { get; set; }
        public string Historico { get; set; }

        public void FazerEstorno()
        {
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(Usuario);

            using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repositorio = new RepositorioDocumentoPagar(sessao);
                var lancamento = repositorio.BuscarLancamento(LancamentoId);

                lancamento.Estornar();
                repositorio.Salvar(lancamento.DocumentoPagar);

                if (lancamento.TipoLancamento == TipoLancamento.Pagamento)
                { 
                    var servicoCaixa = new ServicoRegistroDeCaixa(sessao, ELocalEventoCaixa.Gestao);

                    servicoCaixa.RegistrarEstornoDespesa(
                        Usuario,
                        lancamento.Valor,
                        DateTime.Now, 
                        ETipoPagamento.Dinheiro,
                        EOrigemFluxoCaixaIndividual.DocumentoPagar,
                        Historico
                    );
                }

                sessao.Transaction.Commit();
            }
        }
    }
}