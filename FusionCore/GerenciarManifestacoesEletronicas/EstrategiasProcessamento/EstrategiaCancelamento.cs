using DFe.Utils;
using NFe.Classes.Servicos.DistribuicaoDFe.Schemas;

namespace FusionCore.GerenciarManifestacoesEletronicas.EstrategiasProcessamento
{
    public class EstrategiaCancelamento : IEstrategia
    {
        public TipoDfe ETipoDfe { get; } = TipoDfe.ProcEventoNfe;
        public TipoEvento? ETipoEvento { get; } = TipoEvento.Cancelamento;

        public bool Criar(ItemDistribuicaoDFe item, RepositorioDistribuicaoDFe repositorio)
        {
            var procEvento = FuncoesXml.XmlStringParaClasse<procEventoNFe>(item.XmlDescompactado);

            var nfeResumida = repositorio.BuscarNfeResumidaPela(procEvento.evento.infEvento.chNFe);
            if (nfeResumida == null)
            {
                return false;
            }

            nfeResumida.StatusManifestacao = StatusManifestacao.Desconhecida;
            nfeResumida.AddCancelamento(item.XmlDescompactado);
            repositorio.Update(nfeResumida);

            return true;
        }
    }
}