using System;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Flags;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models
{
    public sealed class AbaCabecalhoModel : ViewModel
    {
        private Nfeletronica _nfe;
        private readonly ISessaoManager _sessaoManager;

        public AbaCabecalhoModel(ISessaoManager sessaoManager)
        {
            ModalidadeFrete = ModalidadeFrete.SemFrete;
            EmitidaEm = DateTime.Now;
            PermiteOpcaoInformacaoIbpt = true;
            IncluirInformacaoIbpt = true;
            IsEnableTipoOperacao = true;
            _sessaoManager = sessaoManager;

        }

        public bool Selecionado
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public DateTime EmitidaEm
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime? SaidaEm
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public TipoOperacao TipoOperacao
        {
            get => GetValue<TipoOperacao>();
            set
            {
                SetValue(value);
                DefinirPermissaoParaInformacaoIbpt();
                DefinirValorParaInformacaoIbpt();
            }
        }

        public string NaturezaOperacao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public FinalidadeEmissao FinalidadeEmissao
        {
            get => GetValue<FinalidadeEmissao>();
            set
            {
                SetValue(value);
                DefinirPermissaoParaInformacaoIbpt();
                DefinirValorParaInformacaoIbpt();
            }
        }

        public ModalidadeFrete ModalidadeFrete
        {
            get => GetValue<ModalidadeFrete>();
            set => SetValue(value);
        }

        public string InformacaoAdicional
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string NomeEmitente
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string DocumentoUnico
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string InscricaoEstadual
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public RegimeTributario RegimeTributario
        {
            get => GetValue<RegimeTributario>();
            set => SetValue(value);
        }

        public bool IsNovo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string DescricaoCertificado
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int NumeroDocumento
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public short SerieDocumento
        {
            get => GetValue<short>();
            set => SetValue(value);
        }

        public bool IsEnabledAlocarNovoNumero
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsEnableTipoOperacao
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IncluirInformacaoIbpt
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool PermiteOpcaoInformacaoIbpt
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public event EventHandler ProximoPassoCalled;
        public event EventHandler AlterarEmissorCalled;
        public event EventHandler AlterarNumeroCalled;

        private void DefinirPermissaoParaInformacaoIbpt()
        {
            if (FinalidadeEmissao == FinalidadeEmissao.Devolucao || TipoOperacao == TipoOperacao.Entrada)
            {
                PermiteOpcaoInformacaoIbpt = false;
                return;
            }

            PermiteOpcaoInformacaoIbpt = true;
        }

        private void DefinirValorParaInformacaoIbpt()
        {
            if (PermiteOpcaoInformacaoIbpt)
            {
                IncluirInformacaoIbpt = true;
                return;
            }

            IncluirInformacaoIbpt = false;
        }

        public void OnProximoPassoCalled()
        {
            try
            {
                if (!Enum.IsDefined(typeof(TipoOperacao), TipoOperacao))
                    throw new InvalidOperationException("Você não escolheu um Tipo da operação");

                if (string.IsNullOrWhiteSpace(NaturezaOperacao))
                    throw new InvalidOperationException("Você não escolheu a Natureza da operação");

                if (!Enum.IsDefined(typeof(FinalidadeEmissao), FinalidadeEmissao))
                    throw new InvalidOperationException("Você não escolheu a finalidade da emissão");

                if (!Enum.IsDefined(typeof(ModalidadeFrete), ModalidadeFrete))
                    throw new InvalidOperationException("Você não escolheu a Modalidade do frete");

                if (EmitidaEm > DateTime.Now)
                    throw new InvalidOperationException("Você não pode emitir a nota com data e hora superior a atual");
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
                return;
            }

            ProximoPassoCalled?.Invoke(this, new EventArgs());
        }

        public void PreecherCom(AbaPerfilNfeDTO perfil)
        {
            TipoOperacao = perfil.TipoOperacao;
            FinalidadeEmissao = perfil.FinalidadeEmissao;
            NaturezaOperacao = perfil.NaturezaOperacao;
            InformacaoAdicional = perfil.Observacao;
            IsEnabledAlocarNovoNumero = false;
        }

        public void PreecherCom(EmissorFiscal emissor)
        {
            DescricaoCertificado = emissor.Descricao;
            NomeEmitente = emissor.Empresa.RazaoSocial;
            DocumentoUnico = emissor.Empresa.DocumentoUnico;
            InscricaoEstadual = emissor.Empresa.InscricaoEstadual;
            RegimeTributario = emissor.Empresa.RegimeTributario;
        }

        public void PreecherCom(Nfeletronica nfe)
        {
            _nfe = nfe;

            var emissor = nfe.Emitente.CarregarDadosEmissor(_sessaoManager);

            SetValue(emissor.Descricao, nameof(DescricaoCertificado));
            SetValue(nfe.EmitidaEm, nameof(EmitidaEm));
            SetValue(nfe.SaidaEm, nameof(SaidaEm));
            SetValue(nfe.TipoOperacao, nameof(TipoOperacao));
            SetValue(nfe.NaturezaOperacao, nameof(NaturezaOperacao));
            SetValue(nfe.FinalidadeEmissao, nameof(FinalidadeEmissao));

            SetValue(nfe.Emitente.Empresa.RazaoSocial, nameof(NomeEmitente));
            SetValue(nfe.Emitente.Empresa.InscricaoEstadual, nameof(InscricaoEstadual));
            SetValue(nfe.Emitente.DocumentoUnicoSemZeroAEsquerda, nameof(DocumentoUnico));
            SetValue(nfe.Emitente.RegimeTributario, nameof(RegimeTributario));
            SetValue(nfe.InformacaoAdicional, nameof(InformacaoAdicional));
            SetValue(nfe.ModalidadeFrete, nameof(ModalidadeFrete));
            SetValue(nfe.SerieEmissao, nameof(SerieDocumento));
            SetValue(nfe.NumeroEmissao, nameof(NumeroDocumento));
            SetValue(nfe.IncluirInformacaoIbpt, nameof(IncluirInformacaoIbpt));

            SetValue(!nfe.Itens.Any(), nameof(IsEnableTipoOperacao));
            SetValue(nfe.PossuiNumeroAlocado(), nameof(IsEnabledAlocarNovoNumero));

            DefinirPermissaoParaInformacaoIbpt();
        }

        public void OnAlterarEmissorCalled()
        {
            AlterarEmissorCalled?.Invoke(this, EventArgs.Empty);
        }

        public void OnAlterarNumeroCalled()
        {
            AlterarNumeroCalled?.Invoke(this, EventArgs.Empty);
        }
    }
}