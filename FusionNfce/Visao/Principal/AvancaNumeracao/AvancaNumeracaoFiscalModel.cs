using System.Windows;
using System.Windows.Input;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.ConfigNumeroFiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Principal.AvancaNumeracao
{
    public class AvancaNumeracaoFiscalModel : ViewModel
    {
        private readonly Nfce _nfce;
        private int _numeracaoFiscal;

        public AvancaNumeracaoFiscalModel(Nfce nfce)
        {
            _nfce = nfce;
            NumeracaoFiscal = _nfce.NumeroDocumento;
        }

        public int NumeracaoFiscal
        {
            get => _numeracaoFiscal;
            set
            {
                _numeracaoFiscal = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandAlocarProximoNumero => GetSimpleCommand(AlocarProximoNumeroAction);

        private void AlocarProximoNumeroAction(object obj)
        {
            if (!DialogBox.MostraConfirmacao("Deseja realmente alocar uma nova númeração? *-*",
                MessageBoxImage.Question)) return;

            var emissorFiscal = SessaoSistemaNfce.Configuracao.EmissorFiscal;

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioEmissor = new RepositorioEmissorFiscalNfce(sessao);
                repositorioEmissor.Refresh(emissorFiscal);

                new AlocarNumeroFiscalNfce().Alocar(_nfce, emissorFiscal.EmissorFiscalNfce, SessaoSistemaNfce.TipoEmissao);

                transacao.Commit();
            }

            DialogBox.MostraInformacao($"Novo Número Fiscal NFC-e {_nfce.NumeroDocumento:D9}\nBoa sorte para não dar rejeição novamente");

            OnFechar();
        }
    }
}