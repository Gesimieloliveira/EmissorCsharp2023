using FusionPdv.Modelos.FormaPagamento;
using FusionPdv.Servicos.Ecf;

namespace FusionPdv.Servicos.ValidacaoInicial
{
    public class VerificacaoInicial
    {
        public void Executar()
        {
            
            new EcfVerificaSerie().Existe();
            new EcfVerificaGt().Evalido();
            new EcfVerificaDataEhora().Executar();
            CarregarFormasDePagamento.Validar();
            new ExisteAliquota().Executar();
        }
    }
}
