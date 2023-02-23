using System;
using System.Collections.Generic;
using System.Data;
using Fusion.ContextoCompartilhado;
using Fusion.Sessao;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Facades;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Financeiro.Servicos;
using FusionCore.Helpers.Basico;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.DocumentoAReceber
{
    public class FormularioDocumentoReceberContexto : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema;
        private DocumentoReceber _documento;

        public FormularioDocumentoReceberContexto(SessaoSistema sessaoSistema)
        {
            _sessaoSistema = sessaoSistema;

            EmpresaContexto = new EmpresaComboBoxContexto(sessaoSistema);
            TipoDocumentoContexto = new TipoDocumentoComboBoxContexto(sessaoSistema);
            ClienteContexto = new ClientePickerContexto();
            Lancamentos = new List<DocumentoReceberLancamento>();
            PodeEditar = true;
            EhNovoRegistro = true;
            DataEmissao = DateTime.Now;
        }

        public EmpresaComboBoxContexto EmpresaContexto
        {
            get => GetValue<EmpresaComboBoxContexto>();
            private set => SetValue(value);
        }

        public TipoDocumentoComboBoxContexto TipoDocumentoContexto
        {
            get => GetValue<TipoDocumentoComboBoxContexto>();
            set => SetValue(value);
        }

        public ClientePickerContexto ClienteContexto
        {
            get => GetValue<ClientePickerContexto>();
            set => SetValue(value);
        }

        public bool PodeEditar
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public bool EhNovoRegistro
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string StatusDocumento
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Descricao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime? DataEmissao
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public DateTime? DataVencimento
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public decimal ValorDocumento
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorDesconto
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorRecebido
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public byte NumeroParcela
        {
            get => GetValue<byte>();
            set => SetValue(value);
        }

        public decimal ValorRestante
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal JurosCalculado
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal JurosPendente
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorRestanteCorrigido
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorOriginal
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public IList<DocumentoReceberLancamento> Lancamentos
        {
            get => GetValue<IList<DocumentoReceberLancamento>>();
            set => SetValue(value);
        }

        public void CarregarDocumento(int id)
        {
            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao())
            {
                var servico = new ServicoCadastroDeDocumento(sessao);
                var documento = servico.ObterCadastroPeloId(id);

                CarregarDocumento(documento);
            }
        }

        public void CarregarDocumento(DocumentoReceber documento)
        {
            EhNovoRegistro = documento.Id == 0;

            EmpresaContexto.EmpresaSelecionada = documento.Empresa.ToComboBox();
            ClienteContexto.ClienteSelecionado = documento.Cliente;
            TipoDocumentoContexto.TipoSelecionado = (TipoDocumento)documento.TipoDocumento;
            Descricao = documento.Descricao;
            DataEmissao = documento.EmitidoEm;
            DataVencimento = documento.Vencimento;
            StatusDocumento = documento.Situacao.GetDescription();
            NumeroParcela = documento.Parcela;
            ValorOriginal = documento.ValorOriginal;
            ValorDocumento = documento.ValorDocumento;
            ValorDesconto = documento.TotalDesconto;
            JurosCalculado = documento.TotalJuros;
            ValorRecebido = documento.ValorQuitado;
            ValorRestante = documento.ValorRestante;
            JurosPendente = documento.ValorJurosPendente;
            ValorRestanteCorrigido = documento.ValorRestanteCorrigido;
            PodeEditar = documento.Situacao == Situacao.Aberto;

            Lancamentos = new List<DocumentoReceberLancamento>(documento.Lancamentos);

            _documento = documento;
        }

        public void ThrowExceptionSeDadosInvalido()
        {
            if (EmpresaContexto.EmpresaSelecionada == null)
            {
                throw new InvalidOperationException("Preciso de uma Empresa para o documento!");
            }

            if (ClienteContexto.ClienteSelecionado == null)
            {
                throw new InvalidOperationException("Preciso de um Cliente para o documento!");
            }

            if (TipoDocumentoContexto.TipoSelecionado == null)
            {
                throw new InvalidOperationException("Preciso de um Tipo Documento para o documento!");
            }

            if (DataEmissao == null)
            {
                throw new InvalidOperationException("Preciso de uma Data Emissão para o documento!");
            }

            if (DataVencimento == null)
            {
                throw new InvalidOperationException("Preciso de uma Data Vencimento para o documento!");
            }

            if (ValorDocumento <= 0)
            {
                throw new InvalidOperationException("Valor documento precisa ser maior que 0,00");
            }
        }

        public void SalvarAlteracoes()
        {
            using (var session = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadUncommitted))
            {
                var servico = new ServicoCadastroDeDocumento(session);

                if (_documento == null || _documento.Id == 0)
                {
                    _documento = servico.CriarNovo(
                        _sessaoSistema.UsuarioLogado,
                        EmpresaContexto.EmpresaSelecionada,
                        ClienteContexto.ClienteSelecionado,
                        ValorDocumento,
                        DataEmissao.Value,
                        DataVencimento.Value,
                        TipoDocumentoContexto.TipoSelecionado,
                        1,
                        Descricao
                    );

                    _documento.CriarMalote(OrigemDocumento.Manual, _sessaoSistema.UsuarioLogado);
                }

                _documento.Empresa = EmpresaContexto.EmpresaSelecionada.CarregaEmpresa(_sessaoSistema.SessaoManager);
                _documento.Descricao = Descricao ?? string.Empty;
                _documento.EmitidoEm = DataEmissao.Value;
                _documento.Vencimento = DataVencimento.Value;
                _documento.TipoDocumento = TipoDocumentoContexto.TipoSelecionado;

                _documento.AjustarValorPara(ValorDocumento, _sessaoSistema.UsuarioLogado);

                servico.SalvarDocumento(_documento);

                CarregarDocumento(_documento);

                session.Transaction.Commit();
            }
        }

        public void EstornarUltimoLancamento()
        {
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_sessaoSistema.UsuarioLogado);

            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var servicoCaixa = new ServicoRegistroDeCaixa(sessao, ELocalEventoCaixa.Gestao);
                var servico = new ServicoDocumentoReceber(sessao, servicoCaixa);

                servico.EstornarUltimoLancamento(_documento, _sessaoSistema.UsuarioLogado);

                sessao.Transaction.Commit();
            }

            CarregarDocumento(_documento);
        }

        public void FazerCancelamento()
        {
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_sessaoSistema.UsuarioLogado);

            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var servicoCaixa = new ServicoRegistroDeCaixa(sessao, ELocalEventoCaixa.Gestao);
                var servico = new ServicoDocumentoReceber(sessao, servicoCaixa);

                servico.FazerCancelamento(_documento, _sessaoSistema.UsuarioLogado);

                sessao.Transaction.Commit();
            }

            CarregarDocumento(_documento);
        }

    }
}