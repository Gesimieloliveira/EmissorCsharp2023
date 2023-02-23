using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.Cartoes;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Tef;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Contratos;

namespace FusionNfce.Visao.Principal.FinalizarVenda.CartoesPos
{
    public class CartaoPosFormModel : ViewModel, IChildContext
    {
        public CartaoPosFormModel()
        {
            InicializaListaPos();
        }

        private void InicializaListaPos()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                ListaPos = new ObservableCollection<PosNfce>(new RepositorioPosNfce(sessao).BuscarPosParaNFce() ?? new List<PosNfce>());
            }

            if (ListaPos.Count == 0) return;

            PosSelecionado = ListaPos.FirstOrDefault();
        }

        public event EventHandler<CartaoPosFormModel> EnviarDadosCartaoPos; 

        private string _cnpjCredenciadora;
        private CartaoBandeira _cartaoBandeira = CartaoBandeira.Nemnhum;
        private string _numeroAutorizacao;
        private ObservableCollection<PosNfce> _listaPos;
        private PosNfce _posSelecionado;
        public string TituloChild { get; } 
        public event EventHandler SolicitaFechamento;

        public string CnpjCredenciadora
        {
            get => _cnpjCredenciadora;
            set
            {
                if (value == _cnpjCredenciadora) return;
                _cnpjCredenciadora = value;
                PropriedadeAlterada();
            }
        }

        public CartaoBandeira CartaoBandeira
        {
            get => _cartaoBandeira;
            set
            {
                if (value == _cartaoBandeira) return;
                _cartaoBandeira = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroAutorizacao
        {
            get => _numeroAutorizacao;
            set
            {
                if (value == _numeroAutorizacao) return;
                _numeroAutorizacao = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<PosNfce> ListaPos
        {
            get => _listaPos;
            set
            {
                if (Equals(value, _listaPos)) return;
                _listaPos = value;
                PropriedadeAlterada();
            }
        }

        public PosNfce PosSelecionado
        {
            get => _posSelecionado;
            set
            {
                if (Equals(value, _posSelecionado)) return;
                _posSelecionado = value;
                CnpjCredenciadora = value?.CnpjCredenciadora ?? string.Empty;
                PropriedadeAlterada();
            }
        }

        public void OnEnviarDadosCartaoPos()
        {
            EnviarDadosCartaoPos?.Invoke(this, this);
        }
    }
}