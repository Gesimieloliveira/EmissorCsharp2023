using FusionCore.FusionAdm.Pessoas;

namespace FusionCore.FusionAdm.Servico.Pessoas
{
    public class LimiteCreditoServico
    {
        public void ChecarLimiteCredito(Cliente cliente, decimal valorCredito)
        {
            if (!cliente.AplicaLimiteCredito)
            {
                return;
            }

            var credito = cliente.ComputaCreditoDisponivel();

            if (valorCredito > credito)
            {
                throw new LimiteCreditoIndisponivelException(
                    $"Cliente sem limite para crédito disponível. Limite Disponivel é de: {credito:C}");
            }
        }
    }
}