using FusionCore.FusionAdm.Fiscal.NF;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioCobranca : IRepositorio<Cobranca, int>
    {
        void Salvar(Cobranca cobranca);

        void SalvarDuplicata(CobrancaDuplicata cobrancaDuplicata);

        void DeletarComDuplicatas(Cobranca cobranca);
    }
}