using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Fusion.Sessao;
using Fusion.Visao.CteEletronicoOs.Perfil;
using FusionCore.FusionAdm.CteEletronicoOs.Perfil;
using FusionCore.FusionAdm.Servico.CteEletronicoOs.Perfil;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model
{
    public class AbaCteOsPerfilCteOsModel : AbaCTeOSViewModel
    {
        private string _textoPesquisado;
        private ObservableCollection<AbaPerfilCteOsDTO> _lista;
        private AbaPerfilCteOsDTO _itemSelecionado;
        private bool _isGerenciarCteOs;
        public ICommand NovoPerfilCommand => GetSimpleCommand(NovoPerfilAction);

        public event EventHandler<AbaCteOsPerfilCteOsModel> PerfilSelecionado; 

        private void NovoPerfilAction(object obj)
        {
            new PerfilCteOsForm(new PerfilCteOsFormModel(new PerfilCteOs())).ShowDialog();
            AtualizarListaPerfil();
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

        public ObservableCollection<AbaPerfilCteOsDTO> Lista
        {
            get => _lista;
            set
            {
                if (Equals(value, _lista)) return;
                _lista = value;
                PropriedadeAlterada();
            }
        }

        public AbaPerfilCteOsDTO ItemSelecionado
        {
            get => _itemSelecionado;
            set
            {
                if (Equals(value, _itemSelecionado)) return;
                _itemSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandPesquisaRapida => GetSimpleCommand(PesquisaRapidaAction);

        public bool IsGerenciarCteOs
        {
            get => _isGerenciarCteOs;
            set
            {
                if (value == _isGerenciarCteOs) return;
                _isGerenciarCteOs = value;
                PropriedadeAlterada();
            }
        }

        private static void PesquisaRapidaAction(object obj)
        {
            
        }

        public void Inicializa()
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
            IsGerenciarCteOs = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PERFIL_CTE_OS);

            AtualizarListaPerfil();

            HabilitaAba();
        }

        private void AtualizarListaPerfil()
        {
            using (var repositorio = new RepositorioPerfilCteOs(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                var lista = new ServicoPerfilCteOs(repositorio).BuscarPerfilDTO();

                Lista = new ObservableCollection<AbaPerfilCteOsDTO>(lista);
            }
        }

        public void SelecionarPerfil()
        {
            OnPerfilSelecionado();
        }

        protected virtual void OnPerfilSelecionado()
        {
            PerfilSelecionado?.Invoke(this, this);
        }
    }
}