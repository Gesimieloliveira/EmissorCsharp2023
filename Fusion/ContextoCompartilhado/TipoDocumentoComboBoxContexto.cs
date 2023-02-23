using System.Collections.Generic;
using Fusion.Sessao;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.ContextoCompartilhado
{
    public class TipoDocumentoComboBoxContexto : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema;

        public TipoDocumentoComboBoxContexto(SessaoSistema sessaoSistema)
        {
            _sessaoSistema = sessaoSistema;
        }

        public IEnumerable<TipoDocumento> TiposDisponiveis
        {
            get => GetValue<IEnumerable<TipoDocumento>>();
            private set => SetValue(value);
        }

        public TipoDocumento TipoSelecionado
        {
            get => GetValue<TipoDocumento>();
            set => SetValue(value);
        }

        public void CarregarTipos()
        {
            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao())
            {
                var repostirio = new RepositorioTipoDocumento(sessao);
                var tipos = repostirio.BuscaTodos();

                TiposDisponiveis = tipos;
            }

            PropriedadeAlterada(nameof(TiposDisponiveis));
            PropriedadeAlterada(nameof(TipoSelecionado));
        }
    }
}