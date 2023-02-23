using FusionCore.FusionAdm.Configuracoes;

namespace FusionCore.FusionNfce.ConfiguracaoBalanca
{
    public class BalancaNfce
    {
        public byte Id { get; set; } = 1;
        public byte TamanhoCodigo { get; set; } = 4;
        public byte DigitoVerificador { get; set; } = 2;
        public ModoDeOperacao ModoDeOperacao { get; set; } = ModoDeOperacao.Preco;
        public bool Ativo { get; set; }
        public byte CasasDecimais { get; set; } = 2;
        public byte InicioQuantificador { get; set; }

        private BalancaNfce()
        {
            //use factory method
        }

        public static BalancaNfce Cria(Balanca balanca)
        {
            return new BalancaNfce
            {
                Id = balanca.Id,
                TamanhoCodigo = balanca.TamanhoCodigo,
                Ativo = balanca.Ativo,
                InicioQuantificador = balanca.InicioQuantificador,
                CasasDecimais = balanca.CasasDecimais,
                ModoDeOperacao = balanca.ModoDeOperacao,
                DigitoVerificador = balanca.DigitoVerificador
            };
        }
    }
}