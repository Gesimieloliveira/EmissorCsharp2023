using System;
using FusionCore.Repositorio.Base;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.ManifestoSefaz
{
    public class ManifestoDfe : Entidade
    {
        public int Id { get; private set; }
        public string Chave { get; set; }
        public TipoManifesto Tipo { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlResposta { get; set; }
        public DateTime CriadoEm { get; private set; }
        protected override int ReferenciaUnica => Id;

        public ManifestoDfe()
        {
            CriadoEm = DateTime.Now;
        }
    }
}