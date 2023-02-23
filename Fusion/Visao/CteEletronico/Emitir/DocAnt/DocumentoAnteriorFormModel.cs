using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Fusion.Visao.CteEletronico.Emitir.EntidadesModels.DocAnt;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.DocAnt
{
    public class DocumentoAnteriorRetorno
    {
        public DocumentoAnteriorFormModel Model { get; }

        public DocumentoAnteriorRetorno(DocumentoAnteriorFormModel model)
        {
            Model = model;
        }
    }

    public class DocumentoAnteriorFormModel : ViewModel
    {
        private string _documentoUnico;
        private string _inscricaoEstadual;
        private EstadoDTO _estadoUf;
        private string _nomeOuRazaoSocial;
        private GridDocumentoAnterior _gridDocumentoAnterior;
        private ObservableCollection<GridDocumentoTransporte> _gridDocumentosTransportes;
        private ObservableCollection<EstadoDTO> _estados;
        private int _qtdPapel;
        private int _qtdCte;
        private FlyoutAddDocumentoAnteriorModel _flyoutAddDocumentoAnteriorModel;
        private GridDocumentoTransporte _documentoTransporteSelecionado;

        public DocumentoAnteriorFormModel()
        {
            GridDocumentosTransportes = new ObservableCollection<GridDocumentoTransporte>();
            Estados = new ObservableCollection<EstadoDTO>(LocalidadesServico.GetInstancia(false).GetEstados());
        }

        public event EventHandler<DocumentoAnteriorRetorno> DocumentoAnteriorHandler;
        public event EventHandler Fechar;

        public FlyoutAddDocumentoAnteriorModel FlyoutAddDocumentoAnteriorModel
        {
            get { return _flyoutAddDocumentoAnteriorModel; }
            set
            {
                if (Equals(value, _flyoutAddDocumentoAnteriorModel)) return;
                _flyoutAddDocumentoAnteriorModel = value;
                PropriedadeAlterada();
            }
        }

        public string DocumentoUnico
        {
            get { return _documentoUnico; }
            set
            {
                if (value == _documentoUnico) return;
                _documentoUnico = value;
                PropriedadeAlterada();
            }
        }

        public string InscricaoEstadual
        {
            get { return _inscricaoEstadual; }
            set
            {
                if (value == _inscricaoEstadual) return;
                _inscricaoEstadual = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO EstadoUf
        {
            get { return _estadoUf; }
            set
            {
                if (Equals(value, _estadoUf)) return;
                _estadoUf = value;
                PropriedadeAlterada();
            }
        }

        public string NomeOuRazaoSocial
        {
            get { return _nomeOuRazaoSocial; }
            set
            {
                if (value == _nomeOuRazaoSocial) return;
                _nomeOuRazaoSocial = value;
                PropriedadeAlterada();
            }
        }

        public GridDocumentoAnterior GridDocumentoAnterior
        {
            get { return _gridDocumentoAnterior; }
            set
            {
                if (Equals(value, _gridDocumentoAnterior)) return;
                _gridDocumentoAnterior = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridDocumentoTransporte> GridDocumentosTransportes
        {
            get { return _gridDocumentosTransportes; }
            set
            {
                if (Equals(value, _gridDocumentosTransportes)) return;
                _gridDocumentosTransportes = value;
                PropriedadeAlterada();
            }
        }

        public GridDocumentoTransporte DocumentoTransporteSelecionado
        {
            get { return _documentoTransporteSelecionado; }
            set
            {
                if (Equals(value, _documentoTransporteSelecionado)) return;
                _documentoTransporteSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EstadoDTO> Estados
        {
            get { return _estados; }
            set
            {
                if (Equals(value, _estados)) return;
                _estados = value;
                PropriedadeAlterada();
            }
        }

        public int QtdPapel
        {
            get { return _qtdPapel; }
            set
            {
                if (value == _qtdPapel) return;
                _qtdPapel = value;
                PropriedadeAlterada();
            }
        }

        public int QtdCte
        {
            get { return _qtdCte; }
            set
            {
                if (value == _qtdCte) return;
                _qtdCte = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandFlyoutAddDocumento => GetSimpleCommand(FlyoutAddDocumentoAction);

        public ICommand CommandExcluir => GetSimpleCommand(ExcluirAction);

        public ICommand CommandSalvar => GetSimpleCommand(SalvarAction);

        private void ExcluirAction(object obj)
        {
            GridDocumentosTransportes.Remove(DocumentoTransporteSelecionado);
            AtualizarQuantidades();
        }

        private void FlyoutAddDocumentoAction(object obj)
        {
            FlyoutAddDocumentoAnteriorModel = new FlyoutAddDocumentoAnteriorModel();
            FlyoutAddDocumentoAnteriorModel.AddDocumentoAnteriorHandler += AdicionarDocumentoAnteriorCompleted;
            FlyoutAddDocumentoAnteriorModel.IsOpen = true;
        }

        private void AdicionarDocumentoAnteriorCompleted(object sender, RetornoAddDocumentoAnterior e)
        {
            var model = e.Model;

            GridDocumentosTransportes.Add(new GridDocumentoTransporte
            {
                TipoDocumentoAnterior = model.TipoDocumentoAnterior,
                ChaveCTe = model.ChaveCTe,
                DataDeEmissao = model.EmissaoEm,
                Serie = model.Serie,
                SubSerie = model.SubSerie,
                NumeroDocumentoFiscal = model.NumeroDocumento,
                IsCTe = model.IsCte,
                IsNotCTe = model.IsNotCte
            });

            AtualizarQuantidades();
        }

        private void AtualizarQuantidades()
        {
            QtdCte = GridDocumentosTransportes.Count(d => d.IsCTe == true);
            QtdPapel = GridDocumentosTransportes.Count(d => d.IsNotCTe == true);
        }

        private void SalvarAction(object obj)
        {
            try
            {
                Hidratar();
                Validar();
                OnDocumentoAnteriorHandler();
                OnFechar();
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void Hidratar()
        {
            NomeOuRazaoSocial = NomeOuRazaoSocial.TrimOrEmpty();
            DocumentoUnico = DocumentoUnico.TrimOrEmpty();
            InscricaoEstadual = InscricaoEstadual.TrimOrEmpty();
        }

        private void Validar()
        {
            if (NomeOuRazaoSocial.IsNullOrEmpty())
                throw new ArgumentException("Digitar nome ou razão social");

            if (DocumentoUnico.IsNullOrEmpty())
                throw new ArgumentException("Digitar um cpf/cnpj");

            if (InscricaoEstadual.IsNullOrEmpty())
                throw new ArgumentException("Digitar inscrição estadual");

            if (EstadoUf == null)
                throw new ArgumentException("Selecionar um estado(uf)");
        }

        protected virtual void OnDocumentoAnteriorHandler()
        {
            DocumentoAnteriorHandler?.Invoke(this, new DocumentoAnteriorRetorno(this));
        }

        protected virtual void OnFechar()
        {
            Fechar?.Invoke(this, EventArgs.Empty);
        }
    }
}