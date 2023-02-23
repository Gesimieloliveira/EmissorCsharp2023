using System;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.Helpers.Hidratacao;
using NHibernate.Util;

namespace Fusion.Visao.CteEletronico.Builder
{
    public class CteBuilder
    {
        private readonly Cte _cte;

        public CteBuilder(Cte cte)
        {
            _cte = cte;
        }

        public CteBuilder ComCabecalho(AbaCabecalhoCteModel model)
        {
            _cte.Globalizado = model.Globalizado;
            _cte.EmissaoEm = model.EmitidaEm;
            _cte.PerfilCte = model.PerfilCte;
            _cte.PerfilCfop = model.PerfilCfop;
            _cte.TipoCte = model.TipoCte;
            _cte.TipoEmissao = model.TipoEmissao;
            _cte.Modal = model.Modal;
            _cte.TipoServico = model.TipoServico;
            _cte.ValorAverbacao = model.ValorAverbacao != null
                ? Convert.ToDecimal(model.ValorAverbacao.Value.ToString("N2"))
                : (decimal?) null;
            _cte.NaturezaOperacao = model.NaturezaOperacao;
            _cte.Observacao = model.InformacaoAdicional;
            _cte.DataInicio = model.DataInicioEm;
            _cte.DataFinal = model.DataFinalEm;
            _cte.HoraInicio = model.HoraProgramada;
            _cte.HoraFinal = model.HoraFinal;
            _cte.EstadoInicioOperacao = model.InicioEstado;
            _cte.EstadoFinalOperacao = model.FinalEstado;
            _cte.CidadeInicioOperacao = model.InicioCidade;
            _cte.CidadeFinalOperacao = model.FinalCidade;
            _cte.TipoPeriodoData = model.TipoPeriodoData;
            _cte.TipoPeriodoHora = model.TipoPeriodoHora;
            _cte.NomeProdutoPredominante = _cte.NomeProdutoPredominante.TrimOrEmpty();
            _cte.CaracteristicaProdutoPredominante = _cte.CaracteristicaProdutoPredominante.TrimOrEmpty();
            _cte.ChaveCTeComplementado = model.ChaveCteComplementar.TrimOrEmpty();
            _cte.ChaveCteAnulacao = model.ChaveCteAnulacao.TrimOrEmpty();
            _cte.DeclaracaoEmitidaEm = model.DeclaracaoEmitidaEm;

            _cte.CteRodoviario.Cte = _cte;
            _cte.CteRodoviario.Rntrc = model.Rntrc;

            _cte.ValorReceber = decimal.Round(model.ValorAReceber, 2);
            _cte.InformaValorServico(decimal.Round(model.ValorServico, 2));
            _cte.InformaCodigoIbpt(model.Nbs?.Codigo);
            _cte.NumeroFiscalEmissao = model.NumeroDocumento;
            _cte.SerieEmissao = model.SerieDocumento;

            _cte.CteEmitente = CteEmitente.Cria(_cte, model.PerfilCte.EmissorFiscal.Empresa);

            if (model.IsCteSubstituicao)
            {
                var id = _cte.IsSubstituto() ? _cte.CteSubstituicao.CteId : 0;

                _cte.CteSubstituicao = new CteSubstituicao
                {
                    Valor = model.SubstitutoValor,
                    Serie = model.SubstitutoSerie,
                    ChaveSubstituido = model.ChaveSubstituido,
                    ModeloDocumento = model.ModeloDocumento,
                    ChaveAnulacao = model.ChaveAnulacao,
                    ChaveCtePeloTomador = model.ChaveCtePeloTomador,
                    ChaveNfePeloTomador = model.ChaveNfePeloTomador,
                    Cte = _cte,
                    CteId = id,
                    DocumentoUnico = model.CpfCnpj,
                    EmitidoEm = model.SubstitutoEmitidaEm,
                    NumeroDocumentoFiscal = model.SubstitutoNumeroDocumentoFiscal,
                    Subserie = model.SubstitutoSubSerie
                };
            }

            if (model.IsCteSubstituicao == false)
            {
                _cte.CteSubstituicao = null;
            }

            return this;
        }

        public void ComInformacoes(AbaInformacoesCteModel model)
        {
            var remetente = model.Remetente;
            var destinatario = model.Destinatario;
            var expedidor = model.Expedidor;
            var recebedor = model.Recebedor;
            var tomador = model.Tomador;
            var tipoTomador = model.TipoTomador;

            var idExpedidor = _cte.CteExpedidor?.CteId ?? 0;
            var idRecebedor = _cte.CteRecebedor?.CteId ?? 0;

            _cte.CteRemetente = CteRemetente.Cria(_cte, remetente);
            _cte.CteDestinatario = CteDestinatario.Cria(_cte, destinatario);
            _cte.CteTomador = tomador != null ? CteTomador.Cria(_cte, tomador) : _cte.CteTomador;

            _cte.CteExpedidor = expedidor != null ? CteExpedidor.Cria(_cte, expedidor, idExpedidor) : null;
            _cte.CteRecebedor = recebedor != null ? CteRecebedor.Cria(_cte, recebedor, idRecebedor) : null;

            _cte.TipoTomador = tipoTomador;
        }

