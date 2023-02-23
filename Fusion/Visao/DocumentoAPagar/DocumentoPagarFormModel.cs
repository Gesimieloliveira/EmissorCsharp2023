using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Fusion.Base.Notificacoes;
using Fusion.Sessao;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.ValidacaoAnotacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;

namespace Fusion.Visao.DocumentoAPagar
{
    public class DocumentoPagarFormModel : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly DocumentoPagar _documentoPagar;
        private readonly Notificador _notificador;

        public DocumentoPagarFormModel(DocumentoPagar documentoPagar, Notificador notificador)
        {
            _documentoPagar = documentoPagar;
            _notificador = notificador;

            TiposDocumentos = new List<TipoDocumento>();
            DataEmissao = DateTime.Now;
            Vencimento = DateTime.Now.AddDays(30);

        }

        public ICommand BuscarPessoa => GetSimpleCommand(ShowModalBuscaPessoa);

        public ICollection<TipoDocumento> TiposDocumentos
        {
            get => GetValue<ICollection<TipoDocumento>>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Preciso que selecione a empresa!")]
        public EmpresaComboBoxDTO EmpresaSelecionada
        {
            get => GetValue<EmpresaComboBoxDTO>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Preciso que escolha o tipo do documento!")]
        public TipoDocumento TipoDocumento
        {
            get => GetValue<TipoDocumento>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Preciso que escolha um fornecedor!")]
        public Fornecedor Fornecedor
        {
            get => GetValue<Fornecedor>();
            set => SetValue(value);
        }

        [DecimalRequired(ErrorMessage = @"Preciso que informe o valor total do documento!")]
        public decimal? Valor
        {
            get => GetValue<decimal?>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Preciso que informe a data vencimento!")]
        public DateTime? Vencimento
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Preciso que informe a data emissao!")]
        public DateTime? DataEmissao
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public string NumeroDocumento
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Historico
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public byte Parcela
        {
            get => GetValue<byte>();
            set => SetValue(value);
        }

        public Situacao Situacao
        {
            get => GetValue<Situacao>();
            set => SetValue(value);
        }

        private void ShowModalBuscaPessoa(object obj)
        {
            var picker = new PessoaPickerModel(new FornecedorEngine());
            picker.PickItemEvent += FornecedorSelecionado;
            picker.GetPickerView().Show();
        }

        private void FornecedorSelecionado(object sender, GridPickerEventArgs e)
        {
            Fornecedor = e.GetItem<Fornecedor>();
        }

        public void AtualizarModel()
        {
            PreencheTiposDocumentos();

            NumeroDocumento = _documentoPagar.NumeroDocumento;
            Situacao = _documentoPagar.Situacao;
            TipoDocumento = _documentoPagar.TipoDocumento;
            Fornecedor = _documentoPagar.Fornecedor;
            Parcela = _documentoPagar.Parcela;
            Valor = _documentoPagar.ValorAjustado;
            Vencimento = _documentoPagar.Vencimento;
            Historico = _documentoPagar.Descricao.TrimOrEmpty();

            EmpresaSelecionada = _documentoPagar.Empresa?.ToComboBox();
        }

        private void PreencheTiposDocumentos()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTipoDocumento(sessao);
                TiposDocumentos = repositorio.BuscaTodos();
            }
        }

        public void SalvarRegistro()
        {
            ThrowExceptionSeNaoTiverPermissao();
            ThrowExceptionSeExistirErros();

            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioDocumentoPagar(sessao);

                if (_documentoPagar.Id == 0)
                {
                    _documentoPagar.Situacao = Situacao.Aberto;
                    _documentoPagar.ValorOriginal = Valor.Value;
                    _documentoPagar.Parcela = Parcela;
                }

                CalculaSeTemDescontoOuAcrescimo(repositorio);

                _documentoPagar.Empresa = EmpresaSelecionada.CarregaEmpresa(sessao);
                _documentoPagar.Fornecedor = Fornecedor;
                _documentoPagar.TipoDocumento = TipoDocumento;
                _documentoPagar.NumeroDocumento = NumeroDocumento.TrimOrEmpty();
                _documentoPagar.Descricao = Historico.TrimOrEmpty();
                _documentoPagar.ValorAjustado = Valor.Value;
                _documentoPagar.Vencimento = Vencimento;
                _documentoPagar.EmitidoEm = DataEmissao.Value;

                var repositorioMalote = new RepositorioMalote(sessao);
                repositorioMalote.Salvar(_documentoPagar.Malote);
                repositorio.Salvar(_documentoPagar);

                transacao.Commit();
            }

            _notificador.Notificar("documentoPagarSalvo", new NotificacaoArgs(_documentoPagar));
        }

        private void ThrowExceptionSeNaoTiverPermissao()
        {
            var usuarioLogado = _sessaoSistema.UsuarioLogado;

            if (_documentoPagar.Id == 0)
            {
                usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.FINANCEIRO_DOCUMENTO_APAGAR_GERAR_AVULSO);
            }

            if (_documentoPagar.Id != 0)
            {
                usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.FINANCEIRO_DOCUMENTO_APAGAR_ALTERAR);
            }
        }

        private void CalculaSeTemDescontoOuAcrescimo(IRepositorioDocumentoPagar repositorio)
        {
            if (_documentoPagar.Id == 0)
            {
                return;
            }

            var valorAjustado = _documentoPagar.ValorAjustado;
            var valorIgualAjuste = Valor == valorAjustado;

            if (valorIgualAjuste)
            {
                return;
            }

            if (Valor < valorAjustado)
            {
                var valorDesconto = _documentoPagar.ValorAjustado - Valor.Value;

                if (_documentoPagar.ValorQuitado > Valor)
                    throw new InvalidOperationException(
                        "Não e possivel fazer um ajuste para menos, porque temos uma quitação maior que o ajuste " +
                        _documentoPagar.ValorQuitado.ToString("C"));

                var ajusteMenos = DocumentoPagarLancamento.Cria(
                    string.Empty,
                    valorDesconto,
                    TipoLancamento.AjusteParaMenos,
                    _documentoPagar
                );

                repositorio.SalvarLancamento(ajusteMenos);

                return;
            }

            var valorAcresimo = Valor.Value - _documentoPagar.ValorAjustado;

            var ajusteMais = DocumentoPagarLancamento.Cria(
                string.Empty,
                valorAcresimo,
                TipoLancamento.AjusteParaMais,
                _documentoPagar
            );

            _documentoPagar.Situacao = Situacao.Aberto;

            repositorio.SalvarLancamento(ajusteMais);
        }

        public void EstornarDocumento()
        {
            if (_documentoPagar.PossuiLancamentoNaoEstornado())
            {
                throw new InvalidOperationException(
                    "Preciso que estorne os lançamentos existentes para continuar!");
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _documentoPagar.Estornar();

                var repositorio = new RepositorioDocumentoPagar(sessao);
                repositorio.Salvar(_documentoPagar);

                transacao.Commit();
            }

            _notificador.Notificar("documentoPagarEstornado", new NotificacaoArgs(_documentoPagar));
        }
    }
}