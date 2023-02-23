using System.Collections.Generic;
using System.Linq;
using Fusion.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.ContextoCompartilhado
{
    public class EmpresaComboBoxContexto : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema;

        public EmpresaComboBoxContexto(SessaoSistema sessaoSistema)
        {
            _sessaoSistema = sessaoSistema;

            EmpresasDisponiveis = new List<EmpresaComboBoxDTO>();
        }

        public IEnumerable<EmpresaComboBoxDTO> EmpresasDisponiveis
        {
            get => GetValue<IList<EmpresaComboBoxDTO>>();
            private set => SetValue(value);
        }

        public EmpresaComboBoxDTO EmpresaSelecionada
        {
            get => GetValue<EmpresaComboBoxDTO>();
            set => SetValue(value);
        }

        public void CarregarEmpresasDisponiveis()
        {
            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioEmpresa(sessao);

                EmpresasDisponiveis = repositorio.BuscarEmpresaComboBoxDtos();

                if (EmpresaSelecionada == null && EmpresasDisponiveis.Count() == 1)
                {
                    EmpresaSelecionada = EmpresasDisponiveis.First();
                }
            }

            PropriedadeAlterada(nameof(EmpresasDisponiveis));
            PropriedadeAlterada(nameof(EmpresaSelecionada));
        }
    }
}