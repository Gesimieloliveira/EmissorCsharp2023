using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DFe.Classes.Entidades;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF.Integridade;
using FusionCore.FusionNfce.EmissorFiscal;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Pagamento;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Flags;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Observacoes;
using NFe.Classes.Informacoes.Pagamento;
using NFe.Classes.Informacoes.Total;
using NFe.Classes.Informacoes.Transporte;
using Shared.NFe.Classes.Informacoes.InfRespTec;
using ProcessoEmissao = NFe.Classes.Informacoes.Identificacao.Tipos.ProcessoEmissao;
using TipoEmissao = NFe.Classes.Informacoes.Identificacao.Tipos.TipoEmissao;
using ZeusNfe = NFe.Classes.NFe;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtNfceZeus
    {
        public static ZeusNfe ToZeus(this Nfce nfce,
            NfceEmissaoHistorico emissao,
            NfceAutorizadoBaixarXml autorizadoBaixarXml)
        {
            var zeusNfe = new ZeusNfe
            {
                infNFe = GetInfNFe(nfce, emissao, autorizadoBaixarXml)
            };


            IncluirDeOlhoNoImposto(zeusNfe);

            return zeusNfe;
        }

        private static void IncluirDeOlhoNoImposto(ZeusNfe zeusNfe)
        {
            if (zeusNfe.infNFe.infAdic == null)
                zeusNfe.infNFe.infAdic = new infAdic();

            var deolhoNoIMposto = new DeOlhoNoImposto(new SessaoManagerNfce());
            deolhoNoIMposto.SetarNfe(zeusNfe);
            deolhoNoIMposto.IncluirTributosAproximados();
        }

        private static infNFe GetInfNFe(Nfce nfce, NfceEmissaoHistorico emissao, NfceAutorizadoBaixarXml autorizadoBaixarXml)
        {
            var infNFe = new infNFe
            {
                Id = "NFe" + emissao.ChaveTexto.Valor,
                ide = GetIdentificador(nfce, emissao),
                emit = nfce.Emitente.ToZeus(),
                dest = nfce.Destinatario.ToZeus(emissao.Versao.ToZeus()), 
                transp = GetTransportadora(nfce),
                versao = emissao.Versao.GetString(),
                det = GetDetalhe(nfce.ObterOsItens().OrderBy(x => x.NumeroItem)),
                total = GetTotal(nfce),
                pag = GetPagamentos(nfce.ObterOsPagamentos()),
                infRespTec = GetResponsavelTecnico(nfce.Emitente.Empresa.Estado.Id),
                autXML = GetAutXml(autorizadoBaixarXml)
            };

            MontaObservacao(nfce, infNFe);

            return infNFe;
        }

        private static List<autXML> GetAutXml(NfceAutorizadoBaixarXml autorizadoBaixarXml)
        {
            if (autorizadoBaixarXml == null) return null;

            var autXml = new autXML
            {
                CNPJ = autorizadoBaixarXml.DocumentoUnico
            };

            if (autorizadoBaixarXml.DocumentoUnico.Length == 11)
            {
                autXml.CNPJ = null;
                autXml.CPF = autorizadoBaixarXml.DocumentoUnico;
            } 

            return new List<autXML>
            {
                autXml
            };
        }

        private static infRespTec GetResponsavelTecnico(byte ufId)
        {
            if (ExisteResponsavelTecnico(ufId) == false) return null;

            return new infRespTec
            {
                CNPJ = ResponsavelLegal.Cnpj,
                email = ResponsavelLegal.Email,
                fone = ResponsavelLegal.Telefone,
                xContato = ResponsavelLegal.RazaoSocial
            };

        }

        private static bool ExisteResponsavelTecnico(byte ufId)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var responsavelTecnicoRepositorio = new RepositorioResponsavelTecnicoNfce(sessao);

                return responsavelTecnicoRepositorio.ExisteResponsavelTecnico(ufId);
            }
        }

        private static List<pag> GetPagamentos(IEnumerable<FormaPagamentoNfce> obterOsPagamentos)
        {
            var pags = obterOsPagamentos.ToZeus().ToList();
            return pags;
        }

        private static List<det> GetDetalhe(IEnumerable<NfceItem> itens)
        {
            var lista = (from itemFiscal in itens
             select new det
             {
                 nItem = itemFiscal.NumeroItem,
                 prod = itemFiscal.ToZeus(),
                 infAdProd = itemFiscal.Observacao.TrimSefazOrNull(500),
                 imposto = new imposto
                 {
                     vTotTrib = itemFiscal.ValorTributoAproximado,
                     ICMS = itemFiscal.ImpostoIcms.ToZeus(),
                     PIS = itemFiscal.Nfce.RegimeTributario == RegimeTributario.SimplesNacional ? null : itemFiscal.ImpostoPis.ToZeus(),
                     COFINS = itemFiscal.Nfce.RegimeTributario == RegimeTributario.SimplesNacional ? null : itemFiscal.ImpostoCofins.ToZeus()
                 }
             }).ToList();

            return lista;
        }

        private static transp GetTransportadora(Nfce nfce)
        {
            return new transp
            {
                modFrete = nfce.ModalidadeFrete.ToZeus()
            };
        }

        private static ide GetIdentificador(Nfce nfce, NfceEmissaoHistorico emissao)
        {
            var ide = new ide
            {
                cDV = emissao.Chave.DigitoVerificador.Valor,
                dhEmi = emissao.TentouEm.Valor,
                serie = emissao.Chave.Serie.Valor,
                cUF = (Estado)nfce.Emitente.Empresa.Estado.CodigoIbge,
                tpEmis = emissao.Chave.FormaEmissao.ToZeus(),
                cNF = emissao.Chave.CodigoNumerico.Valor.ToString("D8"),
                mod = nfce.Modelo.ToZeus(),
                nNF = emissao.Chave.NumeroFiscal.Valor,
                cMunFG = nfce.Emitente.Empresa.Cidade.CodigoIbge,
                idDest = nfce.DestinoOperacao.ToZeus(),
                finNFe = nfce.FinalidadeEmissao.ToZeus(),
                indFinal = nfce.IndicadorConsumidorFinal.ToZeus(),
                indPres = nfce.IndicadorComprador.ToZeus(),
                natOp = nfce.NaturezaOperacao,
                procEmi = ProcessoEmissao.peAplicativoContribuinte,
                tpAmb = emissao.AmbienteSefaz.ToZeus(),
                tpImp = nfce.TipoDanfe.ToZeus(),
                tpNF = nfce.TipoOperacao.ToZeus(),
                verProc = Assembly.GetEntryAssembly().GetName().Version.ToString()
            };

            var dataAtual = DateTime.Now.Date;
            var dataValidade = new DateTime(2021, 09, 1);

            if (dataAtual >= dataValidade || emissao.AmbienteSefaz == TipoAmbiente.Homologacao)
            {
                ide.indIntermed = IndicadorIntermediador.iiSemIntermediador;
            }

            if (ide.tpEmis == TipoEmissao.teNormal) return ide;

            ide.dhCont = emissao.Contingencia.EntrouEm.Value;
            ide.xJust = emissao.Contingencia.Justificativa.TrimSefaz(256);

            return ide;
        }

        private static total GetTotal(Nfce nfce)
        {
            var total = new total
            {
                ICMSTot = new ICMSTot
                {
                    vBC = nfce.TotalBaseCalculo,
                    vBCST = 0,
                    vDesc = nfce.TotalDesconto,
                    vICMS = nfce.TotalIcms,
                    vICMSDeson = nfce.TotalIcmsDesonerado,
                    vNF = nfce.TotalNfce,
                    vOutro = nfce.TotalAcrescimo,
                    vProd = nfce.TotalProdutosServicos,
                    vST = 0,
                    vTotTrib = nfce.ValorTributoAproximado,
                    vCOFINS = nfce.TotalCofins,
                    vFCPUFDest = 0,
                    vFrete = 0,
                    vICMSUFDest = 0,
                    vICMSUFRemet = 0,
                    vII = 0,
                    vIPI = 0,
                    vPIS = nfce.TotalPis,
                    vSeg = 0,
                    vFCP = 0,
                    vFCPST = 0,
                    vFCPSTRet = 0,
                    vIPIDevol = 0
                }
            };

            return total;
        }

        private static void MontaObservacao(Nfce nfce, infNFe infNFe)
        {
            if (nfce.Observacao.IsNullOrEmpty() && nfce.Troco == 0) {return;}

            infNFe.infAdic = new infAdic { infCpl = string.Empty };

            var observacao = string.Empty;

            if (nfce.Troco != 0)
            {
                observacao += $"Troco: {nfce.Troco:N2};";
            }

            if (nfce.Observacao.IsNotNullOrEmpty())
                observacao += nfce.Observacao;

            infNFe.infAdic.infCpl += observacao.TrimOrEmpty(); ;
        }
    }
}