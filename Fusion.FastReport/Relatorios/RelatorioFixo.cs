using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios
{
    public class RelatorioFixo : RelatorioBase
    {
        private readonly string _arquivoFrx;
        private readonly string _descricao;

        public RelatorioFixo(
            ISessaoManager sessaoManager,
            string arquivoFrx,
            string descricao
        ) : base(sessaoManager)
        {
            _arquivoFrx = arquivoFrx;
            _descricao = descricao;
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx(_arquivoFrx);
        }

        protected override void PrepararDados()
        {
            RegistrarDescricao(_descricao);
        }
    }
}