using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fusion.Visao.CteEletronicoOs.Builder;
using Fusion.Visao.CteEletronicoOs.Emitir.Aba.Flyouts;
using Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.CteEletronicoOs.NumeroFiscal;
using FusionCore.FusionAdm.CteEletronicoOs.Perfil;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NFe.Classes;
using NHibernate.Util;
using static System.String;

namespace Fusion.Visao.CteEletronicoOs.Emitir
{
    public class CteOsEmitirFormModel : ViewModel
    {
        private CteOsBuilder CteOsBuilder { get; set; }

        private AbaCteOsPerfilCteOsModel _abaCteOsPerfilModel;
        private AbaCteOsCabecalhoCteOsModel _abaCteOsCabecalhoCteOsModel;
        private AbaEmitenteTomadorModel _abaEmitenteTomadorModel;
        private AbaServicoSeguroRodoOsModel _abaServicoSeguroRodoOsModel;
        private FlyoutAddSeguroModel _flyoutAddSeguroModel;
        private AbaCTeOsTributacaoModel _abaCTeOsTributacaoModel;
        private bool _jaFoiPreenchidoTaf;
        private bool _jaFoiPreenchidoRegistroEstadual;
        private bool _isEnabled;
        private FlyoutAddDocumentoReferenciadoCteOsModel _flyoutAddDocumentoReferenciadoCteOsModel;

        public CteOsEmitirFormModel(CteOs cteOs)
        {
            CriaCteOsBuilder(cteOs);
        }

