using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoItensVendidos : OpcaoRelatorioFixo<RItensVendidos>
    {
        public override string Descricao { get; } = "Relatório de itens vendidos (nfe, nfc-e e faturamento)";
        public override string Grupo { get; } = "Analises";

        protected override RItensVendidos CriaRelatorio()
        {
            return new RItensVendidos(SessaoManager);
        }

        protected override void OnDevEditarDesenho(string arquivoFrx)
        {
            using (var r = CriaRelatorio())
            {
                r.DevEditarDesenho(arquivoFrx);
            }
        }
    }
}