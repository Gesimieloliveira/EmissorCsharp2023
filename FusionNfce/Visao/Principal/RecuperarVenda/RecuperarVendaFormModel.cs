using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Vo;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using FusionNfce.Visao.Principal.Model;
using NHibernate.Criterion;

namespace FusionNfce.Visao.Principal.RecuperarVenda
{
    public class RecuperarVendaFormModel : ViewModel
    {
        private string _buscaRapidaTexto;
        private readonly SessaoManagerNfce _sessaoManager;

        public ObservableCollection<NfceOpcoesVo> Itens
        {
            get => GetValue<ObservableCollection<NfceOpcoesVo>>();
            set => SetValue(value);
        }

        public NfceOpcoesVo ItemSelecionado
        {
            get => GetValue<NfceOpcoesVo>();
            set => SetValue(value);
        }

        public string BuscaRapidaTexto
        {
            get => _buscaRapidaTexto;
            set
            {
                if (value == _buscaRapidaTexto) return;
                _buscaRapidaTexto = value;
                PropriedadeAlterada();
            }
        }

        public RecuperarVendaFormModel()
        {
            _sessaoManager = new SessaoManagerNfce();
        }

        public event EventHandler<NfceEvent> RetornaNfce;

        public void Inicializar()
        {
            AplicarPesquisaRapida();
        }

        public void OnRetornaNfce()
        {
            Nfce itemBuscado;

            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfce = new RepositorioNfce(sessao);
                itemBuscado = repositorioNfce.GetPeloId(ItemSelecionado.Id);

                if (SessaoSistemaNfce.TipoEmissao == TipoEmissao.Normal
                    && ItemSelecionado.TipoEmissao == TipoEmissao.ContigenciaOfflineNFCe
                    && ItemSelecionado.Status == Status.Aberta
                    && ItemSelecionadoTemHistorio() == false)
                {
                    itemBuscado.TipoEmissao = TipoEmissao.Normal;
                }

                transacao.Commit();
            }

            Application.Current.Dispatcher.Invoke(() => RetornaNfce?.Invoke(this, new NfceEvent(itemBuscado)));
        }

        public void AplicarPesquisaRapida(string input = null)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioNfce(sessao);
                var notas = repositorio.BuscaElegiveisParaRecuperacao();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    var notasFiltradas = notas.Where(n =>
                        n.Chave.IsLike(input, MatchMode.Anywhere)
                        || n.Id.ToString() == input
                        || n.NumeroDocumento.ToString() == input);

                    Itens = new ObservableCollection<NfceOpcoesVo>(notasFiltradas);
                    return;
                }

                Itens = new ObservableCollection<NfceOpcoesVo>(notas);
            }
        }

        public bool ItemSelecionadoTemHistorio()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                return new RepositorioNfce(sessao).ExisteHistorico(ItemSelecionado.Id);
            }
        }
    }
}