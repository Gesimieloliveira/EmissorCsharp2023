using System;
using FusionCore.Helpers.Basico;
using FusionCore.Papeis.Anotacoes;
using FusionCore.Papeis.Enums;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Usuario
{
    public class OpcaoPermissao : ViewModel
    {
        private static readonly Type TypePermissao = typeof(Permissao);

        public OpcaoPermissao(Permissao permissao)
        {
            var info = TypePermissao.GetField(permissao.ToString());
            var attrs = (PermissaoDetalhe[]) info.GetCustomAttributes(typeof(PermissaoDetalhe), false);

            Descricao = attrs[0].Descricao;
            Grupo = attrs[0].Grupo;
            SubGrupo = attrs[0].SubGrupo;
            SubGrupoTexto = SubGrupo.GetDescription();

            Permissao = permissao;
        }

        public Permissao Permissao
        {
            get => GetValue<Permissao>();
            private set => SetValue(value);
        }

        public string Descricao
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public PermissaoGrupo Grupo
        {
            get => GetValue<PermissaoGrupo>();
            private set => SetValue(value);
        }

        public PermissaoSubGrupo SubGrupo
        {
            get => GetValue<PermissaoSubGrupo>();
            private set => SetValue(value);
        }

        public string SubGrupoTexto
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public bool IsChecked
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                PermissaoAlterada?.Invoke(this, this);
            }
        }

        public event EventHandler<OpcaoPermissao> PermissaoAlterada;

        public void DefinirPermissaoSemNotificacao(bool permite)
        {
            SetValue(permite, nameof(IsChecked));
        }
    }
}