using System;
using System.Linq;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Fiscal.NF.Cancelar;
using FusionCore.FusionNfce.Autorizacao;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Servico;
using FusionCore.FusionNfce.Servicos;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Usuario;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Flags;
using NFe.Classes.Servicos.Consulta;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos.Retorno;
using ServicoZeus = NFe.Servicos.ServicosNFe;

namespace FusionNfce.Visao.Cancelamento
{
    public sealed class CanceladorSefaz
    {
        private readonly IDadosServicoSefaz _dadosServico;
        private readonly UsuarioNfce _usuario;

        public CanceladorSefaz(IDadosServicoSefaz dadosServico)
        {
            _dadosServico = dadosServico;
            _usuario = SessaoSistemaNfce.Usuario;
        }

        public EventoCancelamento CancelarDocumento(string justificativa, Nfce nfce)
        {
            using (var financeiro = new ServicoControleFinanceiroNfce(_usuario))
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNfce(sessao);

                financeiro.CancelarFinanceiroNfce(nfce);
                new GeraRegistroCaixa(nfce, sessao, _usuario).EstornarCaixa();

                var evento = FazerCancelamentoNaSefaz(nfce, justificativa);

                var cancelamento = new NfceCancelamento
                {
                    Justificativa = evento.Justificativa,
                    Nfce = nfce,
                    Ambiente = nfce.Emissao.TipoAmbiente,
                    DocumentoUnico = nfce.Emitente.Empresa.Cnpj,
                    Protocolo = evento.NumeroProtocolo,
                    Chave = nfce.NumeroChave,
                    OcorreuEm = DateTime.Now,
                    CodigoUf = nfce.Emitente.Empresa.Estado.CodigoIbge,
                    TipoEvento = 110111,
                    StatusRetorno = evento.Status.Codigo,
                    VersaoAplicacao = ""
                };

                nfce.Cancelamento = cancelamento;

                if (evento.Status.EstaCancelado)
                {
                    nfce.FoiCancelada();
                }

                repositorio.SalvarESincronizar(nfce);

                var itensNaoCancelados = nfce.ObterOsItens().Where(i => i.Cancelado == false);

                foreach (var i in itensNaoCancelados)
                {
                    var estoqueServico = EstoqueServicoNfce.Cria(
                        sessao,
                        i.Produto,
                        OrigemEventoEstoque.CancelamentoNfce,
                        TipoEventoEstoque.Entrada,
                        i.Quantidade
                    );

                    estoqueServico.Acrescentar();
                }

                financeiro.ComitarAlteracoes();
                transacao.Commit();

                return evento;
            }
        }

        private EventoCancelamento FazerCancelamentoNaSefaz(Nfce nfce, string justificativa)
        {
            var cfgBuilder = new ConfiguracaoZeusBuilder(_dadosServico, nfce.TipoEmissaoCancelamento());
            var configuracao = cfgBuilder.GetConfiguracao();
            using (var servico = new ServicoZeus(configuracao))
            {
                var lote = nfce.ReferenciaId;
                var protocolo = nfce.NumeroProtocolo;
                var chave = nfce.NumeroChave;
                var cnpj = nfce.CnpjCpfEmitente;

                var retornoConsulta = ConsularCancelamentoNaSefaz(chave, out var xmlEnvio, out var xmlRetorno, servico);

                if (retornoConsulta != null)
                {
                    var retorno = new RetornoCancelamento(
                        retornoConsulta.retEvento.infEvento.cStat,
                        retornoConsulta.retEvento.infEvento.xMotivo,
                        retornoConsulta.retEvento.infEvento.nProt
                    )
                    {
                        TpAmb = retornoConsulta.retEvento.infEvento.tpAmb,
                        EnvioStr = xmlEnvio,
                        RetornoCompletoStr = xmlRetorno
                    };

                    return new EventoCancelamento(justificativa, nfce, retorno);
                }

                RetornoRecepcaoEvento retornoEvento = null;

                if (SessaoSistemaNfce.IsEmissorNFce())
                {
                    retornoEvento = servico.RecepcaoEventoCancelamento(lote, 1, protocolo, chave, justificativa, cnpj);
                }

                if (retornoEvento?.Retorno == null || retornoEvento.Retorno.retEvento.Count == 0)
                {
                    throw new InvalidOperationException(
                        "Cancelamento na SEFAZ não obteve um retorno. Aguarde alguns minutos e tente novamente!");
                }

                var infEvento = retornoEvento.Retorno.retEvento[0].infEvento;

                var retornoCancelamento = new RetornoCancelamento(infEvento.cStat, infEvento.xMotivo, infEvento.nProt)
                {
                    TpAmb = infEvento.tpAmb,
                    EnvioStr = retornoEvento.EnvioStr,
                    RetornoCompletoStr = retornoEvento.RetornoCompletoStr
                };

                return new EventoCancelamento(justificativa, nfce, retornoCancelamento);
            }
        }

        private procEventoNFe ConsularCancelamentoNaSefaz(
            string chaveAcesso,
            out string xmlEnvio,
            out string xmlRetorno,
            ServicoZeus servico
        ) {
            RetornoNfeConsultaProtocolo retornoConsulta = null;

            if (SessaoSistemaNfce.IsEmissorNFce())
            {
                retornoConsulta = servico.NfeConsultaProtocolo(chaveAcesso);
            }

            var eventoRetornado =
                retornoConsulta.Retorno.procEventoNFe.SingleOrDefault(p => p.evento.infEvento.chNFe == chaveAcesso
                                                                           && p.evento.infEvento.tpEvento ==
                                                                           NFeTipoEvento.TeNfeCancelamento
                                                                           && p.evento.infEvento.nSeqEvento == 1);

            xmlEnvio = retornoConsulta.EnvioStr;
            xmlRetorno = retornoConsulta.RetornoCompletoStr;

            return eventoRetornado;
        }
    }
}