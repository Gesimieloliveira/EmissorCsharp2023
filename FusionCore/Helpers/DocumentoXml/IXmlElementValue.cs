namespace FusionCore.Helpers.DocumentoXml
{
    public interface IXmlElementValue
    {
        bool HasValue { get; }
        T GetValueOrDefault<T>();
        decimal GetDecimalValue();
        string GetValueOrEmpty();
    }
}