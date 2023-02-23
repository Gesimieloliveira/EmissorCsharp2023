using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionCore.FusionAdm.MdfeEletronico.Factory;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using MDFe.Classes.Informacoes;
using MDFe.Classes.Informacoes.Evento.CorpoEvento;
using MDFe.Classes.Retorno.MDFeEvento;
using MDFe.Servicos.EventosMDFe;
using NHibernate.Util;
using Mdfe = FusionCore.FusionAdm.MdfeEletronico.MDFeEletronico;

namespace Fusion.Visao.MdfeEletronico.IncluirPagamento
{
    public class MdfeEventoPagamentoGridModel : ViewModel
    {
        private ObservableCollection<MDFeEventoPagamento> _eventos = new ObservableCollection<MDFeEventoPagamento>();
        private MDFeEventoPagamento _eventoSelecionada;
        private readonly MDFeEletronico _mdfe;

        public MdfeEventoPagamentoGridModel(MDFeEletronico mdfe)
        {
            _mdfe = mdfe;
        }

        public ObservableCollection<MDFeEventoPagamento> Eventos
        {
            get => _eventos;
            set
            {
                _eventos = value;
                PropriedadeAlterada();
            }
        }

        public MDFeEventoPagamento EventoSelecionada
        {
            get => _eventoSelecionada;
            set
            {
                _eventoSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public void CarregarEventosPagamento()
        {
            Eventos = new ObservableCollection<MDFeEventoPagamento>(_mdfe.EventosPagamentos.OrderByDescending(x => x.Id));
        }

        public Mdfe ObterMdfe()
        {
            return _mdfe;
        }

        public void DeletarEventoPagamento(MDFeEventoPagamento eventoPagamento)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);

                foreach (var informacaoPagamento in eventoPagamento.InformacaoPagamentoLista)
                {
                    foreach (var informacaoPagamentoParcela in informacaoPagamento.Parcelas)
                    {
                        repositorioMdfe.Deletar(informacaoPagamentoParcela);
                    }

                    foreach (var componentePagamentoFrete in informacaoPagamento.ComponentePagamentoFrete)
                    {
                        repositorioMdfe.Deletar(componentePagamentoFrete);
                    }

                    repositorioMdfe.Deletar(informacaoPagamento);
                }

                repositorioMdfe.Deletar(eventoPagamento);

                transacao.Commit();
            }

            Eventos.Remove(eventoPagamento);
        }

        public void EnviarEventoParaSefaz(MDFeEventoPagamento eventoPagamento)
        {
            FactoryConfiguracoesZeusMdfe.CarregarConfiguracao(_mdfe.EmissorFiscal.EmissorFiscalMdfe);

            var servicoEvento = new ServicoMDFeEvento();

            var mdfe = MDFe.Classes.Informacoes.MDFe.LoadXmlString(_mdfe.Emissao.XmlAssinado);


            var infPags = ConverterEventoPagamentoParaZeus(eventoPagamento);

            var retornoEventoZeus = servicoEvento.MDFeEventoPagamentoOperacaoTransporte(mdfe
                , 1
                , _mdfe.Emissao.Protocolo
                , new infViagens
                {
                    nroViagem = int.Parse(eventoPagamento.NumeroReferenciaViagens),
                    qtdViagens = int.Parse(eventoPagamento.QuantidadeViagens)
                }
                , infPags);

            if (retornoEventoZeus.InfEvento.CStat != 135) 
                throw new InvalidOperationException(retornoEventoZeus.InfEvento.XMotivo);

            if (retornoEventoZeus.InfEvento.CStat == 135)
                SalvarConfirmacao(retornoEventoZeus, eventoPagamento);
        }

        private void SalvarConfirmacao(MDFeRetEventoMDFe retornoEventoZeus, MDFeEventoPagamento eventoPagamento)
        {
            eventoPagamento.Autorizado = true;
            eventoPagamento.XmlRetorno = retornoEventoZeus.RetornoXmlString;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioMdfe = new RepositorioMdfe(sessao);

                repositorioMdfe.Salvar(eventoPagamento);

                transacao.Commit();
            }
        }

        private List<infPag> ConverterEventoPagamentoParaZeus(MDFeEventoPagamento eventoPagamento)
        {
            var infPags = new List<infPag>();

            eventoPagamento.InformacaoPagamentoLista.ForEach(x =>
            {
                var infPag = new infPag
                {
                    xNome = x.NomeContratante,
                    CNPJ = x.ObterCnpj(),
                    CPF = x.ObterCpf(),
                    idEstrangeiro = x.ObterIdEstrangeiro(),
                    indPag = ConverterIndicadorPagamento(x.IndicadorPagamento),
                    infBanc = new infBanc
                    {
                        CNPJIPEF = x.InformarApenasCnpjIpef ? x.CnpjIpef : null,
                        codAgencia = x.InformarApenasCnpjIpef == false ? x.AgenciaBancaria : null,
                        codBanco = x.InformarApenasCnpjIpef == false ? x.ContaBancaria : null
                    },
                    vContrato = x.ValorTotalContrato
                };

                if (x.IndicadorPagamento == IndicadorPagamento.PagamentoAPrazo)
                {
                    infPag.infPrazo = new List<infPrazo>();

                    x.Parcelas.ForEach(parcela =>
                    {
                        var infPrazo = new infPrazo
                        {
                            dVenc = parcela.DataDeVencimento,
                            nParcela = short.Parse(parcela.Numero.ToString()),
                            vParcela = parcela.Valor
                        };

                        infPag.infPrazo.Add(infPrazo);
                    });
                }

                infPag.Comp = new List<Comp>();

                x.ComponentePagamentoFrete.ForEach(componente =>
                {
                    infPag.Comp.Add(new Comp
                    {
                        tpComp = ConverterTipoComponente(componente.TipoComponente),
                        vComp = componente.Valor,
                        xComp = componente.ObterDescricao()
                    });
                });

                infPags.Add(infPag);
            });

            return infPags;
        }

        private tpComp ConverterTipoComponente(TipoComponente componenteTipoComponente)
        {
            switch (componenteTipoComponente)
            {
                case TipoComponente.ValePedagio:
                    return tpComp.ValePedagio;
                case TipoComponente.ImpostosTaxasContribuicoes:
                    return tpComp.ImpostosTaxasEContribuicoes;
                case TipoComponente.DespesasBancariasMeiosPagamentoOutras:
                    return tpComp.DespesasBancariasEmiosDePagamentoOutras;
                case TipoComponente.Outros:
                    return tpComp.Outros;
                default:
                    throw new ArgumentOutOfRangeException(nameof(componenteTipoComponente), componenteTipoComponente, null);
            }
        }

        private indPag ConverterIndicadorPagamento(IndicadorPagamento objIndicadorPagamento)
        {
            switch (objIndicadorPagamento)
            {
                case IndicadorPagamento.PagamentoAPrazo:
                    return indPag.PagamentoAPrazo;
                case IndicadorPagamento.PagamentoAVista:
                    return indPag.PagamentoAVista;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objIndicadorPagamento), objIndicadorPagamento, null);
            }
        }
    }
}