using System.Collections.Generic;
using FusionCore.FusionNfce.Cidade;
using FusionCore.FusionNfce.Uf;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioCidadeNfce : IRepositorio<CidadeNfce, int>
    {
        void Salvar(CidadeNfce cidade);
        IEnumerable<CidadeNfce> BuscarCidadePorUf(UfNfce uf);
    }
}