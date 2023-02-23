using Fusion.FastReport.Relatorios;

namespace Fusion.Visao.Relatorios.Comum
{
    public sealed class OpcaoRelatorio : OpcaoRelatorioFixo<RelatorioFixo>
    {
        private readonly string _arquivoFrx;

        public OpcaoRelatorio(string grupo, string descricao, string arquivoFrx)
        {
            Grupo = grupo;
            Descricao = descricao;
            _arquivoFrx = arquivoFrx;
        }

        public override string Descricao { get; }
        public override string Grupo { get; }

        protected override RelatorioFixo CriaRelatorio()
        {
            return new RelatorioFixo(SessaoManager, _arquivoFrx, Descricao);
        }
    }
}