using DFe.Utils;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.NF;
using nfeProc = NFe.Classes.nfeProc;

namespace FusionCore.GerenciarManifestacoesEletronicas.EstrategiasProcessamento
{
    public class EstrategiaNfeProc : IEstrategia
    {
        private readonly EmissorFiscalNFE _emissor;

        public EstrategiaNfeProc(EmissorFiscalNFE emissorNfe)
        {
            _emissor = emissorNfe;
        }

        public TipoDfe ETipoDfe { get; } = TipoDfe.NfeProc;
        public TipoEvento? ETipoEvento { get; set; }

        public bool Criar(ItemDistribuicaoDFe itemDfe, RepositorioDistribuicaoDFe repositorio)
        {
            var nfeProc = FuncoesXml.XmlStringParaClasse<nfeProc>(itemDfe.XmlDescompactado);
            if (nfeProc == null)
            {
                return false;
            }

            var chave = new ChaveSefaz(nfeProc.NFe.infNFe.Id.Substring(3, 44));
            var nfeResumida = repositorio.BuscarNfeResumidaPela(chave.Chave);

            if (nfeResumida == null)
            {
                nfeResumida = new NfeResumida
                {
                    AutorizacaoEm = nfeProc.protNFe.infProt.dhRecbto.DateTime,
                    Empresa = _emissor.EmissorFiscal.Empresa,
                    AmbienteSefaz = _emissor.Ambiente,
                    Chave = chave.Chave,
                    DocumentoUnicoEmitente = nfeProc.NFe.infNFe.emit.CNPJ ?? nfeProc.NFe.infNFe.emit.CPF,
                    EmitidaEm = nfeProc.NFe.infNFe.ide.dhEmi.Date,
                    InscricaoEstadualEmitente = nfeProc.NFe.infNFe.emit.IE ?? string.Empty,
                    NumeroFiscal = chave.GetNumeroFiscal(),
                    NumeroProtocolo = nfeProc.protNFe.infProt.nProt,
                    RazaoSocialEmitente = nfeProc.NFe.infNFe.emit.xNome,
                    Serie = chave.GetSerie(),
                    StatusManifestacao = StatusManifestacao.DownloadXml,
                    StatusNfe = StatusNfe.Autorizada,
                    TipoOperacao = (TipoOperacao) nfeProc.NFe.infNFe.ide.tpNF,
                    Valor = nfeProc.NFe.infNFe.total.ICMSTot.vNF,
                    Xml = itemDfe.XmlDescompactado,
                    EmissorFiscal = _emissor.EmissorFiscal
                };

                nfeResumida.AddXmlDownload(itemDfe.XmlDescompactado);
                repositorio.Persistir(nfeResumida);

                return true;
            }

            nfeResumida.StatusManifestacao = StatusManifestacao.DownloadXml;
            nfeResumida.AddXmlDownload(itemDfe.XmlDescompactado);
            repositorio.Update(nfeResumida);

            return true;
        }
    }
}