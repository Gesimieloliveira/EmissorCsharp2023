using DFe.Utils;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using NFe.Classes.Servicos.DistribuicaoDFe.Schemas;

namespace FusionCore.GerenciarManifestacoesEletronicas.EstrategiasProcessamento
{
    public class EstrategiaResumoNfe : IEstrategia
    {
        private readonly EmissorFiscalNFE _emissor;

        public EstrategiaResumoNfe(EmissorFiscalNFE emissorNfe)
        {
            _emissor = emissorNfe;
        }

        public TipoDfe ETipoDfe { get; } = TipoDfe.ResumoNfe;
        public TipoEvento? ETipoEvento { get; set; }

        public bool Criar(ItemDistribuicaoDFe item, RepositorioDistribuicaoDFe repositorio)
        {
            var resNfe = FuncoesXml.XmlStringParaClasse<resNFe>(item.XmlDescompactado);
            var nfeResumida = repositorio.BuscarNfeResumidaPela(resNfe.chNFe);

            if (nfeResumida != null)
            {
                return true;
            }

            var chave = new ChaveSefaz(resNfe.chNFe);

            nfeResumida = new NfeResumida
            {
                Empresa = _emissor.EmissorFiscal.Empresa,
                AmbienteSefaz = _emissor.EmissorFiscal.EmissorFiscalNfe.Ambiente,
                EmissorFiscal = _emissor.EmissorFiscal,
                AutorizacaoEm = resNfe.dhRecbto,
                Chave = resNfe.chNFe,
                DocumentoUnicoEmitente = resNfe.CNPJ ?? resNfe.CPF,
                EmitidaEm = resNfe.dhEmi,
                InscricaoEstadualEmitente = resNfe.IE ?? string.Empty,
                NumeroProtocolo = resNfe.nProt.ToString(),
                RazaoSocialEmitente = resNfe.xNome,
                Serie = chave.GetSerie(),
                StatusNfe = StatusNfeHelper.Converter(resNfe.cSitNFe),
                TipoOperacao = (TipoOperacao) resNfe.tpNF,
                Valor = resNfe.vNF,
                Xml = item.XmlDescompactado,
                NumeroFiscal = chave.GetNumeroFiscal()
            };

            repositorio.Persistir(nfeResumida);
            return true;
        }
    }
}