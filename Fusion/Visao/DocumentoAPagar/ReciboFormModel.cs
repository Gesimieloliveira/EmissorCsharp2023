using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Recibos;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.DocumentoAPagar
{
    public class ReciboFormModel : ViewModel
    {
        private string _nomePessoa;
        private string _documentoUnico;
        private EmpresaComboBoxDTO _empresaSelecionada;
        private ObservableCollection<EmpresaComboBoxDTO> _empresas;
        private CidadeDTO _cidadeSelecionada;
        private string _referente;
        private decimal _valor;
        private bool _isPagando;

        public event EventHandler FocusTela;

        public bool IsPagando
        {
            get => _isPagando;
            set
            {
                if (value == _isPagando) return;
                _isPagando = value;
                PropriedadeAlterada();
            }
        }

        public string NomePessoa
        {
            get => _nomePessoa;
            set
            {
                if (value == _nomePessoa) return;
                _nomePessoa = value;
                PropriedadeAlterada();
            }
        }

        public string DocumentoUnico
        {
            get => _documentoUnico;
            set
            {
                if (value == _documentoUnico) return;
                _documentoUnico = value;
                PropriedadeAlterada();
            }
        }

        public EmpresaComboBoxDTO EmpresaSelecionada
        {
            get => _empresaSelecionada;
            set
            {
                if (Equals(value, _empresaSelecionada)) return;
                _empresaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public CidadeDTO CidadeSelecionada
        {
            get => _cidadeSelecionada;
            set
            {
                if (Equals(value, _cidadeSelecionada)) return;
                _cidadeSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EmpresaComboBoxDTO> Empresas
        {
            get => _empresas;
            set
            {
                if (Equals(value, _empresas)) return;
                _empresas = value;
                PropriedadeAlterada();
            }
        }

        public DateTime DataPagamento
        {
            get => GetValue<DateTime>();
            set => SetValue(value);         
        }

        public string Referente
        {
            get => _referente;
            set
            {
                if (value == _referente) return;
                _referente = value;
                PropriedadeAlterada();
            }
        }

        public ICommand GerarReciboCommand => GetSimpleCommand(GerarReciboAction);
        public ICommand BuscarPessoaCommand => GetSimpleCommand(BuscarPessoaAction);

        public decimal Valor
        {
            get => _valor;
            set
            {
                if (value == _valor) return;
                _valor = value;
                PropriedadeAlterada();
            }
        }

        private void BuscarPessoaAction(object obj)
        {
            var gridPicker = new PessoaPickerModel(new PessoaEngine());

            gridPicker.PickItemEvent += SelecionouPessoa;

            gridPicker.GetPickerView().ShowDialog();
        }

        private void SelecionouPessoa(object sender, GridPickerEventArgs e)
        {
            var pessoa = e.GetItem<PessoaEntidade>();

            NomePessoa = pessoa.Nome;
            DocumentoUnico = pessoa.GetDocumentoUnico();
        }

        private void GerarReciboAction(object obj)
        {
            if (NomePessoa.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Digite um nome para a pessoa");
                return;
            }

            if (DocumentoUnico.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Digite um cpf/cnpj/documento para a pessoa");
                return;
            }

            if (EmpresaSelecionada == null)
            {
                DialogBox.MostraInformacao("Selecione uma empresa");
                return;
            }

            if (CidadeSelecionada == null)
            {
                DialogBox.MostraInformacao("Selecione uma cidade");
                return;
            }

            if (Referente.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Adicionar Referente");
                return;
            }

            if (Valor <= 0)
            {
                DialogBox.MostraInformacao("Valor não pode ser Zero ou menor");
                return;
            }

            var valorDoRecibo = decimal.Parse(Valor.ToString("N2"));

            var reciboDto = new ReciboDTO
            {
                CidadeDTO = CidadeSelecionada,
                Empresa = BuscarEmpresaSelecionada(),
                FeitoEm = DataPagamento,
                Pagando = IsPagando,
                Pessoa = ObterPessoaSelecionada(),
                Valor = valorDoRecibo,
                ValorPorExtenso = valorDoRecibo.EscreverExtenso(),
                Referente = Referente
            };

            var recibo = new RRecibo(new SessaoManagerAdm());
            recibo.ComReciboDto(reciboDto);

            recibo.Visualizar();
        }

        private ReciboPessoaDTO ObterPessoaSelecionada()
        {
            return new ReciboPessoaDTO
            {
                DocumentoUnico = DocumentoUnico,
                Nome = NomePessoa
            };
        }

        public void Inicializa()
        {
            BuscaTodasEmpresas();
            EmpresaSelecionada = Empresas[0];
            CidadeSelecionada = BuscarEmpresaSelecionada().CidadeDTO;
            DataPagamento = DateTime.Now;
            IsPagando = false;
        }

        private EmpresaDTO BuscarEmpresaSelecionada()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmpresa(sessao);
                return repositorio.GetPeloId(EmpresaSelecionada.Id);
            }
        }

        private void BuscaTodasEmpresas()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmpresa(sessao);

                Empresas = new ObservableCollection<EmpresaComboBoxDTO>(repositorio.BuscarEmpresaComboBoxDtos());
            }
        }

        protected virtual void OnFocusTela()
        {
            FocusTela?.Invoke(this, EventArgs.Empty);
        }

        public void Preencher(DocumentoPagar documentoPagar)
        {
            NomePessoa = documentoPagar.Fornecedor.Nome;
            DocumentoUnico = documentoPagar.Fornecedor.Cnpj.Valor;
            EmpresaSelecionada = new EmpresaComboBoxDTO { Id = documentoPagar.Empresa.Id, Nome = documentoPagar.Empresa.NomeFantasia };
            CidadeSelecionada = documentoPagar.Empresa.CidadeDTO;
            Referente = $"Pagamento de {documentoPagar.Descricao}";
            Valor = documentoPagar.ValorQuitado;
        }
    }
}