namespace FusionCore.FusionAdm.Componentes
{
    public static class ComponenteExt
    {
        public static bool IsEmpty<T>(this IComponenteValorUnico<T> componenteValor)
        {
            return string.IsNullOrEmpty(componenteValor.ToString());
        }
    }
}