using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Fusion.Sessao;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Facades;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Financeiro.Repositorios;
using FusionCore.FusionAdm.Financeiro.Servicos;
using FusionCore.Helpers.Pessoa;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.DocumentoAReceber
{
    public class QuitarSelecionadosContexto : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema;
        private readonly IList<DocumentoReceber> _documentos;

        public QuitarSelecionadosContexto(SessaoSistema sessaoSistema)
        {
            _sessaoSistema = sessaoSistema;
            _documentos = new List<DocumentoReceber>();
        }

        public string NomeDoCliente
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string DocumentoDoCliente
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal TotalAbertoSelecionado
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal TotalVencido
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal TotalAbertoBruto
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorRecebimento
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal JurosPendente
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorDesconto
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                SetValue(TotalAbertoBruto - value, nameof(TotalRestante));
            }
        }

        public decimal TotalRestante
        {
            get => GetValue<decimal>();
            set => SetValue(value - ValorDesconto);
        }

        public ETipoRecebimento TipoRecebimento
        {
            get => GetValue<ETipoRecebimento>();
            set => SetValue(value);
        }

        public void PrepararCom(IEnumerable<ResumoDocumentoReceberDTO> items)
        {
            _documentos.Clear();

            int? pessoaId = null;

            TotalAbertoSelecionado = 0.00M;
            TotalVencido = 0.00M;
            JurosPendente = 0.00M;
            TotalAbertoBruto = 0.00M;
            ValorDesconto = 0.00M;
            TotalRestante = 0.00M;
            TipoRecebimento = ETipoRecebimento.Dinheiro;

            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repositorio = new RepositorioDocumentoReceber(sessao);

                foreach (var dto in items)
                {
                    if (pessoaId != null && pessoaId != dto.PessoaId)
                    {
                        throw new InvalidOperationException("Para quitar vários documentos preciso que sejam da mesma Pessoa!");
                    }

                    if (dto.Situacao != Situacao.Aberto)
                    {
                        throw new InvalidOperationException("Para quitar vários documentos preciso que todos etejam Abertos!");
                    }

                    pessoaId = dto.PessoaId;

                    var doc = repositorio.BuscarPeloId(dto.Id);

                    NomeDoCliente = doc.Cliente.Nome;
                    DocumentoDoCliente = doc.Cliente.GetDocumentoUnico();
                    TotalAbertoSelecionado += doc.ValorRestante;
                    TotalVencido += doc.ValorRestanteVencido;
                    JurosPendente += DocumentoReceberHelper.CalcularJurosPendente(doc);
                    TotalAbertoBruto += doc.ValorRestanteCorrigido;
                    _documentos.Add(doc);
                }

                TotalRestante = TotalAbertoBruto - ValorDesconto;
            }
        }

        public void FazerRecebimento()
        {
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_sessaoSistema.UsuarioLogado);

            if (ValorRecebimento <= 0)
            {
                throw new InvalidOperationException("Valor do Recebimento precisa ser maior que 0,00!");
            }

            if (ValorRecebimento > TotalRestante)
            {
                throw new InvalidOperationException("Valor do Recebimento não pode ser maior que o Total Restante!");
            }

            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var servicoCaixa = new ServicoRegistroDeCaixa(sessao, ELocalEventoCaixa.Gestao);
                var servico = new ServicoDocumentoReceber(sessao, servicoCaixa);

                servico.ComputarJuros(_documentos, _sessaoSistema.UsuarioLogado);
                servico.ComputarDesconto(_documentos, ValorDesconto, _sessaoSistema.UsuarioLogado);
                servico.ReceberGrupoDeDocumentos(_documentos, ValorRecebimento, _sessaoSistema.UsuarioLogado, TipoRecebimento);

                sessao.Transaction.Commit();
            }
        }
    }
}