using System;
using FusionCore.FusionNfce.Cliente;

namespace FusionNfce.Visao.Clientes.Model
{
    public class ClienteEvent : EventArgs
    {
        public ClienteEvent(ClienteNfce cliente)
        {
            Cliente = cliente;
        }

        public ClienteNfce Cliente { get; set; }
    }
}