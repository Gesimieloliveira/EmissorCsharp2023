using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Fusion.Visao.Cfop;
using FusionCore.FusionAdm.EntradaOutras;
using FusionCore.FusionAdm.Sessao;
using FusionCore.RecipienteDados;
using FusionCore.RecipienteDados.Adm.Impl;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Estadual;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.FusionAdm.Empresas;

namespace Fusion.Visao.Lancamentos
{
    public class LancamentoCteEntradaFormModel : ViewModel
    {
        private readonly NfCteEntrada _nfCteEntrada;

        public LancamentoCteEntradaFormModel(NfCteEntrada nfCteEntrada)
        {
            _nfCteEntrada = nfCteEntrada;
            _empresaTomador = _nfCteEntrada.EmpresaTomador;
            _cfop = _nfCteEntrada.Cfop;
        }

        private ModeloDocumentoCteEntrada _modeloDocumento;
        private SituacaoFiscal _situacaoFiscal;
        private DateTime _emissaoEm;
        private DateTime _utilizacaoEm;
        private short _serie;
        private short _subserie;
        private int _numero;
        private string _nomeTomador;
        private string _documentoUnicoTomador;
        private string _inscricaoEstadualTomador;
        private string _ufTomador;
        private string _nomeCidadeTomador;
        private string _enderecoTomador;
        private string _telefoneTomador;
        private string _cfopCodigoDescricao;
        private bool _isExcluir;
        private EmpresaDTO _empresaTomador;
        private CfopDTO _cfop;
        private ObservableCollection<TributacaoCst> _tributacoesIcms;
        private TributacaoCst _icms;
        private decimal _valorTotal;
        private decimal _baseCalculoIcms;
        private decimal _valorIcms;

        public ObservableCollection<TributacaoCst> TributacoesIcms
        {
            get => _tributacoesIcms;
            set
            {
                _tributacoesIcms = value;
                PropriedadeAlterada();
            }
        }

        public TributacaoCst Icms
        {
            get => _icms;
            set
            {
                _icms = value;
                PropriedadeAlterada();
            }
        }

        public ModeloDocumentoCteEntrada ModeloDocumento
        {
            get => _modeloDocumento;
            set
            {
                _modeloDocumento = value;
                PropriedadeAlterada();
            }
        }

        public SituacaoFiscal SituacaoFiscal
        {
            get => _situacaoFiscal;
            set
            {
                _situacaoFiscal = value;
                PropriedadeAlterada();
            }
        }

        public DateTime EmissaoEm
        {
            get => _emissaoEm;
            set
            {
                _emissaoEm = value;
                PropriedadeAlterada();
            }
        }

        public DateTime UtilizacaoEm
        {
            get => _utilizacaoEm;
            set
            {
                _utilizacaoEm = value;
                PropriedadeAlterada();
            }
        }

        public short Serie
        {
            get => _serie;
            set
            {
                _serie = value;
                PropriedadeAlterada();
            }
        }

        public short Subserie
        {
            get => _subserie;
            set
            {
                _subserie = value;
                PropriedadeAlterada();
            }
        }

        public int Numero
        {
            get => _numero;
            set
            {
                _numero = value;
                PropriedadeAlterada();
            }
        }

        public string NomeTomador
        {
            get => _nomeTomador;
            set
            {
                _nomeTomador = value;
                PropriedadeAlterada();
            }
        }

        public string DocumentoUnicoTomador
        {
            get => _documentoUnicoTomador;
            set
            {
                _documentoUnicoTomador = value;
                PropriedadeAlterada();
            }
        }

        public string InscricaoEstadualTomador
        {
            get => _inscricaoEstadualTomador;
            set
            {
                _inscricaoEstadualTomador = value;
                PropriedadeAlterada();
            }
        }

        public string UfTomador
        {
            get => _ufTomador;
            set
            {
                _ufTomador = value;
                PropriedadeAlterada();
            }
        }

        public string NomeCidadeTomador
        {
            get => _nomeCidadeTomador;
            set
            {
                _nomeCidadeTomador = value;
                PropriedadeAlterada();
            }
        }

        public string EnderecoTomador
        {
            get => _enderecoTomador;
            set
            {
                _enderecoTomador = value;
                PropriedadeAlterada();
            }
        }

        public string TelefoneTomador
        {
            get => _telefoneTomador;
            set
            {
                _telefoneTomador = value;
                PropriedadeAlterada();
            }
        }

        public string CfopCodigoDescricao
        {
            get => _cfopCodigoDescricao;
            set
            {
                _cfopCodigoDescricao = value;
                PropriedadeAlterada();
            }
        }

