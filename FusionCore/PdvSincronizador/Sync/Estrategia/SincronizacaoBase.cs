using System;
using NHibernate;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public abstract class SincronizacaoBase : ISincronizacao
    {
        public abstract string Tag { get; }
        public bool RegistraEvento { get; set; }
        public DateTime IniciadoEm { get; set; }
        public ISession SessaoAdm { get; set; }
        public ISession SessaoPdv { get; set; }
        public abstract void Sincronizar(DateTime ultimaSincronizacao);
        public void NaoRegistrarEvento()
        {
            RegistraEvento = false;
        }
    }
}