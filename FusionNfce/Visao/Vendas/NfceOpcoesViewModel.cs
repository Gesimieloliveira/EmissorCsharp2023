using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Vo;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.VisaoModel;
using FusionNfce.Visao.Autorizacao.Opcoes;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Vendas
{
    public class NfceOpcoesViewModel : ViewModel
    {
        private string _buscaRapidaTexto;
        private DateTime? _emissaoInicial;
        private DateTime? _emissaoFinal;

        public NfceOpcoesVo ItemSelecionado
        {
            get => GetValue<NfceOpcoesVo>();
            set => SetValue(value);
        }

        public ObservableCollection<NfceOpcoesVo> Itens
        {
            get => GetValue<ObservableCollection<NfceOpcoesVo>>();
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

        public DateTime? EmissaoInicial
        {
            get => _emissaoInicial;
            set
            {
                if (value.Equals(_emissaoInicial)) return;
                _emissaoInicial = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? EmissaoFinal
        {
            get => _emissaoFinal;
            set
            {
                if (value.Equals(_emissaoFinal)) return;
                _emissaoFinal = value;
                PropriedadeAlterada();
            }
        }

        public void Inicializar()
        {
            EmissaoInicial = DateTime.Now;
            EmissaoFinal = DateTime.Now;
            AplicarFiltro();
        }

        public void AplicarFiltro()
        {
            if (DataEmissaoInicialEDataEmissaoFinalIsNotNull() && EmissaoInicial > EmissaoFinal)
            {
                DialogBox.MostraInformacao("Emissão Inicial deve ser menor que Emissão Final");
                return;
            }

            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorio = new RepositorioNfce(sessao);
                IList<NfceOpcoesVo> notas = new List<NfceOpcoesVo>();

                if (SessaoSistemaNfce.IsEmissorNFce())
                {
                    notas = repositorio.BuscarParaOpcoes(BuscaRapidaTexto, EmissaoInicial, EmissaoFinal);
                }

                if (SessaoSistemaNfce.IsEmissorSat())
                {
                    notas = repositorio.BuscaParaOpcoesSat(BuscaRapidaTexto, EmissaoInicial, EmissaoFinal);
                }

                Itens = new ObservableCollection<NfceOpcoesVo>(notas);
                ItemSelecionado = null;
            }
        }

        private bool DataEmissaoInicialEDataEmissaoFinalIsNotNull()
        {
            return EmissaoInicial != null && EmissaoFinal != null;
        }

        public void CancelarNfce()
        {
            if (SessaoSistemaNfce.EstaEmContingencia() && ItemSelecionado.Autorizado)
            {
                DialogBox.MostraInformacao("Essa nota já foi transmitida.\nSair da contingencia para ter ações com essa nota");
                return;
            }

            if (ItemSelecionado.IsCancelada)
            {
                DialogBox.MostraInformacao("Essa nota está cancelada.");
                return;
            }

            Nfce nfceCompleta;

            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorio = new RepositorioNfce(sessao);
                nfceCompleta = repositorio.GetPeloId(ItemSelecionado.Id);
            }

            if (nfceCompleta == null)
            {
                return;
            }

            new NfceOpcoes(new NfceOpcoesModel(nfceCompleta)).ShowDialog();

            AplicarFiltro();
        }
    }
}