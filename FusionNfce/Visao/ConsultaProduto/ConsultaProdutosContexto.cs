using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Repositorio.Filtros.BuscaProdutoNfce;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace FusionNfce.Visao.ConsultaProduto
{
    public class ConsultaProdutosContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private readonly TabelaPrecoNfce _tabelaPrecoNfce;
        private ObservableCollection<IOpcaoBuscaProdutoNfce> _tipoBusca;
        private IOpcaoBuscaProdutoNfce _tipoBuscaSelecionada;

        public event EventHandler<UltimaBuscaEfetuadaDoDia> UltimaBuscaEfetuadaSalvar; 

        public ConsultaProdutosContexto(ISessaoManager sessaoManager, UltimaBuscaEfetuadaDoDia ultimaBuscaEfetuadaDoDia, TabelaPrecoNfce tabelaPrecoNfce)
        {
            _sessaoManager = sessaoManager;
            _tabelaPrecoNfce = tabelaPrecoNfce;

            Produtos = new List<ProdutoBaseDTO>();
            QtdeMaximaItens = SessaoSistemaNfce.Preferencia?.LimiteBuscaGirdProduto ?? 500;
            TipoBusca = new ObservableCollection<IOpcaoBuscaProdutoNfce>
            {
                new FiltroConsultaNfceProdutoBase(),
                new FiltroConsultaNfceProdutoReferencia(),
                new FiltroConsultaNfceProdutoAlternativo()
            };

            TipoBuscaSelecionada = TipoBusca[0];

            CarregarBuscaAntiga(ultimaBuscaEfetuadaDoDia);
        }

        public ObservableCollection<IOpcaoBuscaProdutoNfce> TipoBusca
        {
            get => _tipoBusca;
            set
            {
                if (Equals(value, _tipoBusca)) return;
                _tipoBusca = value;
                PropriedadeAlterada();
            }
        }

        public IOpcaoBuscaProdutoNfce TipoBuscaSelecionada
        {
            get => _tipoBuscaSelecionada;
            set
            {
                if (Equals(value, _tipoBuscaSelecionada)) return;
                _tipoBuscaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public IEnumerable<ProdutoBaseDTO> Produtos
        {
            get => GetValue<IList<ProdutoBaseDTO>>();
            private set
            {
                SetValue(value);
                PropriedadeAlterada(nameof(QtdeMaximaFoiAlcancada));
            }
        }

        public int QtdeMaximaItens
        {
            get => GetValue<int>();
            private set => SetValue(value);
        }

        public ProdutoBaseDTO ProdutoSelecionado
        {
            get => GetValue<ProdutoBaseDTO>();
            set => SetValue(value);
        }

        public string TextoPesquisa
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool QtdeMaximaFoiAlcancada => Produtos.Count() >= QtdeMaximaItens;

        public event EventHandler<ProdutoBaseDTO> FoiSelecionado;

        public void CarregarDadosDosProdutos()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                Produtos = TipoBuscaSelecionada.Listar(QtdeMaximaItens, TextoPesquisa, sessao);

                Produtos.ForEach(produto =>
                {
                    produto.PrecoOriginal = produto.PrecoVenda;
                    var atualizaPrecoProdutoTabela = new AtualizaPrecoProdutoTabelaPreco(_tabelaPrecoNfce);
                    produto = atualizaPrecoProdutoTabela.CalculaProdutoComBaseTabelaPreco(new RepositorioTabelaPrecoNfce(sessao), produto) as ProdutoBaseDTO;
                });
            }
        }

        public void Selecionar()
        {
            if (ProdutoSelecionado == null)
            {
                throw new InvalidOperationException("Nenhum produto para ser selecionado");
            }

            OnUltimaBuscaEfetuadaSalvar();
            FoiSelecionado?.Invoke(this, ProdutoSelecionado);
        }

        private void CarregarBuscaAntiga(UltimaBuscaEfetuadaDoDia ultimaBuscaEfetuadaDoDia)
        {
            if (SessaoSistemaNfce.Preferencia.SalvarUltimaBuscaProduto == false) return;

            if (ultimaBuscaEfetuadaDoDia == null) return;

            TipoBuscaSelecionada = ultimaBuscaEfetuadaDoDia.OpcaoBusca;
            TextoPesquisa = ultimaBuscaEfetuadaDoDia.TextoBuscado;
            CarregarDadosDosProdutos();
        }

        protected virtual void OnUltimaBuscaEfetuadaSalvar()
        {
            UltimaBuscaEfetuadaSalvar?.Invoke(this, new UltimaBuscaEfetuadaDoDia(TipoBuscaSelecionada, TextoPesquisa));
        }
    }
}