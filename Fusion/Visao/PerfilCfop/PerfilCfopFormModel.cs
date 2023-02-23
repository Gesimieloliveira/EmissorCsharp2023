using System;
using System.Windows.Input;
using Fusion.Visao.Cfop;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.PerfilCfop;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PerfilCfop
{
    public class PerfilCfopFormModel : ViewModel
    {
        private byte _sufixo;
        private string _codigo;
        private bool _ativo;
        private CfopDTO _cfop;
        private PerfilCfopDTO _perfil;

        public string DescricaoCfop
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string IdCfop
        {
            get => GetValue();
            set => SetValue(value);
        }

        public CfopDTO Cfop
        {
            get => _cfop;
            set
            {
                _cfop = value;
                IdCfop = value?.Id;
                DescricaoCfop = value?.Descricao;
            }
        }

        public byte Sufixo
        {
            get => _sufixo;
            set
            {
                if (value == _sufixo) return;
                _sufixo = value;
                PropriedadeAlterada();
            }
        }

        public string Codigo
        {
            get => _codigo;
            set
            {
                if (value == _codigo) return;
                _codigo = value;
                PropriedadeAlterada();
            }
        }

        public bool Ativo
        {
            get => _ativo;
            set
            {
                if (value == _ativo) return;
                _ativo = value;
                PropriedadeAlterada();
            }
        }

        public bool MovimentaEstoque
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string Descricao
        {
            get => GetValue();
            set => SetValue(value);
        }

        public bool IsNovo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ICommand CommandPesquisarCfop => GetSimpleCommand(PickerCfopAction);

        public PerfilCfopFormModel(PerfilCfopDTO perfil)
        {
            _perfil = perfil;
        }

        private void PickerCfopAction(object obj)
        {
            var cfopPicker = new CfopPickerModel();
            cfopPicker.GetPickerView().ShowDialog();

            if (cfopPicker.ItemSelecionado == null)
            {
                return;
            }

            var cfopSelecionado = cfopPicker.ItemSelecionado.ItemReal as CfopDTO;

            Cfop = cfopSelecionado;
            Descricao = cfopSelecionado?.Descricao;

            GeraCodigo();
        }

        public void AtualizaView()
        {
            IsNovo = _perfil.Id == 0;

            Cfop = _perfil.Cfop;
            Sufixo = _perfil.Sufixo;
            Codigo = _perfil.Codigo;
            Ativo = _perfil.Ativo;
            Descricao = _perfil.Descricao;
        }

        public void SalvarModel()
        {
            ThrowExceptionSeDadosInvalidos();

            _perfil.Cfop = Cfop;
            _perfil.Sufixo = Sufixo;
            _perfil.Codigo = Codigo;
            _perfil.Ativo = Ativo;
            _perfil.Descricao = Descricao;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilCfop(sessao);
                repositorio.SalvaAlteracoes(_perfil);
            }

            AtualizaView();
        }

        private void ThrowExceptionSeDadosInvalidos()
        {
            if (Cfop == null)
            {
                throw new InvalidOperationException("Preciso que informe um CFOP");
            }

            if (string.IsNullOrEmpty(Descricao))
            {
                throw new InvalidOperationException("Preciso que informe a descrição");
            }
        }

        public void DeletarModel()
        {
            if (_perfil.Id == 0)
            {
                throw new ArgumentException(
                    "Somente é possível deletar um registro existente, o registro atual não existe ainda...");
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilCfop(sessao);
                repositorio.Deleta(_perfil);
            }
        }

        public void GeraCodigo()
        {
            using (var repositorio = new RepositorioComun<UltimoSufixo>(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                var ultimoSufixo = repositorio.Busca(new BuscaUltimoSufixo(Cfop));

                Sufixo = ultimoSufixo.UsarProximoSufixo();
                Codigo = Cfop.Id + ultimoSufixo.UsarProximoSufixo().ToString("D2");
            }
        }
    }
}