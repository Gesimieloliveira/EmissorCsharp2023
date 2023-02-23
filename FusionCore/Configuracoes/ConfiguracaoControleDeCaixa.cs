using System;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.Configuracoes
{
    public class ConfiguracaoControleDeCaixa : EntidadeBase<Guid>, ISincronizavelAdm
    {
        public ConfiguracaoControleDeCaixa()
        {
            Id = Guid.NewGuid();
        }

        protected override Guid ChaveUnica => Id;

        public Guid Id { get; private set; }
        public bool ControlaCaixaNoGestor { get; set; }
        public bool ControlaCaixaNoNfce { get; set; }

        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.ConfiguracaoCaixa;
    }
}