using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fusion.Sessao;
using Fusion.Visao.GerenciadorManifestacoesDestinatarios;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.Facades;
using FusionCore.FusionAdm.Compras;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Compras.NotaFiscal
{
    public class NotaFiscalCompraViewModel : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private NotaFiscalCompra _nota;
        private bool _isRemoverNota;
        private bool _isDataEntradaSaida;

        public NotaFiscalCompraViewModel(NotaFiscalCompra nota = null)
        {
            Itens = new ObservableCollection<ItemCompra>();
            Fornecedor = new FornecedorViewModel();
            Transportadora = new TransportadoraViewModel();

            _nota = nota;
        }

        public ObservableCollection<EmpresaDTO> ListaEmpresas
        {
            get => GetValue<ObservableCollection<EmpresaDTO>>();
            set => SetValue(value);
        }

        public EmpresaDTO Empresa
        {
            get => GetValue<EmpresaDTO>();
            set => SetValue(value);
        }

        public int NumeroDocumento
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public short Serie
        {
            get => GetValue<short>();
            set => SetValue(value);
        }

        public string Chave
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal ValorTotalIpi
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal TotalBaseCalculoIcms
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorTotalIcms
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal TotalBaseCalculoIcmsSt
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorTotalIcmsSt
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorTotalOutros
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorTotalSeguro
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorTotalFrete
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorTotalItens
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorTotalDesconto
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorTotal
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public FornecedorViewModel Fornecedor
        {
            get => GetValue<FornecedorViewModel>();
            set => SetValue(value);
        }

        public TransportadoraViewModel Transportadora
        {
            get => GetValue<TransportadoraViewModel>();
            set => SetValue(value);
        }

        public ModalidadeFrete ModalidadeFrete
        {
            get => GetValue<ModalidadeFrete>();
            set => SetValue(value);
        }

        public ObservableCollection<ItemCompra> Itens
        {
            get => GetValue<ObservableCollection<ItemCompra>>();
            set => SetValue(value);
        }

        public ItemCompra ItemSelecionado
        {
            get => GetValue<ItemCompra>();
            set => SetValue(value);
        }

        public DateTime? EmitidaEm
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public DateTime? EntradaSaidaEm
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public bool ImportadaXml
        {
            get => GetValue<bool>();
            private set
            {
                SetValue(value);
                IsDataEntradaSaida = true;
            }
        }

        public bool IsDataEntradaSaida
        {
            get => _isDataEntradaSaida;
            set
            {
                if (value == _isDataEntradaSaida) return;
                _isDataEntradaSaida = value;
                PropriedadeAlterada();
            }
        }

        public bool PossuiFinanceiro => _nota.PossuiFinanceiro();

        public bool NotaEstaSalva => _nota?.Id > 0;

        public bool IsRemoverNota
        {
            get => _isRemoverNota;
            set
            {
                if (value == _isRemoverNota) return;
                _isRemoverNota = value;
                PropriedadeAlterada();
            }
        }

        public bool NotaTemItens => _nota?.ContaOsItens > 0;
        public bool EmpresaEnabled => _nota == null || _nota.Id <= 0;
        public NotaFiscalCompra GetNota() => _nota;

        public void Inicializar()
        {
            var usuarioLogado = _sessaoSistema.UsuarioLogado;
            var isPermissaoRemoverCompra = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.COMPRAS_REMOVER);

            if (isPermissaoRemoverCompra)
                IsRemoverNota = _nota?.Id > 0;

            if (isPermissaoRemoverCompra == false)
                IsRemoverNota = false;

            CarregaEmpresa();

            if (_nota != null)
            {
                PreencherModel();
                return;
            }

            EmitidaEm = DateTime.Now;
            EntradaSaidaEm = DateTime.Now;
        }

        private void CarregaEmpresa()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioEmpresa(sessao);
                var empresas = repositorio.BuscaTodos();

                ListaEmpresas = new ObservableCollection<EmpresaDTO>(empresas);
                Empresa = empresas.FirstOrDefault();
            }
        }

        private void PreencherModel()
        {
            Empresa = _nota.Empresa;
            Fornecedor.CarregarCom(_nota.Fornecedor);
            Transportadora.CarregarCom(_nota.Transportadora);
            NumeroDocumento = _nota.NumeroDocumento;
            EmitidaEm = _nota.EmitidaEm;
            EntradaSaidaEm = _nota.EntradaSaidaEm;
            Serie = _nota.Serie;
            Chave = _nota.Chave.ToString();
            ValorTotalIpi = _nota.ValorTotalIpi;
            TotalBaseCalculoIcms = _nota.TotalBcIcms;
            ValorTotalIcms = _nota.ValorTotalIcms;
            TotalBaseCalculoIcmsSt = _nota.TotalBcIcmsSt;
            ValorTotalIcmsSt = _nota.ValorTotalIcmsSt;
            ValorTotalOutros = _nota.ValorTotalOutros;
            ValorTotalSeguro = _nota.ValorTotalSeguro;
            ValorTotalFrete = _nota.ValorTotalFrete;
            ValorTotalItens = _nota.ValorTotalItens;
            ValorTotalDesconto = _nota.ValorTotalDesconto;
            ValorTotal = _nota.ValorTotal;
            ImportadaXml = _nota.ImportadoDeXml;

            PropriedadeAlterada(nameof(EmpresaEnabled));
            PropriedadeAlterada(nameof(NotaEstaSalva));
            PropriedadeAlterada(nameof(NotaTemItens));

            Itens = new ObservableCollection<ItemCompra>(_nota.Itens);

            PropriedadeAlterada(nameof(PossuiFinanceiro));
        }

        public int ObtemMaloteDaNota()
        {
            if (_nota.Malote == null)
            {
                throw new InvalidOperationException("Nota não possui malote de documentos a pagar!");
            }

            return _nota.Malote.Id;
        }

        public void SalvarAlteracoes()
        {
            try
            {
                PrepararNotaCompra();

                _nota?.ExisteItemInativoThrow();

                SalvarNota();
                PreencherModel();
                DialogBox.MostraInformacao("Nota de compra foi salva com sucesso!");
            }
            catch (RegraNegocioException e)
            {
                if (e.TemDetalhes)
                {
                    DialogBox.MostraAviso("Nota inválida", string.Join("\n", e.Detalhes));
                    return;
                }

                DialogBox.MostraAviso(e.Message);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void SalvarNota()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNotaFiscalCompra(sessao);
                repositorio.Salvar(_nota);

                if (_nota.PossuiChave)
                    new NfeResumidaImportadaServico(_nota.Chave.Chave).Importada();

                transacao.Commit();
            }
        }

        private void PrepararNotaCompra()
        {
            if (_nota == null)
            {
                _nota = new NotaFiscalCompra(Empresa);
            }

            var emitidaEm = EmitidaEm ?? DateTime.Now;
            var entradaSaidaEm = EntradaSaidaEm ?? DateTime.Now;

            _nota.Empresa = Empresa;
            _nota.NumeroDocumento = NumeroDocumento;
            _nota.Serie = Serie;
            _nota.Chave = string.IsNullOrWhiteSpace(Chave) ? ChaveSefaz.Empty : new ChaveSefaz(Chave);
            _nota.EmitidaEm = emitidaEm;
            _nota.EntradaSaidaEm = entradaSaidaEm;
            _nota.Fornecedor = Fornecedor.Get();
            _nota.Transportadora = Transportadora.Get();
            _nota.ModalidadeFrete = ModalidadeFrete;
            _nota.ValorTotalFrete = ValorTotalFrete;
            _nota.ValorTotalSeguro = ValorTotalSeguro;
            _nota.ValorTotalOutros = ValorTotalOutros;
            _nota.ValorTotalDesconto = ValorTotalDesconto;

            _nota.CalculaTotais();

            NotaFiscalCompraValidator.ValidaOsDados(_nota);
            NotaFiscalCompraValidator.ValidaExistencia(_nota);
        }

        public void RefreshNota()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                sessao.Refresh(_nota);
                _nota.InicializaLazy();
            }

            PreencherModel();
        }

        public void AlteraValoresNota(TotaisCompraChildViewModel model)
        {
            if (_nota == null)
            {
                ValorTotalFrete = model.ValorFrete;
                ValorTotalSeguro = model.ValorSeguro;
                ValorTotalOutros = model.ValorDespesas;
                return;
            }

            _nota.RateiaOsCustosNosItens(model.ValorFrete, model.ValorSeguro, model.ValorDespesas);
            _nota.CalculaTotais();

            if (_nota.Id > 0)
            {
                SalvarNota();
            }

            PreencherModel();
        }

        public void RemoveItemSelecionado()
        {
            _nota.Remover(ItemSelecionado);
            _nota.CalculaTotais();

            SalvarNota();
            PreencherModel();
        }

        public void EstornaDocumentosApagar(IList<DocumentoPagar> documentos)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var facadePagar = new DocumentoPagarFacade(_sessaoManager);
                facadePagar.EstornarDocumentos(documentos, _sessaoSistema.UsuarioLogado, "Estornado por ação do usuário na nota.");

                _nota.Malote = null;
                new RepositorioNotaFiscalCompra(sessao).Salvar(_nota);

                transacao.Commit();
            }

            PropriedadeAlterada(nameof(PossuiFinanceiro));
        }

        public void ExcluiNotaCompra()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var facadePagar = new DocumentoPagarFacade(_sessaoManager);
                var repositorioCompra = new RepositorioNotaFiscalCompra(sessao);

                if (_nota.PossuiFinanceiro())
                {
                    var repositorioPagar = new RepositorioDocumentoPagar(sessao);
                    var documentos = repositorioPagar.BuscarDocumentoPagarDeMalote(_nota.Malote);

                    var total = documentos.Sum(i => i.ValorAjustado);
                    var msgConfirmacao = $"Nota possui documentos a pagar no valor total de: {total:C2}, todos os documentos serão estornados. Deseja continuar?";

                    if (DialogBox.MostraConfirmacao(msgConfirmacao) != MessageBoxResult.Yes)
                    {
                        return;
                    }

                    facadePagar.EstornarDocumentos(documentos, _sessaoSistema.UsuarioLogado, "Exclusão de nota de compra.");
                }

                if (_nota.PossuiChave)
                    new NfeResumidaImportadaServico(_nota.Chave.Chave).Deletou();

                repositorioCompra.Deleta(_nota);
                transacao.Commit();
            }

            OnFechar();
        }
    }
}