namespace Fusion.FastReport.Facades
{
    public interface IObterXml
    {
        string ObterXmlAutorizado(int cupomId);
        string UltimoXmlAssinado(int cupomId);
    }
}