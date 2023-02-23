using System;
using NHibernate;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public interface ISincronizacao
    {
        string Tag { get; }
        bool RegistraEvento { get; set; }
        DateTime IniciadoEm { get; set; }
        ISession SessaoAdm { get; set; }
        ISession SessaoPdv { get; set; }
        void Sincronizar(DateTime ultimaSincronizacao);
    }
}