using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fusion.Visao.CteEletronico.Emitir.DocAnt
{
    public partial class DocumentoAnteriorForm
    {
        private readonly DocumentoAnteriorFormModel _model;

        public DocumentoAnteriorForm(DocumentoAnteriorFormModel model)
        {
            _model = model;
            _model.Fechar += Fechar;
            DataContext = _model;
            InitializeComponent();
        }

        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonDeletar_OnClick(object sender, RoutedEventArgs e)
        {
            _model.CommandExcluir.Execute(null);
        }

        private void SelectCurrentItem(object sender, MouseButtonEventArgs e)
        {
            var item = (ListBoxItem)sender;
            item.IsSelected = true;
        }
    }
}
