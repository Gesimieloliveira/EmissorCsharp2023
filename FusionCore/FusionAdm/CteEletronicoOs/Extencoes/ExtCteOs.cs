using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DFe.DocumentosEletronicos.CTe.Classes.Informacoes.Emitente;
using DFe.DocumentosEletronicos.CTe.Classes.Informacoes.Identificacao;
using DFe.DocumentosEletronicos.CTe.Classes.Informacoes.Impostos;
using DFe.DocumentosEletronicos.CTe.Classes.Informacoes.Impostos.ICMSCTe;
using DFe.DocumentosEletronicos.CTe.Classes.Informacoes.infCTeNormal.infModals.rodoviarioOS;
using DFe.DocumentosEletronicos.CTe.Classes.Informacoes.Tipos;
using DFe.DocumentosEletronicos.CTe.Classes.Informacoes.Valores;
using DFe.DocumentosEletronicos.CTe.CTeOS;
using DFe.DocumentosEletronicos.CTe.CTeOS.Informacoes;
using DFe.DocumentosEletronicos.CTe.CTeOS.Informacoes.Complemento;
using DFe.DocumentosEletronicos.CTe.CTeOS.Informacoes.Identificacao;
using DFe.DocumentosEletronicos.CTe.CTeOS.Informacoes.Impostos;
using DFe.DocumentosEletronicos.CTe.CTeOS.Informacoes.InfCTeNormal;
using DFe.DocumentosEletronicos.CTe.CTeOS.Informacoes.InfRespTec;
using DFe.DocumentosEletronicos.CTe.CTeOS.Informacoes.Tomador;
using DFe.DocumentosEletronicos.Entidades;
using DFe.DocumentosEletronicos.Flags;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.CteEletronicoOs.Flags;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF.Componentes;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Flags;
using NHibernate.Util;
using ExtObject = DFe.Ext.ExtObject;
using ModeloDocumento = DFe.DocumentosEletronicos.Flags.ModeloDocumento;
using TipoAmbiente = DFe.DocumentosEletronicos.Flags.TipoAmbiente;
using TipoEmissao = FusionCore.FusionAdm.CteEletronico.Flags.TipoEmissao;
using TipoServico = FusionCore.FusionAdm.CteEletronicoOs.Flags.TipoServico;

namespace FusionCore.FusionAdm.CteEletronicoOs.Extencoes
{
    public static class ExtCteOs
    {
        public static CTeOS ToDFe(this CteOs cteOs)
        {
            var cteOsZeus = new CTeOS();
            cteOsZeus.versao = VersaoServico.Versao300;
            cteOsZeus.InfCte = new infCteOS();
            cteOsZeus.InfCte.versao = VersaoServico.Versao300;

            Identificacao(cteOs, cteOsZeus);
            InformacoesEmitente(cteOs, cteOsZeus);
            Tomador(cteOs, cteOsZeus);
            Totais(cteOs, cteOsZeus);
            Tributacao(cteOs, cteOsZeus);
            ConverteNormal(cteOs, cteOsZeus);


            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                if (new RepositorioResponsavelTecnico(sessao).ExisteResponsavelTecnico(cteOs.Emitente.EstadoDTO.Id, TipoDocumentoFiscalEletronico.CTeOs))
                    InformarResponsavelTecnico(cteOsZeus);
            }

            if (cteOs.Observacao.IsNullOrEmpty() && ExtObject.IsNull((object)cteOs.TributacaoDifal)) return cteOsZeus;

            cteOsZeus.InfCte.compl = new complOs();
            var observacao = new StringBuilder();

            if (ExtObject.IsNotNull((object)cteOs.TributacaoDifal) && cteOs.TributacaoDifal.Observacao.IsNotNullOrEmpty() && cteOs.ConfigImposto.IsPartilha) observacao.Append(cteOs.TributacaoDifal.Observacao).Append(";");

            observacao.Append(cteOs.Observacao.TrimOrEmpty().RemoverTeclaEnter().TrimSefaz(2000));

            cteOsZeus.InfCte.compl.xObs = observacao.ToString().TrimOrNull();

            return cteOsZeus;
        }

        private static void InformarResponsavelTecnico(CTeOS cteOsZeus)
        {
            cteOsZeus.InfCte.infRespTec = new infRespTec
            {
                xContato = ResponsavelLegal.RazaoSocial,
                email = ResponsavelLegal.Email,
                fone = ResponsavelLegal.Telefone,
                CNPJ = ResponsavelLegal.Cnpj
            };
        }

