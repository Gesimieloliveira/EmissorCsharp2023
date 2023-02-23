using System;
using Fusion.Sessao;
using FusionCore.FusionAdm.Estoque.Movimentacoes;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using static System.Decimal;

namespace Fusion.Visao.MovimentacaoEstoque.Flyouts
{
    public sealed class MovimentoItemFlyoutModel : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private bool _isOpen;
        private string _nomeProduto;
        private decimal _quantidade;
        private decimal _margemLucro;
        private decimal _precoCompra;
        private decimal _precoCompraTotal;
        private decimal _precoVenda;
        private decimal _precoVendaTotal;
        private ProdutoDTO _produto;
        private bool _isTipoEntrada;
        private bool _isTipoSaida;
        private string _ultimaBusca;

        public MovimentoItemFlyoutModel(TipoEventoEstoque tipo)
        {
            _ultimaBusca = string.Empty;
            InputBusca = string.Empty;
            IsTipoEntrada = tipo == TipoEventoEstoque.Entrada;
            IsTipoSaida = tipo == TipoEventoEstoque.Saida;
        }

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        public bool IsTipoEntrada
        {
            get => _isTipoEntrada;
            set
            {
                if (value == _isTipoEntrada) return;
                _isTipoEntrada = value;
                PropriedadeAlterada();
            }
        }

        public bool IsTipoSaida
        {
            get => _isTipoSaida;
            set
            {
                if (value == _isTipoSaida) return;
                _isTipoSaida = value;
                PropriedadeAlterada();
            }
        }

        public string InputBusca
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string NomeProduto
        {
            get => _nomeProduto;
            set
            {
                if (value == _nomeProduto) return;
                _nomeProduto = value;
                PropriedadeAlterada();
            }
        }

        public decimal Quantidade
        {
            get => _quantidade;
            set
            {
                if (value == _quantidade) return;
                _quantidade = value;
                CalcularPrecoCompraTotal();
                CalcularPrecoVendaTotal();
                PropriedadeAlterada();
            }
        }

        public decimal MargemLucro
        {
            get => _margemLucro;
            set
            {
                if (value == _margemLucro) return;
                _margemLucro = value;
                CalcularPrecoVendaComMargemLucro();
                CalcularPrecoVendaTotal();
                PropriedadeAlterada();
            }
        }

        public decimal PrecoCompra
        {
            get => _precoCompra;
            set
            {
                if (value == _precoCompra) return;
                _precoCompra = value;
                CalcularPrecoVendaComMargemLucro();
                CalcularPrecoCompraTotal();
                CalcularPrecoVendaTotal();
                PropriedadeAlterada();
            }
        }

        public decimal PrecoCompraTotal
        {
            get => _precoCompraTotal;
            set
            {
                if (value == _precoCompraTotal) return;
                _precoCompraTotal = value;
                CalcularPrecoCompraComBaseTotal();
                CalcularPrecoVendaComMargemLucro();
                CalcularPrecoVendaTotal();
                PropriedadeAlterada();
            }
        }

        public decimal PrecoVenda
        {
            get => _precoVenda;
            set
            {
                if (value == _precoVenda) return;
                _precoVenda = value;
                CalcularMargemLucroComBasePrecoVenda();
                CalcularPrecoVendaTotal();
                PropriedadeAlterada();
            }
        }

        public decimal PrecoVendaTotal
        {
            get => _precoVendaTotal;
            set
            {
                if (value == _precoVendaTotal) return;
                _precoVendaTotal = value;
                CalcularPrecoVendaComBaseTotal();
                CalcularMargemLucroComBasePrecoVenda();
                PropriedadeAlterada();
            }
        }

        private void CalcularPrecoCompraComBaseTotal()
        {
            try
            {
                var precoUnitario = PrecoCompraTotal / Quantidade;
                _precoCompra = Round(precoUnitario, 4);
            }
            catch (DivideByZeroException)
            {
                _precoCompra = 0;
            }

            PropriedadeAlterada(nameof(PrecoCompra));
        }

        private void CalcularMargemLucroComBasePrecoVenda()
        {
            try
            {
                var margemLucro = ((PrecoVenda * 100) / PrecoCompra) - 100;
                _margemLucro = Round(margemLucro, 6);
            }
            catch (DivideByZeroException)
            {
                _margemLucro = 0;
            }

            PropriedadeAlterada(nameof(MargemLucro));
        }

        private void CalcularPrecoVendaComBaseTotal()
        {
            try
            {
                var precoVendaUnitario = PrecoVendaTotal / Quantidade;
                _precoVenda = Round(precoVendaUnitario, 4);
            }
            catch (DivideByZeroException)
            {
                _precoVenda = 0;
            }

            PropriedadeAlterada(nameof(PrecoVenda));
        }

        private void CalcularPrecoVendaComMargemLucro()
        {
            var fator = 1 + (MargemLucro / 100);
            var precoVenda = PrecoCompra * fator;
            _precoVenda = Round(precoVenda, 4);

            PropriedadeAlterada(nameof(PrecoVenda));
        }

        private void CalcularPrecoVendaTotal()
        {
            var total = PrecoVenda * Quantidade;
            _precoVendaTotal = Round(total, 2);

            PropriedadeAlterada(nameof(PrecoVendaTotal));
        }

        private void CalcularPrecoCompraTotal()
        {
            var total = PrecoCompra * Quantidade;
            _precoCompraTotal = Round(total, 2);
            PropriedadeAlterada(nameof(PrecoCompraTotal));
        }
        
        public event EventHandler<MovimentoItem> ItemNovoSalvo;

        private void OnItemNovoSalvo(MovimentoItem e)
        {
            ItemNovoSalvo?.Invoke(this, e);
        }

        public void CarregarItemPelaBusca()
        {
            if (_ultimaBusca == InputBusca)
            {
                return;
            }

            _ultimaBusca = InputBusca;

            using (var repositorio = new RepositorioProduto(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                var produto = repositorio.BuscaPeloCodigo(InputBusca);
                CarregarDadosComProduto(produto);
            }

            if (_produto == null)
            {
                throw new InvalidOperationException($"Não localizei produto para este código: {InputBusca}");
            }
        }

        public void CarregarDadosComProduto(ProdutoDTO input)
        {
            _produto = input;

            NomeProduto = _produto?.Nome;
            Quantidade = Quantidade == 0 ? 1 : Quantidade;
            MargemLucro = _produto?.MargemLucro ?? 0.00M;
            PrecoCompra = _produto?.PrecoCompra ?? 0.00M;
            PrecoVenda = _produto?.PrecoVenda ?? 0.00M;

            InputBusca = _produto?.Id.ToString() ?? string.Empty;
            _ultimaBusca = InputBusca;
        }

        public void SalvarItem()
        {
            try
            {
                ProcessarSalvamentoItem();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void ProcessarSalvamentoItem()
        {
            //Tipo saida não necessário utilizar margem de lucro, devendo ficar zerado.
            var margemLucro = IsTipoEntrada ? MargemLucro : 0.00M;

            var movimentoItem = new MovimentoItem(_produto, _sessaoSistema.UsuarioLogado)
            {
                PermitePrecoCompraZerado = IsTipoSaida,
                MargemLucro = margemLucro,
                PrecoCompra = PrecoCompra,
                PrecoVenda = PrecoVenda,
                Quantidade = Quantidade,
                PrecoCompraTotal = PrecoCompraTotal,
                PrecoVendaTotal = PrecoVendaTotal
            };

            OnItemNovoSalvo(movimentoItem);
        }
    }
}