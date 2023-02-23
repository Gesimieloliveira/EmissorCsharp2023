using System;
using System.Collections.ObjectModel;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Vendedores;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.VisaoModel;

namespace FusionNfce.Visao.ConsultaVendedores
{
    public class ConsultaVendedorFormModel : ViewModel
    {
        private ObservableCollection<VendedorNfce> _vendedores = new ObservableCollection<VendedorNfce>();
        private string _textoPesquisa;
        private VendedorNfce _vendedorSelecionado;
        private int _qtdeMaximaItens;

        public event EventHandler<VendedorNfce> FoiSelecionado;

        public VendedorNfce VendedorSelecionado
        {
            get => _vendedorSelecionado;
            set
            {
                _vendedorSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<VendedorNfce> Vendedores
        {
            get => _vendedores;
            set
            {
                _vendedores = value;
                PropriedadeAlterada();
            }
        }

        public string TextoPesquisa
        {
            get => _textoPesquisa;
            set
            {
                _textoPesquisa = value;
                PropriedadeAlterada();
            }
        }

        public int QtdeMaximaItens
        {
            get => _qtdeMaximaItens;
            set
            {
                _qtdeMaximaItens = value;
                PropriedadeAlterada();
            }
        }

        public void CarregarDadosDosVendedores()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var vendedores = new RepositorioPessoaNfce(sessao).BuscarTodosVendedoresAtivos();

                Vendedores = new ObservableCollection<VendedorNfce>(vendedores);
            }

            QtdeMaximaItens = Vendedores.Count;
        }

        public void Selecionar()
        {
            if (VendedorSelecionado == null)
            {
                throw new InvalidOperationException("Nenhum produto para ser selecionado");
            }

            FoiSelecionado?.Invoke(this, VendedorSelecionado);
        }
    }
}