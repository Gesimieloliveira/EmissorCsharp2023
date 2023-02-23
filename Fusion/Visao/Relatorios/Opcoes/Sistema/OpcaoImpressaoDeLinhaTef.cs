using System.Collections.Generic;
using Fusion.FastReport.Relatorios.Sistema;
using Fusion.Visao.Relatorios.Comum;
using FusionWPF.Dialogos;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoImpressaoDeLinhaTef : OpcaoRelatorioSistema<RImpressaoTef>
    {
        public override string Descricao { get; } = "Impressão de linha do TEF";

        protected override RImpressaoTef CriaRelatorio()
        {
            return new RImpressaoTef();
        }

        protected override void OnDevEditarDesenho(string arquivoFrx)
        {
            InputBox.ShowInput("Valor da linha 1", out string linha1);
            InputBox.ShowInput("Valor da linha 2", out string linha2);

            using (var r = CriaRelatorio())
            {
                r.ComImpressao(new List<string> {linha1, linha2});
                r.DevEditarDesenho(arquivoFrx);
            }
        }
    }
}