        private static void ConverteNormal(CteOs cteOs, CTeOS cteOsZeus)
        {
            cteOsZeus.InfCte.infCTeNorm = new infCTeNormOs();

            cteOsZeus.InfCte.infCTeNorm.infServico = new infServico();
            cteOsZeus.InfCte.infCTeNorm.infServico.xDescServ = cteOs.Normal.DescricaoServicoPrestado.TrimSefaz(30);
            cteOsZeus.InfCte.infCTeNorm.infServico.infQ =
                new infQOs { qCarga = cteOs.Normal.QuantidadePassageirosVolumes };


            ConverteSeguro(cteOs, cteOsZeus);
            ConverteComponentes(cteOs, cteOsZeus);
            ConverteDocumentoReferenciado(cteOs, cteOsZeus);


            cteOsZeus.InfCte.infCTeNorm.infModal = new infModalOs();
            cteOsZeus.InfCte.infCTeNorm.infModal.versaoModal = versaoModal.veM300;
            var rodoviario = new rodoOS();

            rodoviario.TAF = cteOs.Rodoviario.Taf.TrimSefazOrNull(12);
            rodoviario.NroRegEstadual = cteOs.Rodoviario.NumeroDoRegimeEstadual.TrimSefazOrNull(25);

            AdicionarVeiculo(cteOs, rodoviario);

            if (cteOs.Servico == TipoServico.TransportePessoas)
                rodoviario.infFretamento = new infFretamentoOs
                {
                    dhViagem = cteOs.ViagemEm,
                    tpFretamento = cteOs.TipoFretamento == TipoFretamento.Continuo ? tpFretamento.Continuo : tpFretamento.Eventual
                };

            cteOsZeus.InfCte.infCTeNorm.infModal.ContainerModal = rodoviario;
        }

        private static void ConverteComponentes(CteOs cteOs, CTeOS cteOsZeus)
        {
            if (cteOs.Componentes.IsNullOrEmpty()) return;

            var compXml = new List<Comp>();

            cteOs.Componentes.ForEach(comp =>
            {
                compXml.Add(new Comp
                {
                    vComp = comp.Valor,
                    xNome = comp.Nome
                });
            });

            cteOsZeus.InfCte.vPrest.Comp = compXml;
        }

        private static void ConverteDocumentoReferenciado(CteOs cteOs, CTeOS cteOsZeus)
        {
            if (cteOs.DocumentoReferenciado.IsNullOrEmpty()) return;

            var infDocRef = cteOs.DocumentoReferenciado.Select(cteOsDocumentoReferenciado => new infDocRef
            {
                dEmi = cteOsDocumentoReferenciado.EmitidaEm,
                nDoc = cteOsDocumentoReferenciado.Numero,
                serie = cteOsDocumentoReferenciado.Serie,
                subserie = cteOsDocumentoReferenciado.SubSerie,
                vDoc = cteOsDocumentoReferenciado.Valor
            })
                .ToList();

            cteOsZeus.InfCte.infCTeNorm.infDocRef = infDocRef;
        }

        private static void AdicionarVeiculo(CteOs cteOs, rodoOS rodoviario)
        {
            if (cteOs.Veiculo == null) return;

            rodoviario.veic = new veicOs();
            rodoviario.veic.placa = cteOs.Veiculo.Placa;
            rodoviario.veic.UF = rodoviario.veic.UF.SiglaParaEstado(cteOs.Veiculo.SiglaUf);

            if (cteOs.Veiculo.TipoProprietario == TipoPropriedadeVeiculo.Terceiro)
            {
                var proprietario = cteOs.Veiculo.CarregaTransPortadora();

                var veiculo = rodoviario.veic;
                veiculo.prop = new prop();

                var propOs = veiculo.prop;

                propOs.CNPJ = proprietario.GetDocumentoUnico();

                if (proprietario.GetDocumentoUnico().Length == 11)
                {
                    propOs.CNPJ = null;
                    propOs.CPF = proprietario.GetDocumentoUnico();
                }

                propOs.xNome = proprietario.Nome;
                propOs.IE = proprietario.InscricaoEstadual == string.Empty ? "ISENTO" : proprietario.InscricaoEstadual;

                propOs.TAF = proprietario.Taf.TrimOrNull();
                propOs.NroRegEstadual = proprietario.NumeroDoRegistroEstadual.TrimOrNull();

                var endereco = proprietario.GetEnderecoPrincipal();

                if (endereco == null)
                    throw new InvalidOperationException("Proprietário do veículo precisa de um endereço");

                var estado = Estado.AC;

                propOs.UF = estado.SiglaParaEstado(endereco.Cidade.SiglaUf);
            }
        }

