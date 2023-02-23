using System;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Financeiro.Facades;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;

namespace FusionCore.FusionAdm.Fiscal.NF.Autorizacao
{
    public class FinalizadorNfe
    {
        private readonly ISessaoManager _sessaoManager;
        private readonly UsuarioDTO _usuarioAcao;

        public FinalizadorNfe(ISessaoManager sessaoManager, UsuarioDTO usuarioAcao)
        {
            _sessaoManager = sessaoManager;
            _usuarioAcao = usuarioAcao;
        }

        public void FinalizarEmissao(Nfeletronica nfe, EmissaoNfe emissao)
        {
            var emissorFiscal = emissao.EmissorFiscal;
            var certificado = CertificadoDigitalFactory.Cria(emissorFiscal, true);
            var cfg = new ConfiguracaoZeusBuilder(emissorFiscal.EmissorFiscalNfe, emissao.TipoEmissao);

            var situacaoNotaSefaz = new SituacaoNotaSefaz(cfg, certificado);
            ProcessarFinalizacao(emissao, situacaoNotaSefaz);

            if (emissao.IsRejeicao999())
            {
                throw new InvalidOperationException(emissao.GetTextoRejeicao());
            }

            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNfe(sessao);

                try
                {
                    repositorio.Alterar(emissao);

                    if (emissao.IsAutorizadoUsoDaEmissao())
                    {
                        //TODO: Melhorar transação para que a emissão não falhe caso o estoque ou financeiro falhe
                        nfe.Finalizar(EmissaoFinalizadaNfe.CriarFinalizacao(nfe, emissao));

                        repositorio.SalvarAlteracoes(nfe);

                        GeradorFinanceiro.Gerar(sessao, nfe, _usuarioAcao);

                        new ServicoRegistroDeCaixa(sessao, ELocalEventoCaixa.Gestao).RegistrarVenda(nfe, _usuarioAcao);
                    }

                    if (emissao.IsDenegadoUsoDaEmissao())
                    {
                        nfe.Finalizar(EmissaoFinalizadaNfe.CriarFinalizacao(nfe, emissao));

                        repositorio.SalvarAlteracoes(nfe);

                        NfeEstoqueHelper.DenegacaoNfe(nfe, _usuarioAcao, sessao);
                    }

                    transacao.Commit();
                }
                catch (System.Exception e)
                {
                    transacao.Rollback();
                    sessao.Clear();

                    sessao.Load(nfe, nfe.Id);
                    sessao.Load(emissao, emissao.Id);

                    throw;
                }
            }
        }

        private void ProcessarFinalizacao(EmissaoNfe emissao, SituacaoNotaSefaz situacaoNotaSefaz)
        {
            if (emissao.PossuiRecibo())
            {
                var respostaRecibo = situacaoNotaSefaz.GetSituacaoPeloRecibo(emissao.GetRecibo());
                emissao.ProcessarRespotaLote(respostaRecibo);

                if (emissao.IsRejeicao999())
                {
                    throw new InvalidOperationException(emissao.GetTextoRejeicao());
                }
                return;
            }

            var respostaChave = situacaoNotaSefaz.GetSituacaoPelaChave(emissao.Chave);
            emissao.ProcessarRespostaPelaChave(respostaChave);

            if (emissao.IsRejeicao999())
            {
                throw new InvalidOperationException(emissao.GetTextoRejeicao());
            }
        }
    }
}