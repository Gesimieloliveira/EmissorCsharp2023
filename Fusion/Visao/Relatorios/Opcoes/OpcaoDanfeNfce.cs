using Fusion.FastReport.Relatorios.Sistema;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoDanfeNfce : OpcaoRelatorioBase<RDanfeNfce>
    {
        public override string Descricao { get; } = "Impressão do DANFC-E (nfc-e)";

        public override string Grupo { get; } = "Sistema";

        protected override RDanfeNfce CriaRelatorio()
        {
            return new RDanfeNfce();
        }

        public override void Visualizar()
        {
            using (var r = CriaRelatorio())
            {
            }
        }

        public override void EditarDesenho()
        {
        }

        public override void DevEditarDesenho(string arquivoFrx)
        {
        }
    }
}