        private static void ConverteSeguro(CteOs cteOs, CTeOS cteOsZeus)
        {
            if (cteOs.Seguros != null && cteOs.Seguros.Any())
            {
                cteOsZeus.InfCte.infCTeNorm.seg = new List<segOs>();

                cteOs.Seguros.ForEach(seguroOs =>
                {
                    cteOsZeus.InfCte.infCTeNorm.seg.Add(new segOs
                    {
                        nApol = seguroOs.NumeroApolice.TrimSefazOrNull(20),
                        xSeg = seguroOs.NomeSeguradora.TrimSefazOrNull(30),
                        respSeg = ConverteResponsavelSeguro(seguroOs.ResponsavelSeguro)
                    });
                });
            }
        }

        private static respSeg ConverteResponsavelSeguro(ResponsavelSeguro seguroOsResponsavelSeguro)
        {
            switch (seguroOsResponsavelSeguro)
            {
                case ResponsavelSeguro.Nenhum:
                    throw new ArgumentOutOfRangeException(nameof(seguroOsResponsavelSeguro), seguroOsResponsavelSeguro, null);
                case ResponsavelSeguro.Emitente:
                    return respSeg.EmitenteDoCTe;
                case ResponsavelSeguro.TomadorDeServico:
                    return respSeg.TomadorDoServico;
                default:
                    throw new ArgumentOutOfRangeException(nameof(seguroOsResponsavelSeguro), seguroOsResponsavelSeguro, null);
            }
        }

        private static void Tributacao(CteOs cteOs, CTeOS cteOsZeus)
        {
            cteOsZeus.InfCte.imp = new impOs();
            cteOsZeus.InfCte.imp.ICMS = new ICMS();

            cteOsZeus.InfCte.imp.vTotTrib = cteOs.Tributacao.ValorIbpt == 0 ? (decimal?)null : cteOs.Tributacao.ValorIbpt;

            switch (cteOs.Emitente.RegimeTributario)
            {
                case RegimeTributario.SimplesNacional:
                    cteOsZeus.InfCte.imp.ICMS.TipoICMS = new ICMSSN()
                    {
                        CST = CST.ICMS90,
                        indSN = indSN.Sim
                    };
                    break;
                case RegimeTributario.RegimeNormal:
                    {
                        switch (cteOs.TributacaoIcms.TributacaoIcms.Codigo)
                        {
                            case "40":
                                cteOsZeus.InfCte.imp.ICMS.TipoICMS = new ICMS45
                                {
                                    CST = CST.ICMS40
                                };
                                break;
                            case "41":
                                cteOsZeus.InfCte.imp.ICMS.TipoICMS = new ICMS45
                                {
                                    CST = CST.ICMS41
                                };
                                break;
                            case "51":
                                cteOsZeus.InfCte.imp.ICMS.TipoICMS = new ICMS45
                                {
                                    CST = CST.ICMS51
                                };
                                break;

                            case "00":
                                cteOsZeus.InfCte.imp.ICMS.TipoICMS = new ICMS00
                                {
                                    CST = CST.ICMS00,
                                    pICMS = cteOs.TributacaoIcms.Aliquota,
                                    vBC = cteOs.TributacaoIcms.BaseCalculo,
                                    vICMS = cteOs.TributacaoIcms.Valor
                                };
                                break;

                            case "90":
                                cteOsZeus.InfCte.imp.ICMS.TipoICMS = new ICMS90
                                {
                                    CST = CST.ICMS90,
                                    pICMS = cteOs.TributacaoIcms.Aliquota,
                                    vBC = cteOs.TributacaoIcms.BaseCalculo,
                                    vICMS = cteOs.TributacaoIcms.Valor,
                                    pRedBC = cteOs.TributacaoIcms.PercentualReducao,
                                    vCred = cteOs.TributacaoIcms.ValorCredito
                                };
                                break;
                        }

                        break;
                    }
            }

            if (cteOs.ConfigImposto.IsPartilha)
                cteOsZeus.InfCte.imp.ICMSUFFim = new ICMSUFFim
                {
                    pFCPUFFim = cteOs.TributacaoDifal.PercentualFcp,
                    pICMSInter = cteOs.TributacaoDifal.PercentualAliquotaInterestadual,
                    pICMSUFFim = cteOs.TributacaoDifal.PercentualAliquotaInterna,
                    vBCUFFim = cteOs.TributacaoDifal.BaseCalculo,
                    vFCPUFFim = cteOs.TributacaoDifal.ValorIcmsFcp,
                    vICMSUFFim = cteOs.TributacaoDifal.ValorIcmsUfTermino,
                    vICMSUFIni = cteOs.TributacaoDifal.ValorIcmsUfInicio
                };

            if (cteOs.TributacaoFederal != null) AddTributacaoFederal(cteOsZeus, cteOs);
        }

