using System;

namespace FusionCore.ManifestoSefaz
{
    public class JaManifestadoException : InvalidOperationException
    {
        public JaManifestadoException(string message) : base(message)
        {
        }
    }
}