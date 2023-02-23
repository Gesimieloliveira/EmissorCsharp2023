using System;
using System.Data;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Facades;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;

namespace FusionCore.FusionAdm.Financeiro.Servicos
{
    public class ServicoPagarDocumento
    {
        private readonly ISessaoManager _sessaoManager;

        public ServicoPagarDocumento(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public UsuarioDTO Usuario { get; set; }
        public int DocumentoId { get; set; }
        public decimal ValorPagamento { get; set; }
        public bool MarcarComoQuitado { get; set; }
        public string Historico { get; set; }

        public void FazerPagamento()
        {
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(Usuario);

            using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repositorio = new RepositorioDocumentoPagar(sessao);
                var documento = repositorio.GetPeloId(DocumentoId);

                var diferencaPagamento = ValorPagamento - documento.ValorEmAberto;

                if (MarcarComoQuitado && diferencaPagamento > 0)
                {
                    documento.AdicionarJuros(Math.Abs(diferencaPagamento), "JUROS POR DIFERENÇA EM QUITAÇÃO");
                }

                if (MarcarComoQuitado && diferencaPagamento < 0)
                {
                    documento.AdicionarDesconto(Math.Abs(diferencaPagamento), "DESCONTO POR DIFERENÇA EM QUITAÇÃO");
                }

                documento.AdicionarPagamento(ValorPagamento, Historico);
                repositorio.Salvar(documento);

                var servicoCaixa = new ServicoRegistroDeCaixa(sessao, ELocalEventoCaixa.Gestao);

                servicoCaixa.RegistrarDespesa(
                    Usuario,
                    ValorPagamento,
                    DateTime.Now, 
                    ETipoPagamento.Dinheiro,
                    EOrigemFluxoCaixaIndividual.DocumentoPagar,
                    $"PAGAMENTO NO DOCUMENTO A PAGAR NÚMERO {documento.Id}"
                );

                sessao.Transaction.Commit();
            }
        }
    }
}