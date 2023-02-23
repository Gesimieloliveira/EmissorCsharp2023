using System;
using FusionCore.Preferencias;
using FusionCore.Sessao;
using OpenAC.Net.Balanca;
using OpenAC.Net.Devices;
using static FusionCore.Preferencias.Preferencias;

namespace FusionCore.BalancaIntegrada
{
    public static class BalancaCheckoutFactory
    {
        public class Config
        {
            public ProtocoloBalanca Protocolo { get; set; }
            public string Porta { get; set; }
            public int Baud { get; set; }
            public int DelayMonitoramento { get; set; }
        }

        public static OpenBal ConfigurarSerial(Config config, bool monitorar = false)
        {
            var balanca = new OpenBal<SerialConfig>();

            balanca.Protocolo = config.Protocolo;
            balanca.Device.Porta = config.Porta;
            balanca.Device.Baud = config.Baud;
            balanca.Device.TimeOut = 5000;
            balanca.Device.ControlePorta = false;
            balanca.DelayMonitoramento = config.DelayMonitoramento;
            balanca.IsMonitorar = monitorar;

            return balanca;
        }

        public static OpenBal ConfigurarSerial(PreferenciaSistemaService preferencias, bool monitorar = false)
        {
            if (!preferencias.Obter(Balanca.Checkout.UsarBalanca, false))
                throw new InvalidOperationException("Balança checkout ainda não foi configurada");

            var config = new Config
            {
                Protocolo = preferencias.Obter(Balanca.Checkout.Protocolo, ProtocoloBalanca.Toledo),
                Porta = preferencias.Obter(Balanca.Checkout.Porta, ""),
                Baud = preferencias.Obter(Balanca.Checkout.Baud, 9600),
                DelayMonitoramento = preferencias.Obter(Balanca.Checkout.DelayMonitor, 300)
            };

            var balanca = ConfigurarSerial(config, monitorar);

            return balanca;
        }
    }
}