using System.Collections.Generic;
using FusionCore.Extencoes;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate.Util;
using ExtObject = DFe.Ext.ExtObject;

namespace FusionCore.NfceSincronizador.Sync
{
    public class EnviarInutilizacao : ISincronizavelPadrao
    {
        public void RealizarSincronizacao()
        {
            Sincroniza();
        }

        private void Sincroniza()
        {
            var todasInutilizacoes = BuscarInutilizacoes();

            todasInutilizacoes.ForEach(inut =>
            {
                var sessaoServidor = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao();
                var sessaoNfce = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

                var transacaoServidor = sessaoServidor.BeginTransaction();
                var transacaoNfce = sessaoNfce.BeginTransaction();

                var repositorioNfce = new RepositorioNfce(sessaoNfce);
                var repositorioNfceAdm = new RepositorioInutilizacao(sessaoServidor);

                using (repositorioNfce)
                using (repositorioNfceAdm)
                using (transacaoNfce)
                using (transacaoServidor)
                {
                    var inutilizacaoAdm = BuscarInutilizacaoPorUuid(inut.Uuid, repositorioNfceAdm);

                    if (ExtObject.IsNotNull(inutilizacaoAdm))
                    {
                        repositorioNfce.Refresh(inut);
                        inut.Sincronizado = true;

                        sessaoServidor.Flush();
                        sessaoNfce.Flush();
                        transacaoServidor.Commit();
                        transacaoNfce.Commit();
                        return;
                    }

                    repositorioNfceAdm.Salvar(new NfeInutilizacaoNumeracaoDTO
                    {
                        Ano = inut.Ano,
                        NumeroFinal = inut.NumeroFinal,
                        Uuid = inut.Uuid,
                        CnpjEmitente = inut.CnpjEmitente,
                        CodigoUfSolicitante = inut.CodigoUfSolicitante,
                        InutilizacaoEm = inut.InutilizacaoEm,
                        Justificativa = inut.Justificativa,
                        ModeloDocumento = inut.ModeloDocumento,
                        NumeroInicial = inut.NumeroInicial,
                        Protocolo = inut.Protocolo,
                        Serie = inut.Serie
                    });

                    inut.Sincronizado = true;
                    repositorioNfce.Salvar(inut);

                    sessaoServidor.Flush();
                    sessaoNfce.Flush();

                    transacaoServidor.Commit();
                    transacaoNfce.Commit();
                }
            });
        }

        private NfeInutilizacaoNumeracaoDTO BuscarInutilizacaoPorUuid(string uuid,
            RepositorioInutilizacao repositorioNfceAdm)
        {
            return uuid.IsNullOrEmpty() ? null : repositorioNfceAdm.BuscarPorUuid(uuid);
        }

        private IEnumerable<NfceInutilizacao> BuscarInutilizacoes()
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao();

            var repositorio = new RepositorioNfce(sessao);

            using (sessao)
            {
                return repositorio.BuscarTodasInutilizacoesPendentes();
            }
        }
    }
}