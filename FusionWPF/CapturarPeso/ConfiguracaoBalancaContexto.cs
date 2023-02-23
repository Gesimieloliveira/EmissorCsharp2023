using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.BalancaIntegrada;
using FusionCore.Preferencias;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using OpenAC.Net.Balanca;
using static FusionCore.Preferencias.Preferencias;

namespace FusionWPF.CapturarPeso
{
    public sealed class ConfiguracaoBalancaContexto : ViewModel
    {
        private readonly PreferenciaSistemaService _preferencias;

        public ConfiguracaoBalancaContexto(PreferenciaSistemaService preferencias)
        {
            _preferencias = preferencias;
            ProtocolosDisponiveis = new List<string>();
            PortasDisponiveis = new List<string>();
            VelocidadesDisponiveis = new List<int>();
        }

        public bool UsarBalancaIntegrada
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public IList<string> ProtocolosDisponiveis
        {
            get => GetValue<IList<string>>();
            set => SetValue(value);
        }

        public IList<string> PortasDisponiveis
        {
            get => GetValue<IList<string>>();
            set => SetValue(value);
        }

        public IList<int> VelocidadesDisponiveis
        {
            get => GetValue<IList<int>>();
            set => SetValue(value);
        }

        public string Protocolo
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Porta
        {
            get => GetValue();         
            set => SetValue(value);
        }

        public int VelocidadePorta
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int DelayMonitoramento
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public void CarregarDados()
        {
            UsarBalancaIntegrada = _preferencias.Obter(Balanca.Checkout.UsarBalanca, false);
            Porta = _preferencias.Obter(Balanca.Checkout.Porta, PortasDisponiveis.FirstOrDefault());
            Protocolo = _preferencias.Obter(Balanca.Checkout.Protocolo, ProtocoloBalanca.Toledo.ToString());
            VelocidadePorta = _preferencias.Obter(Balanca.Checkout.Baud, 9600);
            DelayMonitoramento = _preferencias.Obter(Balanca.Checkout.DelayMonitor, 300);
        }

        public void SalvarConfiguracoes()
        {
            ThrowExceptionSeDadosInvalidos();

            _preferencias.Salvar(Balanca.Checkout.UsarBalanca, UsarBalancaIntegrada.ToString());
            _preferencias.Salvar(Balanca.Checkout.Porta, Porta ?? string.Empty);
            _preferencias.Salvar(Balanca.Checkout.Protocolo, Protocolo);
            _preferencias.Salvar(Balanca.Checkout.Baud, VelocidadePorta.ToString());
            _preferencias.Salvar(Balanca.Checkout.DelayMonitor, DelayMonitoramento.ToString());
        }

        private void ThrowExceptionSeDadosInvalidos()
        {
            if (!UsarBalancaIntegrada)
            {
                return;
            }

            if (!PortasDisponiveis.Any())
            {
                throw new InvalidOperationException("Nenhuma porta COM disponível");
            }

            if (string.IsNullOrWhiteSpace(Porta))
            {
                throw new InvalidOperationException("Porta COM é obrigatória");
            }
        }

        public void TentarConectar()
        {
            var config = new BalancaCheckoutFactory.Config
            {
                Protocolo = ProtocoloBalanca.Toledo,
                Porta = Porta,
                Baud = VelocidadePorta
            };

            using (var openBal = BalancaCheckoutFactory.ConfigurarSerial(config, monitorar: false))
            {
                openBal.Conectar();

                if (!openBal.Conectado)
                    throw new InvalidOperationException("Não foi possível conectar na balança!");
            }
        }
    }
}