        public void ComTributacao(AbaTributacaoModel abaTributacaoModel)
        {
            _cte.CteConfigImposto = CriaConfigImposto(_cte, abaTributacaoModel);
            _cte.CteImpostoCst = CriaImpostoCst(_cte, abaTributacaoModel);
            _cte.CteImpostoDifal = CriaImpostoDifal(_cte, abaTributacaoModel);
        }

        private CteConfigImposto CriaConfigImposto(Cte cte, AbaTributacaoModel abaTributacaoModel)
        {
            var configImposto = new CteConfigImposto
            {
                Cte = cte,
                CteId = 0,
                IsCalculosAutomaticos = abaTributacaoModel.IsCalcularTributosAutomatico,
                IsCreditoIcmsAutomatico = abaTributacaoModel.IsCreditoAutomatico,
                IsPartilha = abaTributacaoModel.IsPartilhaIcms
            };

            if (cte.CteConfigImposto != null)
                configImposto.CteId = cte.Id;

            return configImposto;
        }

        private CteImpostoCst CriaImpostoCst(Cte cte, AbaTributacaoModel abaTributacaoModel)
        {
            var impostoCst = new CteImpostoCst
            {
                BaseCalculoIcms = abaTributacaoModel.BaseCalculoIcms,
                ValorCredito = abaTributacaoModel.ValorCredito,
                PercentualIcms = abaTributacaoModel.PercentualIcms,
                PercentualCredito = abaTributacaoModel.PercentualCredito,
                ValorIcms = abaTributacaoModel.ValorIcms,
                BaseCalculoIcmsSt = abaTributacaoModel.BaseCalculoIcmsSt,
                PercentualIcmsSt = abaTributacaoModel.PercentualIcmsSt,
                ValorIcmsSt = abaTributacaoModel.ValorIcmsSt,
                TributacaoIcms = abaTributacaoModel.TributacaoIcmsSelecionado,
                Cte = cte,
                PercentualReducaoBc = abaTributacaoModel.PercentualReducaoIcms
            };

            if (cte.CteImpostoCst != null)
                impostoCst.CteId = cte.Id;

            return impostoCst;
        }

        private CteImpostoDifal CriaImpostoDifal(Cte cte, AbaTributacaoModel abaTributacaoModel)
        {
            var impostoDifal = new CteImpostoDifal
            {
                PercentualFcp = abaTributacaoModel.PercentualFcp,
                Observacao = abaTributacaoModel.Observacao,
                ValorIcmsFcp = abaTributacaoModel.ValorIcmsFcp,
                BaseCalculo = abaTributacaoModel.PartilhaBaseCalculo,
                PercentualAliquotaInterestadual = abaTributacaoModel.PercentualInterestadual,
                PercentualAliquotaInterna = abaTributacaoModel.PercentualInternoUfTermino,
                PercentualProvisorio = abaTributacaoModel.PercentualPartilhaUfTermino,
                ValorIcmsUfInicio = abaTributacaoModel.ValorIcmsPartilhaUfInicio,
                ValorIcmsUfTermino = abaTributacaoModel.ValorIcmsPartilhaUfTermino,
                Cte = cte
            };

            if (cte.CteImpostoDifal != null)
                impostoDifal.CteId = cte.Id;

            return impostoDifal;
        }

        public CteBuilder ComDocumentosOriginarios(AbaDocumentosOriginariosModel model)
        {
            _cte.CteDocumentoImpressos.Clear();
            _cte.CteDocumentoNfes.Clear();
            _cte.CteDocumentoOutros.Clear();
            _cte.CteDocumentoAnteriores.Clear();

            MontaListaDeDocumentoImpressos(model);
            MontaListaDeDocumentoNfes(model);
            MontaListaDeDocumentoOutros(model);
            MontaListaDeDocumentosAnteriores(model);

            _cte.ValorTotalCarga = decimal.Parse(model.TotalCarga.ToString("N2"));
            _cte.CalcularTotalCargaAutomatico = model.CalcularTotalCargaAutomatico;

            _cte.InformaValorServico(model.Cabecalho.ValorServico);
            _cte.ValorAverbacao = model.Cabecalho.ValorAverbacao;
            _cte.ValorReceber = model.Cabecalho.ValorAReceber;

            return this;
        }

