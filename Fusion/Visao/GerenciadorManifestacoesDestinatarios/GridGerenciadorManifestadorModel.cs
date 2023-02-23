using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.GerenciarManifestacoesEletronicas;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using JetBrains.Annotations;

namespace Fusion.Visao.GerenciadorManifestacoesDestinatarios
{
    public class GridGerenciadorManifestadorModel : ViewModel
    {
        private NfeResumidaGrid _nfeResumidaSelecionada;

        public NfeResumidaGrid NfeResumidaSelecionada
        {
            get => _nfeResumidaSelecionada;
            set
            {
                if (Equals(value, _nfeResumidaSelecionada)) return;
                _nfeResumidaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        private ObservableCollection<NfeResumidaGrid> _colecaoObservavelNfeResumida;
        [CanBeNull]
        private EmpresaComboBoxDTO _empresaSelecionada;
        private ObservableCollection<EmpresaComboBoxDTO> _empresas;

        public GridGerenciadorManifestadorModel()
        {
            AtualizarInformacoes();
            AtualizarGrid();
        }

        public void AtualizarInformacoes()
        {
            BuscaTodasEmpresas();
        }

        public ObservableCollection<EmpresaComboBoxDTO> Empresas
        {
            get => _empresas;
            set
            {
                if (Equals(value, _empresas)) return;
                _empresas = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<NfeResumidaGrid> ColecaoObservavelNfeResumida
        {
            get => _colecaoObservavelNfeResumida;
            set
            {
                if (Equals(value, _colecaoObservavelNfeResumida)) return;
                _colecaoObservavelNfeResumida = value;
                PropriedadeAlterada();
            }
        }
        
        public void AtualizarGrid()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioDistribuicaoDfe = new RepositorioDistribuicaoDFe(sessao);

                var lista = repositorioDistribuicaoDfe.BuscaTodosNfeResumida(EmpresaSelecionada?.Id);

                ColecaoObservavelNfeResumida = new ObservableCollection<NfeResumidaGrid>(lista);
            }
        }

        [CanBeNull]
        public EmpresaComboBoxDTO EmpresaSelecionada
        {
            get => _empresaSelecionada;
            set
            {
                if (Equals(value, _empresaSelecionada)) return;
                _empresaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        private void BuscaTodasEmpresas()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmpresa(sessao);

                Empresas = new ObservableCollection<EmpresaComboBoxDTO>(repositorio.BuscarEmpresaComboBoxDtos());
            }
        }

        public DadosDestinatarioDTO GetDadosDestinatarioDTO()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioEmpresa = new RepositorioEmpresa(sessao);

                return repositorioEmpresa.GetDadosDestinatarioDTO(EmpresaSelecionada.Id);
            }
        }
    }
}