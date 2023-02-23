using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TerminalOffline;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Emissores;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Terminal
{
    public class TerminalOfflineFormModel : ViewModel
    {
        private TerminalOffline _terminal;
        private ObservableCollection<EmissorFiscal> _emissoresFiscaisSelecionados = new ObservableCollection<EmissorFiscal>();

        public TerminalOfflineFormModel(TerminalOffline terminalOffline)
        {
            _terminal = terminalOffline;
        }

        public bool EPossivelResetar
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Porfavor digitar uma descrição")]
        public string Descricao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public EmissorFiscal EmissorFiscal
        {
            get => GetValue<EmissorFiscal>();
            set => SetValue(value);
        }

        [Range(10, 3600, ErrorMessage = @"Intervalo vai de 10 segundos até 3600 (60 min)")]
        public int IntervaloSync
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public bool Ativo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ObservableCollection<EmissorFiscal> ListaEmissorFiscal
        {
            get => GetValue<ObservableCollection<EmissorFiscal>>();
            set => SetValue(value);
        }

        public string BindTerminal
        {
            get => GetValue<string>();
            set
            {
                SetValue(value);
                TerminalVinculado = !string.IsNullOrWhiteSpace(value);
            }
        }

        public bool TerminalVinculado
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public string Observacao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ObservableCollection<EmissorFiscal> EmissoresFiscaisSelecionados
        {
            get => _emissoresFiscaisSelecionados;
            set
            {
                if (Equals(value, _emissoresFiscaisSelecionados)) return;
                _emissoresFiscaisSelecionados = value;
                PropriedadeAlterada();
            }
        }

        public ICommand ComandoAdicionarEmissor => GetSimpleCommand(AdicionarEmissorAcao);

        private void AdicionarEmissorAcao(object obj)
        {
            try
            {
                if (EmissorFiscal == null)
                {
                    throw new InvalidOperationException("Preciso que escolha um Emissor antes.");
                }

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorioTerminalOffline = new RepositorioTerminalOffline(sessao);

                    if (JaEstaAdicionadoNaLista(EmissorFiscal) || repositorioTerminalOffline.EmissorJaVinculado(EmissorFiscal))
                    {
                        throw new InvalidOperationException("Emissor fiscal a um vinculado terminal");
                    }

                    if (repositorioTerminalOffline.EmissorParaFaturamento(EmissorFiscal))
                    {
                        throw new InvalidOperationException("Emissor fiscal para faturamento");
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
                return;
            }

            EmissoresFiscaisSelecionados.Add(EmissorFiscal);
            EmissorFiscal = null;
        }

        private bool JaEstaAdicionadoNaLista(EmissorFiscal emissorFiscal)
        {
            return EmissoresFiscaisSelecionados.Any(x => x == emissorFiscal);
        }

        public void BuscarEmissoresFiscal()
        {
            using (var repositorio = new RepositorioComun<EmissorFiscal>(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                var todosEmissorFiscal = repositorio.Busca(new TodosEmissorFiscalComboBox());

                ListaEmissorFiscal = new ObservableCollection<EmissorFiscal>(todosEmissorFiscal);
            }
        }

        public void Inicializar()
        {
            BuscarEmissoresFiscal();
            BuscaTerminal();

            Descricao = _terminal.Descricao;
            Ativo = _terminal.Ativo;
            IntervaloSync = _terminal.IntervaloSync / 1000 == 0 ? 10 : _terminal.IntervaloSync / 1000;
            BindTerminal = _terminal.BindTerminal;
            Observacao = _terminal.Observacao;

            EmissoresFiscaisSelecionados = new ObservableCollection<EmissorFiscal>(_terminal.EmissorFiscalLista);

            if (_terminal.Id == 0) return;

            if(!string.IsNullOrEmpty(_terminal.BindTerminal))
                EPossivelResetar = true;
        }

        private void BuscaTerminal()
        {
            if (_terminal.Id == 0) return;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _terminal = new RepositorioTerminalOffline(sessao).GetPeloId(_terminal.Id);
            }
        }

        public void SalvarModel()
        {
            ThrowExceptionSeExistirErros();

            if (EmissoresFiscaisSelecionados.Count == 0)
                throw new InvalidOperationException("Vincule um emissor fiscal pelo menos");

            _terminal.Descricao = Descricao;
            _terminal.Ativo = Ativo;
            _terminal.IntervaloSync = IntervaloSync * 1000;
            _terminal.BindTerminal = BindTerminal ?? string.Empty;
            _terminal.Observacao = Observacao ?? string.Empty;
            _terminal.EmissorFiscalLista.Clear();
            _terminal.Impressora = _terminal.Impressora ?? string.Empty;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transaction = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioTerminalOffline(sessao);

                repositorio.Salvar(_terminal);

                foreach (var emissorFiscal in EmissoresFiscaisSelecionados)
                {
                    var repositorioEmissorFiscal = new RepositorioEmissorFiscal(sessao);
                    var emissorBuscado = repositorioEmissorFiscal.GetPeloId(emissorFiscal.Id);

                    emissorBuscado.TerminalOffline = _terminal;
                    repositorioEmissorFiscal.Salvar(emissorBuscado);
                }
                transaction.Commit();
            }
        }

        public void DesvinculacaoBindTerminal()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _terminal.BindTerminal = string.Empty;

                var repositorio = new RepositorioTerminalOffline(sessao);
                repositorio.Salvar(_terminal);

                transacao.Commit();
            }

            BindTerminal = string.Empty;
        }

        public void RemoverEmissorLista(EmissorFiscal emissorSelecionadoLista)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioEmissorFiscal = new RepositorioEmissorFiscal(sessao);
                var emissor = repositorioEmissorFiscal.GetPeloId(emissorSelecionadoLista.Id);

                emissor.TerminalOffline = null;

                transacao.Commit();
            }
            EmissoresFiscaisSelecionados.Remove(emissorSelecionadoLista);
        }
    }
}