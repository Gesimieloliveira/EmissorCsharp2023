using System;
using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;
using Fusion.Visao.Pessoa;
using FusionCore.FusionAdm.Pessoas;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.Aba
{
    public partial class AbaInformacoesCte
    {
        private AbaInformacoesCteModel _model;

        public AbaInformacoesCte()
        {
            InitializeComponent();
        }

        private void AbaRemetenteDestinatario_OnLoaded(object sender, RoutedEventArgs e)
        {
            _model = DataContext as AbaInformacoesCteModel;
        }

        private void OnClickPickerNovoRemetente(object sender, RoutedEventArgs e)
        {
            var model = new PessoaFormModel();
            model.RegistroSalvo += CriouRemetente;
            new PessoaForm(model).ShowDialog();
        }

        private void CriouRemetente(object sender, PessoaEntidade e)
        {
            _model.CarregaRemetenteCom(e);
        }

        private void OnClickPassoAnterior(object sender, RoutedEventArgs e)
        {
            _model.Anterior();
        }

        private void OnClickProximoPasso(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.Proximo();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}