        private static void AddTributacaoFederal(CTeOS cteOsZeus, CteOs cteOs)
        {
            var inf = new infTribFed
            {
                vPIS = cteOs.TributacaoFederal.ValorPis,
                vCOFINS = cteOs.TributacaoFederal.ValorCofins,
                vCSLL = cteOs.TributacaoFederal.ValorClss,
                vINSS = cteOs.TributacaoFederal.ValorInss,
                vIR = cteOs.TributacaoFederal.ValorImpostoRenda
            };

            cteOsZeus.InfCte.imp.infTribFed = inf;
        }

        private static void Totais(CteOs cteOs, CTeOS cteOsZeus)
        {
            cteOsZeus.InfCte.vPrest = new vPrestOs();
            cteOsZeus.InfCte.vPrest.vTPrest = cteOs.PrecoServico.Valor;
            cteOsZeus.InfCte.vPrest.vRec = cteOs.PrecoServico.AReceber;
        }

        private static void Tomador(CteOs cteOs, CTeOS cteOsZeus)
        {
            cteOsZeus.InfCte.toma = new tomaOs();

            var documentoUnico = cteOs.Tomador.GetDocumentoUnico();

            if (documentoUnico.Length == 14) cteOsZeus.InfCte.toma.CNPJ = documentoUnico.TrimSefaz(14);

            if (documentoUnico.Length == 11) cteOsZeus.InfCte.toma.CPF = documentoUnico.TrimSefaz(11);

            cteOsZeus.InfCte.toma.IE = cteOs.Tomador.InscricaoEstadual.TrimSefazOrNull(14);
            cteOsZeus.InfCte.toma.xNome = cteOs.Tomador.Nome.TrimSefaz(60);
            cteOsZeus.InfCte.toma.xFant = cteOs.Tomador.NomeFantasia.TrimSefazOrNull(60);
            cteOsZeus.InfCte.toma.fone = cteOs.Tomador.Telefones != null && cteOs.Tomador.Telefones.Any()
                ? cteOs.Tomador.Telefones[0].Numero.TrimSefazOrNull(14)
                : null;

            var enderecos = cteOs.Tomador.Enderecos;

            if (enderecos == null || !enderecos.Any()) throw new InvalidOperationException("Adicionar endereco no tomador de serviço");

            var endereco = enderecos[0];

            cteOsZeus.InfCte.toma.enderToma = new enderTomaOs();
            cteOsZeus.InfCte.toma.enderToma.xLgr = endereco.Logradouro.TrimSefaz(60);
            cteOsZeus.InfCte.toma.enderToma.nro = endereco.Numero.TrimSefaz(60);
            cteOsZeus.InfCte.toma.enderToma.xCpl = endereco.Complemento.TrimSefazOrNull(60);
            cteOsZeus.InfCte.toma.enderToma.xBairro = endereco.Bairro.TrimSefaz(60);
            cteOsZeus.InfCte.toma.enderToma.cMun = endereco.Cidade.CodigoIbge;
            cteOsZeus.InfCte.toma.enderToma.xMun = endereco.Cidade.Nome.TrimSefaz(60);
            cteOsZeus.InfCte.toma.enderToma.CEP = long.Parse(endereco.Cep);
            cteOsZeus.InfCte.toma.enderToma.UF = cteOsZeus.InfCte.toma.enderToma.UF.SiglaParaEstado(endereco.Cidade.SiglaUf);
        }

