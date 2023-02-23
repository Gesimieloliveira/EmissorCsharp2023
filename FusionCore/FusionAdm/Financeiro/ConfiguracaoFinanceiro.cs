using System;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.Financeiro
{
    public class ConfiguracaoFinanceiro : EntidadeBase<byte>, ISincronizavelAdm
    {
        public ConfiguracaoFinanceiro()
        {
            AlteradoEm = DateTime.Now;
        }

        private byte Id { get; set; } = 1;
        protected override byte ChaveUnica => Id;
        public string Referencia => Id.ToString();
        public bool ImprimirComprovanteCrediario { get; set; }
        public decimal TaxaDeJurosMensal { get; set; }
        public DateTime? AlteradoEm { get; set; }

        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.ConfiguracaoFinanceiro;
    }
}