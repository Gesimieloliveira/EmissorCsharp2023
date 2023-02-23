using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionCore.AutorizacaoOperacao.PayloadTypes
{
    public class NfeCancelada : IPayload
    {
        public NfeCancelada(int id, decimal valor)
        {
            Id = id;
            Valor = valor;
        }

        public int Id { get; private set; }
        public decimal Valor { get; private set; }

    }
}
