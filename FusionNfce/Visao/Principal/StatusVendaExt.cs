using FusionNfce.Visao.Principal.Contratos;
using FusionNfce.Visao.Principal.Implementacoes;

namespace FusionNfce.Visao.Principal
{
    public static class StatusVendaExt
    {
        private static readonly ComandoVazio ComandoVazioPrototype = new ComandoVazio();

        public static IComandoVenda GetCommando(this StatusVenda comando)
        {
            switch (comando)
            {
                case StatusVenda.Livre:
                    return new ComandoAbrirVenda();
                case StatusVenda.Venda:
                    return new ComandoVenderItem();
            }

            return ComandoVazioPrototype;
        }
    }
}