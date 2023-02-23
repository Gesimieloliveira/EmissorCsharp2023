using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Acbr
{
    public class AcbrMonitorPlus : IDisposable
    {
        private AcbrEndereco _acbrEndereco;
        private Socket _socket;

        public AcbrMonitorPlus()
        {
            CriaSeNaoExistirArquivoDefault();

            Conectar();
        }

        private void CriaSeNaoExistirArquivoDefault()
        {
            _acbrEndereco = AcbrMonitorPersistXml.LoadConfigAcbrEndereco();

            if (_acbrEndereco != null) return;

            _acbrEndereco = new AcbrEndereco();
            AcbrMonitorPersistXml.Persistir(_acbrEndereco);
        }

        private void Conectar()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(_acbrEndereco.Ip, _acbrEndereco.Porta);
        }

        public void EnviarComando(AcbrMonitorPlusComando acbrComando, params string[] parametros)
        {
            var comando = MontarComando(acbrComando, parametros);
            _socket.Send(comando);
        }

        public void Dispose()
        {
            _socket.Disconnect(false);
        }

        private static byte[] MontarComando(AcbrMonitorPlusComando acbrComando, IEnumerable<string> parametros)
        {
            string[] comando = {acbrComando.ToString()};

            comando[0] = comando[0].Replace("_", ".");

            comando[0] = comando[0].Insert(comando[0].Length, "(");

            var enumerable = parametros as string[] ?? parametros.ToArray();
            if (!enumerable.Any())
            {
                comando[0] = comando[0].Remove(comando[0].Length - 1);
            }

            enumerable.ForEach(parametro =>
            {
                parametro = parametro.Insert(parametro.Length, ",");
                comando[0] = comando[0].Insert(comando[0].Length, parametro);
            });

            if (enumerable.Any())
            {
                comando[0] = comando[0].Remove(comando[0].Length - 1);
                comando[0] = comando[0].Insert(comando[0].Length, ")");
            }

            comando[0] = (comando[0] + (char) 13 + (char) 10);
            comando[0] = (comando[0] + (char) 46 + (char) 13 + (char) 10);

            Console.Out.Write(comando[0]);

            return Encoding.UTF8.GetBytes(comando[0]);
        }
    }
}