        public CteBuilder ComInformacoesCarga(AbaInformacoesCargaCteModel model)
        {
            _cte.CteInfoQuantidadeCargas.Clear();

            _cte.NomeProdutoPredominante = model.NomeProdutoPredominante.TrimOrEmpty();
            _cte.CaracteristicaProdutoPredominante = model.CaracteristicaProdutoPredominante.TrimOrEmpty();

            MontaListaInfoQuantidadeCarga(model);

            return this;
        }

        public CteBuilder ComVeiculosNovos(AbaInformacoesCargaCteModel model)
        {
            _cte.CteVeiculoTransportados.Clear();

            MontaListaDeVeiculoTransportados(model);

            return this;
        }

        private void MontaListaDeDocumentoImpressos(AbaDocumentosOriginariosModel model)
        {
            model?.ListaDocumentoImpressos.ForEach(di =>
            {
                var documentoImpresso = new CteDocumentoImpresso
                {
                    Cte = _cte,
                    BaseCalculoIcms = di.ValorBaseCalculoIcms,
                    BaseCalculoIcmsSt = di.ValorBaseCalculoIcmsSt,
                    PerfilCfop = di.PerfilCfop,
                    EmitidaEm = di.EmitidaEm,
                    ModeloNotaFiscal = di.ModeloNotaFiscal,
                    Numero = di.Numero,
                    NumeroPedido = di.NumeroPedidoNf,
                    NumeroRomaneiro = di.NumeroRomaneiro,
                    PinSuframa = di.PinSuframa,
                    PrevisaoEntregaEm = di.DataPrevistaEntrega,
                    Serie = di.Serie,
                    TotalBaseCalculoIcms = di.ValorTotalIcms,
                    TotalBaseCalculoIcmsSt = di.ValorTotalIcmsSt,
                    TotalNf = di.ValorTotalNf,
                    TotalPesoKg = di.PesoTotalEmKg,
                    TotalProdutos = di.ValorTotalProduto
                };

                _cte.CteDocumentoImpressos.Add(documentoImpresso);
            });
        }

        private void MontaListaDeDocumentoNfes(AbaDocumentosOriginariosModel model)
        {
            model?.ListaDocumentoNfe.ForEach(nfe =>
            {
                var documentoNfe = new CteDocumentoNfe
                {
                    Cte = _cte,
                    Chave = nfe.ChaveNfe,
                    PinSuframa = nfe.PinSuframa,
                    PrevisaoEntregaEm = nfe.PrevisaoEntregaEm,
                    Valor = nfe.TotalNFe
                };

                _cte.CteDocumentoNfes.Add(documentoNfe);
            });
        }

        private void MontaListaDeDocumentoOutros(AbaDocumentosOriginariosModel model)
        {
            model?.ListaDocumentoOutroDocumento.ForEach(od =>
            {
                var documentoOutro = new CteDocumentoOutro
                {
                    Cte = _cte,
                    DescricaoOutro = od.DescricaoOutros,
                    EmitidoEm = od.EmitidoEm,
                    Numero = od.Numero,
                    PrevisaoEntregaEm = od.PrevisaoEntregaEm,
                    TipoDocumento = od.TipoDocumento,
                    Valor = od.ValorTotal
                };

                _cte.CteDocumentoOutros.Add(documentoOutro);
            });
        }

        private void MontaListaDeDocumentosAnteriores(AbaDocumentosOriginariosModel model)
        {
            model?.ListaDocumentoAnterior.ForEach(dt =>
            {
                _cte.CteDocumentoAnteriores.Add(dt.CteDocumentoAnterior);
            });
        }

        private void MontaListaInfoQuantidadeCarga(AbaInformacoesCargaCteModel model)
        {
            model?.ListaCarga.ForEach(c =>
            {
                var infoCarga = new CteInfoQuantidadeCarga
                {
                    Cte = _cte,
                    Quantidade = c.Quantidade,
                    TipoUnidadeMedidaDescricao = c.TipoMedida ?? string.Empty,
                    UnidadeMedida = c.UnidadeMedida
                };

                _cte.CteInfoQuantidadeCargas.Add(infoCarga);
            });
        }

        private void MontaListaDeVeiculoTransportados(AbaInformacoesCargaCteModel model)
        {
            model.ListaVeiculoParaTransporte.ForEach(t =>
            {
                var veiculo = new CteVeiculoTransportado
                {
                    Cte = _cte,
                    Chassi = t.Chassi,
                    CodigoMarcaModelo = t.CodigoMarcaModelo,
                    Cor = t.Cor,
                    DescricaoCor = t.DescricaoCor,
                    FreteUnitario = t.FreteUnitario,
                    ValorUnitario = t.ValorUnitario
                };

                _cte.CteVeiculoTransportados.Add(veiculo);
            });
        }

        public decimal ObterValorTotalCarga()
        {
            return _cte.ValorTotalCarga;
        }

        public Cte Construir()
        {
            return _cte;
        }
    }
}