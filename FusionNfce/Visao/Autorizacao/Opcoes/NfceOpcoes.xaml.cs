using System;
using System.Windows.Input;

namespace FusionNfce.Visao.Autorizacao.Opcoes
{
    public partial class NfceOpcoes
    {
        private readonly NfceOpcoesModel _model;

        public NfceOpcoes(NfceOpcoesModel nfceOpcoesModel)
        {
            _model = nfceOpcoesModel;
            _model.FecharTela += FecharTela;
            DataContext = nfceOpcoesModel;
            InitializeComponent();
        }

        private void FecharTela(object sender, EventArgs e)
        {
            Close();
        }

        private void NfceOpcoes_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    _model.EnviaEmailAction(sender);
                    break;
                case Key.F2:
                    break;
                case Key.F3:
                    _model.ImprimirAction(sender);
                    break;
                case Key.F4:
                    _model.CancelarAction(sender);
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }
    }
}
