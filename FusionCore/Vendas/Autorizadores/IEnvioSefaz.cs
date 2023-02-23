namespace FusionCore.Vendas.Autorizadores
{
    public interface IEnvioSefaz
    {
        IEnvioSefaz CriaCupomFiscal();


        IEnvioSefaz AlocarNumeracaoFiscal();


        IEnvioSefaz CriaHistorico();


        IEnvioSefaz Autorizar();
    }
}