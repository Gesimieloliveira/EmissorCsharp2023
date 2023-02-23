using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionCore.AutorizacaoOperacao.PayloadTypes
{
    public  class NfceCancelada : IPayload
    {
        public NfceCancelada(int id, decimal valor)
        {
            Id = id;
            Valor = valor;
        }

        public int Id { get; private set; }
        public decimal Valor { get; private set; }

    }
}
