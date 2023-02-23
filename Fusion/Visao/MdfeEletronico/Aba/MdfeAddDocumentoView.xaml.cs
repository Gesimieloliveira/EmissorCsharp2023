using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Fusion.Visao.MdfeEletronico.Aba.Model;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Controles;
using FusionWPF.FusionAdm.Cidades;
using FusionWPF.FusionAdm.CteEletronico;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class MdfeAddDocumentoView
    {
        private readonly FusionWindow _window;
        private readonly AbaMdfeCarregamentoModel _modelCarregamento;

        public MdfeAddDocumentoView(FusionWindow window, AbaMdfeCarregamentoModel modelCarregamento)
        {
            _window = window;
            _modelCarregamento = modelCarregamento;

            InitializeComponent();
            Contexto = new MdfeAddDocumentoContexto(modelCarregamento);
        }

        public MdfeAddDocumentoContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
            TbCidade.Focus();

            if (Contexto.Municipio == null)
            {
                Dispatcher?.BeginInvoke(AcaoBuscarMunicipioDescarregamento);
            }
        }

        private void BuscarCicadeClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoBuscarMunicipioDescarregamento();
        }

        private void AcaoBuscarMunicipioDescarregamento()
        {
            var selecionado = false;

            var cidadeModel = new CidadePickerModel()
            {
                Titulo = "Selecione o Município de Descarregamento"
            };

            cidadeModel.PickItemEvent += (o, args) =>
            {
                Contexto.Municipio = args.GetItem<CidadeDTO>();
                selecionado = true;
            };

            cidadeModel.ShowPickerDialog();

            if (selecionado)
            {
                TbChave.Focus();
            }
        }

        private void SalvarAteracoesClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Contexto.SalvarAlteracoes();
                Keyboard.Focus(TbChave);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                TbChave.Focus();
            }
        }

        private void AdicionarProdutoPerigosoClickHandler(object sender, RoutedEventArgs e)
        {
            var view = new ProdutoPerigosoView();

            view.Contexto.Sucesso += (o, args) =>
            {
                Contexto.AdicionarProdutoPerigoso(args);

                view.Close(true);
            };

            _window.ShowChildWindowAsync(view);
        }

        private void RemoverProdutoPerigosoClickHandler(object sender, RoutedEventArgs e)
        {
            Contexto.RemoverProdutoPerigosoSelecionado();
        }

        private void BuscarDocumentoClickHandler(object sender, RoutedEventArgs e)
        {
            if (_modelCarregamento.TipoEmitente == MDFeTipoEmitente.PrestadorServicoDeTransporte)
            {
                var modelCte = new CtePickerModel();
                modelCte.PickItemEvent += CteSelecionada;
                modelCte.GetPickerView().ShowDialog();
                return;
            }

            var modelNfe = new NfePickerModel();
            modelNfe.PickItemEvent += SelecionaNfeCompleted;
            modelNfe.GetPickerView().ShowDialog();
        }

        private void CteSelecionada(object sender, GridPickerEventArgs e)
        {
            var picked = e.GetItem<CtePickerDTO>();

            if (picked == null) return;

            Contexto.Chave = picked.Chave;
            Contexto.SetValorTotal(picked.ValorServico);
        }

        private void SelecionaNfeCompleted(object sender, GridPickerEventArgs e)
        {
            var nfepicker = e.GetItem<NfePickerDTO>();

            if (nfepicker == null) return;

            Contexto.Chave = nfepicker.Chave;
            Contexto.SetValorTotal(nfepicker.TotalNf);
        }
    }
}