using System;

namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public static class FabricaCalculoPeloTipoAjuste
    {
        public static ICalculoAjustePreco ObterCalculadoraDeAjuste(TipoAjustePreco ajustePreco)
        {
            switch (ajustePreco)
            {
                case TipoAjustePreco.AcrecimoPrecoVenda:
                    return new AcrecimoPrecoVenda();
                case TipoAjustePreco.DescontoPrecoVenda:
                    return new DescontoPrecoVenda();
                default:
                    throw new ArgumentOutOfRangeException(nameof(ajustePreco), ajustePreco, null);
            }
        }
    }
}