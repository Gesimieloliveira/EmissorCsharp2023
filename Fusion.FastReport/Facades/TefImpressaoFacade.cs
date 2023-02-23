using Fusion.FastReport.Relatorios.Sistema;

namespace Fusion.FastReport.Facades
{
    public class TefImpressaoFacade
    {
        public void Imprimir(string[] imagemTef, string nomeImpressora)
        {
            using (var report = new RImpressaoTef())
            {
                report.ComImpressao(imagemTef);
                report.Imprimir(nomeImpressora);
            }
        }
    }
}