using System.Collections.ObjectModel;
using FusionCore.FusionAdm.Fiscal.Contratos;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models
{
    public class VolumesModel : ModelBase
    {
        private IVolume _selecionado;
        private Nfeletronica _nfe;
        private ObservableCollection<IVolume> _volumes;

        public VolumesModel()
        {
            Volumes = new ObservableCollection<IVolume>();
        }

        public ObservableCollection<IVolume> Volumes
        {
            get => _volumes;
            private set
            {
                if (Equals(value, _volumes)) return;
                _volumes = value;
                PropriedadeAlterada();
            }
        }

        public IVolume Selecionado
        {
            get => _selecionado;
            set
            {
                if (Equals(value, _selecionado)) return;
                _selecionado = value;
                PropriedadeAlterada();
            }
        }

        public void Adicionar(IVolume volume)
        {
            if (_nfe != null && _nfe.Id != 0)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioNfe(sessao);
                    repositorio.Persistir(volume);
                }
            }

            Volumes.Add(volume);
        }

        public void RemoverSelecionado()
        {
            if (Selecionado.Id != 0)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioNfe(sessao);
                    repositorio.Deletar(Selecionado);
                }
            }

            Volumes.Remove(Selecionado);
        }

        public bool NaoTemVolume()
        {
            return Volumes.Count == 0;
        }

        public void PreecherCom(Nfeletronica nfe)
        {
            _nfe = nfe;

            Volumes = new ObservableCollection<IVolume>();
            nfe.Volumes?.ForEach(v => { Volumes.Add(v); });
        }
    }
}