using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using FusionCore.FusionAdm.ContingenciaSefaz;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.NotaFiscalEletronica.Contingencia
{
    public class HistoricoContingenciaViewModel : ViewModel
    {
        public ObservableCollection<ContingenciaNfe> Historicos { get; set; }

        public ContingenciaNfe ContigenciaSelecionada
        {
            get { return GetValue<ContingenciaNfe>(); }
            set { SetValue(value); }
        }

        public IniciarContigenciaViewModel IniciarContigenciaModel
        {
            get { return GetValue<IniciarContigenciaViewModel>(); }
            set { SetValue(value); }
        }

        public HistoricoContingenciaViewModel()
        {
            Historicos = new ObservableCollection<ContingenciaNfe>();
        }

        public void Inicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var contingencias = new RepositorioContingenciaNfe(sessao).BuscaTodos();

                Historicos.Clear();
                contingencias?.OrderByDescending(c => c.Id).ForEach(Historicos.Add);
            }
        }

        public void FinalizarContigenciaSelecionada()
        {
            var contigencia = ContigenciaSelecionada;
            var msgConfirm = $"Deseja finalizar essa contigência; iniciada em: {contigencia.IniciadaEm.ToString("g")}";

            if (DialogBox.MostraConfirmacao(msgConfirm) != MessageBoxResult.Yes)
                return;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioContingenciaNfe(sessao);

                contigencia.FinalizarContigencia();
                repositorio.Alterar(contigencia);
            }

            Inicializar();
        }

        public void AbrirFlyoutIniciarContingencia()
        {
            IniciarContigenciaModel = new IniciarContigenciaViewModel(true);
            IniciarContigenciaModel.IniciadaNovaContingencia += IniciadaNovaContingenciaHandler;
            IniciarContigenciaModel.Inicializar();
        }

        private void IniciadaNovaContingenciaHandler(object sender, ContingenciaNfe e)
        {
            IniciarContigenciaModel.IsOpen = false;
            Inicializar();
        }
    }
}