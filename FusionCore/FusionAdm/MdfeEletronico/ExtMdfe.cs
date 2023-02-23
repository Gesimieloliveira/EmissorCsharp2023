using System;
using System.Collections.Generic;
using System.Linq;
using DFe.Classes.Entidades;
using DFe.Classes.Extensoes;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.MdfeEletronico.Autorizador;
using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionCore.FusionAdm.MdfeEletronico.Extencoes;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.FusionAdm;
using MDFe.Classes.Flags;
using MDFe.Classes.Informacoes;
using MDFe.Utils.Configuracoes;
using NHibernate.Util;
using static System.String;
using ZeusMDFe = MDFe.Classes.Informacoes.MDFe;
using ZeusCondutor = MDFe.Classes.Informacoes.MDFeCondutor;
using ZeusLacre = MDFe.Classes.Informacoes.MDFeLacre;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public static class ExtMdfe
    {
        public static ZeusMDFe ToZeusMdfe(this MDFeEletronico mdfe)
        {
            var zeusMdfe = new ZeusMDFe();

            zeusMdfe.InfMDFe.InfDoc.InfMunDescarga = new List<MDFeInfMunDescarga>();

            PreencherIde(zeusMdfe.InfMDFe.Ide, mdfe);
            PreencherEmitente(zeusMdfe.InfMDFe.Emit, mdfe);

            zeusMdfe.InfMDFe.Seg = new List<MDFeSeg>();

            PreencherSeguro(zeusMdfe.InfMDFe.Seg, mdfe);

            PreencherModalRodoviario(zeusMdfe.InfMDFe.InfModal, mdfe);
            PreencherMunicipioDescarrega(zeusMdfe.InfMDFe.InfDoc.InfMunDescarga, mdfe);
            PreencherTotais(zeusMdfe.InfMDFe.Tot, mdfe);

            PreencherProdutoPredominante(zeusMdfe.InfMDFe, mdfe);

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                if (new RepositorioResponsavelTecnico(sessao).ExisteResponsavelTecnico(mdfe.Emitente.Empresa.EstadoDTO.Id, TipoDocumentoFiscalEletronico.MDFe))
                    ResponsavelTecnico(zeusMdfe);
            }

            PreencherInformacoesAdicionais(zeusMdfe, mdfe);

            zeusMdfe.InfMDFe.Lacres = new List<ZeusLacre>();

            PreencherLacre(zeusMdfe.InfMDFe.Lacres, mdfe);

            return zeusMdfe;
        }

        private static void PreencherProdutoPredominante(MDFeInfMDFe infMDFe, MDFeEletronico mdfe)
        {
            var prodPred = new prodPred();

            if (mdfe.CargaFechada)
            {
                prodPred.infLotacao = new infLotacao
                {
                    infLocalCarrega = new infLocalCarrega
                    {
                        CEP = mdfe.ProdutoPredominante.CepCarregamento
                    },
                    infLocalDescarrega = new infLocalDescarrega
                    {
                        CEP = mdfe.ProdutoPredominante.CepDescarregamento
                    }
                };
            }

            if (mdfe.ProdutoPredominante.TipoCarga != TipoCarga.Nenhuma)
            {
                prodPred.xProd = mdfe.ProdutoPredominante.Nome.TrimSefaz(120);
                prodPred.NCM = mdfe.ProdutoPredominante.Ncm.TrimSefazOrNull(8);
                prodPred.cEAN = mdfe.ProdutoPredominante.CodigoBarras.TrimSefazOrNull(14);
                prodPred.tpCarga = (tpCarga) mdfe.ProdutoPredominante.TipoCarga;
            }

            if (mdfe.ProdutoPredominante.TipoCarga == TipoCarga.Nenhuma) return;

            infMDFe.prodPred = prodPred;
        }

        private static void ResponsavelTecnico(ZeusMDFe zeusMdfe)
        {
            zeusMdfe.InfMDFe.infRespTec = new infRespTec
            {
                CNPJ = ResponsavelLegal.Cnpj,
                email = ResponsavelLegal.Email,
                fone = ResponsavelLegal.Telefone,
                xContato = ResponsavelLegal.RazaoSocial
            };
        }

        private static void PreencherSeguro(ICollection<MDFeSeg> seg, MDFeEletronico mdfe)
        {
            mdfe.SeguroCargas.ForEach(s =>
            {
                var seguro = new MDFeSeg
                {
                    InfResp = new MDFeInfResp
                    {
                        CNPJ = s.CnpjResponsavel.TrimOrNull(),
                        CPF = s.CpfResponsavel.TrimOrNull(),
                        RespSeg = s.Responsavel == MDFeResponsavelSeguro.Emitente
                            ? MDFeRespSeg.EmitenteDoMDFe
                            : MDFeRespSeg.ResponsavelPelaContratacao
                    },
                    NApol = s.NumeroApolice.TrimOrNull()
                };

                if (s.Averbacoes.Count != 0)
                {
                    seguro.NAver = new List<string>();
                    s.Averbacoes.ForEach(x =>
                    {
                        seguro.NAver.Add(x.Averbacao);
                    });
                }

                if (s.NomeSeguradora.IsNotNullOrEmpty() && s.CnpjSeguradora.IsNotNullOrEmpty())
                    seguro.InfSeg = new MDFeInfSeg
                    {
                        XSeg = s.NomeSeguradora.TrimOrNull().RemoverAcentos(),
                        CNPJ = s.CnpjSeguradora.TrimOrNull()
                    };


                seg.Add(seguro);
            });
        }

        private static void PreencherLacre(ICollection<ZeusLacre> lacres, MDFeEletronico mdfe)
        {
            mdfe.Lacres?.ForEach(l =>
            {
                var lacre = new ZeusLacre
                {
                    NLacre = l.Numero
                };

                lacres.Add(lacre);
            });
        }

        private static void PreencherInformacoesAdicionais(ZeusMDFe zeusMdfe, MDFeEletronico mdfe)
        {
            var infadicional = new List<string>();

            if (!IsNullOrEmpty(mdfe.Observacao))
            {
                infadicional.Add(mdfe.Observacao.TrimSefaz());
            }

            if (mdfe.PrevisaoInicioViagemEm != null)
            {
                infadicional.Add($"PREVISÃO INICIO DA VIAGEM: {mdfe.PrevisaoInicioViagemEm?.ToString("dd/MM/yyyy HH:mm")}");
            }

            if (infadicional.Count == 0)
            {
                return;
            }

            var infcpl = Join(";", infadicional);

            zeusMdfe.InfMDFe.InfAdic = new MDFeInfAdic { InfCpl = infcpl.RemoverAcentos()};
        }

        private static void PreencherTotais(MDFeTot tot, MDFeEletronico mdfe)
        {
            tot.QCTe = mdfe.QuantidadeCTe;
            tot.vCarga = Convert.ToDecimal(mdfe.ValorTotalCarga.ToString("N2"));
            tot.QCarga = Convert.ToDecimal(mdfe.PesoBrutoCarga.ToString("N4"));
            tot.QNFe = mdfe.QuantidadeNFe;
            tot.CUnid = mdfe.UnidadeMedida.ToZeusMdfe();
        }

        private static void PreencherMunicipioDescarrega(ICollection<MDFeInfMunDescarga> infMunDescarga,
            MDFeEletronico mdfe)
        {
            foreach (var items in mdfe.Descarregamentos.GroupBy(i => i.Cidade))
            {
                var novoMunicipio = new MDFeInfMunDescarga
                {
                    CMunDescarga = items.Key.CodigoIbge.ToString(),
                    XMunDescarga = items.Key.Nome
                };

                foreach (var i in items)
                {
                    var chave = i.ChaveDocumento;
                    var segundoCodigo = IsNullOrEmpty(i.SegundoCodigoBarras) ? null : i.SegundoCodigoBarras;

                    if (i.ModeloDocumento == ModeloDocumento.NFe)
                    {
                        if (novoMunicipio.InfNFe == null)
                        {
                            novoMunicipio.InfNFe = new List<MDFeInfNFe>();
                        }

                        var inf = new MDFeInfNFe
                        {
                            ChNFe = chave,
                            SegCodBarra = segundoCodigo
                        };

                        foreach (var pp in i.ProdutosPerigosos)
                        {
                            if (inf.Peri == null) inf.Peri = new List<MDFePeri>();

                            inf.Peri.Add(pp.ToZeus());
                        }

                        novoMunicipio.InfNFe.Add(inf);
                    }

                    if (i.ModeloDocumento == ModeloDocumento.CTe)
                    {
                        if (novoMunicipio.InfCTe == null)
                        {
                            novoMunicipio.InfCTe = new List<MDFeInfCTe>();
                        }

                        var inf = new MDFeInfCTe
                        {
                            ChCTe = chave,
                            SegCodBarra = segundoCodigo
                        };

                        foreach (var pp in i.ProdutosPerigosos)
                        {
                            if (inf.Peri == null) inf.Peri = new List<MDFePeri>();

                            inf.Peri.Add(pp.ToZeus());
                        }

                        novoMunicipio.InfCTe.Add(inf);
                    }
                }

                infMunDescarga.Add(novoMunicipio);
            }
        }

        private static void PreencherModalRodoviario(MDFeInfModal infModal, MDFeEletronico mdfe)
        {
            var conversaoEstadoUF = Estado.GO;

            var modalRodoviario = new MDFeRodo {infANTT = new MDFeInfANTT()};

            modalRodoviario.CodAgPorto = mdfe.Rodoviario.CodigoAgendamentoPorto.TrimOrNull();
            modalRodoviario.infANTT.RNTRC = IsNullOrWhiteSpace(mdfe.Rodoviario.Rntrc) ? null : mdfe.Rodoviario.Rntrc;

            PreencherVeiculoTracaoRodoviario(mdfe, modalRodoviario, conversaoEstadoUF);

            PreencherCondutorRodoviario(mdfe, modalRodoviario);

            PreencerVeiculoReboqueRodoviario(mdfe, modalRodoviario, conversaoEstadoUF);

            PreencherValePedagioRodoviario(mdfe, modalRodoviario);

            PreencherContratante(mdfe, modalRodoviario);

            PreencherCiot(mdfe, modalRodoviario);

            PreencherInformacaoPagamento(modalRodoviario.infANTT, mdfe.InformacaoPagamentos);

            infModal.Modal = modalRodoviario;
        }

        private static void PreencherInformacaoPagamento(MDFeInfANTT infAntt, IList<MdfeAutorizacaoInformacaoPagamento> infPagamento)
        {
            if (infPagamento.Count == 0) return;

            infAntt.infPag = new List<infPag>();

            infPagamento.ForEach(pag =>
            {
                var infPag = new infPag
                {
                    CNPJ = pag.ObterCnpj(),
                    CPF = pag.ObterCpf(),
                    idEstrangeiro = pag.ObterIdEstrangeiro(),
                    indPag = pag.IndicadorPagamento == IndicadorPagamento.PagamentoAPrazo ? indPag.PagamentoAPrazo : indPag.PagamentoAVista,
                    vContrato = pag.ValorTotalContrato,
                    xNome = pag.NomeContratante,
                    infPrazo = new List<infPrazo>(),
                    Comp = new List<Comp>()
                };

                var infBanc = new infBanc
                {
                    CNPJIPEF = pag.CnpjIpef
                };

                if (pag.InformarApenasCnpjIpef == false)
                {
                    infBanc.CNPJIPEF = null;
                    infBanc.codAgencia = pag.AgenciaBancaria;
                    infBanc.codBanco = pag.ContaBancaria;
                }

                pag.Parcelas.ForEach(parcela =>
                {
                    infPag.infPrazo.Add(new infPrazo
                    {
                        vParcela = parcela.Valor,
                        dVenc = parcela.DataDeVencimento,
                        nParcela = (short) parcela.Numero
                    });
                });

                pag.ComponentePagamentoFrete.ForEach(comp =>
                {
                    infPag.Comp.Add(new Comp
                    {
                        vComp = comp.Valor,
                        xComp = comp.ObterDescricao(),
                        tpComp = ConverteTipoComponente(comp.TipoComponente)
                    });
                });


                infPag.infBanc = infBanc;
                infAntt.infPag.Add(infPag);
            });
        }

        private static tpComp ConverteTipoComponente(TipoComponente tipoComponente)
        {
            switch (tipoComponente)
            {
                case TipoComponente.ValePedagio:
                    return tpComp.ValePedagio;
                case TipoComponente.ImpostosTaxasContribuicoes:
                    return tpComp.ImpostosTaxasEContribuicoes;
                case TipoComponente.DespesasBancariasMeiosPagamentoOutras:
                    return tpComp.DespesasBancariasEmiosDePagamentoOutras;
                case TipoComponente.Outros:
                    return tpComp.Outros;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tipoComponente), tipoComponente, null);
            }
        }

        private static void PreencherCiot(MDFeEletronico mdfe, MDFeRodo modalRodoviario)
        {
            if (mdfe.Rodoviario.Ciots.IsNullOrEmpty()) return;

            modalRodoviario.infANTT.infCIOT = new List<infCIOT>();

            mdfe.Rodoviario.Ciots.ForEach(c =>
            {
                var ciot = new infCIOT();

                var documentoUnico = c.DocumentoUnico;

                if (documentoUnico.Length == 14)
                    ciot.CNPJ = documentoUnico;

                if (documentoUnico.Length == 11)
                    ciot.CPF = documentoUnico;

                ciot.CIOT = c.Ciot;

                modalRodoviario.infANTT.infCIOT.Add(ciot);
            });
        }

        private static void PreencherContratante(MDFeEletronico mdfe, MDFeRodo modalRodoviario)
        {
            if (mdfe.Rodoviario.Contratantes.IsNullOrEmpty()) return;

            modalRodoviario.infANTT.infContratante = new List<infContratante>();

            mdfe.Rodoviario.Contratantes.ForEach(c =>
            {
                var infContratante = new infContratante();

                var documentoUnico = c.PessoaEntidade.GetDocumentoUnico();

                if (documentoUnico.Length == 11)
                    infContratante.CPF = documentoUnico;

                if (documentoUnico.Length == 14)
                    infContratante.CNPJ = documentoUnico;

                if (documentoUnico.Length > 0 && infContratante.CPF.IsNullOrEmpty() && infContratante.CNPJ.IsNullOrEmpty())
                    infContratante.idEstrangeiro = documentoUnico;

                infContratante.xNome = c.PessoaEntidade.Nome.TrimSefazOrNull(60);

                modalRodoviario.infANTT.infContratante.Add(infContratante);
            });
        }

        private static void PreencherValePedagioRodoviario(MDFeEletronico mdfe, MDFeRodo modalRodoviario)
        {
            if (mdfe.Rodoviario.ValesPedagios == null || mdfe.Rodoviario.ValesPedagios.Count == 0) return;

            modalRodoviario.infANTT.valePed = new MDFeValePed
            {
                Disp = new List<MDFeDisp>(),
                categCombVeic = (categCombVeic) mdfe.CategoriaComercialVeiculo
            };

            var dispositivoPedagio = modalRodoviario.infANTT.valePed.Disp;

            mdfe.Rodoviario.ValesPedagios.ForEach(v =>
            {
                var disp = new MDFeDisp
                {
                    CNPJForn = v.CnpjEmpresaFornecedora.TrimOrNull(),
                    CNPJPg = v.CnpjResponsavelPagamento.TrimOrNull(),
                    NCompra = v.NumeroComprovante.TrimOrNull(),
                    CPFPg = v.CpfResponsavel.TrimOrNull(),
                    vValePed = v.Valor
                };

                dispositivoPedagio.Add(disp);
            });
        }

        private static void PreencerVeiculoReboqueRodoviario(MDFeEletronico mdfe,
            MDFeRodo modalRodoviario,
            Estado conversaoEstadoUF)
        {
            modalRodoviario.VeicReboque = new List<MDFeVeicReboque>();

            mdfe.Rodoviario.VeiculosReboques.ForEach(r =>
            {
                var veiculoReboque = new MDFeVeicReboque
                {
                    Placa = r.Veiculo.Placa,
                    Tara = r.Veiculo.TaraEmKg,
                    UF = conversaoEstadoUF.SiglaParaEstado(r.Veiculo.SiglaUf),
                    CapKG = r.Veiculo.CapacidadeEmKg,
                    RENAVAM = r.Veiculo.Renavam,
                    CInt = r.Veiculo.Id.ToString(),
                    TpCar = r.Veiculo.TipoCarroceria.ToZeusMdfe(),
                    CapM3 = r.Veiculo.CapacidadeEmM3
                };

                if (r.Veiculo.TipoProprietario == TipoPropriedadeVeiculo.Terceiro)
                {
                    var proprietario = r.Veiculo.CarregaProprietario();
                    veiculoReboque.Prop = proprietario.ToMdfe(r.Veiculo);
                }

                modalRodoviario.VeicReboque.Add(veiculoReboque);
            });
        }

        private static void PreencherCondutorRodoviario(MDFeEletronico mdfe, MDFeRodo modalRodoviario)
        {
            modalRodoviario.VeicTracao.Condutor = new List<ZeusCondutor>();

            mdfe.Rodoviario.VeiculoTracao.Condutores.ForEach(c =>
            {
                modalRodoviario.VeicTracao.Condutor.Add(new ZeusCondutor
                {
                    XNome = c.Condutor.Nome.TrimOrEmpty().RemoverAcentos(),
                    CPF = c.Condutor.Cpf.Valor.TrimOrEmpty()
                });
            });
        }

        private static void PreencherVeiculoTracaoRodoviario(
            MDFeEletronico mdfe,
            MDFeRodo modalRodoviario,
            Estado conversaoEstadoUF
        ) {
            modalRodoviario.VeicTracao = new MDFeVeicTracao
            {
                Placa = mdfe.Rodoviario.VeiculoTracao.Veiculo.Placa,
                Tara = mdfe.Rodoviario.VeiculoTracao.Veiculo.TaraEmKg,
                CInt = mdfe.Rodoviario.VeiculoTracao.Veiculo.Id.ToString(),
                CapKG = mdfe.Rodoviario.VeiculoTracao.Veiculo.CapacidadeEmKg,
                CapM3 = mdfe.Rodoviario.VeiculoTracao.Veiculo.CapacidadeEmM3,
                RENAVAM = mdfe.Rodoviario.VeiculoTracao.Veiculo.Renavam,
                TpCar = mdfe.Rodoviario.VeiculoTracao.Veiculo.TipoCarroceria.ToZeusMdfe(),
                TpRod = mdfe.Rodoviario.VeiculoTracao.Veiculo.TipoRodado.ToZeusMdfe(),
                UF = conversaoEstadoUF.SiglaParaEstado(mdfe.Rodoviario.VeiculoTracao.Veiculo.SiglaUf)
            };

            if (mdfe.Rodoviario.VeiculoTracao.Veiculo.TipoProprietario == TipoPropriedadeVeiculo.Terceiro)
            {
                var veiculo = mdfe.Rodoviario.VeiculoTracao.Veiculo;
                var proprietario = veiculo.CarregaProprietario();

                modalRodoviario.VeicTracao.Prop = proprietario.ToMdfe(veiculo);
            }
        }

        private static void PreencherEmitente(MDFeEmit emit, MDFeEletronico mdfe)
        {
            var documentoUnico = mdfe.Emitente.Empresa.Cnpj == string.Empty ? mdfe.Emitente.Empresa.Cpf : mdfe.Emitente.Empresa.Cnpj;

            if (documentoUnico.Length == 11)
                emit.CPF = documentoUnico;

            if (documentoUnico.Length == 14)
                emit.CNPJ = documentoUnico;

            emit.IE = mdfe.Emitente.Empresa.InscricaoEstadual;
            emit.XNome = mdfe.Emitente.Empresa.RazaoSocial.RemoverAcentos();
            emit.XFant = mdfe.Emitente.Empresa.NomeFantasia.IsNullOrEmpty() ? null : mdfe.Emitente.Empresa.NomeFantasia?.RemoverAcentos();

            emit.EnderEmit.XLgr = mdfe.Emitente.Empresa.Logradouro.RemoverAcentos();
            emit.EnderEmit.Nro = mdfe.Emitente.Empresa.Numero;
            emit.EnderEmit.XCpl = mdfe.Emitente.Empresa.Complemento.IsNullOrEmpty()
                ? null
                : mdfe.Emitente.Empresa.Complemento.RemoverAcentos();
            emit.EnderEmit.XBairro = mdfe.Emitente.Empresa.Bairro.RemoverAcentos();
            emit.EnderEmit.CMun = mdfe.Emitente.Empresa.CidadeDTO.CodigoIbge;
            emit.EnderEmit.XMun = mdfe.Emitente.Empresa.CidadeDTO.Nome.RemoverAcentos();
            emit.EnderEmit.CEP = long.Parse(mdfe.Emitente.Empresa.Cep);
            emit.EnderEmit.UF = mdfe.Emitente.Empresa.EstadoDTO.ToZeusMdfe();
            emit.EnderEmit.Fone = mdfe.Emitente.Empresa.Fone1;
            emit.EnderEmit.Email = mdfe.Emitente.Empresa.Email.IsNullOrEmpty() ? null : mdfe.Emitente.Empresa.Email;
        }

        private static void PreencherIde(MDFeIde ide, MDFeEletronico mdfe)
        {
            ide.CUF = MDFeConfiguracao.VersaoWebService.UfEmitente;
            ide.TpAmb = MDFeConfiguracao.VersaoWebService.TipoAmbiente;
            ide.TpEmit = mdfe.TipoEmitente.ToZeusMFDe();
            ide.Mod = global::DFe.Classes.Flags.ModeloDocumento.MDFe;
            ide.Serie = mdfe.SerieEmissao;
            ide.NMDF = mdfe.NumeroFiscalEmissao;
            ide.CMDF = mdfe.CodigoNumericoEmissao;
            ide.Modal = mdfe.Modal.ToZeusMdfe();
            ide.DhEmi = mdfe.EmissaoEm;
            ide.TpEmis = mdfe.TipoEmissao.ToZeusMdfe();
            ide.ProcEmi = MDFeIdentificacaoProcessoEmissao.EmissaoComAplicativoContribuinte;
            ide.VerProc = mdfe.VersaoAplicativo;
            ide.UFIni = mdfe.EstadoCarregamento.ToZeusMdfe();
            ide.UFFim = mdfe.EstadoDescarregamento.ToZeusMdfe();
            ide.DhIniViagem = mdfe.PrevisaoInicioViagemEm;

            if (mdfe.TipoDoTransportador != MDFeTipoDoTransportador.NaoInformado)
            {
                ide.TpTransp = mdfe.TipoDoTransportador.ToZeusMdfe();
            }

            mdfe.MunicipioCarregamentos.ForEach(m =>
            {
                ide.InfMunCarrega.Add(new MDFeInfMunCarrega
                {
                    CMunCarrega = m.Cidade.CodigoIbge.ToString(),
                    XMunCarrega = m.Cidade.Nome.RemoverAcentos()
                });
            });


            mdfe.Percursos.ForEach(p =>
            {
                ide.InfPercurso.Add(new MDFeInfPercurso
                {
                    UFPer = p.Estado.ToZeusMdfe()
                });
            });
        }
    }
}