        private static void InformacoesEmitente(CteOs cteOs, CTeOS cteOsZeus)
        {
            cteOsZeus.InfCte.emit = new emitOs();
            cteOsZeus.InfCte.emit.CNPJ = cteOs.Emitente.Cnpj.TrimSefaz(14);
            cteOsZeus.InfCte.emit.IE = cteOs.Emitente.InscricaoEstadual.TrimSefaz(14);
            cteOsZeus.InfCte.emit.xNome = cteOs.Emitente.RazaoSocial.TrimSefaz(60);
            cteOsZeus.InfCte.emit.xFant = cteOs.Emitente.NomeFantasia.TrimSefazOrNull(60);

            cteOsZeus.InfCte.emit.enderEmit = new enderEmit();
            cteOsZeus.InfCte.emit.enderEmit.xLgr = cteOs.Emitente.Logradouro.TrimSefaz(60);
            cteOsZeus.InfCte.emit.enderEmit.nro = cteOs.Emitente.Numero.TrimSefaz(60);
            cteOsZeus.InfCte.emit.enderEmit.xCpl = cteOs.Emitente.Complemento.TrimSefazOrNull(60);
            cteOsZeus.InfCte.emit.enderEmit.xBairro = cteOs.Emitente.Bairro.TrimSefaz(60);
            cteOsZeus.InfCte.emit.enderEmit.cMun = cteOs.Emitente.CidadeDTO.CodigoIbge;
            cteOsZeus.InfCte.emit.enderEmit.xMun = cteOs.Emitente.CidadeDTO.Nome.TrimSefaz(60);
            cteOsZeus.InfCte.emit.enderEmit.CEP = long.Parse(cteOs.Emitente.Cep);
            cteOsZeus.InfCte.emit.enderEmit.UF =
                cteOsZeus.InfCte.emit.enderEmit.UF.CodigoIbgeParaEstado(cteOs.Emitente.EstadoDTO.CodigoIbge.ToString());
            cteOsZeus.InfCte.emit.enderEmit.fone = cteOs.Emitente.Fone1.IsNotNullOrEmpty()
                ? cteOs.Emitente.Fone1.TrimSefazOrNull(8)
                : cteOs.Emitente.Fone2.TrimSefazOrNull(8);
        }

        private static void Identificacao(CteOs cteOs, CTeOS dfeCteos)
        {
            dfeCteos.InfCte.ide = new ideOs();
            dfeCteos.InfCte.ide.cUF = dfeCteos.InfCte.ide.cUF.CodigoIbgeParaEstado(cteOs.Emitente.EstadoDTO.CodigoIbge.ToString());
            dfeCteos.InfCte.ide.cCT = cteOs.Id;
            dfeCteos.InfCte.ide.CFOP = int.Parse(cteOs.PerfilCfop.Cfop.Id);
            dfeCteos.InfCte.ide.natOp = cteOs.NaturezaOperacao;
            dfeCteos.InfCte.ide.mod = ModeloDocumento.CTeOS;
            dfeCteos.InfCte.ide.serie = cteOs.SerieEmissao;
            dfeCteos.InfCte.ide.nCT = cteOs.NumeroEmissao;
            dfeCteos.InfCte.ide.dhEmi = cteOs.EmissaoEm;
            dfeCteos.InfCte.ide.tpImp = tpImp.Retrado;
            dfeCteos.InfCte.ide.tpEmis = cteOs.TipoEmissao == TipoEmissao.Normal ? tpEmis.teNormal : GetTipoEmissaoSvc(cteOs);
            dfeCteos.InfCte.ide.tpAmb = ConverteAmbienteEmissao(cteOs.Perfil.EmissorFiscal.EmissorFiscalCteOs.Ambiente);
            dfeCteos.InfCte.ide.tpCTe = ConverteTipoCTe(cteOs.Tipo);
            dfeCteos.InfCte.ide.procEmi = procEmi.AplicativoContribuinte;
            dfeCteos.InfCte.ide.verProc = ResponsavelLegal.VersaoSistema;
            dfeCteos.InfCte.ide.cMunEnv = cteOs.Emitente.CidadeDTO.CodigoIbge;
            dfeCteos.InfCte.ide.xMunEnv = cteOs.Emitente.CidadeDTO.Nome;
            dfeCteos.InfCte.ide.UFEnv =
                dfeCteos.InfCte.ide.UFEnv.CodigoIbgeParaEstado(cteOs.Emitente.EstadoDTO.CodigoIbge.ToString());
            dfeCteos.InfCte.ide.modal = ConverteModal(cteOs.Modal);
            dfeCteos.InfCte.ide.tpServ = ConverteTipoServico(cteOs.Servico);

            ConverteIE(cteOs, dfeCteos);

            dfeCteos.InfCte.ide.cMunIni = cteOs.LocalInicialPrestacao.Cidade.CodigoIbge;
            dfeCteos.InfCte.ide.xMunIni = cteOs.LocalInicialPrestacao.Cidade.Nome;
            dfeCteos.InfCte.ide.UFIni =
                dfeCteos.InfCte.ide.UFIni.CodigoIbgeParaEstado(cteOs.LocalInicialPrestacao.EstadoUF.CodigoIbge.ToString());
            dfeCteos.InfCte.ide.cMunFim = cteOs.LocalFinalPrestacao.Cidade.CodigoIbge;
            dfeCteos.InfCte.ide.xMunFim = cteOs.LocalFinalPrestacao.Cidade.Nome;
            dfeCteos.InfCte.ide.UFFim =
                dfeCteos.InfCte.ide.UFFim.CodigoIbgeParaEstado(cteOs.LocalFinalPrestacao.EstadoUF.CodigoIbge.ToString());

            if (cteOs.Percursos.Count > 0)
            {
                var percurso = new List<infPercurso>();

                cteOs.Percursos.ForEach(p =>
                {
                    var estado = Estado.EX.CodigoIbgeParaEstado(p.Estado.CodigoIbge.ToString());

                    percurso.Add(new infPercurso
                    {
                        UFPer = estado
                    });
                });

                dfeCteos.InfCte.ide.infPercurso = percurso;
            }
        }

