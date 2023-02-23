using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using Fusion.Base.Notificacoes;
using Fusion.Sessao;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.ValidacaoAnotacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Parcelamento;
using NHibernate;

namespace Fusion.Visao.DocumentoAPagar
{
    public delegate void GerarContasPagarAntesComitar(ISession session, Malote malote);

    public class GerarContasPagarModel : ViewModel
    {
        private const string DescricaoPadrao = "PARCELA GERADA MANUALMENTE";
        private readonly Notificador _notificador;
        private readonly SessaoSistema _sessaoSistema;

        public GerarContasPagarModel(Notificador notificador, SessaoSistema sessaoSistema)
        {
            _notificador = notificador;
            _sessaoSistema = sessaoSistema;

            ParcelasItems = new List<ParcelaGerada>();
            DataEmissao = DateTime.Now;
            Descricao = DescricaoPadrao;
            EmpresaIsEnable = true;
            FornecedorIsEnable = true;
            DataEmissaoIsEnable = true;
        }

        public GerarContasPagarAntesComitar AntesComitarDelegate;

        public bool UsuarioTemPermissao => _sessaoSistema
            .UsuarioLogado
            .VerificaPermissao
            .IsTemPermissao(Permissao.FINANCEIRO_DOCUMENTO_APAGAR_GERAR_AVULSO);

        [Required(ErrorMessage = "Preciso que selecione uma empresa!")]
        public EmpresaComboBoxDTO EmpresaSelecionada
        {
            get => GetValue<EmpresaComboBoxDTO>();
            set => SetValue(value);
        }

        public bool EmpresaIsEnable
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Preciso que selecione um fornecedor!")]
        public Fornecedor FornecedorSelecionado
        {
            get => GetValue<Fornecedor>();
            set => SetValue(value);
        }

        public bool FornecedorIsEnable
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Preciso que selecione a data de emissão")]
        public DateTime? DataEmissao
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public bool DataEmissaoIsEnable
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        [DecimalRequired(ErrorMessage = "Preciso de um valor total maior que zero!")]
        public decimal? ValorTotal
        {
            get => GetValue<decimal?>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Preciso que selecione qual o tipo de documento!")]
        public TipoDocumento TipoDocumento
        {
            get => GetValue<TipoDocumento>(); 
            set => SetValue(value);
        }

        public IList<ParcelaGerada> ParcelasItems
        {
            get => GetValue<IList<ParcelaGerada>>();
            set => SetValue(value);
        }

        public string NumeroDocumento
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Descricao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string ConfirmacaoAoFehcarSemGerar
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public void SalvarDocumentos()
        {
            if (!UsuarioTemPermissao)
            {
                throw new InvalidOperationException("Usuário sem permissão para gerar documentos a pagar de forma avulsa!");
            }

            ThrowExeceptionSeDadosInvalido();

            var malote = Malote.Cria(OrigemDocumento.Manual, string.Empty, _sessaoSistema.UsuarioLogado, 0);

            foreach (var p in ParcelasItems)
            {
                var doc = new DocumentoPagar
                {
                    Vencimento = p.Vencimento,
                    Descricao = Descricao.TrimOrEmpty(),
                    Fornecedor = FornecedorSelecionado,
                    CentroCusto = null,
                    EmitidoEm = DataEmissao.Value,
                    Empresa = EmpresaSelecionada.CarregaEmpresa(_sessaoSistema.SessaoManager),
                    Malote = malote,
                    Parcela = p.Numero,
                    NumeroDocumento = NumeroDocumento ?? string.Empty,
                    Situacao = Situacao.Aberto,
                    ValorOriginal = p.Valor,
                    ValorAjustado = p.Valor,
                    TipoDocumento = TipoDocumento
                };

                malote.DocumentosPagar.Add(doc);
            }

            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repositorio = new RepositorioMalote(sessao);
                repositorio.Persiste(malote);
                AntesComitarDelegate?.Invoke(sessao, malote);

                sessao.Transaction.Commit();
            }

            _notificador.Notificar("documentoPagarGerados", new NotificacaoArgs(malote));
        }

        private void ThrowExeceptionSeDadosInvalido()
        {
            ThrowExceptionSeExistirErros();

            if (DataEmissao.Value.Date > DateTime.Today)
            {
                throw new InvalidOperationException("Data emissão não pode ser maior que atual!");
            }

            if (ValorTotal != ParcelasItems.Sum(i => i.Valor))
            {
                throw new InvalidOperationException("Valor das parcelas geradas não bate com o valor informado!");
            }

            if (ParcelasItems.Any(i => i.Vencimento < DataEmissao.Value))
            {
                throw new InvalidOperationException("Não pode existir parcelas com vencimento menor que a data emissão");
            }
        }

        public void ComParcelas(ParcelamentoArgs args)
        {
            ParcelasItems = new List<ParcelaGerada>(args.Parcelas);
            TipoDocumento = BuscarTipoDocumentoCorrespondente(args.TipoDocumento);
        }

        private TipoDocumento BuscarTipoDocumentoCorrespondente(ITipoDocumento tipo)
        {
            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioTipoDocumento(sessao);
                return repositorio.GetPeloId(tipo.Id);
            }
        }
    }
}