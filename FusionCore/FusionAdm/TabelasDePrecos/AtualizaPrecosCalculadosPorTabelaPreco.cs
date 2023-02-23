namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public class AtualizaPrecosCalculadosPorTabelaPreco
    {
        private readonly IAtualizaPrecoVenda _atualizaPrecoVenda;
        private readonly IAtualizaValorUnitario _atualizaValorUnitario;

        public AtualizaPrecosCalculadosPorTabelaPreco(
            IAtualizaPrecoVenda atualizaPrecoVenda,
            IAtualizaValorUnitario atualizaValorUnitario)
        {
            _atualizaPrecoVenda = atualizaPrecoVenda;
            _atualizaValorUnitario = atualizaValorUnitario;
        }

        public void AtualizarPrecos(decimal novoPreco)
        {
            _atualizaPrecoVenda.AtualizarPrecoVenda(novoPreco);
            _atualizaValorUnitario.AtualizarValorUnitario(novoPreco);
        }
    }
}