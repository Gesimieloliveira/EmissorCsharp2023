using System;
using Fusion.Facades;
using FusionCore.Sessao;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Vendas.Repositorio;
using FusionLibrary.VisaoModel;
using NHibernate;

namespace Fusion.Visao.Vendas.Gerenciamento.Opcoes
{
    public class FaturamentoOpcaoContexto : ViewModel
    {
        private readonly FaturamentoSlim _faturamentoSlim;
        private readonly PreferenciasFaturamentoFacade _preferenciaFacade;
        private readonly ISessaoManager _sessaoManager;
        private readonly ImpressorFaturamento _impressor;
        private string _textoBotaoEmitirComFiscal;

        public PreferenciasFaturamentoFacade PreferenciaFacade => _preferenciaFacade;

        public FaturamentoOpcaoContexto(
            FaturamentoSlim faturamentoSlim, 
            ISessaoManager sessaoManager
        ) {
            _preferenciaFacade = new PreferenciasFaturamentoFacade(sessaoManager);
            _impressor = new ImpressorFaturamento(sessaoManager);
            _sessaoManager = sessaoManager;
            _faturamentoSlim = faturamentoSlim;

            TextoBotaoEmitirComFiscal = "Emitir Cupom Fiscal";

            if (faturamentoSlim.IsAutorizado || faturamentoSlim.IsAutorizadoSemInternet)
                TextoBotaoEmitirComFiscal = "Imprimir NFC-e";
        }

        public string TextoBotaoEmitirComFiscal
        {
            get => _textoBotaoEmitirComFiscal;
            set
            {
                _textoBotaoEmitirComFiscal = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<FaturamentoSlim> CompletouAcao;

        public void OnCompletouAcao()
        {
            CompletouAcao?.Invoke(this, _faturamentoSlim);
        }

        public void Visualizar()
        {
            var preferencia = _preferenciaFacade.GetPreferenciaDaMaquina();
            _impressor.Viazualiza(_faturamentoSlim, preferencia);
        }

        public FaturamentoVenda CarregarFaturamento()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var venda = new RepositorioFaturamento(sessao).GetPeloId(_faturamentoSlim.Id);

                foreach (var faturamentoProduto in venda.Produtos)
                    NHibernateUtil.Initialize(faturamentoProduto.Produto.ProdutosAlias);


                if (venda.EstadoAtual == Estado.Cancelado)
                    throw new InvalidOperationException("Faturamento cancelado.");

                if (venda.EstadoAtual != Estado.Finalizado)
                    throw new InvalidOperationException(
                        "Para Emitir um Cupom Fiscal você deve finalizar o faturamento");

                return venda;
            }
        }
    }
}