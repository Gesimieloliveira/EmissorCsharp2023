using System;
using System.IO.Ports;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;
using OpenAC.Net.Balanca;

namespace FusionWPF.CapturarPeso
{
    public partial class ConfiguracaoBalanca
    {
        private readonly ConfiguracaoBalancaContexto _contexto;

        public ConfiguracaoBalanca(ConfiguracaoBalancaContexto contexto)
        {
            InitializeComponent();
            DataContext = contexto;
            _contexto = contexto;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            CarregarOpcoesProtocolo();
            CarregarOpcoesPorta();
            CarregarOpcoesVelocidades();

            _contexto.CarregarDados();
        }

        private void CarregarOpcoesProtocolo()
        {
            foreach (var name in Enum.GetNames(typeof(ProtocoloBalanca)))
            {
                _contexto.ProtocolosDisponiveis.Add(name);
            }
        }

        private void CarregarOpcoesPorta()
        {
            foreach (var portName in SerialPort.GetPortNames())
            {
                _contexto.PortasDisponiveis.Add(portName);
            }
        }

        private void CarregarOpcoesVelocidades()
        {
            _contexto.VelocidadesDisponiveis.Add(75);
            _contexto.VelocidadesDisponiveis.Add(110);
            _contexto.VelocidadesDisponiveis.Add(134);
            _contexto.VelocidadesDisponiveis.Add(150);
            _contexto.VelocidadesDisponiveis.Add(300);
            _contexto.VelocidadesDisponiveis.Add(600);
            _contexto.VelocidadesDisponiveis.Add(1200);
            _contexto.VelocidadesDisponiveis.Add(1800);
            _contexto.VelocidadesDisponiveis.Add(2400);
            _contexto.VelocidadesDisponiveis.Add(4800);
            _contexto.VelocidadesDisponiveis.Add(7200);
            _contexto.VelocidadesDisponiveis.Add(9600);
            _contexto.VelocidadesDisponiveis.Add(14400);
            _contexto.VelocidadesDisponiveis.Add(19200);
            _contexto.VelocidadesDisponiveis.Add(38400);
            _contexto.VelocidadesDisponiveis.Add(57600);
            _contexto.VelocidadesDisponiveis.Add(115200);
            _contexto.VelocidadesDisponiveis.Add(128000);
        }

        private void SalvarClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _contexto.SalvarConfiguracoes();

                if (_contexto.UsarBalancaIntegrada)
                {
                    _contexto.TentarConectar();
                }

                DialogBox.MostraInformacao("Configurações foram salvas com sucesso!");
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }
    }
}