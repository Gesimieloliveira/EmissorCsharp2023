using System;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts.Models
{
    public class FlyoutAlteraEmissorModel : ViewModel
    {
        private readonly Nfeletronica _nfe;
        private readonly ISessaoManager _sessaoManager;

        public bool IsOpen
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<EmissorFiscal> EmissoresDisponiveis
        {
            get { return GetValue<ObservableCollection<EmissorFiscal>>(); }
            set { SetValue(value); }
        }

        public EmissorFiscal EmissorSelecionado
        {
            get { return GetValue<EmissorFiscal>(); }
            set { SetValue(value); }
        }

        public FlyoutAlteraEmissorModel(ISessaoManager sessaoManager, Nfeletronica nfe)
        {
            _sessaoManager = sessaoManager;
            _nfe = nfe;
        }

        public event EventHandler<Nfeletronica> EmissorAlterado;

        public void Inicializar()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioEmissor = new RepositorioEmissorFiscal(sessao);
                var emissores = repositorioEmissor.BuscaTodosNfe();

                EmissoresDisponiveis = new ObservableCollection<EmissorFiscal>(emissores);
                EmissorSelecionado = _nfe.Emitente.CarregarDadosEmissor(_sessaoManager);
            }
        }

        public void SalvarAlteracao()
        {
            ThrowExceptionSeAlteracaoRegimeInvalida();

            _nfe.Emitente.AlterarEmissor(EmissorSelecionado);

            _nfe.NumeroEmissao = 0;
            _nfe.SerieEmissao = 0;

            SalvarAlteracao(_nfe);
            EmissorAlterado?.Invoke(this, _nfe);

            IsOpen = false;
        }

        private void ThrowExceptionSeAlteracaoRegimeInvalida()
        {
            if (!_nfe.Itens.Any())
            {
                return;
            }

            if (_nfe.Emitente.RegimeTributario != EmissorSelecionado.Empresa.RegimeTributario)
            {
                throw new InvalidOperationException("Regime tributário das empresas são diferentes, necessário remover os itens antes!");
            }
        }

        private void SalvarAlteracao(Nfeletronica _nfe)
        {
            if (_nfe.Id <= 0)
            {
                return;
            }

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioNfe(sessao);
                repositorio.SalvarAlteracoes(_nfe);
            }
        }
    }
}