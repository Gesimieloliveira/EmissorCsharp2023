namespace FusionCore.FusionAdm.Fiscal.NF.Cancelar
{
    public interface IProcessadorCancelamento
    {
        void Processar(EventoCancelamento evento);
    }
}