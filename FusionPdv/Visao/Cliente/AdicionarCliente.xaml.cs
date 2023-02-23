using System;
using System.Windows;
using System.Windows.Input;
using FusionPdv.Modelos;
using FusionPdv.Validacao;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.Cliente
{

    public partial class AdicionarCliente
    {
        private readonly AdicionarClienteModel _adicionarClienteModel;

        private ClienteCupom _clienteCupom;

        public AdicionarCliente()
        {
            InitializeComponent();
            _adicionarClienteModel = new AdicionarClienteModel();
            DataContext = _adicionarClienteModel;
            TbCpfOuCpnj.Focus();
        }

        private void AdicionarCliente_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5:
                    var consultarCliente = new ConsultarCliente();
                    consultarCliente.ShowDialog();
                    var cliente = consultarCliente.Retorno();
                    _adicionarClienteModel.RecebeCliente(cliente);
                    break;
                case Key.F2:
                    try
                    {
                        _clienteCupom = PreencheObjeto();
                    }
                    catch (InvalidOperationException ex)
                    {
                        DialogBox.MostraErro(ex.Message);
                        return;
                    }
                    
                    Close();
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private ClienteCupom PreencheObjeto()
        {
            try
            {
                return new ClienteCupom
                {
                    Cliente = _adicionarClienteModel.ClienteRetorno(),
                    Observacao = _adicionarClienteModel.Observacao
                };

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        private void BtConfirmar_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _clienteCupom = PreencheObjeto();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraErro(ex.Message);
                return;
            }
            Close();
        }

        private void BtCancelar_OnClick(object sender, RoutedEventArgs e)
        {
            _adicionarClienteModel.Cliente = null;
            _adicionarClienteModel.Observacao = null;
            Close();
        }

        private void BtLocalizar_OnClick(object sender, RoutedEventArgs e)
        {
            var consultarCliente = new ConsultarCliente();
            consultarCliente.ShowDialog();
            _adicionarClienteModel.RecebeCliente(consultarCliente.Retorno());
        }

        public ClienteCupom Retorno()
        {
            return _clienteCupom;
        }

        private void TbCpfOuCpnj_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            BuscarPorCpfOuCnpj();
        }

        private void BuscarPorCpfOuCnpj()
        {


            try
            {
                var cliente = _adicionarClienteModel.BuscarClientePorCpfOuCpj();

                if (cliente == null) return;

                _adicionarClienteModel.CpfOuCnpj = _adicionarClienteModel.Mascara(_adicionarClienteModel.CpfOuCnpj);

                _adicionarClienteModel.Cliente = cliente;

                _adicionarClienteModel.CpfOuCnpj =
                    _adicionarClienteModel.Mascara(string.IsNullOrEmpty(cliente.Cpf) ? cliente.Cnpj : cliente.Cpf);
                _adicionarClienteModel.Nome = cliente.Nome;
                _adicionarClienteModel.Endereco = cliente.Endereco;
            }
            catch (ExceptionCpfOuCnpj ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message);
            }
        }

        private void TbCpfOuCpnj_OnLostFocus(object sender, RoutedEventArgs e)
        {
            BuscarPorCpfOuCnpj();
        }
    }
}
