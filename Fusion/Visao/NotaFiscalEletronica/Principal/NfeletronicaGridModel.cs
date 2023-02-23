using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fusion.Sessao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.NfeEletronica;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Principal
{
    public class NfeletronicaGridModel : ViewModel
    {
        private readonly UsuarioDTO _usuario = SessaoSistema.Instancia.UsuarioLogado;

        public NfeletronicaGridModel()
        {
            Filtro = new NfeGridFiltroBuilder();
        }

        public ObservableCollection<NfeletronicaGrid> NotasFiscais
        {
            get { return GetValue<ObservableCollection<NfeletronicaGrid>>(); }
            set { SetValue(value); }
        }

        public NfeletronicaGrid NotaSelecionada
        {
            get { return GetValue<NfeletronicaGrid>(); }
            set { SetValue(value); }
        }

        public string TextoPesquisa
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public NfeGridFiltroBuilder Filtro
        {
            get => GetValue<NfeGridFiltroBuilder>();
            private set => SetValue(value);
        }

        public void Inicializar()
        {
            AplicarPesquisa();
        }

        public void AplicarPesquisa()
        {
            IList<NfeletronicaGrid> nfes;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioNfe(sessao);
                nfes = repositorio.BuscaRegistros(Filtro);
            }

            NotasFiscais = new ObservableCollection<NfeletronicaGrid>(nfes.OrderByDescending(n => n.Id));
        }

        public void EditarSelecionada()
        {
            if (NotaSelecionada.IsFinalizada)
            {
                OpcoesDaNota();
                return;
            }

            new NfeletronicaWizzard(NotaSelecionada).ShowDialog();
            Inicializar();
        }

        public void NovaNota()
        {
            new NfeletronicaWizzard().ShowDialog();
            Inicializar();
        }

        public void OpcoesDaNota()
        {
            if (NotaSelecionada.IsFinalizada != true)
            {
                DialogBox.MostraInformacao("Nota ainda não foi finalizada");
                return;
            }

            new NfeletronicaOpcoes(NotaSelecionada).ShowDialog();
            Inicializar();
        }

        public void AplicarFiltro()
        {
            AplicarPesquisa();
        }
    }
}