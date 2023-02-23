using FusionCore.Recibos;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RRecibo : RelatorioBase
    {
        private ReciboDTO _reciboDto;

        public RRecibo(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RRecibo>("FrRecibo.frx");
        }

        public void ComReciboDto(ReciboDTO reciboDto)
        {
            _reciboDto = reciboDto;
        }

        protected override void PrepararDados()
        {
            RegistraDados("recibo", new [] { _reciboDto });
        }
    }
}