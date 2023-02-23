using System;
using System.Collections.Generic;
using System.Linq;
using DFe.Classes.Entidades;
using FusionCore.Core.Flags;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Financeiro.Extencoes;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Autorizacao;
using FusionCore.FusionAdm.Fiscal.NF.Integridade;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Cobranca;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Observacoes;
using NFe.Classes.Informacoes.Pagamento;
using NFe.Classes.Informacoes.Total;
using NFe.Classes.Informacoes.Transporte;
using NHibernate.Util;
using Shared.NFe.Classes.Informacoes.InfRespTec;
using DestinoOperacao = NFe.Classes.Informacoes.Identificacao.Tipos.DestinoOperacao;
using FormaPagamento = NFe.Classes.Informacoes.Pagamento.FormaPagamento;
using InvalidOperationException = System.InvalidOperationException;
using ProcessoEmissao = NFe.Classes.Informacoes.Identificacao.Tipos.ProcessoEmissao;
using ZeusNFe = NFe.Classes.NFe;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores
{
    public static class ConversorNfeParaZeus
    {
        public static ZeusNFe ToZeus(this Nfeletronica nfe, IEmissaoXml emissao, ISessaoManager sessaoManager)
        {
            var zeusNfe = new ZeusNFe
            {
                infNFe = GetInfNFe(nfe, emissao, sessaoManager)
            };

            using (var sessao = sessaoManager.CriaSessao())
            {
                if (new RepositorioResponsavelTecnico(sessao).ExisteResponsavelTecnico(
                        nfe.Emitente.Empresa.EstadoDTO.Id,
                        TipoDocumentoFiscalEletronico.NFe))
                    InformacoesResponsavelTecnico(zeusNfe);
            }


            if (!nfe.IncluirInformacaoIbpt)
            {
                return zeusNfe;
            }

            if (zeusNfe.infNFe.ide.indFinal == ConsumidorFinal.cfNao)
            {
                return zeusNfe;
            }

            var deOlhoNoImposto = new DeOlhoNoImposto(sessaoManager);
            deOlhoNoImposto.SetarNfe(zeusNfe);
            deOlhoNoImposto.IncluirTributosAproximados();

            return deOlhoNoImposto.GetNfe();
        }

        private static void InformacoesResponsavelTecnico(ZeusNFe zeusNfe)
        {
            zeusNfe.infNFe.infRespTec = new infRespTec
            {
                CNPJ = ResponsavelLegal.Cnpj,
                email = ResponsavelLegal.Email,
                fone = ResponsavelLegal.Telefone,
                xContato = ResponsavelLegal.RazaoSocial
            };
        }

        private static infNFe GetInfNFe(Nfeletronica nfe, IEmissaoXml emissao, ISessaoManager sessaoManager)
        {
            var emissor = nfe.Emitente.CarregarDadosEmissor(sessaoManager);

            var infNfe = new infNFe
            {
                ide = GetIdentificador(nfe, emissao),
                emit = nfe.Emitente.ToZeus(),
                dest = nfe.Destinatario.ToZeus(),
                det = GetDetalhe(nfe.Itens),
                transp = GetTransporte(nfe),
                exporta = GetExportacao(nfe),
                Id = emissao.TagId,
                versao = emissao.VersaoDocumento,
                total = GetTotal(nfe),
                infAdic = GetInformacaoAdicional(nfe),
                autXML = GetAutorizadoABaixarXml(emissor.AutorizadoBaixarXml),
                entrega = GetEntrega(nfe.LocalEntrega, nfe.Destinatario)
            };

            MontarCobranca(nfe, infNfe);
            MontarPagamento(nfe, infNfe);

            if (emissao.Ambiente == TipoAmbiente.Homologacao)
            {
                infNfe.dest.xNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }

            return infNfe;
        }

        private static entrega GetEntrega(LocalEntrega localEntrega, DestinatarioNfe destinatario)
        {
            if (localEntrega == null || destinatario.ResideExterior()) return null;

            var endereco = localEntrega.Endereco;

            var entrega = new entrega
            {
                xNome = destinatario.Nome?.TrimSefazOrNull(60),
                xLgr = endereco.Logradouro.TrimSefaz(60),
                nro = endereco.Numero.TrimSefaz(60),
                xCpl = endereco.Complemento?.TrimSefazOrNull(60),
                xBairro = endereco.Bairro.TrimSefaz(60),
                cMun = endereco.Cidade.CodigoIbge,
                xMun = endereco.Cidade.Nome.TrimSefaz(60),
                UF = endereco.Cidade.SiglaUf,
                CEP = Convert.ToInt64(endereco.Cep)
            };

            if (destinatario.DocumentoUnico.Length == 14)
            {
                entrega.CNPJ = destinatario.DocumentoUnico;
                return entrega;
            }

            entrega.CPF = destinatario.DocumentoUnico;
            return entrega;
        }

        private static List<autXML> GetAutorizadoABaixarXml(AutorizadoBaixarXml autorizadoBaixarXml)
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

        private static void MontarCobranca(Nfeletronica nfe, infNFe infNfe)
        {
            if (!nfe.IncluiCobrancaNoXml || nfe.Pagamentos.All(pg => pg.PossuiParcelamento == false))
            {
                return;
            }

            var pgCrediario = nfe.Pagamentos.SingleOrDefault(pg => pg.PossuiParcelamento);

            if (pgCrediario == null)
            {
                return;
            }

            var fat = new fat
            {
                nFat = $"NF-e {nfe.NumeroDocumento}-{nfe.SerieEmissao}",
                vLiq = pgCrediario.Valor,
                vOrig = pgCrediario.Valor,
                vDesc = 0.0m
            };

            var cobranca = new cobr
            {
                fat = fat
            };

            var duplicatas = pgCrediario.Parcelas;
            var dupZeus = new List<dup>();

            duplicatas.ForEach(d =>
            {
                var dup = new dup
                {
                    dVenc = d.Vencimento,
                    nDup = d.Numero.ToString("000"),
                    vDup = d.Valor
                };

                dupZeus.Add(dup);
            });

            cobranca.dup = dupZeus;

            infNfe.cobr = cobranca;
        }

        private static void MontarPagamento(Nfeletronica nfe, infNFe infNfe)
        {
            var zPag = new pag { detPag = new List<detPag>() };

            if (nfe.SemPagamento
                || nfe.FinalidadeEmissao == FinalidadeEmissao.Ajuste
                || nfe.FinalidadeEmissao == FinalidadeEmissao.Devolucao)
            {
                zPag.detPag.Add(new detPag
                {
                    tPag = FormaPagamento.fpSemPagamento,
                    vPag = 0.0m
                });

                infNfe.pag = new List<pag> { zPag };

                return;
            }

            foreach (var pgNfe in nfe.Pagamentos)
            {
                var detPag = new detPag
                {
                    indPag = pgNfe.Especie.ToZeusIndicadorPg(),
                    tPag = pgNfe.Especie.ToZeusPagamento(),
                    vPag = pgNfe.Valor
                };

                if (pgNfe.Especie.EhCartao())
                {
                    detPag.card = new card
                    {
                        tpIntegra = TipoIntegracaoPagamento.TipNaoIntegrado,
                        tBand = BandeiraCartao.bcOutros
                    };
                }

                //TODO: Remover utilização de FormaPagamento do TipoDocumento na conversão de detPag
                if (pgNfe.Especie == ETipoPagamento.CreditoLoja)
                {
                    detPag.tPag = pgNfe.TipoDocumento?.FormaPagamento.ToZeusNfe() ?? FormaPagamento.fpCreditoLoja;

                    if (pgNfe.TipoDocumento?.FormaPagamento.ToZeusNfe() == FormaPagamento.fpOutro)
                    {
                        detPag.xPag = pgNfe.TipoDocumento?.Descricao;
                    }
                }

                zPag.detPag.Add(detPag);
            }

            infNfe.pag = new List<pag> { zPag };
        }

        private static infAdic GetInformacaoAdicional(Nfeletronica nfe)
        {
            return new infAdic
            {
                infCpl = nfe.ComputaInformacaoAdicional().TrimSefaz(),
                infAdFisco = nfe.ComputaInformacaoAdicionalFisco().TrimSefazOrNull()
            };
        }

        private static ide GetIdentificador(Nfeletronica nfe, IEmissaoXml emissao)
        {
            if (nfe.Destinatario.IndicadorDestinoOperacao == null)
                throw new InvalidOperationException("Indicador destino operação não pode ser nulo.");
            if (nfe.Destinatario.IndicadorOperacaoFinal == null)
                throw new InvalidOperationException("Indicador Operação Final não pode ser nulo.");
            if (nfe.Destinatario.IndicadorPresenca == null)
                throw new InvalidOperationException("Indicador Operação Final não pode ser nulo.");

            var ide = new ide
            {
                cDV = emissao.Chave.Dv,
                dhEmi = nfe.EmitidaEm,
                dhSaiEnt = nfe.SaidaEm,
                serie = nfe.SerieEmissao,
                cUF = (Estado)nfe.Emitente.Empresa.EstadoDTO.CodigoIbge,
                tpEmis = emissao.TipoEmissao.ToZeus(),
                cNF = emissao.Chave.GetCodigoNumerico().ToString("D8"),
                mod = nfe.Modelo.ToZeus(),
                nNF = nfe.NumeroEmissao,
                cMunFG = nfe.Emitente.Empresa.CidadeDTO.CodigoIbge,
                finNFe = nfe.FinalidadeEmissao.ToZeus(),
                idDest = (DestinoOperacao)nfe.Destinatario.IndicadorDestinoOperacao,
                indFinal = (ConsumidorFinal)nfe.Destinatario.IndicadorOperacaoFinal,
                indPres = (PresencaComprador)nfe.Destinatario.IndicadorPresenca,
                natOp = nfe.NaturezaOperacao.Trim(),
                procEmi = ProcessoEmissao.peAplicativoContribuinte,
                tpAmb = emissao.Ambiente.ToZeus(),
                tpImp = nfe.TipoDanfe.ToZeus(),
                tpNF = nfe.TipoOperacao.ToZeus(),
                verProc = "FUSION NF-E",
                NFref = GetDocumentosReferenciados(nfe)
            };

            if (!emissao.ContingenciaAtivada())
            {
                return ide;
            }

            ide.xJust = emissao.MotivoContingencia;
            ide.dhCont = (DateTime)emissao.InicioContingencia;

            return ide;
        }

        private static List<NFref> GetDocumentosReferenciados(Nfeletronica nfe)
        {
            if (!nfe.Referencias.Any() && !nfe.ReferenciasCf.Any())
                return null;

            var zref = new List<NFref>();

            nfe.Referencias.ForEach(r => zref.Add(CriaReferenciaNfZeus(r)));
            nfe.ReferenciasCf.ForEach(r => zref.Add(CriaReferenciaCfZeus(r)));

            return zref;
        }

        private static NFref CriaReferenciaNfZeus(ReferenciaNfe referencia)
        {
            return new NFref
            {
                refNFe = referencia.ChaveReferenciada.TrimSefaz()
            };
        }

        private static NFref CriaReferenciaCfZeus(ReferenciaCf referenciaCf)
        {
            var cf = new refECF
            {
                mod = referenciaCf.ModeloCupom.TrimSefaz(),
                nCOO = referenciaCf.NumeroCoo,
                nECF = referenciaCf.NumeroEcf
            };

            return new NFref { refECF = cf };
        }

        private static transp GetTransporte(Nfeletronica nfe)
        {
            return nfe.Transportadora.ToZeus(nfe);
        }

        private static exporta GetExportacao(Nfeletronica nfe)
        {
            if (nfe.Destinatario.IndicadorDestinoOperacao != Fiscal.Flags.DestinoOperacao.Exterior
                || nfe.Exportacao == null)
            {
                return null;
            }

            return new exporta
            {
                UFSaidaPais = nfe.Exportacao?.UfSaidaPais,
                xLocDespacho = nfe.Exportacao?.LocalDespacho.TrimSefaz(),
                xLocExporta = nfe.Exportacao?.LocalEmbarque.TrimSefaz()
            };
        }

        private static List<det> GetDetalhe(IEnumerable<ItemNfe> itens)
        {
            var itensOrdenados = itens.OrderBy(e => e.NumeroItem);

            return (from itemFiscal in itensOrdenados
                let ipi = itemFiscal.Ipi
                let pis = itemFiscal.Pis
                let cofins = itemFiscal.Cofins
                select new det
                {
                    nItem = itemFiscal.NumeroItem,
                    prod = itemFiscal.ToZeus(),
                    infAdProd = ComputarObsAdicionalItem(itemFiscal),
                    imposto = new imposto
                    {
                        ICMS = itemFiscal.ImpostoIcms.ToZeus(),
                        IPI = ipi.ToZeus(itemFiscal.UsarIpiTagPropria),
                        PIS = pis.ToZeus(),
                        COFINS = cofins.ToZeus(),
                        ICMSUFDest = CriaIcmsInterstadual(itemFiscal)
                    },
                    impostoDevol = ObterImpostoDevolucao(itemFiscal.UsarIpiTagPropria, ipi)
                }).ToList();
        }

        private static impostoDevol ObterImpostoDevolucao(bool usarTagPropria, ImpostoIpi impostoIpi)
        {
            if (usarTagPropria == false) return null;

            return new impostoDevol
            {
                IPI = new IPIDevolvido
                {
                    vIPIDevol = impostoIpi.ValorIpi
                },
                pDevol = impostoIpi.AliquotaIpi
            };
        }

        private static string ComputarObsAdicionalItem(ItemNfe itemFiscal)
        {
            //TODO: Adicionar ANP e UF de CONSUMO após retirar ACBr Monitor como impressor.

            var obsAdicional = new List<string>();

            if (itemFiscal.HasObservacao)
            {
                obsAdicional.Add(itemFiscal.Observacao.Trim());
            }

            var permiteFcp = itemFiscal.ImpostoIcms.Cst.PermiteFcp();
            var permiteFcpSt = itemFiscal.ImpostoIcms.Cst.PermiteFcpSt();

            if (permiteFcp && itemFiscal.ImpostoIcms.AliquotaFcp > 0)
            {
                var ofcp = $"pFCP={itemFiscal.ImpostoIcms.AliquotaFcp:N4}" +
                           $", vBCFCP={itemFiscal.ImpostoIcms.ValorBcFcp:N2}" +
                           $", vFCP={itemFiscal.ImpostoIcms.ValorFcp:N2}";

                obsAdicional.Add(ofcp);
            }

            if (permiteFcpSt && itemFiscal.ImpostoIcms.AliquotaFcpSt > 0)
            {
                var ofcp = $"pFCPST={itemFiscal.ImpostoIcms.AliquotaFcpSt:N4}" +
                           $", vBCFCPST={itemFiscal.ImpostoIcms.ValorBcFcpSt:N2}" +
                           $", vFCPST={itemFiscal.ImpostoIcms.ValorFcpSt:N2}";

                obsAdicional.Add(ofcp);
            }

            if (itemFiscal.AutoAtivarCreditoItem && (itemFiscal.ImpostoIcms.AliquotaCredito != 0 ||
                                                     itemFiscal.ImpostoIcms.ValorCredito != 0))
            {
                var creditoIcmsObs = $"Aliq. Crédito={itemFiscal.ImpostoIcms.AliquotaCredito:N4}%, Valor Credito={itemFiscal.ImpostoIcms.ValorCredito:N2}";

                obsAdicional.Add(creditoIcmsObs);
            }

            if (itemFiscal.Nfe.ContemIpiDevolucao()
                && (itemFiscal.Ipi.AliquotaIpi != 0
                    || itemFiscal.Ipi.ValorIpi != 0))
            {
                var ipiObs = $"Aliq. Ipi={itemFiscal.Ipi.AliquotaIpi:N4}%, Valor Ipi={itemFiscal.Ipi.ValorIpi:N2}";

                obsAdicional.Add(ipiObs);
            }

            if (itemFiscal.Produto.UsarObservacaoNoItemFiscal)
            {
                var observacao = itemFiscal.Produto.Observacao.TrimOrEmpty().RemoverTeclaEnter();

                obsAdicional.Add(observacao);
            }

            return obsAdicional.Count > 0
                ? string.Join(" - ", obsAdicional)
                : null;
        }

        private static ICMSUFDest CriaIcmsInterstadual(ItemNfe itemFiscal)
        {
            if (itemFiscal.IcmsInterstadual == null)
            {
                return null;
            }

            return new ICMSUFDest
            {
                vBCUFDest = itemFiscal.IcmsInterstadual.ValorBcIcmsDestino,
                pFCPUFDest = itemFiscal.IcmsInterstadual.AliquotaCombatePobreza,
                pICMSUFDest = itemFiscal.IcmsInterstadual.AliquotaInternaDestino,
                pICMSInter = itemFiscal.IcmsInterstadual.AliquotaInterstadual,
                pICMSInterPart = itemFiscal.IcmsInterstadual.PercentualParaDestino,
                vFCPUFDest = itemFiscal.IcmsInterstadual.ValorCombatePobreza,
                vICMSUFDest = itemFiscal.IcmsInterstadual.ValorIcmsDestino,
                vICMSUFRemet = itemFiscal.IcmsInterstadual.ValorIcmsOrigem,
                vBCFCPUFDest = itemFiscal.IcmsInterstadual.ValorBcIcmsDestino
            };
        }

        private static total GetTotal(Nfeletronica nfe)
        {
            return new total
            {
                ICMSTot = new ICMSTot
                {
                    vBC = nfe.TotalBcIcms,
                    vBCST = nfe.TotalBcSt,
                    vICMS = nfe.TotalIcms,
                    vCOFINS = nfe.TotalCofins,
                    vICMSUFDest = nfe.Itens.Sum(i => i.IcmsInterstadual?.ValorIcmsDestino ?? 0),
                    vFCPUFDest = nfe.Itens.Sum(i => i.IcmsInterstadual?.ValorCombatePobreza ?? 0),
                    vICMSUFRemet = nfe.Itens.Sum(i => i.IcmsInterstadual?.ValorIcmsOrigem ?? 0),
                    vICMSDeson = 0.00M,
                    vII = 0.00M,
                    vIPI = nfe.CalcularTotalIpi(),
                    vPIS = nfe.TotalPis,
                    vProd = nfe.TotalItens,
                    vST = nfe.TotalSt,
                    vDesc = nfe.TotalDescontoFinal,
                    vSeg = nfe.ValorSeguroFixo,
                    vOutro = nfe.ValorDespesasFixa,
                    vFrete = nfe.ValorFreteFixo,
                    vFCP = nfe.TotalFcp,
                    vFCPST = nfe.TotalFcpSt,
                    vFCPSTRet = 0.00m,
                    vIPIDevol = nfe.CalcularTotalIpiDevolucao(),
                    vNF = nfe.TotalFinal
                }
            };
        }
    }
}