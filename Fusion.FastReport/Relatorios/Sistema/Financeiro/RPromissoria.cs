using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Sistema.Financeiro
{
    public class RPromissoria : RelatorioBase
    {
        private int _maloteId;

        public RPromissoria(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RPromissoriaCarne>("FrPromissoria.frx");
        }

        public void ComMaloteId(int id)
        {
            _maloteId = id;
        }

        protected override void PrepararDados()
        {
            RegistraParametro("IdMalote", _maloteId);
        }
    }
}