using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Fusion.Visao.CteEletronico.Builder;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;
using Fusion.Visao.CteEletronico.Emitir.DocAnt;
using Fusion.Visao.CteEletronico.Emitir.Emissao;
using Fusion.Visao.CteEletronico.Emitir.EntidadesModels;
using Fusion.Visao.CteEletronico.Emitir.EntidadesModels.DocAnt;
using Fusion.Visao.CteEletronico.Emitir.Flyouts.Models;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.FusionAdm.CteEletronico.Autorizador;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.Helper.Diversos;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Flags;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.CteEletronico.Emitir
{
    public sealed class CteEmitirFormModel : ViewModel
    {
        private readonly Cte _cte;
        private AbaPerfilCteModel _abaPerfilModel;
        private AbaCabecalhoCteModel _abaCabecalhoCteModel;
        private AbaInformacoesCteModel _abaInformacoesCteModel;
        private AbaDocumentosOriginariosModel _abaDocumentosOriginariosModel;
        private AbaInformacoesCargaCteModel _abaInformacoesCargaCteModel;
        private FlyoutAdicionarNfeModel _flyoutAdicionarNfeModel;
        private FlyoutAdicionarNotaFiscalImpressaModel _flyoutAdicionarNotaFiscalImpressaModel;
        private FlyoutAdicionarOutroDocumentoModel _flyoutAdicionarOutroDocumentoModel;
        private FlyoutInformacaoCargaModel _flyoutInformacaoCargaModel;
        private FlyoutAddVeiculoParaTransporteModel _flyoutAddVeiculoParaTransporteModel;
        private FlyoutAbaInformacoesImportacaoCteModel _flyoutAbaInformacoesImportacaoCteModel;
        private AbaTributacaoModel _abaTributacaoModel;
        private FlyoutAddComponenteValorPrestacaoModel _flyoutAddComponenteValorPrestacaoModel;
        private bool _edicaoCte;
        private CteBuilder CteBuilder { get; }
        private CteFacade CteFacade { get; }


        public FlyoutAddComponenteValorPrestacaoModel FlyoutAddComponenteValorPrestacaoModel
        {
            get => _flyoutAddComponenteValorPrestacaoModel;
            set
            {
                if (Equals(value, _flyoutAddComponenteValorPrestacaoModel)) return;
                _flyoutAddComponenteValorPrestacaoModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAbaInformacoesImportacaoCteModel FlyoutAbaInformacoesImportacaoCteModel
        {
            get => _flyoutAbaInformacoesImportacaoCteModel;
            set
            {
                if (Equals(value, _flyoutAbaInformacoesImportacaoCteModel)) return;
                _flyoutAbaInformacoesImportacaoCteModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddVeiculoParaTransporteModel FlyoutAddVeiculoParaTransporteModel
        {
            get => _flyoutAddVeiculoParaTransporteModel;
            set
            {
                if (Equals(value, _flyoutAddVeiculoParaTransporteModel)) return;
                _flyoutAddVeiculoParaTransporteModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutInformacaoCargaModel FlyoutInformacaoCargaModel
        {
            get => _flyoutInformacaoCargaModel;
            set
            {
                if (Equals(value, _flyoutInformacaoCargaModel)) return;
                _flyoutInformacaoCargaModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaInformacoesCargaCteModel AbaInformacoesCargaCteModel
        {
            get => _abaInformacoesCargaCteModel;
            set
            {
                if (Equals(value, _abaInformacoesCargaCteModel)) return;
                _abaInformacoesCargaCteModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAdicionarOutroDocumentoModel FlyoutAdicionarOutroDocumentoModel
        {
            get => _flyoutAdicionarOutroDocumentoModel;
            set
            {
                if (Equals(value, _flyoutAdicionarOutroDocumentoModel)) return;
                _flyoutAdicionarOutroDocumentoModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAdicionarNotaFiscalImpressaModel FlyoutAdicionarNotaFiscalImpressaModel
        {
            get => _flyoutAdicionarNotaFiscalImpressaModel;
            set
            {
                if (Equals(value, _flyoutAdicionarNotaFiscalImpressaModel)) return;
                _flyoutAdicionarNotaFiscalImpressaModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAdicionarNfeModel FlyoutAdicionarNfeModel
        {
            get => _flyoutAdicionarNfeModel;
            set
            {
                if (Equals(value, _flyoutAdicionarNfeModel)) return;
                _flyoutAdicionarNfeModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaDocumentosOriginariosModel AbaDocumentosOriginariosModel
        {
            get => _abaDocumentosOriginariosModel;
            set
            {
                if (Equals(value, _abaDocumentosOriginariosModel)) return;
                _abaDocumentosOriginariosModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaInformacoesCteModel AbaInformacoesCteModel
        {
            get => _abaInformacoesCteModel;
            set
            {
                if (Equals(value, _abaInformacoesCteModel)) return;
                _abaInformacoesCteModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaCabecalhoCteModel AbaCabecalhoCteModel
        {
            get => _abaCabecalhoCteModel;
            set
            {
                if (Equals(value, _abaCabecalhoCteModel)) return;
                _abaCabecalhoCteModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaPerfilCteModel AbaPerfilModel
        {
            get => _abaPerfilModel;
            set
            {
                if (Equals(value, _abaPerfilModel)) return;
                _abaPerfilModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaTributacaoModel AbaTributacaoModel
        {
            get => _abaTributacaoModel;
            set
            {
                if (Equals(value, _abaTributacaoModel)) return;
                _abaTributacaoModel = value;
                PropriedadeAlterada();
            }
        }

        public List<RetornoImportacaoXmlCteEventArgs> HistoricoImportacao { get; set; } =
            new List<RetornoImportacaoXmlCteEventArgs>();

        public CteEmitirFormModel(Cte cte)
        {
            _cte = cte;

            CteBuilder = new CteBuilder(_cte);
            CteFacade = new CteFacade(CteBuilder);

            FlyoutAdicionarNfeModel = new FlyoutAdicionarNfeModel();
            FlyoutAdicionarNotaFiscalImpressaModel = new FlyoutAdicionarNotaFiscalImpressaModel();
            FlyoutAdicionarOutroDocumentoModel = new FlyoutAdicionarOutroDocumentoModel();
            FlyoutInformacaoCargaModel = new FlyoutInformacaoCargaModel();
            FlyoutAddVeiculoParaTransporteModel = new FlyoutAddVeiculoParaTransporteModel();

            AbaPerfilModel = new AbaPerfilCteModel();
            AbaCabecalhoCteModel = new AbaCabecalhoCteModel();
            AbaInformacoesCteModel = new AbaInformacoesCteModel(_cte);
            AbaDocumentosOriginariosModel = new AbaDocumentosOriginariosModel(CteFacade, AbaCabecalhoCteModel);
            AbaInformacoesCargaCteModel = new AbaInformacoesCargaCteModel(CteFacade);
            AbaTributacaoModel = new AbaTributacaoModel();

            AbaDocumentosOriginariosModel.AdicionaNfeCall += AdicionaDocumentoNFe;
            AbaDocumentosOriginariosModel.AdicionaNfImpressaCall += AdicionarNfImpressa;
            AbaDocumentosOriginariosModel.AdicionaNfOutroDocumentoCall += AdicionarOutrosDocumento;
            AbaDocumentosOriginariosModel.AdicionaDocumentoAnteriorCall += AdicioanrDocumentoAnteriorCompleted;
            AbaDocumentosOriginariosModel.ProximoPasso += ProximoPassoInformacoesCarga;
            AbaDocumentosOriginariosModel.PassoAnterior += PassoAnteriorAbaInformacoesCte;
            AbaDocumentosOriginariosModel.AdicionaComponenteValorPrestacaoCall +=
                AdicionarComponenteValorPrestacaoCompleted;

            AbaTributacaoModel.Visivel = true;
            AbaTributacaoModel.AnteriorHandler += AnteriorDocumentosOriginariosClick;
            AbaTributacaoModel.ProximoHandler += ProximoInformacoesCargaClick;
            AbaInformacoesCargaCteModel.Header = AbaInformacoesCargaCteModel.HeaderPasso6;

            AbaInformacoesCargaCteModel.AdicionarInformacaoCargaCall += AdicionarInformacaoCarga;
            AbaInformacoesCargaCteModel.AdicionarVeiculoNovoCall += AdicionarVeiculoParaTransporte;
            AbaInformacoesCargaCteModel.PassoAnterior += AnteriorAbaDocumentosOriginarios;
            AbaInformacoesCargaCteModel.EmitirCte += EmitirCte;

            AbaPerfilModel.PerfilSelecionado += PerfilSelecionado;
            AbaCabecalhoCteModel.ProximoPasso += ProximoPassoRemetenteDestinatario;
            AbaCabecalhoCteModel.DeletaSubstituto += DeletaSubsituto;
            AbaCabecalhoCteModel.AbrirFlyoutImportarXmlEventHandler += ImportarXmlInformacoesCte;
            AbaCabecalhoCteModel.AlocarNumeracaoNovaParaCTe += AlocarNumeracaoNova;
            AbaInformacoesCteModel.ProximoPasso += ProximoPassoInformacaoDocumentos;
            AbaInformacoesCteModel.PassoAnterior += PassoAnteriorCabecalho;

            FlyoutAdicionarNfeModel.AdicionaDocumentoNfe += AdicionarDocumentoNfe;
            FlyoutAdicionarNotaFiscalImpressaModel.AdicionarNotaFiscalImpressa += AdicionarNotaFiscalImpressa;
            FlyoutAdicionarOutroDocumentoModel.AdicionarOutroDocumento += AdicionarOutrosDocumentos;

            FlyoutInformacaoCargaModel.AdicionarInformacaoCarga += AdicionarCarga;
            FlyoutAddVeiculoParaTransporteModel.AdicionaVeiculoParaTransporte += RetornoVeiculoDeTransporte;

            if (_cte.Id != 0)
            {
                AtualizaModels(_cte.PerfilCte.EmissorFiscal.Empresa.RegimeTributario);
            }
        }

        private void AlocarNumeracaoNova(object sender, EventArgs e)
        {
            var cte = CteBuilder.Construir();

            new AlocarNumeracaoCTe(cte).AlocarNumeroFiscal();
            AbaCabecalhoCteModel.AtualizaNumeracao(cte);
        }

        private void DeletaSubsituto(object sender, TipoCte e)
        {
            if (e != TipoCte.CteDeSubstituicao)            
                CteFacade.DeletaCteSubstituto();    
        }

        private void AdicionarComponenteValorPrestacaoCompleted(object sender, EventArgs e)
        {
            FlyoutAddComponenteValorPrestacaoModel = new FlyoutAddComponenteValorPrestacaoModel
            {
                IsOpen = true
            };
            FlyoutAddComponenteValorPrestacaoModel.SalvarComponenteValorPrestacaoHandler +=
                SalvarComponenteValorPrestacaoAction;
        }

        private void SalvarComponenteValorPrestacaoAction(object sender, FlyoutAddComponenteValorPrestacaoModel e)
        {
            var componente = SalvarComponente(e);
            AbaDocumentosOriginariosModel.AdicionarComponente(GridComponenteValorPrestacao.Cria(componente));
        }

        private CteComponenteValorPrestacao SalvarComponente(FlyoutAddComponenteValorPrestacaoModel model)
        {
            var componente = new CteComponenteValorPrestacao
            {
                Cte = CteBuilder.Construir(),
                Nome = model.NomeDoComponente.TrimOrEmpty(),
                Valor = model.ValorDoComponente.Arredonda(2)
            };

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                CteFacade.SalvarComponente(sessao, componente);

                transacao.Commit();
            }

            return componente;
        }

        private void ProximoInformacoesCargaClick(object sender, AbaTributacaoModel e)
        {
            if (e.TributacaoIcmsSelecionado == null)
                throw new InvalidOperationException("Selecionar um CST");

            CteBuilder.ComTributacao(e);

            SalvarCte();

            AbaInformacoesCargaCteModel.Habilitado = true;
            AbaInformacoesCargaCteModel.Selecionado = true;
        }

        private void AnteriorDocumentosOriginariosClick(object sender, EventArgs e)
        {
            AbaDocumentosOriginariosModel.Selecionado = true;
        }

        private void ImportarXmlCompleted(object sender, RetornoImportacaoXmlCteEventArgs e)
        {
            FlyoutAbaInformacoesImportacaoCteModel = null;

            if (e.IsImportarDocumentoNFe)
            {
                AbaDocumentosOriginariosModel.VerificaSeChaveExisteJa(e.Chave);

                if (HistoricoImportacao.Any(x => x.Chave == e.Chave))
                {
                    throw new InvalidOperationException("Chave de NF-e já existe na lista de importação");
                }
            }

            HistoricoImportacao.Add(e);
            AbaCabecalhoCteModel.ImportacaoXml(e);
            AbaInformacoesCteModel.ImportacaoXml(e);
        }

        private void ImportarXmlInformacoesCte(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = @"Arquivos XML|*.xml"
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (dialog.CheckFileExists == false)
            {
                DialogBox.MostraAviso("Não encontrei o XML informado");
                return;
            }

            try
            {
                FlyoutAbaInformacoesImportacaoCteModel = new FlyoutAbaInformacoesImportacaoCteModel();
                FlyoutAbaInformacoesImportacaoCteModel.CarregarXml(dialog.FileName);
                FlyoutAbaInformacoesImportacaoCteModel.ImportarXmlEventHandler += ImportarXmlCompleted; 
                FlyoutAbaInformacoesImportacaoCteModel.IsOpen = true;
            }
            catch (RegraNegocioException ex)
            {
                DialogBox.MostraAviso(ex.JoinMessages(Environment.NewLine));
            }
        }

        public event EventHandler FecharTela;

        private void EmitirCte(object sender, RetornoInformacoesCarga e)
        {
            try
            {
                if (e.AbaInformacoesCargaCteModel.ListaCarga.Count == 0 
                    && CteBuilder.Construir().IsNormal()) 
                {
                    DialogBox.MostraInformacao("Adicionar no mínimo uma carga");
                    return;
                }                

                CteBuilder.ComInformacoesCarga(e.AbaInformacoesCargaCteModel);
                CteBuilder.ComVeiculosNovos(e.AbaInformacoesCargaCteModel); 

                AbaCabecalhoCteModel.Validacao();
                AbaInformacoesCteModel.Validacao();

                SalvarCte();

                var cte = CteBuilder.Construir();

                var regimeTributario = cte.PerfilCte.EmissorFiscal.Empresa.RegimeTributario;

                if (regimeTributario != RegimeTributario.SimplesNacional)
                {
                    if (cte.CteImpostoCst == null)
                    {
                        throw new ArgumentException("Adicionar um CST Tributação");
                    }
                }

                if (cte.EmissaoEm <= DateTime.Now.AddMonths(-12)) throw new ArgumentException("Emissão Em muito antigo não e permitido");

                Refresh(cte);

                var emissaoSefazCteViewModel = new EmissaoSefazCteViewModel(cte, new SessaoManagerAdm());

                emissaoSefazCteViewModel.AtualizarNumeracao += delegate(object o, Cte cte1)
                {
                    AbaCabecalhoCteModel.AtualizaNumeracao(cte1);
                };

                emissaoSefazCteViewModel.Autorizado += (o, cte1) =>
                {
                    var opcoes = new CteEletronicaOpcoes(cte1);
                    opcoes.ShowDialog();
                    
                    OnFecharTela();
                };

                var emissaoSefazCte = new EmissaoSefazCteView(emissaoSefazCteViewModel);
                emissaoSefazCte.Closed += (o, args) =>
                {
                    if (EdicaoCte == false)
                        OnFecharTela();
                };

                emissaoSefazCte.ShowDialog();

                EdicaoCte = true;
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void Refresh(Cte cteRefresh)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                new RepositorioCte(sessao).Refresh(cteRefresh);
            }
        }

        private void AnteriorAbaDocumentosOriginarios(object sender, EventArgs e)
        {
            var regimeTributario = CteBuilder.Construir().CteEmitente.Emitente.RegimeTributario;

            if (regimeTributario != RegimeTributario.SimplesNacional)
            {
                AbaTributacaoModel.SetBaseCalculoPadrao(CteBuilder.Construir().ValorServico);
                AbaTributacaoModel.Selecionado = true;
                return;
            }

            AbaDocumentosOriginariosModel.Selecionado = true;
        }

        private void PassoAnteriorAbaInformacoesCte(object sender, EventArgs e)
        {
            AbaInformacoesCteModel.Selecionado = true;
        }

        private void RetornoVeiculoDeTransporte(object sender, RetornoVeiculoParaTransporte e)
        {
            var veiculoTransportado = SalvarVeiculoTransportado(e.FlyoutAddVeiculoParaTransporteModel);
            AbaInformacoesCargaCteModel.AdicionaVeiculoDeTransporte(GridVeiculoParaTransporteModel.Cria(veiculoTransportado));
        }

        private void AdicionarCarga(object sender, RetornoFlyoutInformacaoCarga e)
        {
            var infoCarga = new CteInfoQuantidadeCarga
            {
                Cte = CteBuilder.Construir(),
                Quantidade = e.FlyoutInformacaoCargaModel.Quantidade,
                TipoUnidadeMedidaDescricao = e.FlyoutInformacaoCargaModel.TipoMedida,
                UnidadeMedida = e.FlyoutInformacaoCargaModel.UnidadeMedida
            };

            SalvarInfoCarga(infoCarga);
            AbaInformacoesCargaCteModel.AdicionaCarga(GridInformacaoCarga.Cria(infoCarga));
        }

        private void AdicionarOutrosDocumentos(object sender, RetornoOutroDocumento e)
        {
            var documentoOutro = SalvarDocumentoOutro(e.FlyoutAdicionarOutroDocumentoModel);
            AbaDocumentosOriginariosModel.AdicionarDocumentoOutroDocumento(GridOutroDocumentoModel.Cria(documentoOutro));
        }

        private void AdicionarNotaFiscalImpressa(object sender, RetornoNotaFiscalImpressa e)
        {
            var documentoImpresso = SalvarDocumentoImpresso(e.FlyoutAdicionarNotaFiscalImpressaModel);
            AbaDocumentosOriginariosModel.AdicionarDocumentoNotaFiscalImpressa(
                GridDocumentoNotaFiscalImpressaModel.Cria(documentoImpresso));
        }

        private void AdicionarDocumentoNfe(object sender, RetornoFlyoutNfe e)
        {
            AbaDocumentosOriginariosModel.VerificaSeChaveExisteJa(e.FlyoutAdicionarNfeModel.ChaveNfe);

            var documentoNfe = SalvarDocumentoNfe(e.FlyoutAdicionarNfeModel);
            AbaDocumentosOriginariosModel.AdicionarDocumentoNfe(GridDocumentoNfeModel.Cria(documentoNfe));
        }

        private void AdicionarVeiculoParaTransporte(object sender, EventArgs e)
        {
            FlyoutAddVeiculoParaTransporteModel.LimaCampos();
            FlyoutAddVeiculoParaTransporteModel.IsOpen = true;
        }

        private void AdicionarInformacaoCarga(object sender, EventArgs e)
        {
            FlyoutInformacaoCargaModel.LimpaCampos();
            FlyoutInformacaoCargaModel.IsOpen = true;
        }

        private void ProximoPassoInformacoesCarga(object sender, RetornaDocumentos e)
        {
            CteBuilder.ComDocumentosOriginarios(e.AbaDocumentosOriginariosModel);
            AtualizaBotaoSubcontratacaoAbaDocumentoOriginarios(CteBuilder.Construir());
            AbaInformacoesCargaCteModel.ValorTotalCarga = CteBuilder.ObterValorTotalCarga();

            SalvarCte();

            var regimeTributario = CteBuilder.Construir().CteEmitente.Emitente.RegimeTributario;

            if (regimeTributario != RegimeTributario.SimplesNacional)
            {
                AbaTributacaoModel.Habilitado = true;
                AbaTributacaoModel.Selecionado = true;
                return;
            }

            AbaInformacoesCargaCteModel.Habilitado = true;
            AbaInformacoesCargaCteModel.Selecionado = true;
        }

        private void CteComplementarDefaultNomeProduto()
        {
            var cte = CteBuilder.Construir();
            AbaInformacoesCargaCteModel.IsNaoEComplementar = cte.IsNormal();
            if (cte.IsNormal()) return;

            AbaInformacoesCargaCteModel.NomeProdutoPredominante = string.Empty;
            AbaInformacoesCargaCteModel.CaracteristicaProdutoPredominante = string.Empty;
        }

        private void AdicionarOutrosDocumento(object sender, EventArgs e)
        {
            if (AbaDocumentosOriginariosModel.ListaDocumentoImpressos.Count > 0 ||
                AbaDocumentosOriginariosModel.ListaDocumentoNfe.Count > 0)
                throw new InvalidOperationException("Somente pode ter um único tipo de documento por CT-e");

            if (AbaDocumentosOriginariosModel.ListaDocumentoOutroDocumento.Count > 2000)
                throw new InvalidOperationException("Não pode ter mais de 2000 documentos por CT-e");

            FlyoutAdicionarOutroDocumentoModel.LimpaCampos();
            FlyoutAdicionarOutroDocumentoModel.IsOpen = true;
        }

        private void AdicioanrDocumentoAnteriorCompleted(object sender, EventArgs e)
        {
            var model = new DocumentoAnteriorFormModel();

            model.DocumentoAnteriorHandler += SalvarDocumentoAnterior;

            new DocumentoAnteriorForm(model).ShowDialog();
        }

        private void SalvarDocumentoAnterior(object sender, DocumentoAnteriorRetorno e)
        {
            var model = e.Model;

            var documentoAnterior = new CteDocumentoAnterior
            {
                NomeOuRazaoSocial = model.NomeOuRazaoSocial,
                InscricaoEstadual = model.InscricaoEstadual,
                DocumentoUnico = model.DocumentoUnico,
                EstadoUf = model.EstadoUf,
                Cte = CteBuilder.Construir()
            };

            e.Model.GridDocumentosTransportes.ForEach(dt =>
            {
                documentoAnterior.Documentos.Add(new CteDocumentoTransporte
                {
                    ChaveCTe = dt.ChaveCTe,
                    TipoDocumentoAnterior = dt.TipoDocumentoAnterior,
                    CteDocumentoAnterior = documentoAnterior,
                    Serie = dt.Serie,
                    EmissaoEm = dt.DataDeEmissao,
                    SubSerie = dt.SubSerie,
                    NumeroDocumentoFiscal = dt.NumeroDocumentoFiscal
                });
            });


            var repositorio = new RepositorioCte(SessaoHelperFactory.AbrirSessaoAdm());

            using (repositorio)
            {
                repositorio.SalvarDocumentoAnteior(documentoAnterior);
            }

            AbaDocumentosOriginariosModel.AdicionarDocumentoAnterior(GridDocumentoAnterior.Cria(documentoAnterior));
        }

        private void AdicionarNfImpressa(object sender, EventArgs e)
        {
            if (AbaDocumentosOriginariosModel.ListaDocumentoNfe.Count > 0 ||
                AbaDocumentosOriginariosModel.ListaDocumentoOutroDocumento.Count > 0)
                throw new InvalidOperationException("Somente pode ter um único tipo de documento por CT-e");

            if (AbaDocumentosOriginariosModel.ListaDocumentoImpressos.Count > 2000)
                throw new InvalidOperationException("Não pode ter mais de 2000 documentos por CT-e");

            FlyoutAdicionarNotaFiscalImpressaModel.LimpaCampos();
            FlyoutAdicionarNotaFiscalImpressaModel.IsOpen = true;
        }

        private void AdicionaDocumentoNFe(object sender, EventArgs e)
        {
            if (AbaDocumentosOriginariosModel.ListaDocumentoImpressos.Count > 0 ||
                AbaDocumentosOriginariosModel.ListaDocumentoOutroDocumento.Count > 0)
                throw new InvalidOperationException("Somente pode ter um único tipo de documento por CT-e");

            if (AbaDocumentosOriginariosModel.ListaDocumentoNfe.Count > 2000)
                throw new InvalidOperationException("Não pode ter mais de 2000 documentos por CT-e");


            FlyoutAdicionarNfeModel.LimpaCampos();
            FlyoutAdicionarNfeModel.IsOpen = true;
        }

        private void PassoAnteriorCabecalho(object sender, EventArgs e)
        {
            AbaCabecalhoCteModel.Selecionado = true;
        }

        private void ProximoPassoInformacaoDocumentos(object sender, RetornoAbaInformacoes e)
        {
            CteBuilder.ComInformacoes(e.AbaInformacoesCteModel);

            SalvarCabecalho();

            var cte = CteBuilder.Construir();

            var listaGrid = SalvarNfeDoHistorico();

            listaGrid.ForEach(AbaDocumentosOriginariosModel.AdicionarDocumentoNfe);

            if (cte.PerfilCte.DocumentoPadrao 
                && AbaDocumentosOriginariosModel.TodosDocumentosVazios() 
                && !listaGrid.Any()
                && cte.IsNormal())
            {
                var documentoOutro = new CteDocumentoOutro
                {
                    Cte = CteBuilder.Construir(),
                    DescricaoOutro = "OUTROS DOCUMENTOS",
                    Valor = CteBuilder.Construir().ValorServico,
                    Numero = string.Empty,
                    TipoDocumento = TipoDocumento.Outros
                };

                SalvarDocumentoOutro(documentoOutro);

                AbaDocumentosOriginariosModel.AdicionarDocumentoOutroDocumento(GridOutroDocumentoModel.Cria(documentoOutro));

                AbaDocumentosOriginariosModel.TotalCarga = CteBuilder.Construir().ValorServico;
            }

            AbaDocumentosOriginariosModel.IsNaoEComplementar = cte.IsNormal();
            AbaDocumentosOriginariosModel.CalcularTotalCargaAutomatico =
                CteBuilder.Construir().CalcularTotalCargaAutomatico;
            AbaDocumentosOriginariosModel.Habilitado = true;
            AbaDocumentosOriginariosModel.Selecionado = true;

            AtualizaBotaoSubcontratacaoAbaDocumentoOriginarios(cte);

            AbaDocumentosOriginariosModel.AtualizaTotalCarga();
        }

        private void AtualizaBotaoSubcontratacaoAbaDocumentoOriginarios(Cte cte)
        {
            AbaDocumentosOriginariosModel.IsSubcontratacao =
                cte.TipoServico == TipoServico.Subcontratacao || cte.CteDocumentoAnteriores.Count != 0;

            if (cte.TipoServico == TipoServico.Normal && cte.CteDocumentoAnteriores.Count == 0)
                AbaDocumentosOriginariosModel.IsSubcontratacao = false;
        }

        private IEnumerable<GridDocumentoNfeModel> SalvarNfeDoHistorico()
        {
            var documentos = new List<GridDocumentoNfeModel>();

            HistoricoImportacao.ForEach(h =>
            {
                if (!h.IsImportarDocumentoNFe) return;

                var docNfe = CriaDocumentoNFe(h);

                documentos.Add(docNfe);
            });

            HistoricoImportacao.Clear();

            return documentos;
        }

        private GridDocumentoNfeModel CriaDocumentoNFe(RetornoImportacaoXmlCteEventArgs historico)
        {
            var documentoNfe = new CteDocumentoNfe
            {
                Cte = CteBuilder.Construir(),
                Chave = historico.Chave,
                Valor = historico.Total
            };

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCte(sessao);

                repositorio.SalvarDocumentoNfe(documentoNfe);

                transacao.Commit();
            }

            var gridNfeModel = GridDocumentoNfeModel.Cria(documentoNfe);

            return gridNfeModel;
        }

        private void PerfilSelecionado(object sender, PerfilCteSelecionado e)
        {
            AbaCabecalhoCteModel.PreencherCom(e.PerfilCte);
            AbaInformacoesCteModel.IsUsarRemetenteComoDefault = e.PerfilCte.RemetentePadrao;
            AbaInformacoesCargaCteModel.NomeProdutoPredominante = e.PerfilCte.ProdutoPredominante;
    
            AbaCabecalhoCteModel.Habilitado = true;
            AbaCabecalhoCteModel.Selecionado = true;
            AbaCabecalhoCteModel.InicioEstado = e.PerfilCte.EmissorFiscal.Empresa.EstadoDTO;
            AbaCabecalhoCteModel.InicioCidade = e.PerfilCte.EmissorFiscal.Empresa.CidadeDTO;

            var regimeEmpresa = e.PerfilCte.EmissorFiscal.Empresa.RegimeTributario;
            if (regimeEmpresa != RegimeTributario.SimplesNacional)
            {
                AbaTributacaoModel.Visivel = true;
                AbaInformacoesCargaCteModel.Header = AbaInformacoesCargaCteModel.HeaderPasso6;
                return;
            }

            AbaTributacaoModel.AnteriorHandler -= AnteriorDocumentosOriginariosClick;
            AbaTributacaoModel.ProximoHandler -= ProximoInformacoesCargaClick;
            AbaTributacaoModel.Visivel = false;
            AbaInformacoesCargaCteModel.Header = AbaInformacoesCargaCteModel.HeaderPasso5;
        }

        private void ProximoPassoRemetenteDestinatario(object sender, EventArgs e)
        {
            CteBuilder.ComCabecalho(AbaCabecalhoCteModel);
            AbaInformacoesCteModel.Habilitado = true;
            AbaInformacoesCteModel.Selecionado = true;
        }

        private void SalvarCabecalho()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                CteFacade.SalvarCabecalho(sessao);
                transacao.Commit();
            }
        }

        private CteDocumentoNfe SalvarDocumentoNfe(FlyoutAdicionarNfeModel model)
        {
            var documentoNfe = new CteDocumentoNfe
            {
                Cte = CteBuilder.Construir(),
                Valor = model.TotalNFe,
                Chave = model.ChaveNfe,
                PinSuframa = model.PinSuframa,
                PrevisaoEntregaEm = model.PrevisaoEntregaEm
            };

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                CteFacade.SalvarDocumentoNfe(sessao, documentoNfe);

                transacao.Commit();
            }

            return documentoNfe;
        }

        private CteDocumentoImpresso SalvarDocumentoImpresso(FlyoutAdicionarNotaFiscalImpressaModel model)
        {
            var documentoImpresso = new CteDocumentoImpresso
            {
                BaseCalculoIcms = model.ValorBaseCalculoIcms,
                BaseCalculoIcmsSt = model.ValorBaseCalculoIcmsSt,
                Cte = CteBuilder.Construir(),
                EmitidaEm = model.EmitidaEm,
                ModeloNotaFiscal = model.ModeloNotaFiscal,
                Numero = model.Numero,
                NumeroPedido = model.NumeroPedidoNf,
                NumeroRomaneiro = model.NumeroRomaneiro,
                PerfilCfop = model.PerfilCfop,
                PinSuframa = model.PinSuframa,
                PrevisaoEntregaEm = model.DataPrevistaEntrega,
                Serie = model.Serie,
                TotalBaseCalculoIcms = model.ValorTotalIcms,
                TotalProdutos = model.ValorTotalProduto,
                TotalNf = model.ValorTotalNf,
                TotalBaseCalculoIcmsSt = model.ValorBaseCalculoIcmsSt,
                TotalPesoKg = model.PesoTotalEmKg
            };

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                CteFacade.SalvarDocumentoImpresso(sessao, documentoImpresso);

                transacao.Commit();
            }

            return documentoImpresso;
        }

        private CteDocumentoOutro SalvarDocumentoOutro(FlyoutAdicionarOutroDocumentoModel model)
        {
            var documentoOutro = new CteDocumentoOutro
            {
                Numero = model.Numero,
                Cte = CteBuilder.Construir(),
                DescricaoOutro = model.DescricaoOutros,
                EmitidoEm = model.EmitidoEm,
                PrevisaoEntregaEm = model.PrevisaoEntregaEm,
                TipoDocumento = model.TipoDocumento,
                Valor = model.ValorTotal
            };


            SalvarDocumentoOutro(documentoOutro);

            return documentoOutro;
        }

        private void SalvarDocumentoOutro(CteDocumentoOutro documentoOutro)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                CteFacade.SalvarDocumentoOutro(sessao, documentoOutro);

                transacao.Commit();
            }
        }

        private void SalvarInfoCarga(CteInfoQuantidadeCarga infoCarga)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                CteFacade.SalvarInfoCarga(sessao, infoCarga);

                transacao.Commit();
            }
        }

        private void SalvarCte()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                CteFacade.SalvarCte(sessao, CteBuilder.Construir());
                transacao.Commit();
            }

            AbaTributacaoModel.SetBaseCalculoPadrao(CteBuilder.Construir().ValorServico);
        }

        public void SalvarCteAba()
        {
            var cte = CteBuilder.Construir();

            if (cte.Id == 0)
            {
                return;
            }

            CteBuilder.ComCabecalho(AbaCabecalhoCteModel);
            CteBuilder.ComInformacoes(AbaInformacoesCteModel);

            if (AbaDocumentosOriginariosModel.Habilitado)
            {
                CteBuilder.ComDocumentosOriginarios(AbaDocumentosOriginariosModel);
            }

            if (AbaInformacoesCargaCteModel.Habilitado)
            {
                CteBuilder.ComInformacoesCarga(AbaInformacoesCargaCteModel);
            }

            SalvarCte();
        }

        private CteVeiculoTransportado SalvarVeiculoTransportado(FlyoutAddVeiculoParaTransporteModel model)
        {
            var veiculo = new CteVeiculoTransportado
            {
                Chassi = model.Chassi,
                CodigoMarcaModelo = model.CodigoMarcaModelo,
                Cor = model.Cor,
                Cte = CteBuilder.Construir(),
                DescricaoCor = model.DescricaoCor,
                FreteUnitario = model.FreteUnitario,
                ValorUnitario = model.ValorUnitario
            };

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                CteFacade.SalvarVeiculoTransportado(sessao, veiculo);

                transacao.Commit();
            }

            return veiculo;
        }

        private void AtualizaModels(RegimeTributario regimeTributario)
        {
            AbaCabecalhoCteModel.Habilitado = true;
            AbaCabecalhoCteModel.Selecionado = true;

            AbaCabecalhoCteModel.PreencherCom(_cte);
            AbaInformacoesCteModel.PreencerCom(_cte);
            AbaDocumentosOriginariosModel.PreencerCom(_cte);
            AbaInformacoesCargaCteModel.PreencerCom(_cte);
            AbaTributacaoModel.Visivel = true;
            AbaTributacaoModel.PreencherCom(_cte);

            AbaInformacoesCteModel.Habilitado = true;
            AbaDocumentosOriginariosModel.Habilitado = true;
            AbaInformacoesCargaCteModel.Habilitado = true;
            AbaTributacaoModel.Habilitado = true;

            if (regimeTributario == RegimeTributario.SimplesNacional)
            {
                AbaInformacoesCargaCteModel.Header = AbaInformacoesCargaCteModel.HeaderPasso5;
                AbaTributacaoModel.Visivel = false;
                AbaTributacaoModel.AnteriorHandler -= AnteriorDocumentosOriginariosClick;
                AbaTributacaoModel.ProximoHandler -= ProximoInformacoesCargaClick;
            }
        }

        private void OnFecharTela()
        {
            FecharTela?.Invoke(this, EventArgs.Empty);
        }

        public void CarregarInformacaoPadraoAba()
        {
            CteComplementarDefaultNomeProduto();

            if (AbaInformacoesCargaCteModel.Selecionado)
            {
                AdicionarInformacaoCargaPerfil();
            }
        }

        private void AdicionarInformacaoCargaPerfil()
        {
            if (_cte.PerfilCte.Carga.Ativo == false || _cte.CteInfoQuantidadeCargas.Any())
            {
                return;
            }

            if (_cte.IsNormal() == false) return;

            var infoCarga = new CteInfoQuantidadeCarga
            {
                Cte = _cte,
                Quantidade = _cte.PerfilCte.Carga.Quantidade,
                TipoUnidadeMedidaDescricao = _cte.PerfilCte.Carga.TipoMedida,
                UnidadeMedida = _cte.PerfilCte.Carga.Unidade
            };

            SalvarInfoCarga(infoCarga);
            AbaInformacoesCargaCteModel.AdicionaCarga(GridInformacaoCarga.Cria(infoCarga));
        }

        public void VerificaSeTemEmissaoPendente()
        {
            if (_cte.Id == 0)
            {
                EdicaoCte = true;
                return;
            }

            var emissaoPendente = false;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioCte = new RepositorioCte(sessao);

                emissaoPendente = repositorioCte.PossuiEmissaoPendente(_cte);
            }

            if (emissaoPendente)
            {
                EdicaoCte = false;
                EmitirCte(this, new RetornoInformacoesCarga(AbaInformacoesCargaCteModel));
                return;
            }

            EdicaoCte = true;
        }

        public bool EdicaoCte
        {
            get => _edicaoCte;
            set
            {
                _edicaoCte = value;
                PropriedadeAlterada();
            }
        }
    }
}