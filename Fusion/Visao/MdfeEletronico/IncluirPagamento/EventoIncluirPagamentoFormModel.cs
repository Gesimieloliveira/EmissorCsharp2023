using System;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using NHibernate.Util;
using Mdfe = FusionCore.FusionAdm.MdfeEletronico.MDFeEletronico;

namespace Fusion.Visao.MdfeEletronico.IncluirPagamento
{
    public class EventoIncluirPagamentoFormModel : ViewModel
    {
        public EventoIncluirPagamentoFormModel(Mdfe mdfe, MDFeEventoPagamento eventoSelecionado)
        {
            _mdfe = mdfe;
            _eventoSelecionado = eventoSelecionado;
        }

        private ObservableCollection<InformacaoPagamento> _informacoesPagamentos = new ObservableCollection<InformacaoPagamento>();
        private InformacaoPagamento _informacaoPagamentoSelecionada;
        private string _quantidadeTotalViagens;
        private string _numeroReferenciaViagem;
        private readonly Mdfe _mdfe;
        private readonly MDFeEventoPagamento _eventoSelecionado;

        public bool EUmaEdicao { get; set; }

        public string QuantidadeTotalViagens
        {
            get => _quantidadeTotalViagens;
            set
            {
                _quantidadeTotalViagens = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroReferenciaViagem
        {
            get => _numeroReferenciaViagem;
            set
            {
                _numeroReferenciaViagem = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<InformacaoPagamento> InformacoesPagamentos
        {
            get => _informacoesPagamentos;
            set
            {
                _informacoesPagamentos = value;
                PropriedadeAlterada();
            }
        }

        public void IncluirPagamento(IncluirPagamentoFormModel model)
        {
            var informacaoPagamento = model.ObterInformacaoPagamento();

            if (informacaoPagamento.Id == 0 && EUmaEdicao)
            {
                InserirInformacaoPagamento(informacaoPagamento);
                InformacoesPagamentos.Add(informacaoPagamento);
                return;
            }

            if (informacaoPagamento.Id != 0 && EUmaEdicao)
            {
                EditarInformacaoPagamento(informacaoPagamento);

                InformacoesPagamentos.Remove(informacaoPagamento);
                InformacoesPagamentos.Add(informacaoPagamento);
                return;
            }

            InformacoesPagamentos.Add(informacaoPagamento);

        }

        private void EditarInformacaoPagamento(InformacaoPagamento informacaoPagamento)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);

                informacaoPagamento.EventoPagamento = _eventoSelecionado;

                repositorioMdfe.Salvar(informacaoPagamento);

                transacao.Commit();

                _eventoSelecionado.InformacaoPagamentoLista.Remove(informacaoPagamento);
                _eventoSelecionado.InformacaoPagamentoLista.Add(informacaoPagamento);
            }
        }

        private void InserirInformacaoPagamento(InformacaoPagamento informacaoPagamento)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);

                informacaoPagamento.EventoPagamento = _eventoSelecionado;
                informacaoPagamento.Parcelas.ForEach(parcela =>
                {
                    parcela.InformacaoPagamento = informacaoPagamento;
                });
                informacaoPagamento.ComponentePagamentoFrete.ForEach(componente =>
                {
                    componente.InformacaoPagamento = informacaoPagamento;
                });

                repositorioMdfe.Salvar(informacaoPagamento);

                transacao.Commit();
                _eventoSelecionado.InformacaoPagamentoLista.Add(informacaoPagamento);
            }
        }

        public void RemoverInormacaoPagamento()
        {
            if (EUmaEdicao)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorio = new RepositorioMdfe(sessao);

                    InformacaoPagamentoSelecionada.Parcelas.ForEach(repositorio.Deletar);
                    InformacaoPagamentoSelecionada.ComponentePagamentoFrete.ForEach(repositorio.Deletar);
                    repositorio.Deletar(InformacaoPagamentoSelecionada);

                    transacao.Commit();
                }
            }

            InformacoesPagamentos.Remove(InformacaoPagamentoSelecionada);
            _eventoSelecionado.InformacaoPagamentoLista.Remove(InformacaoPagamentoSelecionada);
        }

        public InformacaoPagamento InformacaoPagamentoSelecionada
        {
            get => _informacaoPagamentoSelecionada;
            set
            {
                _informacaoPagamentoSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public void SalvarEventoMdfe()
        {

            NumeroReferenciaViagem = NumeroReferenciaViagem.TrimOrEmpty();
            QuantidadeTotalViagens = QuantidadeTotalViagens.TrimOrEmpty();

            if (NumeroReferenciaViagem.Length == 0)
                throw new InvalidOperationException("Número Referência Viagens");

            if (QuantidadeTotalViagens.Length == 0)
                throw new InvalidOperationException("Quantidade Total de Viagens");

            if (EUmaEdicao)
                AtualizarEvento();

            if (EUmaEdicao == false)
                SalvarUmNovoEvento();

            OnFechar();
        }

        private void AtualizarEvento()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _eventoSelecionado.QuantidadeViagens = QuantidadeTotalViagens;
                _eventoSelecionado.NumeroReferenciaViagens = NumeroReferenciaViagem;

                var repositorioMdfe = new RepositorioMdfe(sessao);

                repositorioMdfe.Salvar(_eventoSelecionado);

                transacao.Commit();
            }
        }

        private void SalvarUmNovoEvento()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioMdfe(sessao);

                var eventoPagamento = new MDFeEventoPagamento
                {
                    InformacaoPagamentoLista = _informacoesPagamentos.ToList(),
                    NumeroReferenciaViagens = NumeroReferenciaViagem,
                    QuantidadeViagens = QuantidadeTotalViagens,
                    Mdfe = _mdfe
                };

                eventoPagamento.InformacaoPagamentoLista.ForEach(x => { x.EventoPagamento = eventoPagamento; });

                _mdfe.EventosPagamentos.Add(eventoPagamento);


                repositorio.Salvar(eventoPagamento);

                eventoPagamento.InformacaoPagamentoLista.ForEach(x => { repositorio.Salvar(x); });

                eventoPagamento.InformacaoPagamentoLista.ForEach(infPag =>
                {
                    infPag.Parcelas.ForEach(parcela =>
                    {
                        parcela.InformacaoPagamento = infPag;
                        repositorio.Salvar(parcela);
                    });

                    infPag.ComponentePagamentoFrete.ForEach(componente =>
                    {
                        componente.InformacaoPagamento = infPag;
                        repositorio.Salvar(componente);
                    });
                });

                transacao.Commit();
            }
        }

        public void Iniciar()
        {
            if (_eventoSelecionado != null && _eventoSelecionado.Id != 0)
            {
                EUmaEdicao = true;
            }

            if (EUmaEdicao)
            {
                CarregarInformacoes();
            }
        }

        private void CarregarInformacoes()
        {
            QuantidadeTotalViagens = _eventoSelecionado.QuantidadeViagens;
            NumeroReferenciaViagem = _eventoSelecionado.NumeroReferenciaViagens;

            if (_eventoSelecionado.InformacaoPagamentoLista.Count != 0)
                InformacoesPagamentos = new ObservableCollection<InformacaoPagamento>(_eventoSelecionado.InformacaoPagamentoLista);
        }
    }
}