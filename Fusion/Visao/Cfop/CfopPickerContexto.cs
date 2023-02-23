using System;
using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Cfop
{
    public class CfopPickerContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private TipoOperacao? _filtroOperacao;
        private OrigemOperacao? _filtroOrigem;

        public CfopPickerContexto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public string TextoPesquisa
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public CfopDTO CfopSelecionado
        {
            get => GetValue<CfopDTO>();
            set => SetValue(value);
        }

        public IEnumerable<CfopDTO> Cfops
        {
            get => GetValue<IEnumerable<CfopDTO>>();
            private set => SetValue(value);
        }

        public event EventHandler<CfopDTO> CfopFoiSelecionado;

        public void MostrarApenasOsDeOperacao(TipoOperacao operacao)
        {
            _filtroOperacao = operacao;
        }

        public void MostrarApenasOsDeOrigem(OrigemOperacao origem)
        {
            _filtroOrigem = origem;
        }

        public void CarregarDados()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioCfop(sessao);
                var resultado = repositorio.BuscaRapida(TextoPesquisa, _filtroOperacao, _filtroOrigem);

                Cfops = resultado;
            }
        }

        public void NotificarEscolhaDoCfop()
        {
            CfopFoiSelecionado?.Invoke(this, CfopSelecionado);
        }
    }
}