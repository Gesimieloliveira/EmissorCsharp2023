using System;
using System.Data;
using System.Linq;
using FusionCore.CadastroEmpresa;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.Core.Flags;
using FusionCore.DI;
using FusionCore.Papeis.Enums;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace FusionWPF.SharedViews.ControleCaixa
{
    public class LancamentoNoCaixaContexto : ContextoObservado<LancamentoNoCaixaContexto>
    {
        private readonly IControleCaixaProvider _caixaProvider;
        private readonly ISessaoManager _sessaoManager;
        private LancamentoAvulsoCaixa _lancamento;
        private IEmpresa _empresa;

        internal LancamentoNoCaixaContexto(
            ISessaoManager sessaoManager,
            IControleCaixaProvider caixaProvider)
        {
            _sessaoManager = sessaoManager;
            _caixaProvider = caixaProvider;

            IsNovo = true;
            IsChamadoPeloNfce = caixaProvider.GetLocalEvento() == ELocalEventoCaixa.Terminal;
            DestinoLancamento = TipoLancamentoCaixa.LancamentoCaixaIndividual;
            DestinoLancamentoIsEnabled = true;
        }

        public bool IsChamadoPeloNfce
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public TipoOperacao? Operacao
        {
            get => GetValue<TipoOperacao?>();
            set => SetValue(value);
        }

        public TipoLancamentoCaixa? DestinoLancamento
        {
            get => GetValue<TipoLancamentoCaixa?>();
            set => SetValue(value);
        }

        public bool DestinoLancamentoIsEnabled
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public decimal ValorOperacao
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public string Motivo
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool IsNovo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void PrepararEdicao(LancamentoAvulsoCaixaDTO dto)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var r = new RepositorioLancamentoAvulso(sessao);
                _lancamento = r.BuscarPeloId(dto.Id);
            }

            IsNovo = false;
        }

        public void Inicializar()
        {
            DestinoLancamentoIsEnabled = _caixaProvider
                .GetUsuarioLogado()
                .VerificaPermissao
                .IsTemPermissao(Permissao.LANCAMENTO_AVULSO_DIRETO_CAIXA_LOJA);

            CarregarEmpresa();

            if (IsNovo)
            {
                Operacao = TipoOperacao.Saida;
                ValorOperacao = 0.00M;
                Motivo = string.Empty;
                return;
            }

            Operacao = _lancamento.TipoOperacao;
            DestinoLancamento = _lancamento.TipoLancamentoCaixa;
            ValorOperacao = _lancamento.ValorOperacao;
            Motivo = _lancamento.Motivo;
        }

        private void CarregarEmpresa()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = _caixaProvider.GetRepositorioEmpresa(sessao);

                _empresa = repositorio.BuscarTodas().FirstOrDefault();
            }
        }

        public void ValidarInformacoes()
        {
            if (!ExisteCaixaAberto() && DestinoLancamento == TipoLancamentoCaixa.LancamentoCaixaIndividual)
                throw new InvalidOperationException("Você não possui um caixa aberto!");

            if (DestinoLancamento == TipoLancamentoCaixa.LancamentoCaixaLoja)
                _caixaProvider.GetUsuarioLogado()
                    .VerificaPermissao
                    .IsTemPermissaoThrow(Permissao.LANCAMENTO_AVULSO_DIRETO_CAIXA_LOJA);

            if (Operacao == null)
                throw new InvalidOperationException("Preciso do Tipo da Operação");

            if (DestinoLancamento == null)
                throw new InvalidOperationException("Preciso do Tipo Lançamento Caixa");

            if (ValorOperacao <= 0)
                throw new InvalidOperationException("Preciso de um Valor maior que 0,00");

            if (Motivo?.Length <= 5)
                throw new InvalidOperationException("Motivo da operação muito curto");

            if (_empresa == null)
                throw new InvalidOperationException("Empresa não localizada");
        }

        private bool ExisteCaixaAberto()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioCaixaIndividual(sessao);
                var existe = repositorio.ExisteCaixaAbertoPara(
                    _caixaProvider.GetUsuarioLogado(),
                    _caixaProvider.GetLocalEvento()
                );

                return existe;
            }
        }

        public void IncluirLancamento()
        {
            using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var servico = new ServicoLancamentoAvulsoNoCaixa(sessao, _caixaProvider.GetLocalEvento());

                if (IsNovo)
                {
                    var novo = new LancamentoAvulsoCaixa(
                        _empresa,
                        _caixaProvider.GetUsuarioLogado(),
                        _caixaProvider.GetLocalEvento(),
                        Operacao.Value,
                        DestinoLancamento.Value,
                        Motivo,
                        ValorOperacao
                    );

                    servico.IncluirNovoLancamento(novo);
                    sessao.Transaction.Commit();

                    return;
                }

                _lancamento.Alterar(Motivo);

                servico.AlterarLancamento(_lancamento);
                sessao.Transaction.Commit();
            }
        }
    }
}