using System;

namespace FusionWPF.Base.Contratos
{
    public interface IChildContext
    {
        string TituloChild { get; }
        event EventHandler SolicitaFechamento;
    }
}