using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Regras;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Tributacoes.Regras
{
    public class RegraTributacaoSaidaContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private RegraTributacaoSaida _regra;

        public RegraTributacaoSaidaContexto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;

            PerfisDisponiveis = new List<PerfilCfopDTO>();
            CfopDisponveis = new List<CfopDTO>();
            IcmsDisponiveis = new List<TributacaoIcms>();
            CsosnDisponiveis = new List<TributacaoCsosn>();
            IsAtivo = true;
            IsNovo = true;
        }

        public bool IsAtivo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsNovo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public IList<PerfilCfopDTO> PerfisDisponiveis
        {
            get => GetValue<IList<PerfilCfopDTO>>();
            set => SetValue(value);
        }

        public IList<CfopDTO> CfopDisponveis
        {
            get => GetValue<IList<CfopDTO>>();
            set => SetValue(value);
        }

        public IList<TributacaoIcms> IcmsDisponiveis
        {
            get => GetValue<IList<TributacaoIcms>>();
            set => SetValue(value);
        }

        public IList<TributacaoCsosn> CsosnDisponiveis
        {
            get => GetValue<IList<TributacaoCsosn>>();
            set => SetValue(value);
        }

        public string Descricao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public TributacaoIcms Icms
        {
            get => GetValue<TributacaoIcms>();
            set => SetValue(value);
        }

        public TributacaoCsosn Csosn
        {
            get => GetValue<TributacaoCsosn>();
            set => SetValue(value);
        }

        public PerfilCfopDTO PerfilIntermunicipal
        {
            get => GetValue<PerfilCfopDTO>();
            set => SetValue(value);
        }

        public PerfilCfopDTO PerfilInterestadual
        {
            get => GetValue<PerfilCfopDTO>();
            set => SetValue(value);
        }

        public PerfilCfopDTO PerfilExterior
        {
            get => GetValue<PerfilCfopDTO>();
            set => SetValue(value);
        }

        public CfopDTO CfopNfce
        {
            get => GetValue<CfopDTO>();
            set => SetValue(value);
        }

        public void Inicializa()
        {
            CarregarListas();

            if (_regra == null || _regra.Id == 0)
            {
                DefineValoresPadrao();
                return;
            }

            InicializaParaEdicao();
        }

        private void DefineValoresPadrao()
        {
            PerfilIntermunicipal = PerfisDisponiveis.FirstOrDefault(i => i.Codigo == "510201");
            PerfilInterestadual = PerfisDisponiveis.FirstOrDefault(i => i.Codigo == "610201");
            PerfilExterior = PerfisDisponiveis.FirstOrDefault(i => i.Codigo == "710201");
            CfopNfce = CfopDisponveis.FirstOrDefault(i => i.Id == "5102");
        }

        public event EventHandler<RegraTributacaoSaida> SalvoSucesso;

        private void InicializaParaEdicao()
        {
            SetValue(_regra.Id == 0, nameof(IsNovo));
            SetValue(_regra.Ativo, nameof(IsAtivo));
            SetValue(_regra.Descricao, nameof(Descricao));
            SetValue(_regra.Cst, nameof(Icms));
            SetValue(_regra.Csosn, nameof(Csosn));
            SetValue(_regra.CfopIntermunicipal, nameof(PerfilIntermunicipal));
            SetValue(_regra.CfopInterestadual, nameof(PerfilInterestadual));
            SetValue(_regra.CfopExterior, nameof(PerfilExterior));
            SetValue(_regra.CfopNfce, nameof(CfopNfce));
        }

        public void Edicao(RegraTributacaoSaidaSlim slim)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                _regra = new RepositorioRegraTributacao(sessao).GetPeloId(slim.Id);
            }
        }

        private void CarregarListas()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                PerfisDisponiveis = new RepositorioPerfilCfop(sessao).PorOperacao(TipoOperacao.Saida);
                CfopDisponveis = new RepositorioCfop(sessao).BuscarCfopParaNfce();
                IcmsDisponiveis = new RepositorioTributacao(sessao).TodasTributacoesIcmsNFe();
                CsosnDisponiveis = new RepositorioTributacao(sessao).TodasTributacoesCsosn();
            }
        }

        public void SalvaAlteracoes()
        {
            if (_regra == null)
            {
                _regra = new RegraTributacaoSaida();
            }

            _regra.Update(IsAtivo, Descricao, Icms, Csosn, PerfilIntermunicipal, PerfilInterestadual, PerfilExterior, CfopNfce);
            _regra.Valida();

            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                new RepositorioRegraTributacao(sessao).Persiste(_regra);
                transacao.Commit();
            }

            SalvoSucesso?.Invoke(this, _regra);
        }
    }
}