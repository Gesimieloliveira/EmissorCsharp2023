using System;
using System.Windows.Input;
using System.Windows.Threading;
using FusionCore.ControleCaixa.Facades;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Papeis.Enums;
using FusionLibrary.VisaoModel;
using FusionNfce.Visao.PedidoVendaLista;
using FusionNfce.Visao.Vendas;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Principal
{
    public class RetornoConversaoPedidoVenda
    {
        private readonly Nfce _nfce;

        public RetornoConversaoPedidoVenda(Nfce nfce)
        {
            _nfce = nfce;
        }

        public Nfce GetNfce()
        {
            return _nfce;
        }
    }

    public class MaisOpcoesFormModel : ViewModel
    {
        private readonly Dispatcher _dispatcher;
        private bool _isPossuiGestor;

        public MaisOpcoesFormModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            IsPossuiGestor = SessaoSistemaNfce.AcessoConcedido.PossuiFusionGestor;
        }

        public ICommand CommandVendas => GetSimpleCommand(VendasAction);
        public ICommand CommandPedidoVenda => GetSimpleCommand(PedidoVendaAction);

        public bool IsPossuiGestor
        {
            get => _isPossuiGestor;
            set
            {
                _isPossuiGestor = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<RetornoConversaoPedidoVenda> RetornoConversaoPedidoVenda;

        private void PedidoVendaAction(object obj)
        {
            try
            {
                ControleCaixaNfceFacade.ThrowExcetpionSeNaoExistirCaixaAberto(SessaoSistemaNfce.Usuario);
                SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_CONVERTER_PEDIDO_VENDA);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
                return;
            }

            var model = new PedidoVendaListaFormModel();
            var dialog = new PedidoVendaListaForm(model);

            dialog.Closed += FecharTela;

            model.NfceFoiConvertida += delegate (object sender, Nfce nfce)
            {
                OnRetornoConversaoPedidoVenda(new RetornoConversaoPedidoVenda(nfce));
            };

            _dispatcher.BeginInvoke(new Action(() => { dialog.ShowDialog();}));

            OnFechar();
        }

        private void FecharTela(object sender, EventArgs e)
        {
            OnFechar();
        }

        private void VendasAction(object obj)
        {
            SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_VISUALIZAR_VENDAS);

            var model = new NfceOpcoesViewModel();
            var dialog = new NfceOpcoesWindow(model);

            _dispatcher.BeginInvoke(new Action(() => { dialog.ShowDialog(); }));
            OnFechar();
        }

        protected virtual void OnRetornoConversaoPedidoVenda(RetornoConversaoPedidoVenda e)
        {
            RetornoConversaoPedidoVenda?.Invoke(this, e);
            OnFechar();
        }
    }
}