using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Fusion.Sessao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models
{
    public class PerfilSelecionadoEventArgs : EventArgs
    {
        public AbaPerfilNfeDTO Perfil { get; }

        public PerfilSelecionadoEventArgs(AbaPerfilNfeDTO perfil)
        {
            Perfil = perfil;
        }
    }

    public sealed class AbaPerfilPickerModel : ViewModel
    {

        private string _textoPesquisado;
        private IEnumerable<AbaPerfilNfeDTO> _cache;
        private bool _isPermissaoCadastrarPerfil;
        public ObservableCollection<AbaPerfilNfeDTO> Lista { get; set; }
        public AbaPerfilNfeDTO ItemSelecionado { get; set; }

        public ICommand CommandPesquisaRapida => GetSimpleCommand(PesquisaRapida);

        public bool Selecionado
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string TextoPesquisado
        {
            get => _textoPesquisado;
            set
            {
                if (value == _textoPesquisado) return;
                _textoPesquisado = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPermissaoCadastrarPerfil
        {
            get => _isPermissaoCadastrarPerfil;
            set
            {
                if (value == _isPermissaoCadastrarPerfil) return;
                _isPermissaoCadastrarPerfil = value;
                PropriedadeAlterada();
            }
        }

        private void PesquisaRapida(object obj)
        {
            FazerPesquisa();
        }

        public void FazerPesquisa()
        {
            Lista.Clear();
            if (string.IsNullOrEmpty(TextoPesquisado))
            {
                FazCache();
                _cache.ForEach(p => Lista.Add(p));
                return;
            }

            var listatemp = _cache.Where(p => p.Descricao.Contains(TextoPesquisado) || p.RazaoSocialEmpresa.Contains(TextoPesquisado) || p.CnpjEmpresa == TextoPesquisado).ToList();

            if (listatemp.Count == 0) return;

            listatemp.ForEach(Lista.Add);
        }

        public AbaPerfilPickerModel()
        {
            Lista = new ObservableCollection<AbaPerfilNfeDTO>();
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            IsPermissaoCadastrarPerfil = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PERFIL_NFE);
        }

        public event EventHandler<PerfilSelecionadoEventArgs> PerfilSelecionadoCalled;

        public void OnLoadedView()
        {
            if (Selecionado == false)
                return;

            FazCache();

            _cache.ForEach(p => Lista.Add(p));
        }

        private void FazCache()
        {
            Lista.Clear();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilNfe(sessao);
                _cache = repositorio.BuscaParaSelecaoAbaPerfil();
            }
        }

        public void SelecionaPerfil()
        {
            if (ItemSelecionado == null) return;
            OnPerfilSelecionadoCalled(new PerfilSelecionadoEventArgs(ItemSelecionado));
        }

        private void OnPerfilSelecionadoCalled(PerfilSelecionadoEventArgs e)
        {
            PerfilSelecionadoCalled?.Invoke(this, e);
        }
    }
}