using System.Collections.ObjectModel;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Flyouts
{
    public class FlyoutAddPercursoModel : ViewModel
    {
        private readonly CteOsEmitirFormModel _cteOsEmitirModel;

        public FlyoutAddPercursoModel(CteOsEmitirFormModel cteOsEmitirModel)
        {
            _cteOsEmitirModel = cteOsEmitirModel;
        }

        public bool IsOpen
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ObservableCollection<EstadoDTO> Estados
        {
            get => GetValue<ObservableCollection<EstadoDTO>>();
            set => SetValue(value);
        }

        public EstadoDTO Estado
        {
            get => GetValue<EstadoDTO>();
            set => SetValue(value);
        }

        public void Inicializar()
        {
            var localidades = LocalidadesServico.GetInstancia();
            Estados = new ObservableCollection<EstadoDTO>(localidades.GetEstados());
        }

        public void AdicionarPercurso()
        {
            if (Estado == null)
            {
                throw new RegraNegocioException("Preciso que informe um estado para o percurso");
            }

            var percurso = new CteOsPercurso(Estado);
            _cteOsEmitirModel.AdicionaPercurso(percurso);
        }

        public void Limpar()
        {
            Estado = null;
        }
    }
}