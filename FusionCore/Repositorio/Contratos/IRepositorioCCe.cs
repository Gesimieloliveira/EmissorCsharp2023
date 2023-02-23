using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.CCe;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioCCe : IRepositorio<CartaCorrecaoNfe, int>
    {
        void Persistir(CartaCorrecaoNfe cce);
        void Alterar(CartaCorrecaoNfe cce);
        IList<CartaCorrecaoNfe> BuscaPelaNfe(Nfeletronica nfe);
    }
}