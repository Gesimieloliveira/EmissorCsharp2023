using System;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace FusionCore.Configuracoes
{
    public class ConfiguracaoEstoque : Entidade, ISincronizavelAdm
    {
        public ConfiguracaoEstoque()
        {
            AlteradoEm = DateTime.Now;
            Id = 1;
        }

        public byte Id { get; private set; }
        public DateTime AlteradoEm { get; set; }
        public bool BloqueiaEstoqueNegativo { get; set; }

        protected override int ReferenciaUnica => Id;
        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel { get; set; } = EntidadeSincronizavel.ConfiguracaoEstoque;
    }
}