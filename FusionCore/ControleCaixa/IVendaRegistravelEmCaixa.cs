using System.Collections.Generic;

namespace FusionCore.ControleCaixa
{
    public interface IVendaRegistravelEmCaixa
    {
        IEnumerable<OperacaoCaixa> ObterOperacoes();
    }
}