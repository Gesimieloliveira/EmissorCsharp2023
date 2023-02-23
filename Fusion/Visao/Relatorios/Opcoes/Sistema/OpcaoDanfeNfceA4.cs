using System.IO;
using Fusion.FastReport.Relatorios.Sistema;
using Fusion.Visao.Relatorios.Comum;
using FusionWPF.Dialogos;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoDanfeNfceA4 : OpcaoRelatorioSistema<RDanfeNfceA4>
    {
        public override string Descricao { get; } = "Impressão do DANFC-E A4 (nfc-e)";

        protected override RDanfeNfceA4 CriaRelatorio()
        {
            return new RDanfeNfceA4();
        }

        protected override void OnDevEditarDesenho(string arquivoFrx)
        {
            InputBox.ShowInput("Caminho XML", out string xml);
            InputBox.ShowInput("Está cancelado (S/N)?", out string cancelado);

            var xmlstring = File.ReadAllText(xml);

            using (var r = CriaRelatorio())
            {
                r.ComXml(xmlstring, cancelado?.ToUpper() == "S", null, string.Empty);
                r.DevEditarDesenho(arquivoFrx);
            }
        }
    }
}