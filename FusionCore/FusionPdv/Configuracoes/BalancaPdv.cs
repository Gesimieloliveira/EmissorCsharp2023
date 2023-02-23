using FusionCore.FusionAdm.Configuracoes;

namespace FusionCore.FusionPdv.Configuracoes
{
    public class BalancaPdv
    {
        public byte Id { get; set; } = 1;
        public byte TamanhoCodigo { get; set; } = 4;
        public byte DigitoVerificador { get; set; } = 2;
        public ModoDeOperacao ModoDeOperacao { get; set; } = ModoDeOperacao.Preco;
        public bool Ativo { get; set; }
        public byte CasasDecimais { get; set; } = 2;
    }
}