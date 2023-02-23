using System;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;

namespace FusionCore.FusionAdm.Configuracoes
{
    public class ConfiguracaoFrenteCaixa : ISincronizavelAdm
    {
        public virtual byte Id { get; set; } = 1;
        public virtual byte[] Logo { get; set; }
        public virtual bool IsBloquearVendaParaResolverPendencia { get; set; } = true;
        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.ConfiguracaoFrenteCaixa;
        public DateTime? AlteradoEm { get; set; }
        public decimal? ValorMinimoParaForcarClienteNaVenda { get; set; }
        public bool IsSegundaViaContingencia { get; set; }
    }
}