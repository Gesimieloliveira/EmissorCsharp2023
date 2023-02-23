using System.Collections.ObjectModel;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Tributacoes.Estadual;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Fiscal.Estadual
{
    public class AliquotaInternaFormModel : ViewModel
    {
        public AliquotaInternaFormModel()
        {
            AtualizarListagem();
        }

        private AliquotaInterna _aliquotaInternaSelecionada;
        private ObservableCollection<AliquotaInterna> _aliquotasInternas;

        public ObservableCollection<AliquotaInterna> AliquotasInternas
        {
            get => _aliquotasInternas;
            set
            {
                _aliquotasInternas = value;
                PropriedadeAlterada();
            }
        }

        public AliquotaInterna AliquotaInternaSelecionada
        {
            get => _aliquotaInternaSelecionada;
            set
            {
                _aliquotaInternaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public void AtualizarListagem()
        {
            using (var repositorioAliquotaInterna = new RepositorioAliquotaInterna(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                AliquotasInternas = new ObservableCollection<AliquotaInterna>(repositorioAliquotaInterna.BuscaTodos());
            }
        }
    }
}