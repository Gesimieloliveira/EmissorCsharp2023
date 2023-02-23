using System;
using System.Windows;
using System.Windows.Input;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.Cliente
{
    public partial class ConsultarCliente
    {
        private readonly ConsultarClienteModel _consultarClienteModel;
        private ClienteDt _cliente;

        public ConsultarCliente()
        {
            InitializeComponent();
            _consultarClienteModel = new ConsultarClienteModel();
            DataContext = _consultarClienteModel;
            TbConsulta.Focus();

        }

        private void ConsultarCliente_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5:
                    BuscarClientePorNome();
                    break;
                case Key.F2:
                    Close();
                    break;
                case Key.Escape:
                    _consultarClienteModel.ClienteSelecionado = null;
                    Close();
                    break;
            }
        }

        private void BuscarClientePorNome()
        {
            try
            {
                _consultarClienteModel.ConsultarClientePorNome();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void TBConsulta_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (!e.IsDown || e.Key != Key.Down) return;
            BuscarClientePorNome();
            LbListaDeCliente.Focus();
            _consultarClienteModel.ClienteSelecionado = _consultarClienteModel.PrimeiroItemDaLista;
        }

        private void TBConsulta_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuscarClientePorNome();
            }
        }

        private void BtBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            BuscarClientePorNome();
        }

        private void LbListaDeCliente_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _cliente = _consultarClienteModel.ClienteSelecionado;
                Close();
            }
        }

        private void BtConfirmar_Click(object sender, RoutedEventArgs e)
        {
            _cliente = _consultarClienteModel.ClienteSelecionado;
            Close();
        }

        private void BtCancelar_OnClick(object sender, RoutedEventArgs e)
        {
            _consultarClienteModel.ClienteSelecionado = null;
            Close();
        }

        public ClienteDt Retorno()
        {
            return _cliente;
        }
    }
}
