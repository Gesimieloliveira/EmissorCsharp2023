using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Sistema.Financeiro
{
    public class RPromissoriaCarne : RelatorioBase
    {
        private int _maloteId;

        public RPromissoriaCarne(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RPromissoriaCarne>("FrPromissoriaCarne.frx");
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