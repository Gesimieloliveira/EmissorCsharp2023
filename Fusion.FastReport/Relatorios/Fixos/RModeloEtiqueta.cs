using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RModeloEtiqueta : RelatorioBase
    {
        private int _produtoId;

        public RModeloEtiqueta(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RModeloEtiqueta>("FrEtiquetaModeloPadrao.frx");
        }

        public void ComParametroId(int produtoId)
        {
            _produtoId = produtoId;
        }

        protected override void PrepararDados()
        {
            RegistraParametro("idProduto", _produtoId);
        }
    }
}