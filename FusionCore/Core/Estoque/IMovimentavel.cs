using FusionCore.FusionAdm.Servico.Estoque;

namespace FusionCore.Core.Estoque
{
    public interface IMovimentavel
    {
        bool MovimentaEstoque { get; }
        EstoqueModel CriaMovimentoInclusao();
        EstoqueModel CriaMovimentoRemocao();
    }
}