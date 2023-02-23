using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Fusion.Sessao;
using FusionCore.FusionAdm.CteEletronico;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace Fusion.Visao.CteEletronico.Emitir.Aba.Models
{
    public class PerfilCteSelecionado : EventArgs
    {
        public PerfilCte PerfilCte { get; }

        public PerfilCteSelecionado(PerfilCte perfilCte)
        {
            PerfilCte = perfilCte;
        }
    }

    public class AbaPerfilCteModel : ModelBase
    {
        private IList<PerfilCteListBoxDTO> _cache;
        private string _textoPesquisado;
        private PerfilCteListBoxDTO _itemSelecionado;
        private bool _isGerenciarPerfilCte;

        public string TextoPesquisado
        {
            get { return _textoPesquisado; }
            set
            {
                if (value == _textoPesquisado) return;
                _textoPesquisado = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandPesquisaRapida => GetSimpleCommand(PesquisaRapida);
        public ObservableCollection<PerfilCteListBoxDTO> Lista { get; set; }

        public PerfilCteListBoxDTO ItemSelecionado
        {
            get { return _itemSelecionado; }
            set
            {
                if (Equals(value, _itemSelecionado)) return;
                _itemSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarPerfilCte
        {
            get => _isGerenciarPerfilCte;
            set
            {
                if (value == _isGerenciarPerfilCte) return;
                _isGerenciarPerfilCte = value;
                PropriedadeAlterada();
            }
        }

        public AbaPerfilCteModel()
        {
            _cache = new List<PerfilCteListBoxDTO>();
            Lista = new ObservableCollection<PerfilCteListBoxDTO>();

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            IsGerenciarPerfilCte = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PERFIL_CTE);
        }

        public event EventHandler<PerfilCteSelecionado> PerfilSelecionado;

        private void PesquisaRapida(object obj)
        {
            EfetuaPesquisaRapida();
        }

        public void EfetuaPesquisaRapida()
        {
            if (string.IsNullOrEmpty(TextoPesquisado))
            {
                AtualizaListaComNovosDados(_cache);
                return;
            }

            var lista = _cache.Where(p => p.Descricao.ToUpper().Contains(TextoPesquisado.ToUpper())
                                          || p.RazaoSocialEmpresa.ToUpper().Contains(TextoPesquisado.ToUpper())
                                          || p.CnpjEmpresa == TextoPesquisado).ToList();

            AtualizaListaComNovosDados(lista);
        }

        public void Inicializar()
        {
            FazCache();
        }

        private void FazCache()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilCte(sessao);

                _cache = repositorio.BuscaTodosParaListBoxEmissao();

                AtualizaListaComNovosDados(_cache);
            }
        }

        private void AtualizaListaComNovosDados(IList<PerfilCteListBoxDTO> lista)
        {
            Lista.Clear();
            lista.ForEach(Lista.Add);
        }

        public void SelecionaPerfil()
        {
            OnPerfilSelecionado(BuscarPerfil());
        }

        private PerfilCte BuscarPerfil()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilCte(sessao);

                return repositorio.GetPeloId(ItemSelecionado.Id);
            }
        }

        protected virtual void OnPerfilSelecionado(PerfilCte e)
        {
            PerfilSelecionado?.Invoke(this, new PerfilCteSelecionado(e));
        }
    }
}