        public bool IsExcluir
        {
            get => _isExcluir;
            set
            {
                _isExcluir = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorTotal
        {
            get => _valorTotal;
            set
            {
                _valorTotal = value;
                PropriedadeAlterada();
            }
        }

        public decimal BaseCalculoIcms
        {
            get => _baseCalculoIcms;
            set
            {
                _baseCalculoIcms = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorIcms
        {
            get => _valorIcms;
            set
            {
                _valorIcms = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandPesquisarTomador => GetSimpleCommand(PesquisarTomadorAction);
        public ICommand CommandPesquisarCfop => GetSimpleCommand(PesquisarCfopAction);
        public ICommand CommandSalvar => GetSimpleCommand(SalvarAction);
        public ICommand CommandExcluir => GetSimpleCommand(ExcluirAction);


        private void PesquisarTomadorAction(object obj)
        {
            var model = new EmpresaPickerModel();
            model.PickItemEvent += TomadorSelecionado;

            model.GetPickerView().ShowDialog();
        }

        private void TomadorSelecionado(object sender, GridPickerEventArgs e)
        {
            var empresaPickerDto = e.GetItem<EmpresaPickerModelDto>();

            _empresaTomador = empresaPickerDto.GetEmpresa();

            AtualizaViewTomador();
        }

        private void AtualizaViewTomador()
        {
            NomeTomador = _empresaTomador.RazaoSocial;
            DocumentoUnicoTomador = _empresaTomador.Cnpj;
            InscricaoEstadualTomador = _empresaTomador.InscricaoEstadual;
            UfTomador = _empresaTomador.CidadeDTO.SiglaUf;
            NomeCidadeTomador = _empresaTomador.CidadeDTO.Nome;
            EnderecoTomador = _empresaTomador.Endereco;
            TelefoneTomador = _empresaTomador.Fone1;
        }

        private void PesquisarCfopAction(object obj)
        {
            var cfopModel = new CfopPickerModel();
            cfopModel.PickItemEvent += CfopSelecionado;

            cfopModel.GetPickerView().ShowDialog();
        }

        private void CfopSelecionado(object sender, GridPickerEventArgs e)
        {
            _cfop = e.GetItem<CfopDTO>();
            AtualizaViewCfop();
        }

        private void AtualizaViewCfop()
        {
            CfopCodigoDescricao = _cfop.ToString();
        }

        private void SalvarAction(object obj)
        {
            try
            {
                if (_empresaTomador == null) throw new InvalidOperationException("Selecionar um tomador/empresa");
                if (_cfop == null) throw new InvalidOperationException("Selecionar um cfop");
                if (_icms == null) throw new InvalidOperationException("Selecionar um CST/CSOSN");

                AtualizaDadosNfEntrada();

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorio = new RepositorioNfCteEntrada(sessao);

                    repositorio.SalvarOuAtualizar(_nfCteEntrada);

                    transacao.Commit();
                }

                DialogBox.MostraInformacao("Registro salvo com sucesso");
                OnFechar();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void AtualizaDadosNfEntrada()
        {
            _nfCteEntrada.ModeloDocumento = ModeloDocumento;
            _nfCteEntrada.SituacaoFiscal = SituacaoFiscal;
            _nfCteEntrada.EmissaoEm = EmissaoEm;
            _nfCteEntrada.UtilizacaoEm = UtilizacaoEm;
            _nfCteEntrada.Serie = Serie;
            _nfCteEntrada.Subserie = Subserie;
            _nfCteEntrada.Numero = Numero;
            _nfCteEntrada.EmpresaTomador = _empresaTomador;
            _nfCteEntrada.Cfop = _cfop;
            _nfCteEntrada.IcmsCst = Icms;
            _nfCteEntrada.ValorTotal = ValorTotal;
            _nfCteEntrada.BaseCalculoIcms = BaseCalculoIcms;
            _nfCteEntrada.ValorIcms = ValorIcms;
        }

        private void ExcluirAction(object obj)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNfCteEntrada(sessao);

                repositorio.Deletar(_nfCteEntrada);
                
                transacao.Commit();
            }

            DialogBox.MostraInformacao("Registro deletado com sucesso");
            OnFechar();
        }

        public void Inicializa()
        {
            PreencherCst();

            IsExcluir = _nfCteEntrada.Id != 0;
            ModeloDocumento = _nfCteEntrada.ModeloDocumento;
            SituacaoFiscal = _nfCteEntrada.SituacaoFiscal;
            EmissaoEm = _nfCteEntrada.EmissaoEm;
            UtilizacaoEm = _nfCteEntrada.UtilizacaoEm;
            Serie = _nfCteEntrada.Serie;
            Subserie = _nfCteEntrada.Subserie;
            Numero = _nfCteEntrada.Numero;
            Icms = _nfCteEntrada.IcmsCst;
            ValorTotal = _nfCteEntrada.ValorTotal;
            BaseCalculoIcms = _nfCteEntrada.BaseCalculoIcms;
            ValorIcms = _nfCteEntrada.ValorIcms;

            if (_nfCteEntrada.Id == 0) return;

            AtualizaViewTomador();
            AtualizaViewCfop();
        }

        private void PreencherCst()
        {
            TributacoesIcms = new ObservableCollection<TributacaoCst>(RecipienteFactory.Get<RecipienteTributacaoCst>().GetTodos());
        }
    }
}