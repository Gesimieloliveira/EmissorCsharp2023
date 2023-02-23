using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using FusionCore.FusionAdm.TabelasDePrecos.Dtos;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;
using FusionCore.FusionNfce.Preferencias;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace FusionNfce.Visao.Configuracao
{
    public class PreferenciasTerminalContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private PreferenciaTerminal _preferencia;
        private bool _solicitaDadosCartaoPos;
        private bool _isNfce;
        private int _limiteBuscaGirdProduto;
        private bool _salvarUltimaBuscaProduto;
        private string _nomeFantasiaCustomizado;
        private bool _naoImprimir;
        private LayoutImpressao _layoutImpressao = LayoutImpressao.Impressao80M;

        public PreferenciasTerminalContexto(ISessaoManager sessaoManager)
        {
            ImpressorasExistentes = new List<string>();

            _sessaoManager = sessaoManager;
            IsNfce = SessaoSistemaNfce.IsEmissorNFce();
        }

        public bool IsNfce
        {
            get => _isNfce;
            set
            {
                if (value == _isNfce) return;
                _isNfce = value;
                PropriedadeAlterada();
            }
        }

        public bool SolicitaInformacaoItem
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool VisualizaAntesDeImprimir
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public IEnumerable<string> ImpressorasExistentes
        {
            get => GetValue<IEnumerable<string>>();
            set => SetValue(value);
        }

        public string NomeImpressora
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool SolicitaDadosCartaoPos
        {
            get => _solicitaDadosCartaoPos;
            set
            {
                if (value == _solicitaDadosCartaoPos) return;
                _solicitaDadosCartaoPos = value;
                PropriedadeAlterada();
            }
        }

        public int LimiteBuscaGirdProduto
        {
            get => _limiteBuscaGirdProduto;
            set
            {
                if (value == _limiteBuscaGirdProduto) return;
                _limiteBuscaGirdProduto = value;
                PropriedadeAlterada();
            }
        }

        public bool SalvarUltimaBuscaProduto
        {
            get => _salvarUltimaBuscaProduto;
            set
            {
                if (value == _salvarUltimaBuscaProduto) return;
                _salvarUltimaBuscaProduto = value;
                PropriedadeAlterada();
            }
        }

        public string NomeFantasiaCustomizado
        {
            get => _nomeFantasiaCustomizado;
            set
            {
                _nomeFantasiaCustomizado = value;
                PropriedadeAlterada();
            }
        }

        public bool NaoImprimir
        {
            get => _naoImprimir;
            set
            {
                _naoImprimir = value;
                PropriedadeAlterada();
            }
        }

        public LayoutImpressao LayoutImpressao
        {
            get => _layoutImpressao;
            set
            {
                if (value == _layoutImpressao) return;
                _layoutImpressao = value;
                PropriedadeAlterada();
            }
        }

        public IEnumerable<TabelaPrecoDto> ListaTabelasDisponiveis
        {
            get => GetValue<IEnumerable<TabelaPrecoDto>>();
            set => SetValue(value);
        }

        public TabelaPrecoDto TabelaPrecoPadraoSelecionada
        {
            get => GetValue<TabelaPrecoDto>();
            set => SetValue(value);
        }

        public bool ConfirmacaoTabelaPadraoAntesVenda
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void CarregarImpressoras()
        {
            var prints = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            ImpressorasExistentes = prints;
        }

        public void CarregarTabelas()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var repositorio = new RepositorioTabelaPrecoNfce(sessao);
                ListaTabelasDisponiveis = repositorio.BuscarTodasTabelasDto();
            }
        }

        public void CarregarInformacoes()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioPreferencia(sessao);
                _preferencia = repositorio.BuscarExistente();
            }

            if (_preferencia == null) return;

            SolicitaInformacaoItem = _preferencia.SolicitaInformacaoItem;
            VisualizaAntesDeImprimir = _preferencia.VisualizaAntesDeImprimir;
            NomeImpressora = _preferencia.NomeImpressora;
            SolicitaDadosCartaoPos = _preferencia.SolicitaDadosCartaoPos;
            LimiteBuscaGirdProduto = _preferencia.LimiteBuscaGirdProduto;
            SalvarUltimaBuscaProduto = _preferencia.SalvarUltimaBuscaProduto;
            NomeFantasiaCustomizado = _preferencia.NomeFantasiaCustomizado;
            NaoImprimir = _preferencia.NaoImprimir;
            LayoutImpressao = _preferencia.LayoutImpressao;
            TabelaPrecoPadraoSelecionada = ListaTabelasDisponiveis.FirstOrDefault(i => i.Id == _preferencia.TabelaPrecoPadrao);
            ConfirmacaoTabelaPadraoAntesVenda = _preferencia.ConfirmacaoTabelaPadraoAntesVenda;
        }

        public void SalvarAlteracoes()
        {
            if (string.IsNullOrWhiteSpace(NomeImpressora))
            {
                throw new InvalidOperationException("Preciso que escolha uma impressora");
            }

            if (LimiteBuscaGirdProduto < 99)
            {
                throw new InvalidOperationException("Limite de busca de produtos. Mínimo 99");
            }

            if (LimiteBuscaGirdProduto > 2000000)
            {
                throw new InvalidOperationException("Limite de busca de produtos máximo 2.000.000");
            }

            if (_preferencia == null)
            {
                _preferencia = new PreferenciaTerminal();
            }

            _preferencia.SolicitaInformacaoItem = SolicitaInformacaoItem;
            _preferencia.VisualizaAntesDeImprimir = VisualizaAntesDeImprimir;
            _preferencia.NomeImpressora = NomeImpressora;
            _preferencia.SolicitaDadosCartaoPos = SolicitaDadosCartaoPos;
            _preferencia.LimiteBuscaGirdProduto = LimiteBuscaGirdProduto;
            _preferencia.SalvarUltimaBuscaProduto = SalvarUltimaBuscaProduto;
            _preferencia.NomeFantasiaCustomizado = NomeFantasiaCustomizado.TrimOrEmpty();
            _preferencia.NaoImprimir = NaoImprimir;
            _preferencia.LayoutImpressao = LayoutImpressao;
            _preferencia.ConfirmacaoTabelaPadraoAntesVenda = ConfirmacaoTabelaPadraoAntesVenda;
            _preferencia.TabelaPrecoPadrao = TabelaPrecoPadraoSelecionada?.Id;

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioPreferencia(sessao);
                repositorio.SalvarAlteracoes(_preferencia);
            }

            SessaoSistemaNfce.Preferencia = _preferencia;
        }
    }
}