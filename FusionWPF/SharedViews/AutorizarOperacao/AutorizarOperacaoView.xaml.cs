using FusionCore.AutorizacaoOperacao;
using FusionCore.AutorizacaoOperacao.Autorizacao;
using FusionCore.CadastroUsuario;
using FusionCore.Papeis.Enums;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using System;
using System.Windows;

namespace FusionWPF.SharedViews.AutorizarOperacao
{
    /// <summary>
    /// Interaction logic for Confirmacao.xaml
    /// </summary>
    public partial class AutorizarOperacaoView
    {

        private readonly Action _action;
        private readonly ISessaoManager _sessaoManager;
        private readonly IUsuario _usuario;
        private readonly IAutorizarUsuario _autorizarUsuario;

        public AutorizarOperacaoView(ISessaoManager sessaoManager, IAutorizarUsuario autorizarUsuario, IUsuario usuario, string agregado, Permissao permissao, IPayload payload, Action action)
        {
            InitializeComponent();
            _sessaoManager = sessaoManager;
            _usuario = usuario;
            _autorizarUsuario = autorizarUsuario;

            DataContext = new AutorizarOperacaoViewModel(
                _sessaoManager,
                _autorizarUsuario,
                _usuario,
                permissao,
                payload,
                agregado);

            _action = action;

        }

        private AutorizarOperacaoViewModel Contexto => DataContext as AutorizarOperacaoViewModel;

        public void ExecutarAcao()
        {
            if (Contexto.VerificarPermissaoUsuarioLogado())
            {
                _action?.Invoke();
                return;
            }

            var dialogResult = ShowDialog();

            if (dialogResult == true)
            {
                _action?.Invoke();
                return;
            }
        }

        private void AutorizarClickHandler(object sender, RoutedEventArgs e)
        {
            var errorMessage = Contexto.Autorizar();

            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                DialogResult = true;
                return;
            }

            DialogBox.MostraAviso(errorMessage);
        }

    }
}
