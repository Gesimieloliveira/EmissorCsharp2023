using FusionCore.Repositorio.Dtos.Consultas;

namespace FusionCore.FusionAdm.ConverterVendaParaNfe
{
    public interface IConverteVendaParaNFe
    {
        int Executar();
        void AdicionarPerfilNfe(AbaPerfilNfeDTO perfilNfe);
    }
}