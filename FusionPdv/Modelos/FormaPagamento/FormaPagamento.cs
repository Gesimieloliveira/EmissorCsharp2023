using System;

namespace FusionPdv.Modelos.FormaPagamento
{
    public interface IFormaPagamento
    {
        int Id { get; }
        string Nome { get; }
        string CodigoEcf { get; set; }
        bool Ecf { get; }
        DateTime? AlteradoEm { get; set; }
    }
}
