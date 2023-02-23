using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DFe.Classes.Flags;
using DFe.Utils;
using FusionCore.Core.Flags;
using FusionCore.DFe.RegrasNegocios.Chave;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Tipos;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionNfce.Extencoes;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Flags;
using FusionCore.Tributacoes.Regras;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Infraestrutura;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Xml;
using MotorTributarioNet.Facade;
using MotorTributarioNet.Flags;
using MotorTributarioNet.Impostos.Csts;
using NFe.Classes;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.ProdEspecifico;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Observacoes;
using NFe.Classes.Informacoes.Pagamento;
using NFe.Classes.Informacoes.Total;
using NFe.Classes.Informacoes.Transporte;
using NFe.Classes.Servicos.Autorizacao;
using NFe.Classes.Servicos.Tipos;
using NFe.Utils;
using NFe.Utils.Autorizacao;
using NFe.Utils.InformacoesSuplementares;
using NFe.Utils.NFe;
using NFe.Utils.Validacao;
using OpenAC.Net.Core.Extensions;
using Shared.NFe.Classes.Informacoes.InfRespTec;
using DestinoOperacao = NFe.Classes.Informacoes.Identificacao.Tipos.DestinoOperacao;
using ModeloDocumento = FusionCore.FusionAdm.Fiscal.Flags.ModeloDocumento;
using OrigemMercadoria = FusionCore.FusionAdm.Fiscal.FlagsImposto.OrigemMercadoria;
using TipoAmbiente = FusionCore.FusionAdm.Fiscal.Flags.TipoAmbiente;
using TipoEmissao = FusionCore.FusionAdm.Fiscal.Flags.TipoEmissao;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class ConstruirXmlNfceDeUmaVenda
    {
        private readonly FaturamentoVenda _venda;
        private readonly CupomFiscal _cupomFiscal;

        public ConstruirXmlNfceDeUmaVenda(FaturamentoVenda venda, CupomFiscal cupomFiscal)
        {
            _venda = venda;
            _cupomFiscal = cupomFiscal;
        }

        public NFe.Classes.NFe Construir()
        {
            var tentouEm = _cupomFiscal.EmitirEm;

            var chave = new GerarChaveFiscal((int)ModeloDocumento.NFCe
                , (int)_cupomFiscal.TipoEmissao, _cupomFiscal.CodigoNumerico
                , _venda.Empresa.EstadoDTO.CodigoIbge
                , tentouEm.Date, long.Parse(_venda.Empresa.DocumentoUnico)
                , _cupomFiscal.NumeroFiscal, _cupomFiscal.Serie);

            var nfe = ConverteFaturamentoParaZeus(_venda, chave.Chave, tentouEm);

            var xmlAssinado = new AssinadorSefaz(CertificadoDigitalFactory.Cria(BuscarEmissorFiscalDadosCertificado(), true))
                .Assina(nfe.ObterXmlString().RemoverAcentos(), nfe.infNFe.Id);

            nfe = FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(xmlAssinado.OuterXml);

            var tokenNfce = BuscarTokenNfceParaFaturamento(_cupomFiscal.EmissorFiscalId);

            var urlQrCode = nfe.infNFeSupl.ObterUrlQrCode(
                nfe,
                VersaoQrCode.QrCodeVersao2,
                tokenNfce.ObterToken(),
                tokenNfce.Csc);

            var urChave = nfe.infNFeSupl.ObterUrl(
                nfe.infNFe.ide.tpAmb,
                nfe.infNFe.ide.cUF,
                TipoUrlConsultaPublica.UrlConsulta,
                VersaoServico.Versao400,
                VersaoQrCode.QrCodeVersao2);

            nfe.infNFeSupl = new infNFeSupl
            {
                qrCode = urlQrCode,
                urlChave = urChave
            };

            var envio = new enviNFe4(
                "4.00",
                1,
                IndicadorSincronizacao.Assincrono,
                new List<NFe.Classes.NFe> { nfe }
            );

            var xmlEnvio = envio.ObterXmlString().RemoverAcentos();

            Validador.Valida(ServicoNFe.NFeAutorizacao, VersaoServico.Versao400, xmlEnvio, true, Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets", "Schemas.Nfe"));

            return nfe;
        }
        private EmissorFiscal BuscarEmissorFiscalDadosCertificado()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioEmissorFiscal(sessao).BuscarEmissorDadosCertificado(_cupomFiscal.EmissorFiscalId);
            }
        }
        private TokenNfce BuscarTokenNfceParaFaturamento(byte emissorFiscalId)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioEmissorFiscal(sessao).BuscarTokenNfceFaturamento(emissorFiscalId);
            }
        }

        private NFe.Classes.NFe ConverteFaturamentoParaZeus(FaturamentoVenda venda, string chave, DateTime tentouEm)
        {
            var nfe = new NFe.Classes.NFe
            {
                infNFe = new infNFe
                {
                    Id = $"NFe{chave}",
                    ide = MontarIdentificador(venda, tentouEm, chave),
                    emit = MontarEmitente(venda),
                    dest = MontarDestinatario(venda),
                    transp = MontarTransportadora(),
                    versao = "4.00",
                    det = MontarItensFiscal(venda.Produtos.OrderBy(x => x.Numero)),
                    pag = MontaPagamentos(venda.Pagamentos),
                    infRespTec = MontaInformacaoResponsavel(),
                    infAdic = MontaObservacao(venda.Observacao)
                }
            };
            var total = MontarTotais(venda, nfe.infNFe.det);
            nfe.infNFe.total = total;

            return nfe;
        }

        private ide MontarIdentificador(FaturamentoVenda venda, DateTime tentouEm, string chaveSefaz)
        {
            var chave = new ChaveSefaz(chaveSefaz);

            var ide = new ide
            {
                cDV = chave.Dv,
                dhEmi = tentouEm,
                serie = _cupomFiscal.Serie,
                cUF = (global::DFe.Classes.Entidades.Estado)venda.Empresa.EstadoDTO.CodigoIbge,
                tpEmis = ObterTipoEmissao(),
                cNF = _cupomFiscal.CodigoNumerico.ToString("D8"),
                mod = global::DFe.Classes.Flags.ModeloDocumento.NFCe,
                nNF = _cupomFiscal.NumeroFiscal,
                cMunFG = venda.Empresa.CidadeDTO.CodigoIbge,
                idDest = DestinoOperacao.doInterna,
                finNFe = FinalidadeNFe.fnNormal,
                indFinal = ConsumidorFinal.cfConsumidorFinal,
                indPres = PresencaComprador.pcPresencial,
                natOp = "Venda de Mercadorias",
                procEmi = NFe.Classes.Informacoes.Identificacao.Tipos.ProcessoEmissao.peAplicativoContribuinte,
                tpAmb = ObterAmbienteSefaz(),
                tpImp = TipoImpressao.tiNFCe,
                tpNF = TipoNFe.tnSaida,
                verProc = ResponsavelLegal.VersaoSistema
            };

            if (ide.tpEmis == NFe.Classes.Informacoes.Identificacao.Tipos.TipoEmissao.teNormal) return ide;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var contingencia = new TodasContingencias(sessao).GetPeloId(_cupomFiscal.ContingenciaId.Value);

                ide.dhCont = contingencia.EntrouEm;
                ide.xJust = "Entrou em contingência devo a problemas de comunicação com a sefaz";
            }

            return ide;
        }
        private emit MontarEmitente(FaturamentoVenda venda)
        {
            var empresa = venda.Empresa;

            var emit = new emit
            {
                CNPJ = empresa.Cnpj.Trim(),
                CRT = empresa.RegimeTributario.ToZeus(),
                IE = empresa.InscricaoEstadual.TrimSefaz(14),
                xFant = empresa.NomeFantasia.TrimSefaz(60),
                xNome = empresa.RazaoSocial.TrimSefaz(60),
                enderEmit = new enderEmit
                {
                    CEP = empresa.Cep.Trim(),
                    UF = (global::DFe.Classes.Entidades.Estado)empresa.EstadoDTO.CodigoIbge,
                    cMun = empresa.CidadeDTO.CodigoIbge,
                    xMun = empresa.CidadeDTO.Nome.TrimSefaz(60),
                    cPais = 1058,
                    xPais = "BRASIL",
                    nro = empresa.Numero.TrimSefaz(60),
                    xBairro = empresa.Bairro.TrimSefaz(60),
                    xCpl = empresa.Complemento.TrimSefazOrNull(60),
                    xLgr = empresa.Logradouro.TrimSefaz(60)
                }
            };

            if (!string.IsNullOrEmpty(empresa.Fone1))
                emit.enderEmit.fone = long.Parse(empresa.Fone1);

            return emit;
        }
        private dest MontarDestinatario(FaturamentoVenda venda)
        {
            var destinatario = venda.Destinatario;

            if (destinatario == null) return null;

            var destinatarioZeus = new dest(VersaoServico.Versao400);

            switch (destinatario.Cliente.GetDocumentoUnico().Length)
            {
                case 14:
                    destinatarioZeus.CNPJ = destinatario.Cliente.GetDocumentoUnico();
                    break;
                case 11:
                    destinatarioZeus.CPF = destinatario.Cliente.GetDocumentoUnico();
                    break;
            }

            destinatarioZeus.xNome = destinatario.Cliente.Nome.IsNullOrEmpty() ? null : destinatario.Cliente.Nome.Trim();


            if (_cupomFiscal.EstaEmHomologacao())
            {
                destinatarioZeus.xNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }

            destinatarioZeus.indIEDest = indIEDest.NaoContribuinte;

            if (destinatario.Cliente.Pessoa.Emails.Count != 0)
            {
                var email = destinatario.Cliente.Pessoa.Emails.FirstOrDefault();

                destinatarioZeus.email = email.Email.Valor.TrimSefazOrNull(60);
            }

            if (!destinatario.IsAddEndereco) return destinatarioZeus;

            DadosObrigatorio(destinatario);

            var enderDest = new enderDest();

            enderDest.xLgr = destinatario.Endereco.Logradouro.TrimSefazOrNull(60);
            enderDest.nro = destinatario.Endereco.Numero.TrimSefazOrNull(60);
            enderDest.xCpl = destinatario.Endereco.Complemento.TrimSefazOrNull(60);
            enderDest.xBairro = destinatario.Endereco.Bairro.TrimSefazOrNull(60);

            if (destinatario.Endereco.Cidade != null)
            {
                enderDest.cMun = long.Parse(destinatario.Endereco.Cidade?.CodigoIbge.ToString());
                enderDest.xMun = destinatario.Endereco.Cidade.Nome.TrimSefazOrNull(60);
                enderDest.UF = destinatario.Endereco.Cidade.SiglaUf;
                enderDest.CEP = destinatario.Endereco.Cep.TrimSefazOrNull(8);
            }

            destinatarioZeus.enderDest = enderDest;

            return destinatarioZeus;
        }
        private transp MontarTransportadora()
        {
            return new transp
            {
                modFrete = ModalidadeFrete.mfSemFrete
            };
        }
        private List<det> MontarItensFiscal(IOrderedEnumerable<FaturamentoProduto> produtos)
        {
            var lista = (from itemFiscal in produtos
                         select new det
                         {
                             nItem = itemFiscal.Numero,
                             prod = MontarProduto(itemFiscal),
                             imposto = new imposto
                             {
                                 vTotTrib = CalculaValorTributoAproximado(itemFiscal),
                                 ICMS = ConverteImpostoIcmsZeus(itemFiscal
                                     , itemFiscal.Produto.OrigemMercadoria
                                     , itemFiscal.Faturamento.Empresa.RegimeTributario),
                                 PIS = itemFiscal.Faturamento.Empresa.RegimeTributario == RegimeTributario.SimplesNacional ? null : ObterPis(itemFiscal),
                                 COFINS = itemFiscal.Faturamento.Empresa.RegimeTributario == RegimeTributario.SimplesNacional ? null : ObterCofins(itemFiscal)
                             }
                         }).ToList();

            return lista;
        }
        private prod MontarProduto(FaturamentoProduto item)
        {
            var totalDesconto = item.TotalDesconto + item.TotalDescontoFixo;

            var prod = new prod
            {
                vProd = item.Total,
                vDesc = totalDesconto == 0 ? (decimal?)null : totalDesconto,
                CFOP = int.Parse(item.Produto.RegraTributacaoSaida.CfopNfce.Id),
                cEAN = ObterGtin(item.Produto.ProdutosAlias),
                cEANTrib = ObterGtin(item.Produto.ProdutosAlias),
                cProd = item.Produto.Id.ToString(),
                indTot = IndicadorTotal.ValorDoItemCompoeTotalNF,
                qCom = item.Quantidade,
                uCom = item.SiglaUnidade,
                vUnCom = item.PrecoUnitario,
                xProd = ObterNomeProduto(item.Produto.Nome),
                CEST = string.IsNullOrWhiteSpace(item.Produto.Cest) ? null : item.Produto.Cest,
                NCM = string.IsNullOrWhiteSpace(item.Produto.Ncm) ? null : item.Produto.Ncm,
                qTrib = item.Quantidade,
                uTrib = item.SiglaUnidade,
                vUnTrib = item.PrecoUnitario
            };

            if (item.Produto.ProdutoUnidadeTributavel != null)
            {
                prod.qTrib = item.Produto.QuantidadeUnidadeTributavel;
                prod.uTrib = item.Produto.ProdutoUnidadeTributavel.Sigla;
                prod.vUnTrib = (item.Total / item.Produto.QuantidadeUnidadeTributavel).RoundABNT(10);
            }

            MontaProdutoComb(item, prod);

            return prod;
        }
        private string ObterGtin(IList<ProdutoAlias> produtoProdutosAlias)
        {
            var codigoBarras = produtoProdutosAlias.SingleOrDefault(x => x.IsCodigoBarras == true)?.Alias;

            return codigoBarras.IsNotNullOrEmpty() ? codigoBarras : "SEM GTIN";
        }
        private string ObterNomeProduto(string produtoNome)
        {
            return _cupomFiscal.AmbienteSefaz == TipoAmbiente.Producao ? produtoNome.TrimSefaz(120) : "NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
        }
        private void MontaProdutoComb(FaturamentoProduto item, prod prod)
        {
            var produto = item.Produto;
            var cProdAnp = produto.CodigoAnp;

            if (string.IsNullOrWhiteSpace(cProdAnp)) return;

            var produtoCodigoAnp = produto.CarregaAnp();

            if (produtoCodigoAnp == null)
                throw new InvalidOperationException("Produto Código ANP não encontrado");

            prod.ProdutoEspecifico = new List<ProdutoEspecifico>
            {
                new comb
                {
                    cProdANP = cProdAnp,
                    UFCons = ObterSiglaUfConsumo(item),
                    descANP = produtoCodigoAnp.Descricao,
                    pGLP = produto.PercentualGlpPetroleo == 0 ? (decimal?)null : produto.PercentualGlpPetroleo,
                    pGNn = produto.PercentualGasNacional == 0 ? (decimal?)null : produto.PercentualGasNacional,
                    pGNi = produto.PercentualGasImportador == 0 ? (decimal?)null : produto.PercentualGasImportador,
                    vPart = produto.ValorDePartida == 0 ? (decimal?)null : produto.ValorDePartida
                }
            };
        }
        private string ObterSiglaUfConsumo(FaturamentoProduto item)
        {
            if (item.Faturamento.Destinatario == null)
                return item.Faturamento.Empresa.CidadeDTO.SiglaUf;

            if (item.Faturamento.Destinatario != null && item.Faturamento.Destinatario.Endereco == null)
                return item.Faturamento.Empresa.CidadeDTO.SiglaUf;

            if (item.Faturamento.Destinatario.Endereco.Cidade == null)
                return item.Faturamento.Empresa.CidadeDTO.SiglaUf;

            return item.Faturamento.Destinatario.Endereco.Cidade.SiglaUf;
        }
        private decimal? CalculaValorTributoAproximado(FaturamentoProduto itemFiscal)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var ibpt = new RepositorioIbpt(sessao).GetPeloNcm(itemFiscal.Produto.Ncm);

                if (ibpt == null) return null;


                var produtoTributavel = new ProdutoTributavelCalculadora
                {
                    ValorProduto = itemFiscal.PrecoUnitario,
                    QuantidadeProduto = itemFiscal.Quantidade,
                    PercentualFederal = ibpt.Nacional,
                    PercentualFederalImportados = ibpt.Importado,
                    PercentualEstadual = ibpt.Estadual
                };

                var calculadoraTributaria = new FacadeCalculadoraTributacao(produtoTributavel);

                var resultadoCalculoIbpt = calculadoraTributaria.CalculaIbpt(produtoTributavel);

                return resultadoCalculoIbpt.TributacaoEstadual + resultadoCalculoIbpt.TributacaoFederal;
            }
        }
        private ICMS ConverteImpostoIcmsZeus(FaturamentoProduto itemFiscal,
            OrigemMercadoria origemMercadoria,
            RegimeTributario regimeTributario)
        {

            var regraTributacaoSaida = itemFiscal.Produto.RegraTributacaoSaida;

            if (regimeTributario != RegimeTributario.SimplesNacional)
            {
                return new ICMS
                {
                    TipoICMS = ObterImpostoCst(itemFiscal, origemMercadoria)
                };
            }

            return new ICMS
            {
                TipoICMS = ObterImpostoCsosn(regraTributacaoSaida, origemMercadoria)
            };
        }
        private ICMSBasico ObterImpostoCst(FaturamentoProduto itemFiscal, OrigemMercadoria origemMercadoria)
        {
            var regraTributacaoSaida = itemFiscal.Produto.RegraTributacaoSaida;

            switch (regraTributacaoSaida.Cst.Codigo)
            {
                case "60": return ObterIcms60(regraTributacaoSaida, origemMercadoria);
                case "30": return ObterCst30(regraTributacaoSaida, origemMercadoria);
                case "00": return ObterCst00(itemFiscal, origemMercadoria);
                case "90": return ObterCst90(itemFiscal, origemMercadoria);
                case "20": return ObterCst20(itemFiscal, origemMercadoria);
                case "41":
                case "40":
                    return ObterCst40(itemFiscal, origemMercadoria);
                default:
                    throw new InvalidOperationException($"Cst {regraTributacaoSaida.Cst.Codigo} não é compatível com nfc-e");
            }
        }
        private ICMSBasico ObterIcms60(RegraTributacaoSaida regraTributacaoSaida, OrigemMercadoria origemMercadoria)
        {
            return new ICMS60
            {
                orig = origemMercadoria.ToZeus(),
                CST = regraTributacaoSaida.Cst.ToCstZeus()
            };
        }
        private ICMSBasico ObterCst30(RegraTributacaoSaida regraTributacaoSaida, OrigemMercadoria origemMercadoria)
        {
            return new ICMS30
            {
                orig = origemMercadoria.ToZeus(),
                CST = regraTributacaoSaida.Cst.ToCstZeus()
            };
        }
        private ICMSBasico ObterCst00(FaturamentoProduto itemFiscal, OrigemMercadoria origemMercadoria)
        {
            var produtoTributavel = new ProdutoTributavelCalculadora
            {
                QuantidadeProduto = itemFiscal.Quantidade,
                Documento = Documento.NFe,
                Desconto = itemFiscal.TotalDesconto + itemFiscal.TotalDescontoFixo,
                PercentualIcms = itemFiscal.Produto.AliquotaIcms,
                ValorProduto = itemFiscal.PrecoUnitario,
                PercentualCofins = itemFiscal.Produto.AliquotaCofins,
                PercentualPis = itemFiscal.Produto.AliquotaPis
            };

            var cst00 = new Cst00(origemMercadoria.ToMotorTributario());
            cst00.Calcula(produtoTributavel);

            var regraTributacaoSaida = itemFiscal.Produto.RegraTributacaoSaida;

            return new ICMS00
            {
                orig = origemMercadoria.ToZeus(),
                CST = regraTributacaoSaida.Cst.ToCstZeus(),
                pICMS = produtoTributavel.PercentualIcms,
                vICMS = decimal.Round(cst00.ValorIcms, 2),
                vBC = decimal.Round(cst00.ValorBcIcms, 2),
                modBC = DeterminacaoBaseIcms.DbiValorOperacao
            };
        }
        private ICMSBasico ObterCst90(FaturamentoProduto itemFiscal, OrigemMercadoria origemMercadoria)
        {
            var produtoTributavel = new ProdutoTributavelCalculadora
            {
                QuantidadeProduto = itemFiscal.Quantidade,
                Documento = Documento.NFe,
                Desconto = itemFiscal.TotalDesconto + itemFiscal.TotalDescontoFixo,
                PercentualIcms = itemFiscal.Produto.AliquotaIcms,
                ValorProduto = itemFiscal.PrecoUnitario,
                PercentualCofins = itemFiscal.Produto.AliquotaCofins,
                PercentualPis = itemFiscal.Produto.AliquotaPis
            };

            var regraTributacaoSaida = itemFiscal.Produto.RegraTributacaoSaida;

            var cst90 = new Cst90(origemMercadoria.ToMotorTributario());
            cst90.Calcula(produtoTributavel);

            return new ICMS90
            {
                orig = origemMercadoria.ToZeus(),
                CST = regraTributacaoSaida.Cst.ToCstZeus(),
                pICMS = itemFiscal.Produto.AliquotaIcms,
                vICMS = decimal.Round(cst90.ValorIcms, 2),
                vBC = decimal.Round(cst90.ValorBcIcms, 2),
                pRedBC = decimal.Round(cst90.PercentualReducaoIcmsBc, 2),
                modBC = DeterminacaoBaseIcms.DbiValorOperacao
            };
        }
        private ICMSBasico ObterCst20(FaturamentoProduto itemFiscal, OrigemMercadoria origemMercadoria)
        {
            var produtoTributavel = new ProdutoTributavelCalculadora
            {
                QuantidadeProduto = itemFiscal.Quantidade,
                Documento = Documento.NFe,
                Desconto = itemFiscal.TotalDesconto + itemFiscal.TotalDescontoFixo,
                PercentualIcms = itemFiscal.Produto.AliquotaIcms,
                ValorProduto = itemFiscal.PrecoUnitario,
                PercentualCofins = itemFiscal.Produto.AliquotaCofins,
                PercentualPis = itemFiscal.Produto.AliquotaPis
            };

            var regraTributacaoSaida = itemFiscal.Produto.RegraTributacaoSaida;

            var cst20 = new Cst20(origemMercadoria.ToMotorTributario());
            cst20.Calcula(produtoTributavel);

            return new ICMS20
            {
                orig = origemMercadoria.ToZeus(),
                CST = regraTributacaoSaida.Cst.ToCstZeus(),
                pICMS = itemFiscal.Produto.AliquotaIcms,
                vICMS = decimal.Round(cst20.ValorIcms, 2),
                vBC = decimal.Round(cst20.ValorBcIcms, 2),
                pRedBC = decimal.Round(cst20.PercentualReducao, 2),
                modBC = DeterminacaoBaseIcms.DbiValorOperacao
            };
        }
        private ICMSBasico ObterCst40(FaturamentoProduto itemFiscal, OrigemMercadoria origemMercadoria)
        {
            var regraTributacaoSaida = itemFiscal.Produto.RegraTributacaoSaida;

            return new ICMS40
            {
                orig = origemMercadoria.ToZeus(),
                CST = regraTributacaoSaida.Cst.ToCstZeus(),
            };
        }
        private ICMSBasico ObterImpostoCsosn(RegraTributacaoSaida regraTributacaoSaida,
            OrigemMercadoria origemMercadoria)
        {
            switch (regraTributacaoSaida.Csosn.Codigo)
            {
                case "102":
                case "103":
                case "300":
                case "400":
                    return GetCsosn102(regraTributacaoSaida, origemMercadoria);
                case "500": return GetCsosn500(regraTributacaoSaida, origemMercadoria);
                default:
                    throw new InvalidOperationException($"CSOSN {regraTributacaoSaida.Csosn.Codigo} não é compatível com nfc-e");
            }
        }
        private ICMSBasico GetCsosn102(RegraTributacaoSaida regraTributacaoSaida,
            OrigemMercadoria origemMercadoria)
        {
            return new ICMSSN102
            {
                orig = origemMercadoria.ToZeus(),
                CSOSN = ObterCsosnZeus(regraTributacaoSaida.Csosn.Codigo)
            };
        }
        private ICMSBasico GetCsosn500(RegraTributacaoSaida regraTributacaoSaida, OrigemMercadoria origemMercadoria)
        {
            var icms = new ICMSSN500
            {
                orig = origemMercadoria.ToZeus(),
                CSOSN = ObterCsosnZeus(regraTributacaoSaida.Csosn.Codigo)
            };

            return icms;
        }
        public Csosnicms ObterCsosnZeus(string csosn)
        {
            switch (csosn)
            {
                case "101": return Csosnicms.Csosn101;
                case "102": return Csosnicms.Csosn102;
                case "103": return Csosnicms.Csosn103;
                case "201": return Csosnicms.Csosn201;
                case "202": return Csosnicms.Csosn202;
                case "203": return Csosnicms.Csosn203;
                case "300": return Csosnicms.Csosn300;
                case "400": return Csosnicms.Csosn400;
                case "500": return Csosnicms.Csosn500;
                case "900": return Csosnicms.Csosn900;
            }

            throw new InvalidCastException($"Não foi possível utilizar o CSOSN: {csosn}");
        }
        private PIS ObterPis(FaturamentoProduto itemFiscal)
        {
            return new PIS
            {
                TipoPIS = ConvertePis(itemFiscal)
            };
        }
        private PISBasico ConvertePis(FaturamentoProduto itemFiscal)
        {
            switch (itemFiscal.Produto.Pis.Id)
            {
                case "01":
                case "02":
                    return ObterPisAliquota(itemFiscal);
                case "03":
                    return ObterPisQtde(itemFiscal);
                case "04":
                case "05":
                case "06":
                case "07":
                case "08":
                case "09":
                    return ObterPisNaoTributado(itemFiscal);
                case "49":
                    return ObterPisOutros(itemFiscal);
            }

            throw new InvalidOperationException($"PIS inválido: {itemFiscal.Produto.Pis.Id}");
        }
        private PISBasico ObterPisAliquota(FaturamentoProduto itemFiscal)
        {
            var produtoTributavel = new ProdutoTributavelCalculadora
            {
                QuantidadeProduto = itemFiscal.Quantidade,
                Documento = Documento.NFe,
                Desconto = itemFiscal.TotalDesconto + itemFiscal.TotalDescontoFixo,
                PercentualIcms = itemFiscal.Produto.AliquotaIcms,
                ValorProduto = itemFiscal.PrecoUnitario,
                PercentualCofins = itemFiscal.Produto.AliquotaCofins,
                PercentualPis = itemFiscal.Produto.AliquotaPis
            };


            var facade = new FacadeCalculadoraTributacao(produtoTributavel);
            var resultadoCalculoPis = facade.CalculaPis();

            return new PISAliq
            {
                CST = itemFiscal.Produto.ToZeus(),
                pPIS = produtoTributavel.PercentualPis,
                vBC = decimal.Round(resultadoCalculoPis.BaseCalculo, 2),
                vPIS = decimal.Round(resultadoCalculoPis.Valor, 2)
            };
        }
        private PISBasico ObterPisQtde(FaturamentoProduto itemFiscal)
        {
            throw new InvalidCastException("PIS quantidade não disponível");
        }
        private PISBasico ObterPisNaoTributado(FaturamentoProduto itemFiscal)
        {
            return new PISNT
            {
                CST = itemFiscal.Produto.ToZeus(),
            };
        }
        private PISBasico ObterPisOutros(FaturamentoProduto itemFiscal)
        {
            var produtoTributavel = new ProdutoTributavelCalculadora
            {
                QuantidadeProduto = itemFiscal.Quantidade,
                Documento = Documento.NFe,
                Desconto = itemFiscal.TotalDesconto + itemFiscal.TotalDescontoFixo,
                PercentualIcms = itemFiscal.Produto.AliquotaIcms,
                ValorProduto = itemFiscal.PrecoUnitario,
                PercentualCofins = itemFiscal.Produto.AliquotaCofins,
                PercentualPis = itemFiscal.Produto.AliquotaPis
            };


            var facade = new FacadeCalculadoraTributacao(produtoTributavel);
            var resultadoCalculoPis = facade.CalculaPis();

            return new PISOutr
            {
                CST = itemFiscal.Produto.ToZeus(),
                pPIS = produtoTributavel.PercentualPis,
                vBC = decimal.Round(resultadoCalculoPis.BaseCalculo, 2),
                vPIS = decimal.Round(resultadoCalculoPis.Valor, 2)
            };
        }
        private COFINS ObterCofins(FaturamentoProduto itemFiscal)
        {
            return new COFINS
            {
                TipoCOFINS = ConverteCofins(itemFiscal)
            };
        }
        private COFINSBasico ConverteCofins(FaturamentoProduto itemFiscal)
        {
            switch (itemFiscal.Produto.Cofins.Id)
            {
                case "01":
                case "02":
                    return ObterCofinsAliquota(itemFiscal);
                case "03":
                    return ObterCofinsQtde(itemFiscal);
                case "04":
                case "05":
                case "06":
                case "07":
                case "08":
                case "09":
                    return ObterCofinsNaoTributado(itemFiscal);
                case "49":
                    return ObterCofinsOutros(itemFiscal);
            }

            throw new InvalidOperationException($"COFINS inválido: {itemFiscal.Produto.Cofins}");
        }
        private COFINSBasico ObterCofinsAliquota(FaturamentoProduto itemFiscal)
        {
            var produtoTributavel = new ProdutoTributavelCalculadora
            {
                QuantidadeProduto = itemFiscal.Quantidade,
                Documento = Documento.NFe,
                Desconto = itemFiscal.TotalDesconto + itemFiscal.TotalDescontoFixo,
                PercentualIcms = itemFiscal.Produto.AliquotaIcms,
                ValorProduto = itemFiscal.PrecoUnitario,
                PercentualCofins = itemFiscal.Produto.AliquotaCofins,
                PercentualPis = itemFiscal.Produto.AliquotaPis
            };


            var facade = new FacadeCalculadoraTributacao(produtoTributavel);
            var resultadoCalculoCofins = facade.CalculaCofins();

            return new COFINSAliq
            {
                CST = itemFiscal.Produto.ToZeusCofins(),
                pCOFINS = produtoTributavel.PercentualCofins,
                vBC = decimal.Round(resultadoCalculoCofins.BaseCalculo, 2),
                vCOFINS = decimal.Round(resultadoCalculoCofins.Valor, 2)
            };
        }
        private COFINSBasico ObterCofinsQtde(FaturamentoProduto itemFiscal)
        {
            throw new InvalidCastException("Cofins quantidade não disponível");
        }
        private COFINSBasico ObterCofinsNaoTributado(FaturamentoProduto itemFiscal)
        {
            return new COFINSNT
            {
                CST = itemFiscal.Produto.ToZeusCofins(),
            };
        }
        private COFINSBasico ObterCofinsOutros(FaturamentoProduto itemFiscal)
        {
            var produtoTributavel = new ProdutoTributavelCalculadora
            {
                QuantidadeProduto = itemFiscal.Quantidade,
                Documento = Documento.NFe,
                Desconto = itemFiscal.TotalDesconto + itemFiscal.TotalDescontoFixo,
                PercentualIcms = itemFiscal.Produto.AliquotaIcms,
                ValorProduto = itemFiscal.PrecoUnitario,
                PercentualCofins = itemFiscal.Produto.AliquotaCofins,
                PercentualPis = itemFiscal.Produto.AliquotaPis
            };


            var facade = new FacadeCalculadoraTributacao(produtoTributavel);
            var resultadoCalculoCofins = facade.CalculaCofins();

            return new COFINSOutr
            {
                CST = itemFiscal.Produto.ToZeusCofins(),
                pCOFINS = produtoTributavel.PercentualCofins,
                vBC = resultadoCalculoCofins.BaseCalculo,
                vCOFINS = resultadoCalculoCofins.Valor
            };
        }
        private List<pag> MontaPagamentos(IEnumerable<FPagamento> pagamentos)
        {
            var pgs = new List<pag>();
            var pag = new pag
            {
                detPag = new List<detPag>()
            };
            pgs.Add(pag);

            foreach (var pagto in pagamentos)
            {
                switch (pagto.Especie)
                {
                    case ETipoPagamento.Dinheiro:
                        pag.detPag.Add(new detPag
                        {
                            indPag = IndicadorPagamentoDetalhePagamento.ipDetPgVista,
                            tPag = FormaPagamento.fpDinheiro,
                            vPag = pagto.Valor
                        });
                        continue;

                    case ETipoPagamento.CreditoLoja:

                        var descricao = "";
                        var pagamento = FormaPagamento.fpCreditoLoja;

                        if (pagto.TipoDocumento.FormaPagamento.ToString() == "Outro")
                        {
                            pagamento = FormaPagamento.fpOutro;
                            descricao = pagto.TipoDocumento.Descricao;
                        }
                        else
                        {
                            pagamento = FormaPagamento.fpCreditoLoja;
                            descricao = null;
                        }

                        pag.detPag.Add(new detPag
                        {
                            indPag = IndicadorPagamentoDetalhePagamento.ipDetPgPrazo,
                            tPag = pagamento,
                            vPag = pagto.Valor,
                            xPag = descricao
                        });

                        continue;

                    case ETipoPagamento.CartaoCredito:
                        pag.detPag.Add(new detPag
                        {
                            indPag = IndicadorPagamentoDetalhePagamento.ipDetPgVista,
                            tPag = FormaPagamento.fpCartaoCredito,
                            vPag = pagto.Valor,
                            card = new card
                            {
                                tpIntegra = TipoIntegracaoPagamento.TipNaoIntegrado,
                                tBand = BandeiraCartao.bcOutros
                            }
                        });
                        continue;

                    case ETipoPagamento.CartaoDebito:
                        pag.detPag.Add(new detPag
                        {
                            indPag = IndicadorPagamentoDetalhePagamento.ipDetPgVista,
                            tPag = FormaPagamento.fpCartaoDebito,
                            vPag = pagto.Valor,
                            card = new card
                            {
                                tpIntegra = TipoIntegracaoPagamento.TipNaoIntegrado,
                                tBand = BandeiraCartao.bcOutros
                            }
                        });
                        continue;

                    case ETipoPagamento.Pix:
                        pag.detPag.Add(new detPag
                        {
                            indPag = IndicadorPagamentoDetalhePagamento.ipDetPgVista,
                            tPag = FormaPagamento.fpPagamentoInstantaneoPIX,
                            vPag = pagto.Valor
                        });
                        continue;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return pgs;
        }
        private infRespTec MontaInformacaoResponsavel()
        {
            return new infRespTec
            {
                CNPJ = ResponsavelLegal.Cnpj,
                email = ResponsavelLegal.Email,
                fone = ResponsavelLegal.Telefone,
                xContato = ResponsavelLegal.RazaoSocial
            };
        }
        private infAdic MontaObservacao(string observacao)
        {
            if (observacao.IsNullOrEmpty())
                return null;

            return new infAdic
            {
                infCpl = observacao
            };
        }
        private total MontarTotais(FaturamentoVenda venda, List<det> det)
        {
            var total = new total
            {
                ICMSTot = new ICMSTot
                {
                    vBC = det.Sum(x => x.imposto.ICMS.TipoICMS.GetIcmsBcValue()),
                    vBCST = 0,
                    vDesc = CalculaTotalDesconto(det),
                    vICMS = det.Sum(x => x.imposto.ICMS.TipoICMS.GetIcmsValue()),
                    vICMSDeson = det.Sum(x => x.imposto.ICMS.TipoICMS.GetIcmsDesonValue()),
                    vNF = det.Sum(x => x.prod.vProd) - CalculaTotalDesconto(det),
                    vOutro = 0,
                    vProd = det.Sum(x => x.prod.vProd),
                    vST = 0,
                    vTotTrib = det.Where(x => x.imposto.vTotTrib != null).Sum(x => x.imposto.vTotTrib) ?? 0.0m,
                    vCOFINS = det.Where(item => item.imposto.COFINS != null).Sum(item => item.imposto.COFINS.TipoCOFINS.GetCofinsValue()),
                    vFCPUFDest = 0,
                    vFrete = 0,
                    vICMSUFDest = 0,
                    vICMSUFRemet = 0,
                    vII = 0,
                    vIPI = 0,
                    vPIS = det.Where(item => item.imposto.PIS != null).Sum(item => item.imposto.PIS.TipoPIS.GetPisValue()),
                    vSeg = 0,
                    vFCP = 0,
                    vFCPST = 0,
                    vFCPSTRet = 0,
                    vIPIDevol = 0
                }
            };

            return total;
        }
        private decimal CalculaTotalDesconto(IEnumerable<det> det)
        {
            return det.Where(item => item.prod.vDesc != null).Sum(item => item.prod.vDesc.Value);
        }
        private global::DFe.Classes.Flags.TipoAmbiente ObterAmbienteSefaz()
        {
            return _cupomFiscal.AmbienteSefaz == TipoAmbiente.Producao ? global::DFe.Classes.Flags.TipoAmbiente.Producao : global::DFe.Classes.Flags.TipoAmbiente.Homologacao;
        }
        private NFe.Classes.Informacoes.Identificacao.Tipos.TipoEmissao ObterTipoEmissao()
        {
            return _cupomFiscal.TipoEmissao == TipoEmissao.Normal ? NFe.Classes.Informacoes.Identificacao.Tipos.TipoEmissao.teNormal : NFe.Classes.Informacoes.Identificacao.Tipos.TipoEmissao.teOffLine;
        }
        private static void DadosObrigatorio(Destinatario destinatario)
        {
            if (destinatario.Cliente.GetDocumentoUnico().IsNullOrEmpty())
                throw new ArgumentException("CPF/CNPJ Obrigatório");

            if (destinatario.Endereco.Logradouro.IsNullOrEmpty())
                throw new ArgumentException("Logradouro obrigatório");

            if (destinatario.Endereco.Numero.IsNullOrEmpty())
                throw new ArgumentException("Número obrigatório");

            if (destinatario.Endereco.Bairro.IsNullOrEmpty())
                throw new ArgumentException("Bairro obrigatório");

            if (destinatario.Endereco.Cidade == null)
                throw new ArgumentException("Cidade obrigatório");
        }
    }
}