        public AbaCteOsPerfilCteOsModel AbaCteOsPerfilModel
        {
            get => _abaCteOsPerfilModel;
            set
            {
                if (Equals(value, _abaCteOsPerfilModel)) return;
                _abaCteOsPerfilModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaCteOsCabecalhoCteOsModel AbaCteOsCabecalhoCteOsModel
        {
            get => _abaCteOsCabecalhoCteOsModel;
            set
            {
                if (Equals(value, _abaCteOsCabecalhoCteOsModel)) return;
                _abaCteOsCabecalhoCteOsModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaEmitenteTomadorModel AbaEmitenteTomadorModel
        {
            get => _abaEmitenteTomadorModel;
            set
            {
                if (Equals(value, _abaEmitenteTomadorModel)) return;
                _abaEmitenteTomadorModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaServicoSeguroRodoOsModel AbaServicoSeguroRodoOsModel
        {
            get => _abaServicoSeguroRodoOsModel;
            set
            {
                if (Equals(value, _abaServicoSeguroRodoOsModel)) return;
                _abaServicoSeguroRodoOsModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaCTeOsTributacaoModel AbaCTeOsTributacaoModel
        {
            get => _abaCTeOsTributacaoModel;
            set
            {
                if (Equals(value, _abaCTeOsTributacaoModel)) return;
                _abaCTeOsTributacaoModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddSeguroModel FlyoutAddSeguroModel
        {
            get => _flyoutAddSeguroModel;
            set
            {
                if (Equals(value, _flyoutAddSeguroModel)) return;
                _flyoutAddSeguroModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddPercursoModel FlyoutAddPercursoModel
        {
            get => GetValue<FlyoutAddPercursoModel>();
            set => SetValue(value);
        }

        public FlyoutAddComponenteCteOsModel FlyoutAddComponenteCteOsModel
        {
            get => GetValue<FlyoutAddComponenteCteOsModel>();
            set => SetValue(value);
        }

        public FlyoutAddDocumentoReferenciadoCteOsModel FlyoutAddDocumentoReferenciadoCteOsModel
        {
            get => _flyoutAddDocumentoReferenciadoCteOsModel;
            set
            {
                if (Equals(value, _flyoutAddDocumentoReferenciadoCteOsModel)) return;
                _flyoutAddDocumentoReferenciadoCteOsModel = value;
                PropriedadeAlterada();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                PropriedadeAlterada();
            }
        }

        public void Loaded()
        {
            AbaCteOsPerfilModel = new AbaCteOsPerfilCteOsModel();
            AbaCteOsCabecalhoCteOsModel = new AbaCteOsCabecalhoCteOsModel();
            AbaEmitenteTomadorModel = new AbaEmitenteTomadorModel();
            AbaServicoSeguroRodoOsModel = new AbaServicoSeguroRodoOsModel(this);
            AbaCTeOsTributacaoModel = new AbaCTeOsTributacaoModel();

            AbaCteOsPerfilModel.PerfilSelecionado += PerfilSelecionado;
            AbaCteOsCabecalhoCteOsModel.Proximo += ProximoAbaTomadorEmitente;
            AbaCteOsCabecalhoCteOsModel.AlocarNumeracaoFiscal += AlocarNumeracaoFiscal;

            AbaEmitenteTomadorModel.Anterior += AnteriorAbaCabecalho;
            AbaEmitenteTomadorModel.Proximo += ProximoTributacao;

            AbaCTeOsTributacaoModel.Anterior += AbaTributacaoPassoAnterior;
            AbaCTeOsTributacaoModel.Proximo += AbaTributacaoProximoPasso;

            AbaServicoSeguroRodoOsModel.Emitir += EmitirCteOs;
            AbaServicoSeguroRodoOsModel.Anterior += AbaServicoSeguroPassoAnterior;
            AbaServicoSeguroRodoOsModel.AdicioanrSeguro += AdicionarSeguro;
            AbaServicoSeguroRodoOsModel.SeguroDeletadoHandler += SeguroDeletado;
            AbaServicoSeguroRodoOsModel.AdicionarVeiculoHandler += AdicionarVeiculo;
            AbaServicoSeguroRodoOsModel.DeletarVeiculoHandler += DeletarVeiculo;
            AbaServicoSeguroRodoOsModel.SalvarModalRodoviario += SalvarModelRodoviario;
            AbaServicoSeguroRodoOsModel.SalvarCTeNormal += SalvarCTeNormal;

            AbaCteOsPerfilModel.Inicializa();
            AbaCteOsCabecalhoCteOsModel.Inicializa();
            Edicao();
        }

        public void VerificaHistoricoPendente()
        {
            try
            {
                if (EhUmCteOsNovo())
                {
                    IsEnabled = true;
                    return;
                }

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioCteOs(sessao);
                    var emissaoHistorico = repositorio.BuscaUltimaEmissao(CteOsBuilder.Construir());

                    if (emissaoHistorico == null || emissaoHistorico.Finalizada)
                    {
                        IsEnabled = true;
                        return;
                    }
                }

                IsEnabled = false;

                var model = new EmissaoSefazCteOsViewModel(CteOsBuilder.Construir());
                var telaEmissao = new EmissaoSefazCteOsView(model);

                telaEmissao.Closing += delegate
                {
                    if (model.IsAutorizado) OnFechar();

                    IsEnabled = true;
                };

                model.Fechar += (o, args) => { Application.Current.Dispatcher.Invoke(telaEmissao.Close); };
                telaEmissao.ShowDialog();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void AbaTributacaoProximoPasso(object sender, AbaCTeOsTributacaoModel e)
        {
            if (e.TributacaoSelecionada == null) throw new InvalidOperationException("Necessário Selecionar um CST");

            CteOsBuilder.ComTributacao(e);
            SalvarCteOs();

            e.DesabilitaAba();

            AdicionarDefaultAbaServicos();
            AbaServicoSeguroRodoOsModel.HabilitaAba();
        }

        private void AbaTributacaoPassoAnterior(object sender, AbaCTeOsTributacaoModel e)
        {
            e.DesabilitaAba();
            AbaEmitenteTomadorModel.HabilitaAba();
        }

        private void AlocarNumeracaoFiscal(object sender, AbaCteOsCabecalhoCteOsModel e)
        {
            const string msg = "Deseja alocar o próximo número disponivel?";

            if (!DialogBox.MostraConfirmacao(msg, MessageBoxImage.Question)) return;

            var cte = CteOsBuilder.Construir();
            new AlocarNumeroFiscalCteOs().Alocar(cte);

            AbaCteOsCabecalhoCteOsModel.NumeroDocumento = cte.NumeroEmissao;
            AbaCteOsCabecalhoCteOsModel.SerieDocumento = cte.SerieEmissao;

            DialogBox.MostraInformacao("Número Fiscal alocado com sucesso");
        }

        private void SalvarCTeNormal(object sender, AbaServicoSeguroRodoOsModel e)
        {
            CteOsBuilder.CriaCteNormalSeNaoExistir();
            CteOsBuilder.ComDescricaoServicoPrestado(e.DescricaoServicoPrestado.TrimOrEmpty());
            CteOsBuilder.ComQuantidadePassageirosVolumes(e.QuantidadePassageirosOuVolumes.Arredondar(4) ?? 0);

            SalvarCteOs();
        }

        private void SalvarModelRodoviario(object sender, AbaServicoSeguroRodoOsModel e)
        {
            if (ExtValidaListasEColecoes.IsNullOrEmpty(e.Taf) && ExtValidaListasEColecoes.IsNullOrEmpty(e.NumeroRegistroEstadual))
            {
                if (!CteOsBuilder.ExisteRodoViario()) return;

                DeletarRodoviario();
                CteOsBuilder.DeletaRodoviario();

                return;
            }

            if (e.Taf.IsNotNullOrEmpty() || e.NumeroRegistroEstadual.IsNotNullOrEmpty())
            {
                CteOsBuilder.CriaRodoviarioSeNaoExistir();
                CteOsBuilder.ComTaf(e.Taf.TrimOrEmpty());
                CteOsBuilder.ComNumeroRegistroEstadual(e.NumeroRegistroEstadual.TrimOrEmpty());
            }

            SalvarCteOs();
        }

        private void DeletarRodoviario()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCteOs(sessao);
                repositorio.Deletar(CteOsBuilder.Construir().Rodoviario);
                transacao.Commit();
            }
        }

        private void SeguroDeletado(object sender, AbaServicoSeguroRodoOsModel model)
        {
            CteOsBuilder.AtualizarSeguros(model);
        }

        private void AdicionarSeguro(object sender, EventArgs e)
        {
            FlyoutAddSeguroModel = new FlyoutAddSeguroModel
            {
                IsOpen = true
            };

            FlyoutAddSeguroModel.Salvar += SalvarSeguro;
        }

        private void EmitirCteOs(object sender, AbaServicoSeguroRodoOsModel e)
        {
            try
            {
                CteOsBuilder.Validar();

                var model = new EmissaoSefazCteOsViewModel(CteOsBuilder.Construir());
                var telaEmissao = new EmissaoSefazCteOsView(model);

                model.Fechar += (o, args) => { Application.Current.Dispatcher.Invoke(telaEmissao.Close); };
                telaEmissao.ShowDialog();

                if (CteOsBuilder.IsAutorizado() || CteOsBuilder.IsDenegada()) OnFechar();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void AbaServicoSeguroPassoAnterior(object sender, AbaServicoSeguroRodoOsModel e)
        {
            e.DesabilitaAba();
            AbaCTeOsTributacaoModel.HabilitaAba();
        }

        private void ProximoTributacao(object sender, AbaEmitenteTomadorModel e)
        {
            CteOsBuilder.ComEmitenteTomador(e);
            SalvarCteOs();
            e.DesabilitaAba();
            AbaCTeOsTributacaoModel.ConfigurarRegimeTributario(e.Emitente.RegimeTributario);
            AbaCTeOsTributacaoModel.HabilitaAba();
        }

        private void AdicionarDefaultAbaServicos()
        {
            var perfil = CteOsBuilder.GetPerfil();

            AdicionarSeguroDefault(perfil);
            AdicionarVeiculoDefault(perfil);
            AdicionarTaf();
            AdicioanrRegistroEstadual();
            AdicionarDescricaoPadrao(perfil);
            AdicionarQuantidadePadrao(perfil);
        }

        private void AdicionarTaf()
        {
            if (!IsNullOrWhiteSpace(CteOsBuilder.Taf))
            {
                AbaServicoSeguroRodoOsModel.Taf = CteOsBuilder.Taf;
                return;
            }

            if (!_jaFoiPreenchidoTaf)
            {
                CteOsBuilder.CriaRodoviarioSeNaoExistir();
                CteOsBuilder.AutoPreencherTaf();
                SalvarCteOs();

                _jaFoiPreenchidoTaf = true;
            }

            AbaServicoSeguroRodoOsModel.Taf = CteOsBuilder.Taf;
        }

        private void AdicioanrRegistroEstadual()
        {
            if (!IsNullOrWhiteSpace(CteOsBuilder.NumeroRegistroEstadual))
            {
                AbaServicoSeguroRodoOsModel.NumeroRegistroEstadual = CteOsBuilder.NumeroRegistroEstadual;
                return;
            }

            if (!_jaFoiPreenchidoRegistroEstadual)
            {
                CteOsBuilder.CriaRodoviarioSeNaoExistir();
                CteOsBuilder.AutoPreencherNumeroRegistroEstadual();
                SalvarCteOs();

                _jaFoiPreenchidoRegistroEstadual = true;
            }


            AbaServicoSeguroRodoOsModel.NumeroRegistroEstadual = CteOsBuilder.NumeroRegistroEstadual;
        }

        private void AdicionarDescricaoPadrao(PerfilCteOs perfil)
        {
            var cte = CteOsBuilder.Construir();

            if (perfil.DescricaoServico.IsNullOrEmpty() ||
                cte.Normal != null && cte.Normal.DescricaoServicoPrestado.IsNotNullOrEmpty())
                return;

            CteOsBuilder.CriaCteNormalSeNaoExistir();
            CteOsBuilder.ComDescricao(perfil.DescricaoServico);
            AbaServicoSeguroRodoOsModel.DescricaoServicoPrestado = perfil.DescricaoServico;
        }

        private void AdicionarQuantidadePadrao(PerfilCteOs perfil)
        {
            var cte = CteOsBuilder.Construir();

            if (perfil.QuantidadePassageiroVolume == 0) return;

            if (cte.Normal != null && cte.Normal.QuantidadePassageirosVolumes != 0) return;

            CteOsBuilder.CriaCteNormalSeNaoExistir();
            CteOsBuilder.ComQuantidade(perfil.QuantidadePassageiroVolume);
            AbaServicoSeguroRodoOsModel.QuantidadePassageirosOuVolumes = perfil.QuantidadePassageiroVolume;
        }

        private void AdicionarVeiculoDefault(PerfilCteOs perfil)
        {
            var cte = CteOsBuilder.Construir();

            if (perfil.Veiculo == null || cte.Veiculo != null) return;

            AbaServicoSeguroRodoOsModel.Veiculo = perfil.Veiculo;
            AdicionarVeiculo(perfil.Veiculo);
        }

        private void AdicionarSeguroDefault(PerfilCteOs perfil)
        {
            var cte = CteOsBuilder.Construir();

            if (perfil.Seguro == null || cte.Seguros.IsNotNullOrEmpty()) return;

            var seguro = new CteOsSeguro
            {
                CteOs = CteOsBuilder.Construir(),
                Id = 0,
                NomeSeguradora = perfil.Seguro.NomeSeguradora,
                ResponsavelSeguro = perfil.Seguro.ResponsavelSeguro,
                NumeroApolice = perfil.Seguro.NumeroApolice
            };

            SalvarSeguro(seguro);
            CteOsBuilder.AdicionarSeguro(seguro);
            AdicionaSeguro(seguro);
        }

        private void AnteriorAbaCabecalho(object sender, AbaEmitenteTomadorModel e)
        {
            e.DesabilitaAba();
            AbaCteOsCabecalhoCteOsModel.HabilitaAba();
        }

        private void ProximoAbaTomadorEmitente(object sender, AbaCteOsCabecalhoCteOsModel e)
        {
            CteOsBuilder.ComCabecalho(e);

            SalvarCteOs();

            e.DesabilitaAba();
            AbaEmitenteTomadorModel.ComEmitente(CteOsBuilder.Construir().Perfil.EmissorFiscal.Empresa);

            if (AbaEmitenteTomadorModel.Tomador == null && CteOsBuilder.GetPerfil().Tomador != null) AbaEmitenteTomadorModel.Tomador = CteOsBuilder.GetPerfil().Tomador;

            AbaEmitenteTomadorModel.HabilitaAba();
        }

        private void PerfilSelecionado(object sender, AbaCteOsPerfilCteOsModel e)
        {
            CteOsBuilder.ComPerfil(e);

            AbaCteOsCabecalhoCteOsModel.ComInserirCte(CteOsBuilder.Construir());
            AbaCteOsCabecalhoCteOsModel.HabilitaAba();
            AbaCteOsPerfilModel.DesabilitaAba();
        }

        private void CriaCteOsBuilder(CteOs cteOs)
        {
            CteOsBuilder = new CteOsBuilder(cteOs);
        }

        private void Edicao()
        {
            if (EhUmCteOsNovo()) return;

            var cteOs = CteOsBuilder.Construir();
            AbaCteOsCabecalhoCteOsModel.ComAlteracaoCte(cteOs);

            AbaCteOsPerfilModel.DesabilitaAba();
            AbaCteOsCabecalhoCteOsModel.HabilitaAba();

            AbaEmitenteTomadorModel.ComCteOs(cteOs);
            if (cteOs.Tomador != null)
            {
                AbaCteOsCabecalhoCteOsModel.DesabilitaAba();
                AbaEmitenteTomadorModel.HabilitaAba();
            }

            AbaCTeOsTributacaoModel.ComCteOs(cteOs);
            if (cteOs.TributacaoIcms != null)
            {
                AbaEmitenteTomadorModel.DesabilitaAba();
                AbaCTeOsTributacaoModel.HabilitaAba();
            }

            if (cteOs.Seguros.IsNotNullOrEmpty())
            {
                cteOs.Seguros.ForEach(AdicionaSeguro);
                AbaCTeOsTributacaoModel.DesabilitaAba();
                AbaServicoSeguroRodoOsModel.HabilitaAba();
            }

            if (cteOs.Percursos.IsNotNullOrEmpty())
            {
                AbaServicoSeguroRodoOsModel.Percursos = new ObservableCollection<CteOsPercurso>(cteOs.Percursos);
                AbaCTeOsTributacaoModel.DesabilitaAba();
                AbaServicoSeguroRodoOsModel.HabilitaAba();
            }

            if (cteOs.Componentes.IsNotNullOrEmpty())
            {
                var gridComponente = new List<GridComponenteValorPrestacaoCteOs>();
                cteOs.Componentes.ForEach(comp => { gridComponente.Add(new GridComponenteValorPrestacaoCteOs { Componente = comp }); });
                AbaServicoSeguroRodoOsModel.Componentes = new ObservableCollection<GridComponenteValorPrestacaoCteOs>(gridComponente);
                AbaCTeOsTributacaoModel.DesabilitaAba();
                AbaServicoSeguroRodoOsModel.HabilitaAba();
            }

            if (cteOs.DocumentoReferenciado.IsNotNullOrEmpty())
            {
                var gridDocumentoReferenciado = cteOs.DocumentoReferenciado.Select(cteOsDocumentoReferenciado => new GridDocumentoReferenciadoCteOs { DocumentoReferenciado = cteOsDocumentoReferenciado }).ToList();
                AbaServicoSeguroRodoOsModel.DocumentosReferenciados = new ObservableCollection<GridDocumentoReferenciadoCteOs>(gridDocumentoReferenciado);
                AbaCTeOsTributacaoModel.DesabilitaAba();
                AbaServicoSeguroRodoOsModel.HabilitaAba();
            }

            if (cteOs.Veiculo != null)
            {
                AbaServicoSeguroRodoOsModel.Veiculo = cteOs.Veiculo;
                AbaCTeOsTributacaoModel.DesabilitaAba();
                AbaServicoSeguroRodoOsModel.HabilitaAba();
            }

            if (cteOs.Rodoviario != null)
            {
                AbaServicoSeguroRodoOsModel.SetRodoviarioSilencioso(
                    cteOs.Rodoviario.Taf,
                    cteOs.Rodoviario.NumeroDoRegimeEstadual
                );

                AbaCTeOsTributacaoModel.DesabilitaAba();
                AbaServicoSeguroRodoOsModel.HabilitaAba();
            }

            if (cteOs.Normal != null &&
                (cteOs.Normal.DescricaoServicoPrestado.IsNotNullOrEmpty() ||
                 cteOs.Normal.QuantidadePassageirosVolumes != 0)
               )
            {
                AbaServicoSeguroRodoOsModel.SetNormalSilencioso(
                    cteOs.Normal.DescricaoServicoPrestado,
                    cteOs.Normal.QuantidadePassageirosVolumes
                );

                AbaCTeOsTributacaoModel.DesabilitaAba();
                AbaServicoSeguroRodoOsModel.HabilitaAba();
            }
        }

        private bool EhUmCteOsNovo()
        {
            return CteOsBuilder.Construir().Id == 0;
        }

        private void DeletarVeiculo(object sender, AbaServicoSeguroRodoOsModel e)
        {
            CteOsBuilder.ComVeiculo(null);
            SalvarCteOs();
        }

        private void AdicionarVeiculo(object sender, AbaServicoSeguroRodoOsModel e)
        {
            var veiculo = e.Veiculo;

            AdicionarVeiculo(veiculo);
        }

        private void AdicionarVeiculo(Veiculo veiculo)
        {
            CteOsBuilder.ComVeiculo(veiculo);
            SalvarCteOs();
        }

        private void SalvarSeguro(object sender, FlyoutAddSeguroModel e)
        {
            var seguro = new CteOsSeguro
            {
                CteOs = CteOsBuilder.Construir(),
                NomeSeguradora = e.NomeSeguradora,
                NumeroApolice = e.NumeroApolice,
                ResponsavelSeguro = e.ResponsavelSeguroSelecionado
            };

            SalvarSeguro(seguro);
            AdicionaSeguro(seguro);
        }

        private static void SalvarSeguro(CteOsSeguro seguro)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCteOs(sessao);

                repositorio.Salvar(seguro);

                transacao.Commit();
            }
        }

        private void AdicionaSeguro(CteOsSeguro seguro)
        {
            AbaServicoSeguroRodoOsModel.AdicioanrSeguroLista(new GridSeguro
            {
                NomeSeguradora = seguro.NomeSeguradora,
                ResponsavelSeguro = seguro.ResponsavelSeguro,
                NumeroApolice = seguro.NumeroApolice,
                CteOsSeguro = seguro
            });
        }

        private void SalvarCteOs()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCteOs(sessao);

                var cteOs = CteOsBuilder.Construir();

                repositorio.Salvar(cteOs);

                transacao.Commit();
            }
        }

        public void AdicionaPercurso(CteOsPercurso percurso)
        {
            CteOsBuilder.Add(percurso);

            var percrusos = CteOsBuilder.ListarPercusos();

            AbaServicoSeguroRodoOsModel.Percursos = new ObservableCollection<CteOsPercurso>(percrusos);
        }

        public void RemovePercurso(CteOsPercurso percurso)
        {
            CteOsBuilder.Remove(percurso);

            var percursos = CteOsBuilder.ListarPercusos();

            AbaServicoSeguroRodoOsModel.Percursos = new ObservableCollection<CteOsPercurso>(percursos);
        }

        public void RemoveComponente(CteOsComponenteValorPrestacao componente)
        {
            CteOsBuilder.Remove(componente);

            ObterComponentes();
        }

        public void RemoveDocumentoReferenciado(CteOsDocumentoReferenciado documentoReferenciado)
        {
            CteOsBuilder.Remove(documentoReferenciado);

            ObterDocumentosReferenciados();
        }

        public void AdicionaComponente(CteOsComponenteValorPrestacao cteOsComponenteValorPrestacao)
        {
            CteOsBuilder.Add(cteOsComponenteValorPrestacao);

            ObterComponentes();
        }

        public void AdicionaDocumentoReferenciado(CteOsDocumentoReferenciado documentoReferenciado)
        {
            CteOsBuilder.Add(documentoReferenciado);

            ObterDocumentosReferenciados();
        }

        private void ObterDocumentosReferenciados()
        {
            var documentosReferenciados = CteOsBuilder.ListarDocumentosReferenciados();

            var documentosReferenciadosGrid = documentosReferenciados.Select(cteOsDocumentoReferenciado => new GridDocumentoReferenciadoCteOs { DocumentoReferenciado = cteOsDocumentoReferenciado }).ToList();

            AbaServicoSeguroRodoOsModel.DocumentosReferenciados =
                new ObservableCollection<GridDocumentoReferenciadoCteOs>(documentosReferenciadosGrid);
        }

        private void ObterComponentes()
        {
            var componentes = CteOsBuilder.ListarComponentes();

            var componentesGrid = new List<GridComponenteValorPrestacaoCteOs>();

            componentes.ForEach(comp =>
            {
                componentesGrid.Add(new GridComponenteValorPrestacaoCteOs
                {
                    Componente = comp
                });
            });

            AbaServicoSeguroRodoOsModel.Componentes =
                new ObservableCollection<GridComponenteValorPrestacaoCteOs>(componentesGrid);
        }
    }
}