using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RAnaliseLucroPorItem : RelatorioBase
    {
        public static readonly string Descricao = "Relatório análise de lucro por itens";

        public RAnaliseLucroPorItem(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrAnaliseLucroPorItem.frx");
        }

        protected override void PrepararDados()
        {
            RegistrarDescricao(Descricao);
        }
    }
}