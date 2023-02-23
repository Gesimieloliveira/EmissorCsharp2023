using System;

namespace FusionCore.Helpers.Log
{
    public interface IRegistrarLog
    {
        string Path { get; set; }

        void Registrar(string evento);

        void RegistrarException(Exception ex);
    }
}