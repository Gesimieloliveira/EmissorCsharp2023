namespace FusionCore.GerenciarManifestacoesEletronicas.EstrategiasProcessamento
{
    public interface IEstrategia
    {
        TipoDfe ETipoDfe { get; }
        TipoEvento? ETipoEvento { get; }
        bool Criar(ItemDistribuicaoDFe item, RepositorioDistribuicaoDFe repositorio);
    }
}