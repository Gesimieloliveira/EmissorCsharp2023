using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace FusionCore.Helpers.Ambiente
{
    public static class AmbienteHelper
    {
        private static UserPrincipal _currentUser;

        public static string GetNomeMaquina()
        {
            return Environment.MachineName;
        }

        public static string GetNetworkIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
            {
                return ip.ToString();
            }

            return "0.0.0.0";
        }

        public static string GetNomeUsuarioDominio()
        {
            CarregarUsuarioAtual();

            return _currentUser.Name;
        }

        private static void CarregarUsuarioAtual()
        {
            if (_currentUser == null)
            {
                _currentUser = UserPrincipal.Current;
            }
        }

        public static string GetSID()
        {
            CarregarUsuarioAtual();

            return _currentUser.Sid.Value;
        }
    }
}