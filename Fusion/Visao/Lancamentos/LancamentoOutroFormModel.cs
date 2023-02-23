using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.Cfop;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.EntradaOutras;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Pessoa;
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
    public class LancamentoOutroFormModel : ViewModel
    {
        private readonly NfOutro _nfOutro;

        public LancamentoOutroFormModel(NfOutro nfOutro)
        {
            _nfOutro = nfOutro;
        }

        private ModeloDocumentoOutro _modeloDocumento;
        private TipoEmitente _tipoEmitente;
        private SituacaoFiscal _situacaoFiscal;
        private short _serie;
        private int _numero;
        private DateTime _emissaoEm;
        private DateTime _recebimentoEm;
        private string _nomeFornecedor;
        private string _documentoUnicoFornecedor;
        private string _inscricaoEstadualFornecedor;
        private string _ufFornecedor;
        private string _nomeCidadeFornecedor;
        private string _enderecoFornecedor;
        private string _telefoneFornecedor;
        private string _cfopCodigoDescricao;
        private ObservableCollection<TributacaoIcms> _icmsDisponiveis;
        private TributacaoIcms _icms;
        private decimal _baseCalculoIcms;
        private decimal _valorFrete;
        private decimal _valorIcms;
        private decimal _valorSeguro;
        private decimal _aliquotaIcms;
        private decimal _despesasAcessorias;
        private decimal _baseCalculoIcmsSt;
        private decimal _totalDesconto;
        private decimal _valorIcmsSt;
        private decimal _valorTotal;
        private Fornecedor _fornecedor;
        private CfopDTO _cfop;
        private bool _isExcluir;
        private EmpresaDTO _empresa;
        private string _nomeEmpresa;
        private string _documentoUnicoEmpresa;

        public ObservableCollection<TributacaoIcms> IcmsDisponiveis
        {
            get => _icmsDisponiveis;
            set
            {
                _icmsDisponiveis = value;
                PropriedadeAlterada();
            }
        }

        public TributacaoIcms Icms
        {
            get => _icms;
            set
            {
                _icms = value;
                PropriedadeAlterada();
            }
        }

        public ModeloDocumentoOutro ModeloDocumento
        {
            get => _modeloDocumento;
            set
            {
                _modeloDocumento = value;
                PropriedadeAlterada();
            }
        }

        public TipoEmitente TipoEmitente
        {
            get => _tipoEmitente;
            set
            {
                _tipoEmitente = value;
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

        public short Serie
        {
            get => _serie;
            set
            {
                _serie = value;
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

        public DateTime EmissaoEm
        {
            get => _emissaoEm;
            set
            {
                _emissaoEm = value;
                PropriedadeAlterada();
            }
        }

        public DateTime RecebimentoEm
        {
            get => _recebimentoEm;
            set
            {
                _recebimentoEm = value;
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

        public decimal ValorFrete
        {
            get => _valorFrete;
            set
            {
                _valorFrete = value;
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

        public decimal ValorSeguro
        {
            get => _valorSeguro;
            set
            {
                _valorSeguro = value;
                PropriedadeAlterada();
            }
        }

        public decimal AliquotaIcms
        {
            get => _aliquotaIcms;
            set
            {
                _aliquotaIcms = value;
                PropriedadeAlterada();
            }
        }

        public decimal DespesasAcessorias
        {
            get => _despesasAcessorias;
            set
            {
                _despesasAcessorias = value;
                PropriedadeAlterada();
            }
        }

        public decimal BaseCalculoIcmsSt
        {
            get => _baseCalculoIcmsSt;
            set
            {
                _baseCalculoIcmsSt = value;
                PropriedadeAlterada();
            }
        }

        public decimal TotalDesconto
        {
            get => _totalDesconto;
            set
            {
                _totalDesconto = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorIcmsSt
        {
            get => _valorIcmsSt;
            set
            {
                _valorIcmsSt = value;
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

        public string NomeFornecedor
        {
            get => _nomeFornecedor;
            set
            {
                _nomeFornecedor = value;
                PropriedadeAlterada();
            }
        }

        public string DocumentoUnicoFornecedor
        {
            get => _documentoUnicoFornecedor;
            set
            {
                _documentoUnicoFornecedor = value;
                PropriedadeAlterada();
            }
        }

        public string InscricaoEstadualFornecedor
        {
            get => _inscricaoEstadualFornecedor;
            set
            {
                _inscricaoEstadualFornecedor = value;
                PropriedadeAlterada();
            }
        }

        public string UfFornecedor
        {
            get => _ufFornecedor;
            set
            {
                _ufFornecedor = value;
                PropriedadeAlterada();
            }
        }

        public string NomeCidadeFornecedor
        {
            get => _nomeCidadeFornecedor;
            set
            {
                _nomeCidadeFornecedor = value;
                PropriedadeAlterada();
            }
        }

        public string EnderecoFornecedor
        {
            get => _enderecoFornecedor;
            set
            {
                _enderecoFornecedor = value;
                PropriedadeAlterada();
            }
        }

        public string TelefoneFornecedor
        {
            get => _telefoneFornecedor;
            set
            {
                _telefoneFornecedor = value;
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

        public string NomeEmpresa
        {
            get => _nomeEmpresa;
            set
            {
                _nomeEmpresa = value;
                PropriedadeAlterada();
            }
        }

        public string DocumentoUnicoEmpresa
        {
            get => _documentoUnicoEmpresa;
            set
            {
                _documentoUnicoEmpresa = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandPesquisarFornecedor => GetSimpleCommand(PesquisarFornecedorAction);
        public ICommand CommandPesquisarEmpresa => GetSimpleCommand(PesquisarEmpresaAction);

        private void PesquisarEmpresaAction(object obj)
        {
            var model = new EmpresaPickerModel();
            model.PickItemEvent += EmpresaSelecionada;

            model.GetPickerView().ShowDialog();
        }

        private void EmpresaSelecionada(object sender, GridPickerEventArgs e)
        {
            var empresaPickerDto = e.GetItem<EmpresaPickerModelDto>();

            _empresa = empresaPickerDto.GetEmpresa();

            AtualizaDadosEmpresa(_empresa);
        }

        public ICommand CommandPesquisarCfop => GetSimpleCommand(CommandPesquisarCfopAction);

        private void CommandPesquisarCfopAction(object obj)
        {
            var modelPicker = new CfopPickerModel();
            modelPicker.PickItemEvent += CfopSelecionado;

            modelPicker.GetPickerView().ShowDialog();
        }

        private void CfopSelecionado(object sender, GridPickerEventArgs e)
        {
            var cfop = e.GetItem<CfopDTO>();
            _cfop = cfop;

            AtualizaDadosCfop(cfop);
        }

        private void AtualizaDadosCfop(CfopDTO cfop)
        {
            CfopCodigoDescricao = cfop.ToString();
        }

        private void PesquisarFornecedorAction(object obj)
        {
            try
            {
                var modelPicker = new PessoaPickerModel(new FornecedorEngine());
                modelPicker.PickItemEvent += FornecedorSelecionado;

                modelPicker.GetPickerView().ShowDialog();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void FornecedorSelecionado(object sender, GridPickerEventArgs e)
        {
            var fornededor = e.GetItem<Fornecedor>();
            _fornecedor = fornededor;

            if (fornededor.Enderecos == null || fornededor.Enderecos.Count == 0)
            {
                throw new InvalidOperationException("Fornecedor não tem endereço, cadastrar endereço no mesmo");
            }

            AtualizaDadosFornecedor(fornededor);
        }

        private void AtualizaDadosFornecedor(Fornecedor fornededor)
        {
            NomeFornecedor = fornededor.Nome;
            DocumentoUnicoFornecedor = fornededor.GetDocumentoUnico();
            InscricaoEstadualFornecedor = fornededor.InscricaoEstadual;

            var endereco = fornededor.GetEnderecoPrincipal();

            UfFornecedor = endereco.Cidade.SiglaUf;
            NomeCidadeFornecedor = endereco.Cidade.Nome;
            EnderecoFornecedor = endereco.ToString();

            if (fornededor.Telefones == null || fornededor.Telefones.Count == 0)
                return;

            TelefoneFornecedor = fornededor.Telefones.FirstOrDefault()?.Numero;
        }

        public ICommand CommandSalvar => GetSimpleCommand(SalvarAction);
        public ICommand CommandExcluir => GetSimpleCommand(ExcluirAction);

        private void ExcluirAction(object obj)
        {
            if (!DialogBox.MostraConfirmacao("Deseja realmente excluir a nota?", MessageBoxImage.Question)) return;


            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNfOutro(sessao);

                repositorio.Deletar(_nfOutro);

                transacao.Commit();
            }


            DialogBox.MostraInformacao("Excluido com sucesso");
            OnFechar();
        }

        private void SalvarAction(object obj)
        {
            try
            {
                ValidaEmpresa();

                ValidaFornecedor();

                ValidaCfop();

                AtualizaDadosNfOutro();

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorio = new RepositorioNfOutro(sessao);

                    repositorio.SalvarOuAtualizar(_nfOutro);

                    transacao.Commit();
                }


                DialogBox.MostraInformacao("Salvo com sucesso");

               OnFechar();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void AtualizaDadosNfOutro()
        {
            _nfOutro.SituacaoFiscal = SituacaoFiscal;
            _nfOutro.TipoEmitente = TipoEmitente;
            _nfOutro.ModeloDocumento = ModeloDocumento;
            _nfOutro.RecebimentoEm = RecebimentoEm;
            _nfOutro.EmissaoEm = EmissaoEm;
            _nfOutro.Numero = Numero;
            _nfOutro.AliquotaIcms = AliquotaIcms;
            _nfOutro.BaseCalculoIcms = BaseCalculoIcms;
            _nfOutro.BaseCalculoIcmsSt = BaseCalculoIcmsSt;
            _nfOutro.Cfop = _cfop;
            _nfOutro.Cst = Icms;
            _nfOutro.Fornecedor = _fornecedor;
            _nfOutro.Serie = Serie;
            _nfOutro.TotalDesconto = TotalDesconto;
            _nfOutro.ValorDespesasAcessorias = DespesasAcessorias;
            _nfOutro.ValorFrete = ValorFrete;
            _nfOutro.ValorIcms = ValorIcms;
            _nfOutro.ValorIcmsSt = ValorIcmsSt;
            _nfOutro.ValorSeguro = ValorSeguro;
            _nfOutro.ValorTotal = ValorTotal;
            _nfOutro.Empresa = _empresa;
        }

        private void ValidaCfop()
        {
            if (_cfop == null)
                throw new InvalidOperationException("Selecione um cfop");
        }

        private void ValidaEmpresa()
        {
            if (_empresa == null)
                throw new InvalidOperationException("Selecione uma empresa");
        }

        private void ValidaFornecedor()
        {
            if (_fornecedor == null)
                throw new InvalidOperationException("Selecione um fornecedor");
        }

        public void Inicializar()
        {
            CarregaIcmsDisponiveis();
            CarregarModel(_nfOutro);
            IsExcluir = _nfOutro.Id != 0;
        }

        private void CarregaIcmsDisponiveis()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                IcmsDisponiveis = new ObservableCollection<TributacaoIcms>(new RepositorioTributacao(sessao).Todos());
            }

            Icms = IcmsDisponiveis.FirstOrDefault();
        }

        private void CarregarModel(NfOutro nfOutro)
        {
            ModeloDocumento = nfOutro.ModeloDocumento;
            TipoEmitente = nfOutro.TipoEmitente;
            SituacaoFiscal = nfOutro.SituacaoFiscal;
            EmissaoEm = nfOutro.EmissaoEm;
            RecebimentoEm = nfOutro.RecebimentoEm;

            if (nfOutro.Id == 0) return;

            Serie = nfOutro.Serie;
            Numero = nfOutro.Numero;

            AtualizaDadosFornecedor(nfOutro.Fornecedor);
            _fornecedor = nfOutro.Fornecedor;

            AtualizaDadosCfop(nfOutro.Cfop);
            _cfop = nfOutro.Cfop;

            AtualizaDadosEmpresa(nfOutro.Empresa);
            _empresa = nfOutro.Empresa;

            Icms = nfOutro.Cst;

            BaseCalculoIcms = nfOutro.BaseCalculoIcms;
            ValorIcms = nfOutro.ValorIcms;
            AliquotaIcms = _nfOutro.AliquotaIcms;
            BaseCalculoIcmsSt = _nfOutro.BaseCalculoIcmsSt;
            ValorIcmsSt = _nfOutro.ValorIcmsSt;
            ValorFrete = _nfOutro.ValorFrete;
            ValorSeguro = _nfOutro.ValorSeguro;
            DespesasAcessorias = _nfOutro.ValorDespesasAcessorias;
            TotalDesconto = _nfOutro.TotalDesconto;
            ValorTotal = _nfOutro.ValorTotal;
        }

        private void AtualizaDadosEmpresa(EmpresaDTO empresa)
        {
            NomeEmpresa = empresa.RazaoSocial;
            DocumentoUnicoEmpresa = empresa.Cnpj;
        }
    }
}