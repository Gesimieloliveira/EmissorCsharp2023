using System;
using System.Windows;

namespace Fusion.Visao.Usuario
{
    public partial class VincularUsuarioAoPapelForm 
    {
        private VincularUsuarioAoPapelFormModel _model;

        public VincularUsuarioAoPapelForm(VincularUsuarioAoPapelFormModel model)
        {
            InitializeComponent();
            DataContext = model;

            _model = model;
        }

        private void VincularUsuarioClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.AdicionarUsuarioAoPapel();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