        private static tpEmis GetTipoEmissaoSvc(CteOs cteOs)
        {
            switch (Estado.EX.CodigoIbgeParaEstado(cteOs.Emitente.EstadoDTO.CodigoIbge.ToString()))
            {
                case Estado.RR:
                case Estado.PE:
                case Estado.AP:
                case Estado.SP:
                case Estado.MT:
                case Estado.MS:
                    return tpEmis.teSVCRS;
                case Estado.AC:
                case Estado.AL:
                case Estado.AM:
                case Estado.BA:
                case Estado.CE:
                case Estado.DF:
                case Estado.ES:
                case Estado.GO:
                case Estado.MA:
                case Estado.MG:
                case Estado.PA:
                case Estado.PB:
                case Estado.PR:
                case Estado.PI:
                case Estado.RJ:
                case Estado.RN:
                case Estado.RS:
                case Estado.RO:
                case Estado.SC:
                case Estado.SE:
                case Estado.TO:
                case Estado.AN:
                    return tpEmis.teSVCSP;

                default:
                    throw new InvalidOperationException(
                        "Não achei a url do seu estado(uf), tente com outra unidade federativa");
            }
        }

        private static void ConverteIE(CteOs cteOs, CTeOS cteOS)
        {
            var inscricaoEstadualTomador = new InscricaoEstadual(cteOs.Tomador.InscricaoEstadual);

            var indicadorIE = inscricaoEstadualTomador.GetIndicador();

            switch (indicadorIE)
            {
                case IndicadorIE.ContribuinteIcms:
                    cteOS.InfCte.ide.indIEToma = indIEToma.ContribuinteIcms;
                    break;
                case IndicadorIE.Isento:
                    cteOS.InfCte.ide.indIEToma = indIEToma.ContribuinteIsentoDeInscricao;
                    break;
                case IndicadorIE.NaoContribuinte:
                    cteOS.InfCte.ide.indIEToma = indIEToma.NaoContribuinte;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static tpServ ConverteTipoServico(TipoServico servico)
        {
            switch (servico)
            {
                case TipoServico.ExcessoBagagem:
                    return tpServ.excessoBagagem;

                case TipoServico.TransportePessoas:
                    return tpServ.transportePessoas;

                case TipoServico.TransporteValores:
                    return tpServ.transporteValores;

                default:
                    throw new ArgumentOutOfRangeException(nameof(servico), servico, null);
            }
        }

        private static modal ConverteModal(Modal modal)
        {
            switch (modal)
            {
                case Modal.Rodoviario:
                    return global::DFe.DocumentosEletronicos.CTe.Classes.Informacoes.Tipos.modal.rodoviario;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modal), modal, null);
            }
        }

        private static tpCTe ConverteTipoCTe(TipoCte tipoCte)
        {
            switch (tipoCte)
            {
                case TipoCte.Normal:
                    return tpCTe.Normal;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tipoCte), tipoCte, null);
            }
        }

        private static TipoAmbiente ConverteAmbienteEmissao(Fiscal.Flags.TipoAmbiente ambiente)
        {
            switch (ambiente)
            {
                case Fiscal.Flags.TipoAmbiente.Producao:
                    return TipoAmbiente.Producao;
                case Fiscal.Flags.TipoAmbiente.Homologacao:
                    return TipoAmbiente.Homologacao;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ambiente), ambiente, null);
            }
        }
    }
}