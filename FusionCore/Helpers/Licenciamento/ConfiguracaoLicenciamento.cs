using System;
using System.ServiceModel;

namespace FusionCore.Helpers.Licenciamento
{
    public class ConfiguracaoLicenciamento
    {
        public ConfiguracaoLicenciamento(EndpointAddress endpointAddress)
        {
            Binding = new BasicHttpBinding 
            {
                CloseTimeout = TimeSpan.FromSeconds(25),
                OpenTimeout = TimeSpan.FromSeconds(25),
                ReceiveTimeout = TimeSpan.FromSeconds(25),
                SendTimeout = TimeSpan.FromSeconds(25),
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.None
                }
            };

            EndpointAddress = endpointAddress;
        }

        public BasicHttpBinding Binding { get; set; } 
        public EndpointAddress EndpointAddress { get; set; }
    }
}