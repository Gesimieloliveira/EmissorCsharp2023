using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Tef;
using FusionCore.FusionNfce.Pagamento;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Tef;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using FusionNfce.AutorizacaoSatFiscal;
using FusionNfce.AutorizacaoSatFiscal.Criadores;
using FusionWPF.Base.Utils.Dialogs;
using OpenAC.Net.Core;
using OpenAC.Net.DFe.Core.Common;

namespace FusionNfce.Visao.Principal.FinalizarVenda.Tef.Pos
{
    public class EfetuaPagamentoPosFormModel : ViewModel
    {
        private Credenciadora _credenciadora;
        private TipoCartaoPos _tipoPagamentoCartaoPos;

        public event EventHandler Fechar;

        public EfetuaPagamentoPosFormModel()
        {
            Credenciadora = Credenciadora.Outros;
            TipoPagamentoCartaoPos = TipoCartaoPos.Credito;
        }

        public ICommand CommandEfetuarPagamentoPOS => GetSimpleCommand(EfetuarPagamentoPOSAction);

        public ICommand CommandFechar => GetSimpleCommand(FecharAction);

        private void FecharAction(object obj)
        {
            if (!DialogBox.MostraConfirmacao("Deseja cancelar operação?\nSe cancelar vamos voltar para a tela de venda",
                MessageBoxImage.Question)) return;

            OnFechar();
            IsCancelarOperacao = true;
        }

        public bool IsCancelarOperacao { get; set; }

        private void EfetuarPagamentoPOSAction(object obj)
        {
            OnFechar();
        }

        public TipoCartaoPos TipoPagamentoCartaoPos
        {
            get => _tipoPagamentoCartaoPos;
            set
            {
                _tipoPagamentoCartaoPos = value;
                PropriedadeAlterada();
            }
        }

        public Credenciadora Credenciadora
        {
            get => _credenciadora;
            set
            {
                _credenciadora = value;
                PropriedadeAlterada();
            }
        }

        protected virtual void OnFechar()
        {
            Fechar?.Invoke(this, EventArgs.Empty);
        }
    }
}