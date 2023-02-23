using System.ComponentModel;

namespace FusionCore.FusionAdm.PedidoVenda
{
    public enum EstadoAtual
    {
        [Description("Aberto")]
        Aberto = 0,

        [Description("Cancelado")]
        Cancelado = 1,

        [Description("Faturado")]
        Faturado = 2,

        [Description("Finalizado")]
        Finalizado = 3
    }

    public static class EstadoAtualExt
    {
        public static bool PermiteEdicao(this EstadoAtual estado)
        {
            return estado == EstadoAtual.Aberto || estado == EstadoAtual.Finalizado;
        }
    }
}