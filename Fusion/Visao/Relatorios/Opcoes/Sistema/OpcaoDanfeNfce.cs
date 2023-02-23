using System.IO;
using Fusion.FastReport.Relatorios.Sistema;
using Fusion.Visao.Relatorios.Comum;
using FusionWPF.Dialogos;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoDanfeNfce : OpcaoRelatorioSistema<RDanfeNfce>
    {
        public override string Descricao { get; } = "Impressão do DANFC-E (nfc-e)";

        protected override RDanfeNfce CriaRelatorio()
        {
            return new RDanfeNfce();
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