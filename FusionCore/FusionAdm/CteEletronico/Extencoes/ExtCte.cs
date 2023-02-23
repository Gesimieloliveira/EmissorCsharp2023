using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DFe.Ext;
using FusionCore.DFe.XmlCte;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.CteEletronico.Flags.Extencoes;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.AssemblyUtils;
using FusionCore.Helpers.AssemblyUtils.Leitura;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Flags;
using NHibernate.Util;
using DataHora = DFe.Utils.DataHora;
using TipoEmissao = FusionCore.FusionAdm.CteEletronico.Flags.TipoEmissao;

namespace FusionCore.FusionAdm.CteEletronico.Extencoes
{
    public static class ExtCte
    {
        public static FusionCTe ToCteEnvio(this Cte cte, CteEmissaoHistorico historico)
        {
            var fusionCte = new FusionCTe();

            switch (cte.TipoCte)
            {
                case TipoCte.Normal:
                    fusionCte.InformacoesCTe.InformacoesCTeNormal = new FusionInformacaoCTeNormalCTe();
                    break;
                case TipoCte.ComplementoDeValores:
                    fusionCte.InformacoesCTe.FusionInformacaoCTeComplementar = new FusionInformacaoCTeComplementar();
                    break;
                case TipoCte.CteDeAnulacao:
                    fusionCte.InformacoesCTe.FusionInfCteAnu = new FusionInfCteAnu
                    {
                        DeclaracaoEmitidaEm = cte.DeclaracaoEmitidaEm.ParaDataString(),
                        Chave = cte.ChaveCteAnulacao
                    };
                    break;
                case TipoCte.CteDeSubstituicao:
                    fusionCte.InformacoesCTe.InformacoesCTeNormal = new FusionInformacaoCTeNormalCTe
                    {
                        FusionInfCteSub = MontaCteDeSubstituicao(cte.CteSubstituicao)
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Identificacao(cte, fusionCte, historico);
            InformacoesComplementares(cte, fusionCte);
            Emitente(cte, fusionCte);
            Remetente(cte, fusionCte, historico);
            Destinatario(cte, fusionCte, historico);
            Expedidor(cte, fusionCte, historico);
            Recebedor(cte, fusionCte, historico);
            ValoresPrestacaoServico(cte, fusionCte);
            InformacoesCarga(cte, fusionCte);
            InfrmacoesDocumentos(cte, fusionCte);
            InformacoesComponentes(cte, fusionCte);
            InformacoesRodoviario(cte, fusionCte);
            InformacoesVeiculosTransportados(cte, fusionCte);
            InformacoesDocumentoAnterior(cte, fusionCte);
            InformacoesTributacao(cte, fusionCte);
            InformacoesCteComplementar(cte, fusionCte);


            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                if (new RepositorioResponsavelTecnico(sessao).ExisteResponsavelTecnico(cte.CteEmitente.Emitente.EstadoDTO.Id, TipoDocumentoFiscalEletronico.CTe))
                    InformacoesResponsavelTecnico(fusionCte);
            }


            fusionCte.InformacoesCTe.Imposto.TotalTributosFederais = cte.ValorTributoApoximado;
            ComputaObservacoesCte(cte, fusionCte);

            return fusionCte;
        }

        private static FusionInfCteSub MontaCteDeSubstituicao(CteSubstituicao cteSubstituicao)
        {
            var substituicao = new FusionInfCteSub
            {
                ChaveCTeDeAnulacao = cteSubstituicao.ChaveAnulacao.TrimSefazOrNull(44),
                ChaveCTeOriginal = cteSubstituicao.ChaveSubstituido.TrimSefazOrNull(44),
                FusionIndicadorAlteracaoTomador = null
            };

            if (cteSubstituicao.IsTemTomador())
            {
                substituicao.FusionTomaIcms = new FusionTomaICMS
                {
                    RefCte = cteSubstituicao.ChaveCtePeloTomador.TrimSefazOrNull(44),
                    RefNFe = cteSubstituicao.ChaveNfePeloTomador.TrimSefazOrNull(44)
                };
            }

            if (cteSubstituicao.IsTemReferenciaNF())
            {
                if (substituicao.FusionTomaIcms == null)
                {
                    substituicao.FusionTomaIcms = new FusionTomaICMS();
                }

                substituicao.FusionTomaIcms.FusionRefNF = new FusionRefNF
                {
                    Serie = cteSubstituicao.Serie.TrimSefazOrNull(3),
                    SubSerie = cteSubstituicao.Subserie.TrimSefazOrNull(3),
                    NumeroDocumentoFiscal = cteSubstituicao.NumeroDocumentoFiscal.TrimSefazOrNull(6),
                    EmitidoEm = cteSubstituicao.EmitidoEm.ParaDataString(),
                    FusionModeloDocumentoFiscal = cteSubstituicao.GetModelo().TrimSefazOrNull(2),
                    Valor = cteSubstituicao.Valor
                };

                if (cteSubstituicao.DocumentoUnico.Length == 11)
                {
                    substituicao.FusionTomaIcms.FusionRefNF.Cpf = cteSubstituicao.DocumentoUnico.TrimSefazOrNull(11);
                }

                if (cteSubstituicao.DocumentoUnico.Length == 14)
                {
                    substituicao.FusionTomaIcms.FusionRefNF.Cnpj = cteSubstituicao.DocumentoUnico.TrimSefazOrNull(14);
                }
            }
            

            return substituicao;
        }

        private static void InformacoesComponentes(Cte cte, FusionCTe fusionCte)
        {
            var componentes = new List<ComponenteValorPrestacao>();

            cte.CteComponenteValorPrestacaos.ForEach(comp =>
            {
                componentes.Add(new ComponenteValorPrestacao
                {
                    Nome = comp.Nome,
                    Valor = comp.Valor
                });
            });

            if (cte.CteComponenteValorPrestacaos.IsNotNull() && cte.CteComponenteValorPrestacaos.Count != 0)
            {
                fusionCte.InformacoesCTe.ValoresPrestacaoServico.ComponenteValorPrestacaos = componentes;
            }
        }

        private static void InformacoesCteComplementar(Cte cte, FusionCTe fusionCte)
        {
            if (cte.IsNormal()) return;
            if (IsCteAnulacao(cte)) return;

            fusionCte.InformacoesCTe.FusionInformacaoCTeComplementar = new FusionInformacaoCTeComplementar
            {
                ChaveCteComplementado = cte.ChaveCTeComplementado
            };
        }

        private static void InformacoesResponsavelTecnico(FusionCTe fusionCte)
        {
            fusionCte.InformacoesCTe.FusionInformacaoResponsabilidadeCTe = new FusionInformacaoResponsabilidadeCTe
            {
                CNPJ = ResponsavelLegal.Cnpj,
                email = ResponsavelLegal.Email,
                fone = ResponsavelLegal.Telefone,
                xContato = ResponsavelLegal.RazaoSocial
            };
        }

        private static void InformacoesTributacao(Cte cte, FusionCTe fusionCte)
        {
            var imposto = new FusionImpostoCTe();
            fusionCte.InformacoesCTe.Imposto = imposto;
            imposto.FusionIcmsCTe = new FusionIcmsCTe();

            var regimeTributario = cte.PerfilCte.EmissorFiscal.Empresa.RegimeTributario;

            if (regimeTributario == RegimeTributario.SimplesNacional)
            {
                imposto.FusionIcmsCTe.Icms = new FusionIcmsSimplesNacionalCTe();
                if (!cte.EstadoInicioOperacao.Sigla.Equals(cte.EstadoFinalOperacao.Sigla))
                    imposto.FusionIcmsUfFimCTe = new FusionIcmsUfFimCTe
                    {
                        vBCUFFim = 0,
                        pFCPUFFim = 0,
                        pICMSUFFim = 0,
                        pICMSInter = 0,
                        vFCPUFFim = 0,
                        vICMSUFFim = 0,
                        vICMSUFIni = 0
                    };
                return;
            }

            switch (cte.CteImpostoCst.TributacaoIcms.Codigo)
            {
                case "40":
                    imposto.FusionIcmsCTe.Icms = new FusionIcms45CTe
                    {
                        CST = 40
                    };
                    break;
                case "41":
                    imposto.FusionIcmsCTe.Icms = new FusionIcms45CTe
                    {
                        CST = 41
                    };
                    break;
                case "51":
                    imposto.FusionIcmsCTe.Icms = new FusionIcms45CTe
                    {
                        CST = 51
                    };
                    break;

                case "00":
                    imposto.FusionIcmsCTe.Icms = new FusionIcms00CTe
                    {
                        CST = "00",
                        pICMS = cte.CteImpostoCst.PercentualIcms,
                        vBC = cte.CteImpostoCst.BaseCalculoIcms,
                        vICMS = cte.CteImpostoCst.ValorIcms
                    };
                    break;

                case "20":
                    imposto.FusionIcmsCTe.Icms = new FusionIcms20CTe
                    {
                        CST = 20,
                        pICMS = cte.CteImpostoCst.PercentualIcms,
                        vBC = cte.CteImpostoCst.BaseCalculoIcms,
                        vICMS = cte.CteImpostoCst.ValorIcms,
                        pRedBC = cte.CteImpostoCst.PercentualReducaoBc
                    };
                    break;

                case "60":
                    imposto.FusionIcmsCTe.Icms = new FusionIcms60CTe
                    {
                        CST = 60,
                        vBCSTRet = cte.CteImpostoCst.BaseCalculoIcmsSt,
                        vCred = cte.CteImpostoCst.ValorCredito,
                        pICMSSTRet = cte.CteImpostoCst.PercentualIcmsSt,
                        vICMSSTRet = cte.CteImpostoCst.ValorIcmsSt
                    };
                    break;

                case "90":
                    imposto.FusionIcmsCTe.Icms = new FusionIcms90CTe
                    {
                        CST = 90,
                        pICMS = cte.CteImpostoCst.PercentualIcms,
                        vBC = cte.CteImpostoCst.BaseCalculoIcms,
                        vICMS = cte.CteImpostoCst.ValorIcms,
                        pRedBC = cte.CteImpostoCst.PercentualReducaoBc,
                        vCred = cte.CteImpostoCst.ValorCredito
                    };
                    break;
            }

            if (cte.CteImpostoDifal != null && cte.CteConfigImposto.IsPartilha)
            {
                imposto.FusionIcmsUfFimCTe = new FusionIcmsUfFimCTe
                {
                    vBCUFFim = cte.CteImpostoDifal.BaseCalculo,
                    pFCPUFFim = cte.CteImpostoDifal.PercentualFcp,
                    pICMSUFFim = cte.CteImpostoDifal.PercentualAliquotaInterna,
                    pICMSInter = cte.CteImpostoDifal.PercentualAliquotaInterestadual,
                    vFCPUFFim = cte.CteImpostoDifal.ValorIcmsFcp,
                    vICMSUFFim = cte.CteImpostoDifal.ValorIcmsUfTermino,
                    vICMSUFIni = cte.CteImpostoDifal.ValorIcmsUfInicio
                };
            }
        }

        private static void InformacoesDocumentoAnterior(Cte cte, FusionCTe fusionCte)
        {
            if (IsCteComplementoValores(cte)) return;
            if (IsCteAnulacao(cte)) return;

            if (!EnumerableExtensions.Any(cte.CteDocumentoAnteriores)) return;

            fusionCte.InformacoesCTe.InformacoesCTeNormal.FusionDocumentoAnterior =
                new FusionDocumentoAnterior {FusionEmiDocAnts = new List<FusionEmiDocAnt>()};

            cte.CteDocumentoAnteriores.ForEach(dt =>
            {
                var emiDocAnt = new FusionEmiDocAnt {FusionIdDocAnt = new List<FusionIdDocAnt>()};
                var idDocAnt = new FusionIdDocAnt
                {
                    FusionIdDocAntPaps = new List<FusionIdDocAntPap>(),
                    FusionIdDocAntEles = new List<FusionIdDocAntEle>()
                };
                emiDocAnt.FusionIdDocAnt.Add(idDocAnt);

                if (dt.DocumentoUnico.Length == 14)
                    emiDocAnt.Cnpj = dt.DocumentoUnico;

                if (dt.DocumentoUnico.Length == 11)
                    emiDocAnt.Cpf = dt.DocumentoUnico;

                emiDocAnt.IscricaoEstadual = dt.InscricaoEstadual;
                emiDocAnt.SiglaUF = dt.EstadoUf.Sigla;
                emiDocAnt.RazaoSocialOuNomeExpedidor = dt.NomeOuRazaoSocial;

                dt.Documentos.ToList().ForEach(
                    docCte =>
                    {
                        if (docCte.TipoDocumentoAnterior != TipoDocumentoAnterior.CTe)
                        {
                            var docPapel = new FusionIdDocAntPap
                            {
                                FusionTipoDocumentoTransAnt = docCte.TipoDocumentoAnterior.ToXml(),
                                Serie = docCte.Serie,
                                DataEmissao = docCte.EmissaoEm,
                                NumeroDocumentoFiscal = docCte.NumeroDocumentoFiscal,
                            };

                            if (docCte.SubSerie != 0)
                                docPapel.SubSerie = docCte.SubSerie;

                            idDocAnt.FusionIdDocAntPaps.Add(docPapel);
                        }

                        if (docCte.TipoDocumentoAnterior == TipoDocumentoAnterior.CTe)
                        {
                            var docAntEle = new FusionIdDocAntEle {Chave = docCte.ChaveCTe};


                            idDocAnt.FusionIdDocAntEles.Add(docAntEle);
                        }

                        
                    });

                fusionCte.InformacoesCTe.InformacoesCTeNormal.FusionDocumentoAnterior.FusionEmiDocAnts.Add(emiDocAnt);
            });
        }

        private static bool IsCteComplementoValores(Cte cte)
        {
            if (cte.TipoCte == TipoCte.ComplementoDeValores) return true;
            return false;
        }

        private static bool IsCteAnulacao(Cte cte)
        {
            if (cte.TipoCte == TipoCte.CteDeAnulacao) return true;
            return false;
        }

        private static void InformacoesVeiculosTransportados(Cte cte, FusionCTe fusionCte)
        {
            if (IsCteComplementoValores(cte)) return;
            if (IsCteAnulacao(cte)) return;

            var veiculoTransportados = cte.CteVeiculoTransportados;
            var veiculoTransportadosXml = fusionCte.InformacoesCTe.InformacoesCTeNormal.VeiculosTransportados;

            veiculoTransportados.ForEach(v =>
            {
                veiculoTransportadosXml.Add(new FusionVeiculoTransportadoCTe
                {
                    Chassi = v.Chassi.Trim(),
                    ValorUnitario = v.ValorUnitario,
                    CodigoMarcaModelo = v.CodigoMarcaModelo.Trim(),
                    DescricaoCor = v.DescricaoCor.Trim(),
                    FreteUnitario = v.FreteUnitario,
                    CodigoCor = v.Cor.Trim()
                });
            });
        }

        private static void Recebedor(Cte cte, FusionCTe fusionCte, CteEmissaoHistorico historico)
        {
            var cteRecebedor = cte.CteRecebedor;

            if (cteRecebedor == null) return;

            var re = new FusionRecebedorCTe();
            var r = cte.CteRecebedor.Recebedor;

            var documentoUnico = r.GetDocumentoUnico();

            if (documentoUnico.Length == 11) re.Cpf = documentoUnico;
            if (documentoUnico.Length == 14) re.Cnpj = documentoUnico;

            re.Ie = r.InscricaoEstadual.IsNullOrEmpty() ? null : r.InscricaoEstadual;

            re.Nome = r.Nome.TrimSefaz(60);

            if (historico.AmbienteSefaz == TipoAmbiente.Homologacao)
                re.Nome = @"CT-E EMITIDO EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";

            var enderecoPrincipal = r.GetEnderecoPrincipal();

            if (enderecoPrincipal != null)
            {
                var enderecoXml = re.Endereco;
                enderecoXml.Logradouro = enderecoPrincipal.Logradouro.TrimSefaz(60);
                enderecoXml.Numero = enderecoPrincipal.Numero.TrimSefaz(60);
                enderecoXml.Bairro = enderecoPrincipal.Bairro.TrimSefaz(60);
                enderecoXml.CodigoIbgeMunicipio = enderecoPrincipal.Cidade.CodigoIbge;
                enderecoXml.NomeMunicipio = enderecoPrincipal.Cidade.Nome.TrimSefaz(60);
                enderecoXml.Cep = enderecoPrincipal.Cep.IsNullOrEmpty() ? null : enderecoPrincipal.Cep;
                enderecoXml.SiglaUf = enderecoPrincipal.Cidade.SiglaUf;
            }

            var telefone = r.GetPrimeiroTelefone();
            if (telefone != null) re.Telefone = telefone.Numero;

            fusionCte.InformacoesCTe.Recebedor = re;
        }

        private static void Expedidor(Cte cte, FusionCTe fusionCte, CteEmissaoHistorico historico)
        {
            var cteExpedidor = cte.CteExpedidor;

            if (cteExpedidor == null) return;

            var re = new FusionExpedidorCTe();
            var r = cte.CteExpedidor.Expedidor;

            var documentoUnico = r.GetDocumentoUnico();

            if (documentoUnico.Length == 11) re.Cpf = documentoUnico;
            if (documentoUnico.Length == 14) re.Cnpj = documentoUnico;

            re.Ie = r.InscricaoEstadual.IsNullOrEmpty() ? null : r.InscricaoEstadual;

            re.Nome = r.Nome.TrimSefaz(60);

            if (historico.AmbienteSefaz == TipoAmbiente.Homologacao)
                re.Nome = @"CT-E EMITIDO EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";

            var enderecoPrincipal = r.GetEnderecoPrincipal();

            if (enderecoPrincipal != null)
            {
                var enderecoXml = re.Endereco;
                enderecoXml.Logradouro = enderecoPrincipal.Logradouro.TrimSefaz(60);
                enderecoXml.Numero = enderecoPrincipal.Numero.TrimSefaz(60);
                enderecoXml.Bairro = enderecoPrincipal.Bairro.TrimSefaz(60);
                enderecoXml.CodigoIbgeMunicipio = enderecoPrincipal.Cidade.CodigoIbge;
                enderecoXml.NomeMunicipio = enderecoPrincipal.Cidade.Nome.TrimSefaz(60);
                enderecoXml.Cep = enderecoPrincipal.Cep.IsNullOrEmpty() ? null : enderecoPrincipal.Cep;
                enderecoXml.SiglaUf = enderecoPrincipal.Cidade.SiglaUf;
            }

            var telefone = r.GetPrimeiroTelefone();
            if (telefone != null) re.Telefone = telefone.Numero.TrimSefaz(14);

            fusionCte.InformacoesCTe.Expedidor = re;
        }

        private static void InformacoesComplementares(Cte cte, FusionCTe fusionCte)
        {
            var dadosComplementares = fusionCte.InformacoesCTe.DadosComplementares;

            PreencheTipoPeriodoData(cte, dadosComplementares);
            PreencherTipoPeriodoHora(cte, dadosComplementares);
        }

        private static void InformacoesRodoviario(Cte cte, FusionCTe fusionCte)
        {
            if (IsCteComplementoValores(cte)) return;
            if (IsCteAnulacao(cte)) return;

            var xmlRodoviario = fusionCte.InformacoesCTe.InformacoesCTeNormal.Modal.Rodoviario;
            var objetoRodoviario = cte.CteRodoviario;

            xmlRodoviario.Rntrc = objetoRodoviario.Rntrc.IsNullOrEmpty() ? "ISENTO" : objetoRodoviario.Rntrc;
        }

        private static void InfrmacoesDocumentos(Cte cte, FusionCTe fusionCte)
        {
            if (IsCteComplementoValores(cte)) return;
            if (IsCteAnulacao(cte)) return;

            var documentosNfe = fusionCte.InformacoesCTe.InformacoesCTeNormal.InformacaoDocumento.DocumentosNFe;
            var documentosOutros = fusionCte.InformacoesCTe.InformacoesCTeNormal.InformacaoDocumento.DocumentosOutros;
            var documentosImpressos =
                fusionCte.InformacoesCTe.InformacoesCTeNormal.InformacaoDocumento.DocumentosImpressos;
            

            cte.CteDocumentoNfes.ForEach(nfe =>
            {
                var temPin = nfe.PinSuframa != 0;

                var documentoNfe = new FusionDocumentosNFe
                {
                    Chave = nfe.Chave,
                    DataPrevistaEntrega = nfe.PrevisaoEntregaEm?.ToString("yyyy-MM-dd"),
                    PinSuframa = temPin ? nfe.PinSuframa.ToString() : null
                };

                documentosNfe.Add(documentoNfe);
            });


            cte.CteDocumentoOutros.ForEach(o =>
            {
                documentosOutros.Add(new FusionDocuemntoOutroCTe
                {
                    DataEmissao = o.EmitidoEm,
                    Valor = o.Valor == 0 ? (decimal?) null : decimal.Parse(o.Valor.ToString("N2")),
                    Numero = o.Numero.TrimOrNull(),
                    DescricaoOutros = o.DescricaoOutro.TrimOrNull(),
                    DataPrevisaoEntrega = o.PrevisaoEntregaEm,
                    TipoDocumentoOriginario = o.TipoDocumento.ToXml()
                });
            });

            cte.CteDocumentoImpressos.ForEach(di =>
            {
                var temPin = di.PinSuframa != 0;
                var temPeso = di.TotalPesoKg != 0;

                documentosImpressos.Add(new FusionDocumentoImpressoCTe
                {
                    PinSuframa = temPin ? di.PinSuframa.ToString() : null,
                    Numero = di.Numero,
                    Serie = di.Serie,
                    NumeroRomaneiro = di.NumeroRomaneiro.TrimOrNull(),
                    DataEmissao = di.EmitidaEm,
                    ModeloNotaFiscal = di.ModeloNotaFiscal.ToXml(),
                    NumeroPedido = di.NumeroPedido.TrimOrNull(),
                    BaseCalculoIcmsSt = di.BaseCalculoIcmsSt,
                    PesoTotalEmKg = temPeso ? di.TotalPesoKg : (decimal?) null,
                    BaseCacluloIcms = di.BaseCalculoIcms,
                    CfopPredominante = di.PerfilCfop.Cfop.Id,
                    DataPrevista = di.PrevisaoEntregaEm,
                    IcmsStTotal = di.TotalBaseCalculoIcmsSt,
                    ValorTotalNF = di.TotalNf,
                    IcmsTotal = di.TotalBaseCalculoIcms,
                    ValorTotalProdutos = di.TotalProdutos
                });
            });
        }

        private static void InformacoesCarga(Cte cte, FusionCTe fusionCte)
        {
            if (IsCteComplementoValores(cte)) return;
            if (IsCteAnulacao(cte)) return;

            var infCarga = fusionCte.InformacoesCTe.InformacoesCTeNormal.InformacaoCarga;

            infCarga.ValorTotalCarga = cte.ValorTotalCarga;
            infCarga.ValorAverbacao = cte.ValorAverbacao;
            infCarga.NomeProdutoPredominante = cte.NomeProdutoPredominante.Trim();

            var naoAdicionaOutraCaracteristicas = cte.CaracteristicaProdutoPredominante.IsNullOrEmpty();
            infCarga.OutrasCaracteristicas = naoAdicionaOutraCaracteristicas
                ? null
                : cte.CaracteristicaProdutoPredominante;

            var infoQtdCargas = infCarga.InformacoesQuantidadeDaCarga;

            cte.CteInfoQuantidadeCargas.ForEach(c =>
            {
                var cargaCTe = new FusionInformacoesQuantidadeDaCargaCTe
                {
                    DescricaoMedida = c.TipoUnidadeMedidaDescricao.Trim(),
                    Quantidade = c.Quantidade,
                    UnidadeMedida = c.UnidadeMedida.ToXml()
                };

                infoQtdCargas.Add(cargaCTe);
            });
        }

        private static void ValoresPrestacaoServico(Cte cte, FusionCTe fusionCte)
        {
            var valoresXml = fusionCte.InformacoesCTe.ValoresPrestacaoServico;
            valoresXml.ValorAReceber = cte.ValorReceber;
            valoresXml.ValorTotal = cte.ValorServico;
        }

        private static void Destinatario(Cte cte, FusionCTe fusionCte, CteEmissaoHistorico historico)
        {
            var de = fusionCte.InformacoesCTe.Destinatario;
            var d = cte.CteDestinatario.Destinatario;

            var documentoUnico = d.GetDocumentoUnico();

            if (documentoUnico.Length == 11) de.Cpf = documentoUnico;
            if (documentoUnico.Length == 14) de.Cnpj = documentoUnico;

            de.Ie = d.InscricaoEstadual.IsNullOrEmpty() ? null : d.InscricaoEstadual;

            de.Nome = d.Nome.TrimSefaz(60);

            if (historico.AmbienteSefaz == TipoAmbiente.Homologacao)
                de.Nome = "CT-E EMITIDO EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";

            var enderecoPrincipal = d.GetEnderecoPrincipal();
            
            if (enderecoPrincipal != null)
            {
                var enderecoXml = de.Endereco;
                enderecoXml.Logradouro = enderecoPrincipal.Logradouro.TrimSefaz(60);
                enderecoXml.Numero = enderecoPrincipal.Numero.TrimSefaz(60);
                enderecoXml.Bairro = enderecoPrincipal.Bairro.TrimSefaz(60);
                enderecoXml.CodigoIbgeMunicipio = enderecoPrincipal.Cidade.CodigoIbge;
                enderecoXml.NomeMunicipio = enderecoPrincipal.Cidade.Nome.TrimSefaz(60);
                enderecoXml.Cep = enderecoPrincipal.Cep.IsNullOrEmpty() ? null : enderecoPrincipal.Cep;
                enderecoXml.SiglaUf = enderecoPrincipal.Cidade.SiglaUf;
            }

            var telefone = d.GetPrimeiroTelefone();
            if (telefone != null) de.Telefone = telefone.Numero.TrimSefaz(14);
        }

        private static void Remetente(Cte cte, FusionCTe fusionCte, CteEmissaoHistorico historico)
        {
            var re = fusionCte.InformacoesCTe.Remetente;
            var r = cte.CteRemetente.Remetente;

            var documentoUnico = r.GetDocumentoUnico();

            if (documentoUnico.Length == 11) re.Cpf = documentoUnico;
            if (documentoUnico.Length == 14) re.Cnpj = documentoUnico;

            re.Ie = r.InscricaoEstadual.IsNullOrEmpty() ? null : r.InscricaoEstadual;

            re.Nome = r.Nome;

            if (historico.AmbienteSefaz == TipoAmbiente.Homologacao)
                re.Nome = @"CT-E EMITIDO EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";

            var enderecoPrincipal = r.GetEnderecoPrincipal();

            if (enderecoPrincipal != null)
            {
                var enderecoXml = re.Endereco;
                enderecoXml.Logradouro = enderecoPrincipal.Logradouro.TrimSefaz(60);
                enderecoXml.Numero = enderecoPrincipal.Numero.TrimSefaz(60);
                enderecoXml.Bairro = enderecoPrincipal.Bairro.TrimSefaz(60);
                enderecoXml.CodigoIbgeMunicipio = enderecoPrincipal.Cidade.CodigoIbge;
                enderecoXml.NomeMunicipio = enderecoPrincipal.Cidade.Nome.TrimSefaz(60);
                enderecoXml.Cep = enderecoPrincipal.Cep.IsNullOrEmpty() ? null : enderecoPrincipal.Cep;
                enderecoXml.SiglaUf = enderecoPrincipal.Cidade.SiglaUf;
            }

            var telefone = r.GetPrimeiroTelefone();
            if (telefone != null) re.Telefone = telefone.Numero.TrimSefaz(14);
        }

        private static void Emitente(Cte cte, FusionCTe fusionCte)
        {
            var emitente = fusionCte.InformacoesCTe.Emitente;


            var documentoUnico = cte.PerfilCte.EmissorFiscal.Empresa.DocumentoUnico.PadLeft(14, '0');

            emitente.Cnpj = documentoUnico;

            emitente.Ie = cte.PerfilCte.EmissorFiscal.Empresa.InscricaoEstadual.TrimSefaz(14);

            emitente.RazaoSocialOuNome = cte.PerfilCte.EmissorFiscal.Empresa.RazaoSocial.TrimSefaz(60);
            emitente.NomeFantasia = cte.PerfilCte.EmissorFiscal.Empresa.NomeFantasia.TrimSefaz(60);


            var empresa = cte.PerfilCte.EmissorFiscal.Empresa;
            var endereco = emitente.Endereco;
            endereco.Logradouro = empresa.Logradouro.TrimSefaz(60);
            endereco.Numero = empresa.Numero.TrimSefaz(60); 
            endereco.Complemento = empresa.Complemento.IsNullOrEmpty() ? null : empresa.Complemento.TrimSefaz(60);
            endereco.Bairro = empresa.Bairro.TrimSefaz(60);
            endereco.CodigoIbgeMunicipio = empresa.CidadeDTO.CodigoIbge;
            endereco.NomeMunicipio = empresa.CidadeDTO.Nome.TrimSefaz(60);
            endereco.Cep = empresa.Cep;
            endereco.SiglaUf = empresa.CidadeDTO.SiglaUf;
            endereco.Telefone = empresa.Fone1?.TrimSefaz(14);
        }

        private static void Identificacao(Cte cte, FusionCTe fusionCte, CteEmissaoHistorico historico)
        {
            fusionCte.InformacoesCTe.Id = $"CTe{historico.Chave}"; 
            fusionCte.InformacoesCTe.Versao = "3.00";

            var identificacao = fusionCte.InformacoesCTe.Identificacao;

            identificacao.Globalizado = cte.Globalizado;
            identificacao.EstadoUF = cte.PerfilCte.EmissorFiscal.Empresa.EstadoDTO.ToXml();
            identificacao.CodigoNumerico = cte.CodigoNumericoEmissao.ToString("D8");
            identificacao.Cfop = cte.PerfilCfop.Cfop.Id;
            identificacao.NaturezaOperacao = cte.NaturezaOperacao;
            identificacao.TipoDocumentoFiscal = FusionTipoDocumentoFiscalCTe.CTe;
            identificacao.Serie = cte.SerieEmissao;
            identificacao.NumeroDocumento = cte.NumeroFiscalEmissao;
            // ReSharper disable once PossibleInvalidOperationException
            identificacao.EmitidaEm = DataHora.ParaDataHoraStringUtc(cte.EmissaoEm);
            identificacao.FusionDacteCTe = FusionDacteCTe.Retrato;
            identificacao.FusionTipoEmissaoCTe = cte.TipoEmissao.ToXml(cte.CteEmitente.Emitente.EstadoDTO);
            identificacao.DigitoVerificador = historico.ObterDigitoVerificador();
            identificacao.Ambiente = historico.AmbienteSefaz.ToXml();
            identificacao.TipoCTe = cte.TipoCte.ToXml();
            identificacao.TipoProcessoEmissao = FusionTipoProcessoEmissaoCTe.EmissaoAplicativoContribuinte;
            identificacao.VersaoAplicativoEmissor = AssemblyHelper.LerDoAssemblyPrincipal(new Versao3Digito());
            identificacao.CodigoIbgeMunicipioEnvio = cte.PerfilCte.EmissorFiscal.Empresa.CidadeDTO.CodigoIbge;
            identificacao.NomeMunicipioEnvio = cte.PerfilCte.EmissorFiscal.Empresa.CidadeDTO.Nome;
            identificacao.SiglaDeEnvioUF = cte.PerfilCte.EmissorFiscal.Empresa.CidadeDTO.SiglaUf;
            identificacao.Modal = cte.Modal.ToXml();
            identificacao.TipoServico = cte.TipoServico.ToXml();
            identificacao.CodigoIbgeMunicipioInicioOperacao = cte.CidadeInicioOperacao.CodigoIbge;
            identificacao.NomeMunicipioInicioOperacao = cte.CidadeInicioOperacao.Nome;
            identificacao.SiglaDeUfInicioOperacao = cte.EstadoInicioOperacao.Sigla;
            identificacao.CodigoIbgeMunicipioFimOperacao = cte.CidadeFinalOperacao.CodigoIbge;
            identificacao.NomeMunicipioFimOperacao = cte.CidadeFinalOperacao.Nome;
            identificacao.SiglaDeUfFimOperacao = cte.EstadoFinalOperacao.Sigla;
            identificacao.IndicadorRecebedor = FusionIndicadorRecebedorCTe.Nao;

            identificacao.FusionTomadorOutrosCTe = cte.CteTomador.OutrosToXml(cte.TipoTomador);
            identificacao.IndicadorPapelTomador = cte.CteTomador.TomadorXml(cte.TipoTomador);

            var tomadorOutros = identificacao.FusionTomadorOutrosCTe;
            if (tomadorOutros != null)
            {
                var ie = tomadorOutros.IE;

                ResolveIndicadorIcmsTomador(ie, identificacao);
            }

            var papelTomador = identificacao.IndicadorPapelTomador;
            if (papelTomador != null)
            {
                PessoaEntidade tomador;
                switch (papelTomador.TipoTomador)
                {
                    case FusionTipoTomadorCTe.Remetente:
                        tomador = cte.CteRemetente.Remetente;
                        break;
                    case FusionTipoTomadorCTe.Expedidor:
                        tomador = cte.CteExpedidor.Expedidor;
                        break;
                    case FusionTipoTomadorCTe.Recebedor:
                        tomador = cte.CteRecebedor.Recebedor;
                        break;
                    case FusionTipoTomadorCTe.Destinatario:
                        tomador = cte.CteDestinatario.Destinatario;
                        break;
                    case FusionTipoTomadorCTe.Outro:
                        tomador = cte.CteTomador.Tomador;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var ie = tomador.InscricaoEstadual;

                ResolveIndicadorIcmsTomador(ie, identificacao);
            }
        }

        private static void ResolveIndicadorIcmsTomador(string ie, FusionIdentificacaoCTe identificacao)
        {
            if (ie.IsNotNullOrEmpty() && ie.Equals("ISENTO"))
            {
                identificacao.FusionIndicadorIcmsTomador = FusionIndicadorIcmsTomador.ContribuienteIsentoDeIE;
                return;
            }

            identificacao.FusionIndicadorIcmsTomador = ie.IsNotNullOrEmpty()
                ? FusionIndicadorIcmsTomador.ContribuinteIcms
                : FusionIndicadorIcmsTomador.NaoContribuinte;
        }

        private static void PreencheTipoPeriodoData(Cte cte, FusionDadosComplementaresCTe dadosComplementares)
        {
            switch (cte.TipoPeriodoData)
            {
                case TipoPeriodoData.SemDataDefinida:
                    dadosComplementares.Entrega.SemData = new FusionSemDataCTe();
                    break;
                case TipoPeriodoData.NaData:
                case TipoPeriodoData.AteAData:
                case TipoPeriodoData.APartirDaData:
                    if (cte.DataInicio != null)
                        dadosComplementares.Entrega.ComData = new FusionComDataCTe
                        {
                            TipoPeriodoData = cte.TipoPeriodoData.ToXml(),
                            DataProgramada = ((DateTime) cte.DataInicio).ToString("yyyy-MM-dd")
                        };
                    break;
                case TipoPeriodoData.NoPeriodo:
                    if (cte.DataFinal != null)
                        if (cte.DataInicio != null)
                            dadosComplementares.Entrega.NoPeriodo = new FusionNoPeriodoCTe
                            {
                                DataFinal = ((DateTime) cte.DataFinal).ToString("yyyy-MM-dd"),
                                DataInicial = ((DateTime) cte.DataInicio).ToString("yyyy-MM-dd")
                            };
                    break;
            }
        }

        private static void PreencherTipoPeriodoHora(Cte cte, FusionDadosComplementaresCTe dadosComplementares)
        {
            switch (cte.TipoPeriodoHora)
            {
                case TipoPeriodoHora.SemHoraDefinida:
                    dadosComplementares.Entrega.SemHora = new FusionSemHoraCTe();
                    break;
                case TipoPeriodoHora.NoHorario:
                case TipoPeriodoHora.AteOHorario:
                case TipoPeriodoHora.APartirDoHorario:
                    if (cte.HoraInicio != null)
                        dadosComplementares.Entrega.ComHora = new FusionComHoraCTe
                        {
                            TipoPeriodoHora = cte.TipoPeriodoHora.ToXml(),
                            HoraProgramada = ((TimeSpan) cte.HoraInicio).ToString(@"hh\:mm\:ss")
                        };
                    break;
                case TipoPeriodoHora.NoIntervaloDeTempo:
                    if (cte.HoraFinal != null)
                        if (cte.HoraInicio != null)
                            dadosComplementares.Entrega.IntervaloHora = new FusionIntervaloHora
                            {
                                HoraFinal = ((TimeSpan) cte.HoraFinal).ToString(@"hh\:mm\:ss"),
                                HoraInicial = ((TimeSpan) cte.HoraInicio).ToString(@"hh\:mm\:ss")
                            };
                    break;
            }
        }


        private static void ComputaObservacoesCte(Cte cte, FusionCTe fusionCte)
        {
            var obsCte = new StringBuilder(cte.Observacao.TrimSefaz());

            if (cte.TipoEmissao == TipoEmissao.Contingencia)
            {
                obsCte.Append(obsCte.Length > 0 ? ";" : string.Empty);
                obsCte.Append($"Emitida em contingência em: {cte.EmissaoEm:G}");
            }

            if (cte.CteConfigImposto != null && cte.CteConfigImposto.IsPartilha)
            {
                obsCte.Append(obsCte.Length > 0 ? ";" : "");
                obsCte.Append(cte.CteImpostoDifal.Observacao.TrimSefaz());
            }

            if (!string.IsNullOrWhiteSpace(cte.CodigoIbpt))
            {
                obsCte.Append(obsCte.Length > 0 ? ";" : "");
                obsCte.Append(cte.ComputaTextoDeOlhoNoImposto());
            }

            var obs = obsCte.ToString();

            if (string.IsNullOrWhiteSpace(obs))
            {
                return;
            }

            fusionCte.InformacoesCTe.DadosComplementares.ObservacoesGerais = obsCte.ToString();
        }
    }
}