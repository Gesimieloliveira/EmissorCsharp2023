namespace FusionCore.FusionAdm.Componentes
{
    public interface IComponenteValorUnico<out TValor>
    {
        TValor Valor { get; }
        string ToString();
    }
}